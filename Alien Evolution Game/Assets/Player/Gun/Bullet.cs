using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float maxDist = 100;
    public float damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dir = transform.localEulerAngles.z * Mathf.Deg2Rad;
        Vector3 off = new Vector3(Mathf.Cos(dir), Mathf.Sin(dir), 0) * speed;
        transform.position += off;
        if (Vector2.Distance(Vector2.zero, transform.position) > maxDist)
        {
            Destroy(gameObject);
        }
    }
    
}
