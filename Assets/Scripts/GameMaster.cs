using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public Image cursor;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        cursor.transform.position = Mouse.current.position.ReadValue();
    }
}
