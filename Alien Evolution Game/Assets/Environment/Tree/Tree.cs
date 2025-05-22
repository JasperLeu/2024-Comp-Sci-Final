using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class treeInfo
{
    public Sprite leaves;
    public Sprite trunk;
    public Vector2 collisionPos;
    public Vector2 collisionSize;
    public Vector3 shadowScale;
    public Vector3 shadowPos;
}


public class Tree : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer trunkSprite;
    public SpriteRenderer leavesSprite;
    public CapsuleCollider2D collision;
    public Transform shadow;
    public treeInfo[] trees;
    treeInfo t;
    [Header("Info")]
    public Gradient trunkColors;
    public Gradient leavesColors;
    public float health = 1;
    public float age = 0;

    void Start()
    {
        
    }
    void Update()
    {
        age = Mathf.Clamp(age, 0, 1);
        health = Mathf.Clamp(health, 0, 1);
        int i = Mathf.Clamp(Mathf.FloorToInt(age * trees.Length), 0, trees.Length - 1);
        t = trees[i];
        // Trunk / Leaves sprites
        trunkSprite.sprite = t.trunk;
        leavesSprite.sprite = t.leaves;
        trunkSprite.color = trunkColors.Evaluate(health);
        leavesSprite.color = leavesColors.Evaluate(health);
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
