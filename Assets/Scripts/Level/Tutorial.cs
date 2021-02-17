using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] tutorials;
    [Space]
    public Image moveUp;
    public Image moveDown;
    public Image moveLeft;
    public Image moveRight;
    public Image reload;
    public Image dash;
    public Image interact;
    public Image showMap;
    [Space]
    public InputActionReference moveAction;
    public InputActionReference reloadAction;
    public InputActionReference dashAction;
    public InputActionReference interactAction;
    public InputActionReference showMapAction;
    public TutorialDoor door;

    private void Start()
    {
        moveUp.sprite = Resources.Load<Sprite>($"KeyIcons/{moveAction.action.GetBindingDisplayString(2).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}"); ;
        moveDown.sprite = Resources.Load<Sprite>($"KeyIcons/{moveAction.action.GetBindingDisplayString(3).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}");
        moveLeft.sprite = Resources.Load<Sprite>($"KeyIcons/{moveAction.action.GetBindingDisplayString(4).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}");
        moveRight.sprite = Resources.Load<Sprite>($"KeyIcons/{moveAction.action.GetBindingDisplayString(5).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}");
        reload.sprite = Resources.Load<Sprite>($"KeyIcons/{reloadAction.action.GetBindingDisplayString(0).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}");
        dash.sprite = Resources.Load<Sprite>($"KeyIcons/{dashAction.action.GetBindingDisplayString(0).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}");
        interact.sprite = Resources.Load<Sprite>($"KeyIcons/{interactAction.action.GetBindingDisplayString(0).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}");
        showMap.sprite = Resources.Load<Sprite>($"KeyIcons/{showMapAction.action.GetBindingDisplayString(0).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}");
    }

    public void Moved()
    {
        if (tutorials[0].activeSelf)
        {
            tutorials[0].SetActive(false);
            tutorials[1].SetActive(true);
        }
    }
    public void Shot()
    {
        if (tutorials[1].activeSelf)
        {
            tutorials[1].SetActive(false);
            tutorials[2].SetActive(true);
        }
    }
    public void Reloaded()
    {
        if (tutorials[2].activeSelf)
        {
            tutorials[2].SetActive(false);
            tutorials[3].SetActive(true);
        }
    }
    public void Dashed()
    {
        if (tutorials[3].activeSelf)
        {
            tutorials[3].SetActive(false);
            tutorials[4].SetActive(true);
        }
    }
    public void ShowedMap()
    {
        if (tutorials[4].activeSelf)
        {
            tutorials[4].SetActive(false);
            tutorials[5].SetActive(true);
            door.Open();
        }
    }
}
