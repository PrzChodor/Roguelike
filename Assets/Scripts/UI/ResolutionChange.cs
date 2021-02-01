using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionChange : MonoBehaviour
{
    private Resolution[] resolutions;
    public Dropdown fullScreen;
    public Toggle vSync;

    void Start()
    {
        Dropdown dropdown = GetComponent<Dropdown>();
        resolutions = Screen.resolutions;
        Array.Reverse(resolutions);

        dropdown.options = new List<Dropdown.OptionData>();
        foreach (var res in resolutions)
        {
            dropdown.options.Add(new Dropdown.OptionData($"{res.width}x{res.height} {res.refreshRate}hz"));
        }

        LoadSettings();
    }

    public void ChangeResolution(Dropdown change)
    {
        GetComponent<AudioSource>().Play();
        Resolution res = resolutions[change.value];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRate);
        PlayerPrefs.SetInt("width", res.width);
        PlayerPrefs.SetInt("height", res.height);
        PlayerPrefs.SetInt("refreshRate", res.refreshRate);
    }

    public void ChangeFullscreen(Dropdown change)
    {
        GetComponent<AudioSource>().Play();
        Screen.fullScreenMode = (FullScreenMode)((change.value == 2) ? 3 : change.value);
        PlayerPrefs.SetInt("fullScreenMode", (change.value == 2) ? 3 : change.value);
    }

    public void ChangeVSync(Toggle change)
    {
        GetComponent<AudioSource>().Play();
        QualitySettings.vSyncCount = Convert.ToInt32(change.isOn);
        PlayerPrefs.SetInt("vSyncCount", QualitySettings.vSyncCount);
    }

    public void LoadSettings()
    {
        int index = -1;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
                index = i;
        }

        if (index != -1)
            GetComponent<Dropdown>().value = index;

        fullScreen.value = (int)Screen.fullScreenMode;

        if (PlayerPrefs.HasKey("vSyncCount"))
            vSync.isOn = Convert.ToBoolean(QualitySettings.vSyncCount);
        else
            vSync.isOn = false;
    }
}
