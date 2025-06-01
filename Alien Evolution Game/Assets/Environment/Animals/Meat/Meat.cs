using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour
{
    public float lifeTime;
    float lifeTimer;
    public float foodValue;
    public SpriteRenderer meatSprite;
    public Gradient lifeGradient;
    void Start()
    {
        
    }
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }
        meatSprite.color = lifeGradient.Evaluate(lifeTimer / lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Hunger>().foodStored++;
            Destroy(gameObject);
        }
    }
}
