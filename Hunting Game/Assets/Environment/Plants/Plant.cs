using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class plantInfo
{
    public bool animated = false;
    public Sprite[] parts;
    public bool collision = true;
    public Vector2 collisionPos;
    public Vector2 collisionSize;
    public Vector3 shadowPos;
    public Vector3 shadowScale;
}


public class Plant : MonoBehaviour
{
    [Header("References")]
    GameManager gameManager;
    public Ground ground;
    public CapsuleCollider2D collision;
    public Transform shadow;
    public SpriteRenderer[] partSprites;
    public Gradient[] partGradients;
    plantInfo t;

    [Header("Info")]
    // Sprites and their colors
    public plantInfo[] states;
    // health
    public float health = 1;
    public AnimationCurve fertilityDecayRate;
    public float fertility;
    [Header("Age")]
    public float age = 0;
    public float ageTime;
    public AnimationCurve ageDecayRate;

    [Header("Reproduction")]
    public float reproductionChance;
    public Vector2 reproductionDistRange;
    public float reproductionCooldown;
    float reproductionTimer = 0;
    public int maxReproductionCount;
    int reproductionCount = 0;
    public float minSpawnDist;
    public int maxSpaceChecks = 10;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.plants.Add(gameObject);
        ground = GameObject.FindGameObjectWithTag("Ground").GetComponent<Ground>();
    }
    void Update()
    {
        // Update Stats
        Vector2 pos = ground.getPixelPos(transform.position);
        fertility = ground.fertilityTexture.GetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)).r;
        health += fertilityDecayRate.Evaluate(fertility) * Time.deltaTime;
        health += ageDecayRate.Evaluate(age) * Time.deltaTime;
        age += Time.deltaTime / ageTime * fertility;

        // Clamp values
        age = Mathf.Clamp(age, 0, 1);
        health = Mathf.Clamp(health, 0, 1);

        // Get which state to display
        int i = Mathf.Clamp(Mathf.FloorToInt(age * states.Length), 0, states.Length - 1);
        t = states[i];

        // Update Sprite Colors
        for (int s = 0; s < t.parts.Length; s++)
        {
            if (!t.animated)
            {
                partSprites[s].sprite = t.parts[s];
            }
            partSprites[s].color = partGradients[s].Evaluate(health);
        }
        // Collision
        if (t.collision)
        {
            collision.offset = t.collisionPos;
            collision.size = t.collisionSize;
        }
        // Shadow
        shadow.localPosition = t.shadowPos;
        shadow.localScale = t.shadowScale;

        // Reproduction
        reproductionTimer += Time.deltaTime;
        bool canReproduce = reproductionCount < maxReproductionCount && reproductionTimer > reproductionCooldown && age > .5f;
        if (Random.Range(0f, 1f) < reproductionChance * Time.deltaTime * (health / 2 + .5f) && canReproduce)
        {
            reproductionTimer = 0;
            reproductionCount++;
            // Spawn a new plant within a certain distance range from the current plant
            float dir;
            float dist;
            Vector3 spawnPos;
            bool validPos = true;
            int spaceChecks = 0;
            do
            {
                validPos = true;
                spaceChecks++;
                dir = Random.Range(0f, 2 * Mathf.PI);
                dist = Random.Range(reproductionDistRange.x, reproductionDistRange.y);
                spawnPos = transform.position + new Vector3(Mathf.Cos(dir), Mathf.Sin(dir), 0) * dist;
                if (Vector2.Distance(spawnPos, Vector2.zero) < gameManager.wellRadius)
                {
                    validPos = false;
                    continue;
                }
                foreach (GameObject plant in gameManager.plants)
                {
                    if (Vector2.Distance(plant.transform.position, spawnPos) < Mathf.Min(minSpawnDist, plant.GetComponent<Plant>().minSpawnDist))
                    {
                        validPos = false;
                        break;
                    }
                    if (spawnPos.x < -30 || spawnPos.x > 30 || spawnPos.y < -30 || spawnPos.y > 30)
                    {
                        validPos = false;
                        break;
                    }
                }
            }
            while (!validPos && spaceChecks < maxSpaceChecks);
            if (spaceChecks < maxSpaceChecks)
            {
                GameObject newPlant = Instantiate(gameObject, spawnPos, Quaternion.identity);
                newPlant.GetComponent<Plant>().age = 0;
                newPlant.GetComponent<Plant>().health = 1;
                newPlant.transform.parent = transform.parent; 
            }
        }

        // death
        if (health == 0)
        {
            gameManager.plants.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
