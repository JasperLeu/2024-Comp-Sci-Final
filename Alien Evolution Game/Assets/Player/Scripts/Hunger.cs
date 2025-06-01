using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hunger : MonoBehaviour
{
    [Header("Dying")]
    public ParticleSystem deathEffect;
    public GameObject bodyObj;
    public DeathUI deathUI;
    public Score score;
    [Header("Hunger")]
    public string eatKey = "e";
    public float food;
    public int maxFood;
    public float starveRate;
    public HungerBar bar;
    public float flashSpeed;
    [Header("Food Storage")]
    public int foodStored;
    public TextMeshProUGUI foodStoredText;

    void Start()
    {
        // Initialize food and hunger
        food = maxFood;
        bar.food = food;
        bar.maxFood = maxFood;
    }

    void Update()
    {
        // Update hunger
        food -= starveRate * Time.deltaTime;

        // Update hunger ui
        food = Mathf.Clamp(food, 0, maxFood);
        bar.food = food;
        foodStoredText.text = foodStored.ToString();

        // Eating food
        if (Input.GetKeyDown(eatKey) && foodStored > 0)
        {
            food += 1;
            foodStored--;
            bar.food = food;
        }

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
            // update death screen
            deathUI.score = score.score;
            deathUI.isDead = true;
        }
    }
}
