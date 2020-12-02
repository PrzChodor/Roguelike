using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        var mouse = Mouse.current.position.ReadValue() - screenCenter;
        mouse.x = Mathf.Clamp(mouse.x / screenCenter.x, -1, 1);
        mouse.y = Mathf.Clamp(mouse.y / screenCenter.y, -1, 1);

        mouse *= 2;

        var target = new Vector3(player.position.x + mouse.x, player.position.y + mouse.y, -10);
        Camera.main.transform.position = target;
    }

    private void FixedUpdate()
    {
        var screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        var mouse = Mouse.current.position.ReadValue() - screenCenter;
        mouse.x = Mathf.Clamp(mouse.x / screenCenter.x, -1, 1);
        mouse.y = Mathf.Clamp(mouse.y / screenCenter.y, -1, 1);

        if (Mathf.Abs(mouse.x) < 0.2f && Mathf.Abs(mouse.y) < 0.2f)
            mouse = Vector2.zero;

        mouse *= 2;

        var target = new Vector3(player.position.x + mouse.x, player.position.y + mouse.y, -10);
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, target, ref velocity, smoothTime);
    }
}
