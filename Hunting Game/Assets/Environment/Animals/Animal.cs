using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Movement")]
    public Rigidbody2D rb;
    public float speed;
    public Vector2 targetPos;
    public Vector2 distRange;
    public float arriveThresh;
    Vector2 moveVec;

    [Header("Idling")]
    public bool idle = false;
    public Vector2 idleDelayRange;
    int idleDelay;
    int idleCounter = 0;
    public Vector2 idleTimeRange;
    float idleTime;
    float idleTimer;
    float maxRunTime;
    float runningTime;

    [Header("Animations")]
    public Animator anim;

    [Header("Aging")]
    public GameObject childPrefab;
    public float age;
    public float ageTime;
    public float ageDelay;
    float ageTimer;
    public AnimationCurve sizeOverAge;

    [Header("Reproduction")]
    public Vector2 rDistRange;
    public Vector2 rDelayRange;
    public int maxChildren;
    int childrenCount = 0;
    float rTimer = 0;
    float rDelay;

    [Header("Hunger")]
    public float food = 1;
    public float starveRate;
    public Gradient colorOverHunger;

    [Header("Health")]
    public float health = 1;
    public AnimationCurve passiveHealingOverAge;

    [Header("Eating")]
    public float eatThresh;
    public float eatRange;
    public float eatAmount;
    public string eatAnimTrigger;

    [Header("Fertilization")]
    public Ground ground;
    public Vector2 fRadiusRange;
    public Vector2 fDelayRange;
    public float fAdd;
    float fTimer = 0;
    float fDelay;

    [Header("Death")]
    public GameObject hitEffect;
    public GameObject deathEffect;
    public GameObject meatAsset;
    public GameObject statText;
    public Vector3 statTextOffset;


    void Start()
    {
        // initialize values
        health = 1;
        childrenCount = 0;
        idleCounter = 0;
        // Initialize Timers
        ageTimer = 0;
        idleDelay = Mathf.RoundToInt(Random.Range(idleDelayRange[0], idleDelayRange[1]));
        idleTimer = 0;
        idleTime = Random.Range(idleTimeRange[0], idleTimeRange[1]);
        rDelay = Random.Range(rDelayRange[0], rDelayRange[1]);
        rTimer = 0;
        fDelay = Random.Range(fDelayRange[0], fDelayRange[1]);
        fTimer = 0;


        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        ground = GameObject.FindGameObjectWithTag("Ground").GetComponent<Ground>();
        transform.localScale = Vector3.one * sizeOverAge.Evaluate(age);
        startWalking();
    }
    void Update()
    {
        // MOVEMENT
        if (!idle)
        {
            runningTime += Time.deltaTime;
            // Determine closest cardinal direction
            Vector2 dirVec = rb.velocity.normalized;
            float absX = Mathf.Abs(dirVec.x);
            float absY = Mathf.Abs(dirVec.y);

            float animX = 0;
            float animY = 0;

            if (absX > absY)
            {
                animX = dirVec.x > 0 ? 1 : -1;
                animY = 0;
            }
            else
            {
                animX = 0;
                animY = dirVec.y > 0 ? 1 : -1;
            }

            anim.SetFloat("x", animX);
            anim.SetFloat("y", animY);

            Debug.DrawLine(transform.position, targetPos, Color.red);
            if (Vector2.Distance(transform.position, targetPos) <= arriveThresh)
            {

                if (idleCounter == idleDelay)
                {
                    goIdle();
                }
                else
                {
                    idleCounter++;
                    startWalking();
                }
            }
            if (runningTime > maxRunTime)
            {
                goIdle();
            }
            // Eating when arriving at target position
            if (food < eatThresh)
            {
                bool inRange = false;
                foreach (GameObject plant in gameManager.plants)
                {
                    if (Vector2.Distance(transform.position, plant.transform.position) < eatRange)
                    {
                        gameManager.plants.Remove(plant);
                        Destroy(plant);
                        inRange = true;
                        break;
                    }
                }
                if (inRange)
                {
                    // Eating
                    goIdle();
                    food += eatAmount;
                    anim.SetTrigger(eatAnimTrigger);
                }
            }
        }
        // IDLING
        else
        {
            // Idle Animation
            anim.SetFloat("x", 0);
            anim.SetFloat("y", 0);

            // Check if idle time is over
            idleTimer += Time.deltaTime;
            if (idleTimer > idleTime)
            {
                startWalking();
            }
        }


        // REPRODUCTION LOGIC
        if (age == 1)
        {
            rTimer += Time.deltaTime;
            if (rTimer > rDelay && childrenCount < maxChildren)
            {
                childrenCount++;
                rTimer = 0;
                rDelay = Random.Range(rDelayRange[0], rDelayRange[1]);
                float dir = Random.Range(0f, 2 * Mathf.PI);
                float dist = Random.Range(rDistRange[0], rDistRange[1]);
                Vector2 spawnPos = new Vector3(Mathf.Cos(dir), Mathf.Sin(dir), 0) * dist + transform.position;
                GameObject child = Instantiate(childPrefab, spawnPos, Quaternion.identity, transform.parent);
                child.GetComponent<Animal>().food = 0.5f + food / 2;
                child.GetComponent<Animal>().age = 0;
            }
        }

        // FERTILIZING GROUND
        fTimer += Time.deltaTime;
        if (fTimer > fDelay)
        {
            fTimer = 0;
            fDelay = Random.Range(fDelayRange[0], fDelayRange[1]);
            float radius = Random.Range(fRadiusRange[0], fRadiusRange[1]);
            ground.FertilizeArea(transform.position, fAdd, radius);
        }

        // HEALTH
        health += passiveHealingOverAge.Evaluate(age) * Time.deltaTime;

        // HUNGER
        food -= starveRate * Time.deltaTime;
        GetComponentInChildren<SpriteRenderer>().color = colorOverHunger.Evaluate(food);

        // AGING
        ageTimer += Time.deltaTime;
        if (ageTimer > ageDelay)
        {
            ageTimer = 0;
            age += ageDelay / ageTime;
            transform.localScale = Vector3.one * sizeOverAge.Evaluate(age);
        }

        // clamp values
        health = Mathf.Clamp(health, 0, 1);
        age = Mathf.Clamp(age, 0, 1);
        food = Mathf.Clamp(food, 0, 1);

        // DYING
        if (food <= 0 || health <= 0)
        {
            die();
        }
    }

    // Initialize Walking
    void startWalking()
    {
        idle = false;
        Vector2 startPos = transform.position;
        float angle;
        do
        {
            angle = Random.Range(0, Mathf.PI * 2);
            targetPos = startPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(distRange[0], distRange[1]);
        }
        while(!(targetPos[0] > -30 && targetPos[0] < 30 && targetPos[1] > -30 && targetPos[1] < 30));
        moveVec = new Vector2(targetPos.x - startPos.x, targetPos.y - startPos.y).normalized * speed;
        runningTime = 0;
        maxRunTime = Vector2.Distance(startPos, targetPos) / speed + .5f;
        rb.velocity = moveVec;
    }

    // Going idle
    void goIdle()
    {
        idle = true;
        idleDelay = Mathf.RoundToInt(Random.Range(idleDelayRange[0], idleDelayRange[1]));
        idleTimer = 0;
        idleCounter = 0;
        rb.velocity = Vector2.zero;
    }

    // Check for collision with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        startWalking();
    }

    // Check for bullet collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            health -= collision.GetComponent<Bullet>().damage;
            if (health <= 0)
            {
                // if player killed the animal
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<Score>().score += player.GetComponent<Shooting>().killScore;
                player.GetComponent<Shooting>().ammo += player.GetComponent<Shooting>().ammoPerKill;
                GameObject scoreText = Instantiate(statText, transform.position + statTextOffset, Quaternion.identity);
                scoreText.GetComponent<FloatingStat>().stat = "+" + player.GetComponent<Shooting>().killScore + " Score";
                GameObject ammoText = Instantiate(statText, transform.position + statTextOffset + new Vector3(0, -1, 0), Quaternion.identity);
                ammoText.GetComponent<FloatingStat>().stat = "+" + player.GetComponent<Shooting>().ammoPerKill + " Ammo";
            }
            startWalking();
            GameObject effect = Instantiate(hitEffect, collision.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(effect, 2f);
        }

    }

    // death function
    private void die()
    {
        Instantiate(meatAsset, transform.position, Quaternion.identity);
        deathEffect.GetComponentInChildren<ParticleSystem>().Play();
        deathEffect.transform.parent = null;
        Destroy(deathEffect, 2f);
        Destroy(gameObject);
    }
}
