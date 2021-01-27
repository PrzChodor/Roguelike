using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelEvent : UnityEvent<Level> { }

public class LevelMaster : MonoBehaviour
{
    public LevelEvent OnLevelChange;
    public Level currentLevel;
    public PlayerController player;
    public Image blackScreen;
    public AudioSource doorSound;
    private int currentLevelID;
    private List<Room> rooms;

    private void Awake()
    {
        OnLevelChange = new LevelEvent();
        OnLevelChange.AddListener(player.LevelChange);
        OnLevelChange.Invoke(currentLevel);
    }

    private void Start()
    {
        currentLevel.Activate();
        rooms = MapGenerator.GenerateMap();
    }

    public void DeactivateCurrent()
    {
        currentLevel.Deactivate();
    }

    public void ChangeLevel(int level, string direction)
    {
        StartCoroutine(Transition(level, direction));
    }

    Level LoadLevel(int id)
    {
        if (currentLevel.ItemsOnExit() == 0)
            rooms[currentLevelID].ItemsCollected = true;

        Destroy(currentLevel.gameObject);
        currentLevelID = id;
        var room = rooms[id];
        var levelGO = Instantiate(Resources.Load($"Levels/{room.Code}") as GameObject);
        var level = levelGO.GetComponent<Level>();
        if (room.Bottom != -1)
            level.doorBottom.toLevel = room.Bottom;
        if (room.Top != -1)
            level.doorTop.toLevel = room.Top;
        if (room.Left != -1)
            level.doorLeft.toLevel = room.Left;
        if (room.Right != -1)
            level.doorRight.toLevel = room.Right;
        if (room.Cleared)
            level.DestroyEnemies();
        if (room.ItemsCollected)
            level.DestroyItems();
        return level;
    }

    public void OnCleared()
    {
        rooms[currentLevelID].Cleared = true;
    }

    IEnumerator Transition(int level, string direction)
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.dashActive = false;
        Time.timeScale = 0;

        float duration = 0.3f;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 1);

        currentLevel = LoadLevel(level);
        var newPosition = new Vector3();
        switch (direction)
        {
            case "Door B":
                newPosition = currentLevel.doorBottom.transform.GetChild(0).position;
                GetComponent<MapManager>().AddLevel(level, rooms[level].Code.Substring(0, rooms[level].Code.IndexOf(" ")), 0);
                break;
            case "Door L":
                newPosition = currentLevel.doorLeft.transform.GetChild(0).position;
                GetComponent<MapManager>().AddLevel(level, rooms[level].Code.Substring(0, rooms[level].Code.IndexOf(" ")), 1);
                break;
            case "Door R":
                newPosition = currentLevel.doorRight.transform.GetChild(0).position;
                GetComponent<MapManager>().AddLevel(level, rooms[level].Code.Substring(0, rooms[level].Code.IndexOf(" ")), 2);
                break;
            case "Door T":
                newPosition = currentLevel.doorTop.transform.GetChild(0).position;
                GetComponent<MapManager>().AddLevel(level, rooms[level].Code.Substring(0, rooms[level].Code.IndexOf(" ")), 3);
                break;
            default:
                break;
        }

        player.firstFrame = true;
        player.transform.position = newPosition;
        Camera.main.transform.position = newPosition;
        OnLevelChange.Invoke(currentLevel);

        Time.timeScale = 1;
        yield return new WaitForSeconds(0.05f);
        currentLevel.land.BuildNavMesh();
        currentLevel.flying.BuildNavMesh();
        Time.timeScale = 0;

        elapsedTime = 0.0f;

        while (elapsedTime < duration * 2)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        blackScreen.color = new Color(0, 0, 0, 0);

        Time.timeScale = 1;
        currentLevel.Activate();
        yield return null;
    }
}
