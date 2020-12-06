using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI text;

    private int frames = 0;

    private void Start()
    {
        StartCoroutine(OnEverySecond());
    }

    void Update()
    {
        frames++;
    }

    IEnumerator OnEverySecond()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1);
            text.text = $"{frames}fps";
            frames = 0;
        }
    }
}
