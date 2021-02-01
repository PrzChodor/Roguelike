using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip menuMusic;
    public List<AudioClip> gameMusic;
    public AudioClip deathMusic;

    public static MusicPlayer instance;

    private AudioSource audioSource;
    private IEnumerator coroutine;

    private void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceenChanged;
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceenChanged(Scene scene, LoadSceneMode mode)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        if (scene.name == "Game")
        {
            audioSource.loop = false;
            playRandomMusic();
        }
        else if (scene.name == "Menu")
        {
            audioSource.clip = menuMusic;
            audioSource.Play();
            audioSource.loop = true;
        }
        print(scene.name);
    }

    void playRandomMusic()
    {
        audioSource.clip = gameMusic[Random.Range(0, gameMusic.Count)];
        audioSource.Play();
        coroutine = WaitForClipEnd();
        StartCoroutine(coroutine);
    }

    IEnumerator WaitForClipEnd()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        playRandomMusic();
    }
}
