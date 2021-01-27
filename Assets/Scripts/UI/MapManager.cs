using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public List<GameObject> mapCells;
    public CanvasGroup map;
    public GameObject emptyLayer;
    private Dictionary<int, (int id, int layer)> LayerAndID;
    private RectTransform currentLevel;
    private int currentLayer = 1;

    private void Start()
    {
        LayerAndID = new Dictionary<int, (int, int)>();
        LayerAndID.Add(0, (0, currentLayer));
        currentLevel = map.transform.GetChild(currentLayer).GetChild(0).GetComponent<RectTransform>();
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
        if (LayerAndID.ContainsKey(id))
        {
            currentLevel = map.transform.GetChild(LayerAndID[id].layer).GetChild(LayerAndID[id].id).GetComponent<RectTransform>();
            currentLayer = LayerAndID[id].layer;
            AdjustToCurrent();
            ShowLayer();
        }
        else
        {
            FindGoodLayer(direction);
            LayerAndID.Add(id, (map.transform.GetChild(currentLayer).childCount, currentLayer));
            var newLevel = Instantiate(Resources.Load($"Map/{type}") as GameObject, map.transform.GetChild(currentLayer));

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

            currentLevel = map.transform.GetChild(LayerAndID[id].layer).GetChild(LayerAndID[id].id).GetComponent<RectTransform>();
            AdjustToCurrent();
            ShowLayer();
        }
    }

    private void AdjustToCurrent()
    {
        for (int i = 1; i < map.transform.childCount; i++)
        {
            map.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = -currentLevel.anchoredPosition;
        }
    }

    private void FindGoodLayer(int direction)
    {
        Vector2 newPosition = Vector2.zero;

        switch (direction)
        {
            case 0:
                newPosition = currentLevel.anchoredPosition + new Vector2(0, 70);
                break;
            case 1:
                newPosition = currentLevel.anchoredPosition + new Vector2(70, 0);
                break;
            case 2:
                newPosition = currentLevel.anchoredPosition - new Vector2(70, 0);
                break;
            case 3:
                newPosition = currentLevel.anchoredPosition - new Vector2(0, 70);
                break;
            default:
                break;
        }

        while (true)
        {
            var samePosition = 0;

            foreach (Transform child in map.transform.GetChild(currentLayer))
            {
                if (child.GetComponent<RectTransform>().anchoredPosition == newPosition)
                {
                    samePosition++;
                }
            }

            if (samePosition == 0)
                break;
            else
            {
                if (map.transform.childCount - 1 == currentLayer)
                    Instantiate(emptyLayer, map.transform);
                currentLayer++;
            }
        }
    }

    private void ShowLayer()
    {
        for (int i = 1; i < map.transform.childCount; i++)
        {
            map.transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0;
        }

        map.transform.GetChild(currentLayer).GetComponent<CanvasGroup>().alpha = 0.7f;
    }
}
