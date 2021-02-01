using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string mixerParameter;

    private void Start()
    {
        if (PlayerPrefs.HasKey(mixerParameter))
            GetComponent<Slider>().value = PlayerPrefs.GetFloat(mixerParameter);
        else
            GetComponent<Slider>().value = 0.5f;
    }

    public void SetLevel(float sliderValue)
    {
        if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
        PlayerPrefs.SetFloat(mixerParameter, sliderValue);
        mixer.SetFloat(mixerParameter, Mathf.Log10(sliderValue) * 20);
    }
}
