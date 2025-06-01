using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public upgrade upgrade;
    public Shooting player;
    public void updateUI()
    {
        GetComponent<Image>().sprite = upgrade.icon;
    }
    public void applyUpgrade()
    {
        player.maxAmmo += upgrade.maxAmmoChange;
        player.damage += upgrade.damageChange;
        player.fireDelay = Mathf.Max(0.1f, player.fireDelay + upgrade.fireDelayChange);
        player.bulletSpeed += upgrade.bulletSpeedChange;
        player.ammo = player.maxAmmo;
    }
}
