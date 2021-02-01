using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Image blackScreen;
    private AudioSource source;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public AudioMixer mixer;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(TransitionIn());
        LoadSettings();
    }

    public void Play()
    {
        source.Play();
        StartCoroutine(TransitionOut());
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

    IEnumerator TransitionOut()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), elapsedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 1);

        SceneManager.LoadScene("Game");
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

    public void LoadSettings()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        GetComponent<PlayerInput>().actions.LoadBindingOverridesFromJson(rebinds);

        if (PlayerPrefs.HasKey("width") && PlayerPrefs.HasKey("fullScreenMode"))
            Screen.SetResolution(PlayerPrefs.GetInt("width"), PlayerPrefs.GetInt("height"), (FullScreenMode)PlayerPrefs.GetInt("fullScreenMode"), PlayerPrefs.GetInt("refreshRate"));
        else if (PlayerPrefs.HasKey("width"))
            Screen.SetResolution(PlayerPrefs.GetInt("width"), PlayerPrefs.GetInt("height"), Screen.fullScreenMode, PlayerPrefs.GetInt("refreshRate"));
        else if (PlayerPrefs.HasKey("fullScreenMode"))
            Screen.fullScreenMode = (FullScreenMode)PlayerPrefs.GetInt("fullScreenMode");


        if (PlayerPrefs.HasKey("vSyncCount"))
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("vSyncCount");
        else
            QualitySettings.vSyncCount = 0;


        if (PlayerPrefs.HasKey("MasterVolume"))
            mixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
        else
            mixer.SetFloat("MasterVolume", Mathf.Log10(0.5f) * 20);

        if (PlayerPrefs.HasKey("MusicVolume"))
            mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        else
            mixer.SetFloat("MusicVolume", Mathf.Log10(0.5f) * 20);

        if (PlayerPrefs.HasKey("SFXVolume"))
            mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        else
            mixer.SetFloat("SFXVolume", Mathf.Log10(0.5f) * 20);
    }
}
