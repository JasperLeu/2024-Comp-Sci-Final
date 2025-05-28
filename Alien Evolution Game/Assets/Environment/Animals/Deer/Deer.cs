using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Deer : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D rb;
    public float speed;
    public Vector2 targetPos;
    public Vector2 distRange;
    public float arriveThresh;
    Vector2 moveVec;

    [Header("Idling")]
    public bool idle = false;
    public float idleChance;
    public Vector2 idleTimeRange;
    float idleTime;
    float idleTimer;

    [Header("Animations")]
    public Animator anim;
    public GameObject hitEffect;
    public GameObject deathEffect; 

    [Header("Stats")]
    public float food = 1;
    public float starveRate;
    public float eatThreshold = .5f;
    public float health = 1;
    void Start()
    {
        idleTimer = 0;
        startWalking();
    }
    void Update()
    {
        // MOVEMENT
        if (!idle)
        {
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

            if (Vector2.Distance(transform.position, targetPos) <= arriveThresh)
            {
                float val = Random.Range(0f, 1f);
                if (val < idleChance)
                {
                    goIdle();
                }
                else
                {
                    startWalking();
                }
            }
        }
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

        // HUNGER LOGIC
        // loose food
        food -= starveRate * Time.deltaTime;
        // eat food if on grass or near a plant

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
        rb.velocity = moveVec;
    }

    // Going idle
    void goIdle()
    {
        idle = true;
        idleTime = Random.Range(idleTimeRange[0], idleTimeRange[1]);
        idleTimer = 0;
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        startWalking();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            health -= collision.GetComponent<Bullet>().damage;
            startWalking();
            GameObject effect = Instantiate(hitEffect, collision.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(effect, 2f);
        }

    }

    // death function
    private void die()
    {
        deathEffect.GetComponentInChildren<ParticleSystem>().Play();
        deathEffect.transform.parent = null;
        Destroy(deathEffect, 2f);
        Destroy(gameObject);
    }
}
