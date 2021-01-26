using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    public SpriteRenderer background;
    public CanvasGroup deathScreen;
    public Transform player;
    public float smoothTime = 0.3f;
    public bool playerDead;
    private bool falling;
    private Vector3 velocity = Vector3.zero;
    private float lerp = 0;
    private float duration = 2;
    private Light2D light2D;
    private bool zoomed;

    private void Update()
    {
        if (!playerDead)
            UpdateCamera();
        else if (!zoomed)
            ZoomOnPlayer();
    }

    private void UpdateCamera()
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

    private void ZoomOnPlayer()
    {
        var target = new Vector3(player.position.x, player.position.y + 0.2f, -10);
        if (light2D == null)
        {
            light2D = player.GetComponentInChildren<Light2D>();
            light2D.shadowIntensity = 0;
            player.GetComponent<SpriteRenderer>().color = Color.white;
            player.GetComponent<SortingGroup>().sortingOrder = 11;
            falling = player.GetComponent<PlayerController>().falling;
        }

        lerp += Time.deltaTime / duration;
        Camera.main.GetComponent<PixelPerfectCamera>().assetsPPU = (int)Mathf.Lerp(32, 72, lerp);
        deathScreen.alpha = Mathf.Lerp(0, 1, 2 * (lerp - 0.5f));

        if (!falling)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target, lerp);
            light2D.pointLightOuterRadius = Mathf.Lerp(8, 2, lerp);
            background.color = Color.Lerp(new Color(0.17f, 0.17f, 0.17f, 0), new Color(0.17f, 0.17f, 0.17f, 1), lerp);
        }
        else
        {
            Camera.main.transform.position = target;
            light2D.pointLightInnerRadius = 0;
            light2D.pointLightOuterRadius = 2;
            background.color = new Color(0.17f, 0.17f, 0.17f, 1);
        }

        if (lerp > 1.0f)
            zoomed = true;
    }
}
