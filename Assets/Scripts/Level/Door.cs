using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    private Animator animator;
    public bool opened = true;
    public string direction;
    public int toLevel;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
        opened = true;
        animator.SetTrigger("Open");
    }

    public void Close()
    {
        opened = false;
        animator.SetTrigger("Close");
    }

    public virtual void Enter()
    {
        if (opened)
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelMaster>().ChangeLevel(toLevel, direction);
    }
}
