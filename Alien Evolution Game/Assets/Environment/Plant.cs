using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class plantInfo
{
    public Sprite[] parts;
    public Vector2 collisionPos;
    public Vector2 collisionSize;
    public Vector3 shadowPos;
    public Vector3 shadowScale;
}


public class Plant : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager;
    // Sprites and their colors
    public plantInfo[] states;
    public SpriteRenderer[] partSprites;
    public Gradient[] partGradients;

    public CapsuleCollider2D collision;
    public Transform shadow;
    plantInfo t;

    [Header("Info")]
    // health
    public float health = 1;
    public float healthTime;
    public float deathTime;
    // age
    public float age = 0;
    public float ageTime;

    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
    }
    void Update()
    {
        // Update Stats
        bool inSoil = false;
        foreach (Fertilizer f in gameManager.soil)
        {
            if (Vector2.Distance(f.transform.position, transform.position) <= f.radius)
            {
                inSoil = true;
            }
        }
        if (inSoil)
        {
            if (age == 1)
            {
                health -= Time.deltaTime / deathTime;
            }
            else
            {
                health -= Time.deltaTime / healthTime;
            }
            health += Time.deltaTime / gameManager.healTime;
            age += Time.deltaTime / ageTime;
        }
        else
        {
            health -= Time.deltaTime / deathTime;
        }

        // Clamp values
        age = Mathf.Clamp(age, 0, 1);
        health = Mathf.Clamp(health, 0, 1);

        // Get which state to display
        int i = Mathf.Clamp(Mathf.FloorToInt(age * states.Length), 0, states.Length - 1);
        t = states[i];
        
        // Update Sprite Colors
        for(int s = 0; s < t.parts.Length; s++)
        {
            partSprites[s].sprite = t.parts[s];
            partSprites[s].color = partGradients[s].Evaluate(health);
        }
        // Collision
        collision.offset = t.collisionPos;
        collision.size = t.collisionSize;
        // Shadow
        shadow.localPosition = t.shadowPos;
        shadow.localScale = t.shadowScale;

        // death
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}
