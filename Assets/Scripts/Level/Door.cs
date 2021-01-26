using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    private Animator animator;
    private bool opened = true;
    public InputAction interact;
    public string direction;
    public int toLevel;
    private UIManager ui;

    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIManager>();
        animator = GetComponent<Animator>();
        interact.performed += _ => Enter();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && opened && !collision.isTrigger && !collision.GetComponent<PlayerController>().falling)
            ui.ShowInteraction();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && opened && !collision.isTrigger)
            ui.HideInteraction();
    }

    public void Enter()
    {
        if (opened)
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelMaster>().ChangeLevel(toLevel, direction);
    }
}
