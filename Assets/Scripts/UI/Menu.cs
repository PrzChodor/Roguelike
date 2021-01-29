using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Image blackScreen;
    private AudioSource source;
    public GameObject mainMenu;
    public GameObject optionsMenu;

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
        source.Play();
        StartCoroutine(OnExit());
    }

    public void Options()
    {
        source.Play();
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void MainMenu()
    {
        source.Play();
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ShowTab(int tab)
    {
        source.Play();
        for (int i = 0; i < 3; i++)
        {
            optionsMenu.transform.GetChild(i).gameObject.SetActive(false);
            optionsMenu.transform.GetChild(3).GetChild(i).GetComponent<Button>().interactable = true;
        }
        optionsMenu.transform.GetChild(tab).gameObject.SetActive(true);
        optionsMenu.transform.GetChild(3).GetChild(tab).GetComponent<Button>().interactable = false;
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
