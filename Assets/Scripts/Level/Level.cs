using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    UnityEvent OnActivation;
    UnityEvent OnDeactivation;
    UnityEvent OnCloseDoors;
    UnityEvent OnOpenDoors;
    List<GameObject> enemies;

    public Door doorLeft;
    public Door doorRight;
    public Door doorTop;
    public Door doorBottom;
    public CompositeCollider2D floor;
    public NavMeshSurface2d land;
    public NavMeshSurface2d flying;

    private void Awake()
    {
        OnActivation = new UnityEvent();
        OnDeactivation = new UnityEvent();
        OnCloseDoors = new UnityEvent();
        OnOpenDoors = new UnityEvent();
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy")).FindAll(g => g.transform.IsChildOf(this.transform));
    }

    private void Start()
    {
        var levelMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelMaster>();

        OnOpenDoors.AddListener(levelMaster.OnCleared);

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
    }

    public void OnEnemyDeath()
    {
        enemies.RemoveAt(0);
        if (enemies.Count <= 0)
            OpenDoors();
    }

    public void Activate()
    {
        OnActivation.Invoke();
        if (enemies.Count > 0)
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

    public void DestroyEnemies()
    {
        enemies.ForEach(Destroy);
        enemies.Clear();
    }
}
