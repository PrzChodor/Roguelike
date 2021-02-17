using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDoor : Door
{
    private void Start()
    {
        Close();
    }

    public override void Enter()
    {
        if (opened)
            SceneManager.LoadScene("Menu");
    }
}
