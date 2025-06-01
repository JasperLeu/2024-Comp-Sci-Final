using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthEffect : MonoBehaviour
{
    public bool live = false;
    public int offset = 30;
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 30 + transform.position.y);
    }
    void Update()
    {
        if (live)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 30 + transform.position.y);
        }
    }
}
