using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{

    public GameObject heartBase;
    public GameObject heartFull;
    public GameObject heartHalf;
    public GameObject healthBarBase;
    public GameObject healthBarFull;

    private int lastMaxHealth;

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

        if(health % 2 == 1)
            GameObject.Instantiate(heartHalf, healthBarFull.transform);
    }
}
