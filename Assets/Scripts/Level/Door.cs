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
    private UIManager ui;

    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIManager>();
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

    public void Enter()
    {
        if (opened)
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelMaster>().ChangeLevel(toLevel, direction);
    }
}
