using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    [Header("Dying")]
    public bool isDead;
    public ParticleSystem deathEffect;
    public GameObject bodyObj;
    [Header("Hunger")]
    public int food;
    public int maxFood;
    public float starveDelay;
    float starveTimer;
    public HungerBar bar;
    public float flashSpeed;
    void Start()
    {
        // Initialize food and hunger
        food = Mathf.RoundToInt(maxFood / 2);
        bar.maxHunger = maxFood;
        bar.food = food;
        bar.initializeBar();
    }

    void Update()
    {
        if (!isDead)
        {
            // Update hunger
            starveTimer += Time.deltaTime;
            if (starveTimer > starveDelay)
            {
                starveTimer = 0;
                food -= 1;
            }

            // Update hunger bar
            food = Mathf.Clamp(food, 0, maxFood);
            bar.food = food;
            if (starveTimer > starveDelay / 2)
            {
                bar.flashingSpeed = (starveTimer+.5f) / starveDelay * flashSpeed;
            }
            else
            {
                bar.flashingSpeed = 0;
            }

            // DYING
            if (food == 0)
            {
                isDead = true;
                bodyObj.SetActive(false);
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                GetComponent<Collider2D>().enabled = false;
                deathEffect.Play();
            }
        }
    }
}
