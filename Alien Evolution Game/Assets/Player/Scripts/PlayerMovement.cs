using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public Animator anim;
    bool idle = true;
    Vector2 moveInput;
    Vector2 animVec;

    void Start()
    {
        animVec = new Vector2(0, -.5f);
    }
    void Update()
    {
        // Get Inputs
        bool w = Input.GetKey(KeyCode.W);
        bool a = Input.GetKey(KeyCode.A);
        bool s = Input.GetKey(KeyCode.S);
        bool d = Input.GetKey(KeyCode.D);
        moveInput = Vector2.zero;
        if (w)
        {
            moveInput[1]++;
        }
        if (s)
        {
            moveInput[1]--;
        }
        if (a)
        {
            moveInput[0]--;
        }
        if (d)
        {
            moveInput[0]++;
        }

        // Animations
        if (Input.GetKeyDown("w") && !s || moveInput == new Vector2(0, 1))
        {
            animVec = new Vector2(0, 1);
        }
        else if (Input.GetKeyDown("s") && !w || moveInput == new Vector2(0, -1))
        {
            animVec = new Vector2(0, -1);
        }
        else if (Input.GetKeyDown("a") && !d || moveInput == new Vector2(-1, 0))
        {
            animVec = new Vector2(-1 , 0);
        }
        else if (Input.GetKeyDown("d") && !a || moveInput == new Vector2(1, 0))
        {
            animVec = new Vector2(1, 0);
        }
        else if (moveInput == Vector2.zero)
        {
            animVec = animVec.normalized * .5f;
        }
        anim.SetFloat("x", animVec[0]);
        anim.SetFloat("y", animVec[1]);
    }

    void FixedUpdate()
    {
        // Apply Speed
        rb.velocity = speed * moveInput.normalized;
    }
}
