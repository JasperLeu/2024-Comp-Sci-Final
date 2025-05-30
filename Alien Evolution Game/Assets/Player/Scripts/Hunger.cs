using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    [Header("Dying")]
    public ParticleSystem deathEffect;
    public GameObject bodyObj;
    [Header("Hunger")]
    public float food;
    public float maxFood;
    public float starveRate;
    public HungerBar bar;
    public float flashSpeed;
    void Start()
    {
        // Initialize food and hunger
        food = maxFood/2;
        bar.food = food;
        bar.maxFood = maxFood;
    }

    void Update()
    {
        // Update hunger
        food -= starveRate * Time.deltaTime;

        // Update hunger bar
        food = Mathf.Clamp(food, 0, maxFood);
        bar.food = food;

        // DYING
        if (food == 0)
        {
            bodyObj.SetActive(false);
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Collider2D>().enabled = false;
            deathEffect.Play();
            // Detach death effect and camera from player
            deathEffect.transform.parent = null;
            GetComponentInChildren<Camera>().transform.parent = null;
            gameObject.SetActive(false);
        }
    }
}
