using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEvent : UnityEvent<Level> { }

public class LevelMaster : MonoBehaviour
{
    public LevelEvent OnLevelChange;
    public Level currentLevel;
    public PlayerController player;

    private void Awake()
    {
        OnLevelChange = new LevelEvent();
        OnLevelChange.AddListener(player.LevelChange);
        OnLevelChange.Invoke(currentLevel);
    }

    public void DeactivateCurrent()
    {
        currentLevel.Deactivate();
    }
}
