using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Deer : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D rb;
    public float speed;
    public float dir;
    public Vector2 distRange;
    Vector2 moveVec;
    float targetDist;
    Vector2 startPos;

    [Header("Idling")]
    public bool idle = false;
    public float idleChance;
    public Vector2 idleTimeRange;
    float idleTime;
    float idleTimer;

    [Header("Animations")]
    public Animator anim;
    [Header("Stats")]
    public float food = 1;
    public float starveRate;
    public float eatThreshold = .5f;
    void Start()
    {
        idleTimer = 0;
        startWalking();
    }
    void Update()
    {
        // MOVEMENT
        if (!idle)
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            if (Vector2.Distance(pos, startPos) >= targetDist)
            {
                float val = Random.Range(0f, 1f);
                if (val < idleChance)
                {
                    goIdle();
                }
                else
                {
                    startWalking();
                }
            }
        }
        else
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > idleTime)
            {
                startWalking();
            }
        }

        // HUNGER LOGIC
        // loose food
        food -= starveRate * Time.deltaTime;
        // eat food if on grass or near a plant

        // DYING
        if (food == 0)
        {
            Destroy(gameObject);
        }
    }

    // Initialize Walking
    void startWalking()
    {
        idle = false;
        targetDist = Random.Range(distRange[0], distRange[1]);
        dir = Random.Range(0, 2 * Mathf.PI);
        moveVec = new Vector2(Mathf.Cos(dir) * speed, Mathf.Sin(dir) * speed);
        startPos = new Vector2(transform.position.x, transform.position.y);
        rb.velocity = moveVec;
    }

    // Go idle
    void goIdle()
    {
        idle = true;
        idleTime = Random.Range(idleTimeRange[0], idleTimeRange[1]);
        idleTimer = 0;
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        startWalking();
    }
}
