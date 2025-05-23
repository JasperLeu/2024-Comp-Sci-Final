using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fertilizer : MonoBehaviour
{
    [Header("Effect")]
    public float radius;
    public float effect;
    [Header("Variables")]
    public GameManager gameManager;
    public float duration;
    public SpriteRenderer image;
    float timer;
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        gameManager.soil.Add(this);
        transform.localScale = Vector3.one * radius;
        transform.position = new Vector3(transform.position.x, transform.position.y, 99);
    }
    void Update()
    {
        // Update stuff
        timer += Time.deltaTime;
        // update opacity
        Color col = image.color;
        col.a = 1 - timer / duration;
        image.color = col;
        // die
        if (timer > duration) 
        {
            gameManager.soil.Remove(this);
            Destroy(gameObject); 
        }
    }
}
