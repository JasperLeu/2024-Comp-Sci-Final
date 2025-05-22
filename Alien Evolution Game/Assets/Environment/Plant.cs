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
    public Vector3 shadowScale;
    public Vector3 shadowPos;
}


public class Plant : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer[] partSprites;
    public Gradient[] partGradients;
    public CapsuleCollider2D collision;
    public Transform shadow;
    public plantInfo[] states;
    plantInfo t;
    [Header("Info")]
    public float health = 1;
    public float age = 0;

    void Start()
    {
        
    }
    void Update()
    {
        age = Mathf.Clamp(age, 0, 1);
        health = Mathf.Clamp(health, 0, 1);
        int i = Mathf.Clamp(Mathf.FloorToInt(age * states.Length), 0, states.Length - 1);
        t = states[i];
        // Trunk / Leaves sprites
        for(int s = 0; s < t.parts.Length; s++)
        {
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
            GetComponentInParent<GameManager>().trees.Remove(this);
            Destroy(gameObject);
        }
    }
}
