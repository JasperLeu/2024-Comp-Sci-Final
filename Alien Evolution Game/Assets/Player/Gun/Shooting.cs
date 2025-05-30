using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [Header("Refs")]
    public GameObject bullet;
    public PlayerMovement playerMovement; // Reference to PlayerMovement
    [Header("Stats")]
    public int ammo;
    public int maxAmmo;
    public float damage;
    public float fireDelay;
    float delayTimer;
    public float bulletSpeed;

    [Header("Shoot Offsets")]
    public Vector3 upOffset;
    public Vector3 downOffset;
    public Vector3 leftOffset;
    public Vector3 rightOffset;
    Vector3 shootOffset;

    [Header("UI Stuff")]
    public TextMeshProUGUI ammoCountText;
    public Image loadingCircle;
    public Sprite[] loadingFrames;

    void Start()
    {
        ammo = maxAmmo;
        delayTimer = fireDelay;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        delayTimer += Time.deltaTime;

        // Ammo loading circle
        float index = Mathf.Clamp(delayTimer / fireDelay * loadingFrames.Length, 0, loadingFrames.Length-1);
        Sprite sprite = loadingFrames[Mathf.RoundToInt(index)];
        loadingCircle.sprite = sprite;
        // Text update
        ammoCountText.text = ammo.ToString();


        // Use animVec as the shooting direction
        Vector2 shootDir = playerMovement.animVec.normalized;
        if (shootDir == Vector2.zero)
            shootDir = Vector2.right; // fallback direction
        if (shootDir == Vector2.up)
        {
            shootOffset = upOffset;
        }
        else if (shootDir == Vector2.down)
        {
            shootOffset = downOffset;
        }
        else if (shootDir == Vector2.left)
        {
            shootOffset = leftOffset;
        }
        else if (shootDir == Vector2.right)
        {
            shootOffset = rightOffset;
        }
        shootOffset.x *= transform.localScale.x;
        shootOffset.y *= transform.localScale.y;

        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        if (delayTimer > fireDelay && Input.GetKeyDown(KeyCode.Space) && ammo > 0)
        {
            // Change values
            delayTimer = 0;
            ammo--;

            // Spawn a bullet
            GameObject b = Instantiate(bullet, transform.position + shootOffset, Quaternion.identity);
            b.transform.localEulerAngles = new Vector3(0, 0, angle);
            b.GetComponent<Bullet>().speed = bulletSpeed;
            b.GetComponent<Bullet>().damage = damage;
        }
    }
}
