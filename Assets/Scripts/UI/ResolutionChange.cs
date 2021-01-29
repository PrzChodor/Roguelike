using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionChange : MonoBehaviour
{
    Resolution[] resolutions;

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
    }

    public void ChangeResolution(Dropdown change)
    {
        Resolution res = resolutions[change.value];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRate);
    }

    public void ChangeFullscreen(Dropdown change)
    {
        Screen.fullScreenMode = (FullScreenMode)((change.value == 2) ? 3 : change.value);
    }

    public void ChangeVSync(Toggle change)
    {
        QualitySettings.vSyncCount = Convert.ToInt32(change.isOn);
    }
}
