using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSetSettingValues : MonoBehaviour
{
    public string SetSettingValue(string settingToChange, string settingValue = "null")
    {
        switch (settingToChange)
        {
            case "Window Mode":
                SetWindowedMode(settingValue);
                if (Screen.fullScreen) return "Full screen";
                return "Windowed";
            case "Resolution":
                SetResolutionSize(settingValue);
                return "1920x1080";
            case "Streamer Mode":
                SetStreamerMode(settingValue);
                if (PlayerPrefs.GetInt("Streamer Mode") == 1) return "On";
                return "Off";
            default:
                Debug.LogError(settingToChange);
                return "null";
        }
    }

    void SetWindowedMode(string setting)
    {
        switch (setting)
        {
            case "Windowed":
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log("Set windowed mode to windowed!");
                break;
            case "Full screen":
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Debug.Log("Set windowed mode to full screen!");
                break;
        }
    }

    void SetResolutionSize(string setting)
    {
        //Screen.SetResolution()
        Debug.Log("set thing to " + setting);
    }

    void SetStreamerMode(string setting)
    {
        if (setting == "On") PlayerPrefs.SetInt("Streamer Mode", 1);
        else if (setting == "Off") PlayerPrefs.SetInt("Streamer Mode", 0);
    }
}
