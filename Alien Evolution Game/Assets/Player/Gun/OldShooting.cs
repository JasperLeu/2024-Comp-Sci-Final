using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldShooting : MonoBehaviour
{
    [Header("Refs")]
    public GameObject bullet;
    public Camera cam;
    public Vector3 shootOffset;
    [Header("Stats")]
    public float damage;
    public float fireDelay;
    float delayTimer;
    public float bulletSpeed;

    void Start()
    {

    }
    void Update()
    {
        delayTimer += Time.deltaTime;

        // Get mouse position and calculate angle
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPoint = cam.ScreenToWorldPoint(mousePosition);
        Vector2 off = worldPoint - transform.position + shootOffset;
        float angle = Mathf.Atan2(off.y, off.x) * Mathf.Rad2Deg;

        // Shoot shoot shoot
        if (delayTimer > fireDelay && Input.GetMouseButtonDown(0))
        {
            GameObject b = Instantiate(bullet, transform.position + shootOffset, Quaternion.identity);
            b.transform.localEulerAngles = new Vector3(0, 0, angle);
            b.GetComponent<Bullet>().speed = bulletSpeed;
            b.GetComponent<Bullet>().damage = damage;
            delayTimer = 0;
        }
    }
}
