using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public List<GameObject> mapCells;
    public CanvasGroup map;
    private Dictionary<int, int> idPairs;
    private RectTransform currentLevel;

    private void Start()
    {
        idPairs = new Dictionary<int, int>();
        idPairs.Add(0, 0);
        currentLevel = map.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        HideMap();
    }

    public void SwitchMap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShowMap();
        }
        else if (context.canceled)
        {
            HideMap();
        }
    }

    public void HideMap()
    {
        var rectTransform = map.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(1300, 700);
    }

    public void ShowMap()
    {
        var rectTransform = map.GetComponent<RectTransform>();
        rectTransform.offsetMin = Vector2.zero;
    }

    public void AddLevel(int id, string type, int direction)
    {
        if (idPairs.ContainsKey(id))
        {
            currentLevel = map.transform.GetChild(0).GetChild(idPairs[id]).GetComponent<RectTransform>();
            AdjustToCurrent();
        }
        else
        {
            idPairs.Add(id, map.transform.GetChild(0).childCount);
            var newLevel = Instantiate(Resources.Load($"Map/{type}") as GameObject, map.transform.GetChild(0));

            switch (direction)
            {
                case 0:
                    newLevel.GetComponent<RectTransform>().anchoredPosition = currentLevel.anchoredPosition + new Vector2(0, 70);
                    break;
                case 1:
                    newLevel.GetComponent<RectTransform>().anchoredPosition = currentLevel.anchoredPosition + new Vector2(70, 0);
                    break;
                case 2:
                    newLevel.GetComponent<RectTransform>().anchoredPosition = currentLevel.anchoredPosition - new Vector2(70, 0);
                    break;
                case 3:
                    newLevel.GetComponent<RectTransform>().anchoredPosition = currentLevel.anchoredPosition - new Vector2(0, 70);
                    break;
                default:
                    break;
            }

            currentLevel = map.transform.GetChild(0).GetChild(idPairs[id]).GetComponent<RectTransform>();
            AdjustToCurrent();
        }
    }

    private void AdjustToCurrent()
    {
        map.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = -currentLevel.anchoredPosition;
    }
}
