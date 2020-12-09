using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    UnityEvent OnActivation;
    UnityEvent OnDeactivation;
    UnityEvent OnCloseDoors;
    UnityEvent OnOpenDoors;
    int enemyCount;

    public Door doorLeft;
    public Door doorRight;
    public Door doorTop;
    public Door doorBottom;
    public CompositeCollider2D floor;

    private void Awake()
    {
        OnActivation = new UnityEvent();
        OnDeactivation = new UnityEvent();
        OnCloseDoors = new UnityEvent();
        OnOpenDoors = new UnityEvent();
    }

    private void Start()
    {
        var enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy")).FindAll(g => g.transform.IsChildOf(this.transform));
        enemyCount = enemies.Count;

        if (doorLeft != null)
        {
            OnCloseDoors.AddListener(doorLeft.Close);
            OnOpenDoors.AddListener(doorLeft.Open);
        }
        if (doorRight != null)
        {
            OnCloseDoors.AddListener(doorRight.Close);
            OnOpenDoors.AddListener(doorRight.Open);
        }
        if (doorTop != null)
        {
            OnCloseDoors.AddListener(doorTop.Close);
            OnOpenDoors.AddListener(doorTop.Open);
        }
        if (doorBottom != null)
        {
            OnCloseDoors.AddListener(doorBottom.Close);
            OnOpenDoors.AddListener(doorBottom.Open);
        }

        foreach (var item in enemies)
        {
            var enemy = item.GetComponent<Enemy>();
            OnActivation.AddListener(enemy.Activate);
            OnDeactivation.AddListener(enemy.Deactivate);
        }

        Activate();
    }

    public void OnEnemyDeath()
    {
        enemyCount--;
        if (enemyCount <= 0)
            OpenDoors();
    }

    public void Activate()
    {
        OnActivation.Invoke();
        if (enemyCount > 0)
            CloseDoors();
    }

    public void Deactivate()
    {
        OnDeactivation.Invoke();
    }

    private void OpenDoors()
    {
        OnOpenDoors.Invoke();
    }

    private void CloseDoors()
    {
        OnCloseDoors.Invoke();
    }
}
