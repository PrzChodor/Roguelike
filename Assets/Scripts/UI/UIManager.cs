﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ammoCount;
    [Space]
    public GameObject dashBase;
    public GameObject dashFull;
    public GameObject dashBarBase;
    public GameObject dashBarFull;
    [Space]
    public GameObject heartBase;
    public GameObject heartFull;
    public GameObject heartHalf;
    public GameObject healthBarBase;
    public GameObject healthBarFull;

    private int lastMaxHealth;
    private int lastMaxDashes;

    public void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoCount.text = $"{currentAmmo}/{maxAmmo}";
    }

    public void UpdateDashUI(int maxDashes, int currentDashes, float DashCooldown, float DashCooldownBase)
    {
        if (maxDashes != lastMaxDashes)
        {
            foreach (Transform child in dashBarBase.transform)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < maxDashes; i++)
            {
                GameObject.Instantiate(dashBase, dashBarBase.transform);
            }
            lastMaxDashes = maxDashes;
        }

        foreach (Transform child in dashBarFull.transform)
            GameObject.Destroy(child.gameObject);

        for (int i = 0; i < currentDashes; i++)
        {
            GameObject.Instantiate(dashFull, dashBarFull.transform);
        }

        GameObject.Instantiate(dashFull, dashBarFull.transform);

        var lastCharge = dashBarFull.transform.GetChild(dashBarFull.transform.childCount - 1).GetComponent<Image>();
        lastCharge.fillAmount = DashCooldown / DashCooldownBase;
    }

    public void UpdateHealthUI(int maxHealth, int health)
    {
        if (maxHealth != lastMaxHealth)
        {
            foreach (Transform child in healthBarBase.transform)
                GameObject.Destroy(child.gameObject);

            for (int i = 0; i < maxHealth / 2; i++)
            {
                GameObject.Instantiate(heartBase, healthBarBase.transform);
            }
            lastMaxHealth = maxHealth;
        }

        foreach (Transform child in healthBarFull.transform)
            GameObject.Destroy(child.gameObject);

        for (int i = 0; i < health / 2; i++)
        {
            GameObject.Instantiate(heartFull, healthBarFull.transform);
        }

        if (health % 2 == 1)
            GameObject.Instantiate(heartHalf, healthBarFull.transform);
    }
}