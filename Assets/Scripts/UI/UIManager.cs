using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject hud;
    public GameObject deathScreen;
    public GameObject pauseScreen;
    public GameObject crosshair;
    public Slider bossHP;
    [Space]
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
    [Space]
    public CanvasGroup interactButton;
    public InputActionReference interactAction;


    private GameObject player;
    private int lastMaxHealth;
    private int lastMaxDashes;

    private void Awake()
    {
        deathScreen.SetActive(false);
        pauseScreen.SetActive(false);
        HideInteraction();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        interactButton.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, 0.7f));
    }

    public void ShowDeathScreen()
    {
        hud.SetActive(false);
        crosshair.SetActive(false);
        deathScreen.SetActive(true);
        GetComponent<CameraController>().playerDead = true;
        Cursor.visible = true;
    }

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

    public void ShowInteraction()
    {
        var path = $"KeyIcons/{interactAction.action.GetBindingDisplayString(0).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}";
        interactButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        interactButton.alpha = 1;
    }

    public void HideInteraction()
    {
        interactButton.alpha = 0;
    }

    public void ShowPauseMenu()
    {
        pauseScreen.SetActive(true);
        crosshair.SetActive(false);
        Cursor.visible = true;
    }

    public void HidePauseMenu()
    {
        pauseScreen.SetActive(false);
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    public void ShowBossHP()
    {
        bossHP.gameObject.SetActive(true);
    }

    public void UpdateBossHP(float value)
    {
        bossHP.value = value;
    }
}
