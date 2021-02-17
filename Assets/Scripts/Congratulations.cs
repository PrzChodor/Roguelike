using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Congratulations : MonoBehaviour
{
    public Image blackScreen;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Cursor.visible = true;
        StartCoroutine(TransitionIn());
    }

    public void Menu()
    {
        source.Play();
        StartCoroutine(TransitionOut("Menu"));
    }

    IEnumerator TransitionOut(string scene)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), elapsedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 1);

        SceneManager.LoadScene(scene);
        yield return null;
    }

    IEnumerator TransitionIn()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), elapsedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 0);

        yield return null;
    }
}
