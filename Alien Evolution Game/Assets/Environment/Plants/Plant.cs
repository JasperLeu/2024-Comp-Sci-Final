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
    public Vector2 healthRates;
    public Vector2 agedHealthRates;
    // age
    public float age = 0;
    public float ageTime;

    void Start()
    {
        ground = GameObject.FindGameObjectWithTag("Ground").GetComponent<Ground>();
    }
    void Update()
    {
        // Update Stats
        int xPos = Mathf.FloorToInt(transform.position.x) + ground.width / 2;
        int yPos = Mathf.FloorToInt(transform.position.y) + ground.height / 2;
        float fertility = ground.fertilityTexture.GetPixel(xPos, yPos).r;
        float decay = Mathf.Lerp(agedHealthRates[0], agedHealthRates[1], fertility);
        float agedDecay = Mathf.Lerp(healthRates[0], healthRates[1], fertility);
        health += Mathf.Lerp(decay, agedDecay, age) * Time.deltaTime;
        age += Time.deltaTime / ageTime * fertility;

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
