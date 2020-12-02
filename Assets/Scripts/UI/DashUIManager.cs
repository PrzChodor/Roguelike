using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUIManager : MonoBehaviour
{
    public GameObject dashBase;
    public GameObject dashFull;
    public GameObject dashBarBase;
    public GameObject dashBarFull;

    private int lastMaxDashes;

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
}
