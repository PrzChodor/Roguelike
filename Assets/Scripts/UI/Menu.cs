using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Image blackScreen;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play()
    {
        source.Play();
        StartCoroutine(Transition());
    }

    public void Exit()
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine(OnExit());
    }

    IEnumerator Transition()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), elapsedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        SceneManager.LoadScene("Game");
        yield return null;
    }
    IEnumerator OnExit()
    {
        yield return new WaitWhile(() => source.isPlaying);
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
