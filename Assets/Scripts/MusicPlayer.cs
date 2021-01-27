using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    Object[] music;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
        music = Resources.LoadAll("Music", typeof(AudioClip));
        audioSource.clip = music[0] as AudioClip;
    }

    void Start()
    {
        playRandomMusic();
    }

    void playRandomMusic()
    {
        audioSource.clip = music[Random.Range(0, music.Length)] as AudioClip;
        audioSource.Play();
        StartCoroutine(WaitForClipEnd());
    }

    IEnumerator WaitForClipEnd()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        playRandomMusic();
    }
}
