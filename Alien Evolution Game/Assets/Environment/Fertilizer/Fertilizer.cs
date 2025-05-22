using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fertilizer : MonoBehaviour
{
    [Header("Effect")]
    public float radius;
    public float effect;
    [Header("Variables")]
    public float duration;
    public SpriteRenderer image;
    float timer;
    void Start()
    {
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
        if (timer > duration) { Destroy(gameObject); }

        // Apply effects
        foreach (Tree t in GetComponentInParent<GameManager>().trees)
        {
            Vector2 tPos = new Vector2(t.transform.position[0], t.transform.position[1]);
            Vector2 pos = new Vector2(transform.position[0], transform.position[1]);
            if (Vector2.Distance(tPos, pos) <= radius){
                t.health += Time.deltaTime / duration * effect;
            }
        }
    }
}
