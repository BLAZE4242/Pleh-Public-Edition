using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] PauseMenu currentMenu;
    [SerializeField] Transform screen;
    [SerializeField] AudioMixer mixer;
    [Header("Option")]
    [SerializeField] GameObject optionPrefab;
    [SerializeField] Transform optionParent;
    [SerializeField] GameObject eventSystem;
    [SerializeField] Slider volumeSlider;
    bool isSelectingSetting = false;

    int selectedIndexField = 0;
    int selectedIndex
    {
        get {return selectedIndexField;}
        set {
            int settingsLength = currentMenu.menuOptions.Length - 1;
            if (isSelectingSetting)
            {
                settingsLength = FindObjectsOfType<PauseSettingText>().Length - 1;
            }

            if(value < 0)
            {
                selectedIndexField = settingsLength;
            }
            else if(value > settingsLength)
            {
                selectedIndexField = 0;
            }
            else
            {
                selectedIndexField = value;
            }
        }
    }
    [SerializeField] Transform plutoHead;
    [SerializeField] Transform plutoSelectingHead;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] Color settingDefaultColour = Color.yellow;
    [Header("Override options")]
    [SerializeField] PauseScreenOption resolutionOption;
    [SerializeField] TextMeshProUGUI version;
    [SerializeField] TextMeshProUGUI pauseTitle;
    bool wasCursorLocked, wasCursorVisible;

    // Key is the setting title and value is setting value
    private string tempSettingToChange;
    private int tempLastIndex;


    void Start()
    {
        wasCursorLocked = Cursor.lockState == CursorLockMode.Locked;
        wasCursorVisible = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        settingsMenu.SetActive(false);
        //AssignVariables();

        if (FindObjectsOfType<EventSystem>().Length < 1)
        {
            eventSystem.SetActive(true);
        }

        version.text = "V " + Application.version;

        @null();
    }

    private void @null()
    {
        if (FindObjectOfType<nullManager>() != null)
        {
            pauseTitle.text = "null";
            foreach (TextMeshProUGUI text in currentMenu.GetComponentsInChildren<TextMeshProUGUI>())
            {
                text.text = "null";
            }
        }
    }

    void AssignVariables()
    {
        foreach (var res in Screen.resolutions)
        {
            resolutionOption.settingOptions.Add(res.width + "x" + res.height);
            Debug.Log(res.width + "x" + res.height);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            volumeSlider.GetComponentInParent<Canvas>().enabled = false;
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            volumeSlider.GetComponentInParent<Canvas>().enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if(!isSelectingSetting) StartCoroutine(MoveIndex(selectedIndex - 1));
            else StartCoroutine(MoveIndex(selectedIndex + 1));
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if(!isSelectingSetting) StartCoroutine(MoveIndex(selectedIndex + 1));
            else StartCoroutine(MoveIndex(selectedIndex - 1));
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if(!isSelectingSetting)
            {
                currentMenu.menuOptions[selectedIndex].OptionPress();
            }
            else
            {
                string selectedOption = FindObjectsOfType<PauseSettingText>()[selectedIndex].GetComponent<TextMeshProUGUI>().text;
                foreach (PauseSettingText option in FindObjectsOfType<PauseSettingText>())
                {
                    Destroy(option.gameObject);
                }

                settingsMenu.SetActive(false);
                isSelectingSetting = false;

                GetComponent<PauseSetSettingValues>().SetSettingValue(tempSettingToChange, selectedOption);

                plutoSelectingHead.gameObject.SetActive(false);
                plutoHead.gameObject.SetActive(true);
                selectedIndex = tempLastIndex;
            }
        }
    }

    IEnumerator MoveIndex(int newSelectedIndex)
    {
        
        Transform targetPlutoHead = plutoHead;
        if (isSelectingSetting) targetPlutoHead = plutoSelectingHead;

        selectedIndex = newSelectedIndex;
        Vector3 currentHeadPos = targetPlutoHead.position;
        if (!isSelectingSetting)
        {
            targetPlutoHead.position = new Vector3(currentHeadPos.x, currentMenu.menuOptions[selectedIndex].transform.position.y, currentHeadPos.z);
        }
        else
        {
            PauseSettingText[] settingTexts = FindObjectsOfType<PauseSettingText>();
            yield return new WaitForEndOfFrame();
            targetPlutoHead.position = new Vector3(currentHeadPos.x, settingTexts[selectedIndex].transform.position.y, currentHeadPos.z);
        }
    }

    public void UnPause()
    {
        //wasCursorLocked = Cursor.lockState == CursorLockMode.Locked;
        //wasCursorVisible = Cursor.visible;
        FindObjectOfType<pause>().UnPauseGame();
    }

    public void ShowPauseMenu(PauseMenu menu)
    {
        foreach (PauseMenu menuInScene in FindObjectsOfType<PauseMenu>())
        {
            menuInScene.gameObject.SetActive(false);
        }
        menu.gameObject.SetActive(true);

        currentMenu = menu;

        StartCoroutine(MoveIndex(0));

        int index = -1;
        switch (menu.name)
        {
            case "Quality":
                index = QualitySettings.GetQualityLevel();
                break;
            case "Window Mode":
                index = fullscreenToInt();
                break;
            case "Streamer Mode":
                index = PlayerPrefs.GetInt("Streamer Mode");
                break;
            case "Volume":
                if(PlayerPrefs.HasKey("Volume"))
                {
                    volumeSlider.value = float.Parse(PlayerPrefs.GetString("Volume").Split('|')[1]);
                }
                return;

        }

        if (currentMenu.name != "Reset 1") @null();

        if (index == -1) return;

        foreach (PauseScreenOption text in menu.menuOptions)
        {
            text.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        menu.menuOptions[index].GetComponent<TextMeshProUGUI>().color = Color.yellow;

    }

    int fullscreenToInt()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen: return 0;
            case FullScreenMode.Windowed: return 1;
        }
        return 0;
    }

    public void LoadScene(TextMeshProUGUI scene)
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj != gameObject && obj.GetComponent<Gas>() == null && obj.GetComponent<Canvas>() == null && obj.GetComponentInParent<Canvas>() == null)
            {
                Destroy(obj);
            }
        }
        UnPause();
        PlayerPrefs.DeleteKey("thats it");
        if (scene != null) SceneManager.LoadScene(scene.text);
    }

    public void LoadScene(string scene)
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj != gameObject && obj.GetComponent<Gas>() == null && obj.GetComponent<Canvas>() == null && obj.GetComponentInParent<Canvas>() == null)
            {
                Destroy(obj);
            }
        }
        UnPause();
        PlayerPrefs.DeleteKey("thats it");
        if (scene != null) SceneManager.LoadScene(scene);
    }

    public void ChangeGlotchiness(TextMeshProUGUI number)
    {
        PlayerPrefs.SetInt("glotchiness", int.Parse(number.text));
    }

    public void ResetData()
    {
        string[] keys = { "playerName", "Scene Save", "Streamer Mode", "glotchiness", "thats it" };
        foreach (string key in keys)
        {
            PlayerPrefs.DeleteKey(key);
        }
            Time.timeScale = 1;
            AudioListener.pause = false;
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                if (obj != gameObject && obj.GetComponent<Gas>() == null && obj.GetComponent<Canvas>() == null && obj.GetComponentInParent<Canvas>() == null)
                {
                    Destroy(obj);
                }
            }
            SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QualityChange(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
    }

    public void VolumeChange(Slider volume)
    {
        float targetVolume = volume.value;
        if (targetVolume == 0) targetVolume = -80;
        mixer.SetFloat("Volume", Mathf.Log10(targetVolume) * 20);

        PlayerPrefs.SetString("Volume", Mathf.Log10(targetVolume) * 20 + "|" + volume.value);
    }

    public void WindowModeChange(int fullscreenMode)
    {
        if (fullscreenMode == 0) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public void StreamerModeChange(int streamerMode)
    {
        PlayerPrefs.SetInt("Streamer Mode", streamerMode);
        FindObjectOfType<DRP>().DoShit();
    }

    public void RemoveProgress(bool statsIncluded)
    {
        PlayerPrefs.DeleteKey("Streamer Mode");
        PlayerPrefs.DeleteKey("playerName");
        PlayerPrefs.DeleteKey("glotchiness");
        PlayerPrefs.DeleteKey("meme data");
        PlayerPrefs.DeleteKey("Scene Save");
        PlayerPrefs.DeleteKey("thats it");
        PlayerPrefs.DeleteKey("Backup");
        if (statsIncluded) PlayerPrefs.DeleteKey("chair");

        Application.Quit();
    }

    public void DeleteALLprogressplsdontdo()
    {
        PlayerPrefs.DeleteAll();
    }

    public void deleteAllachievmeentsonceagaindontdo()
    {
        Gas.DeleteStat(true);
    }

    public void ShowSelectOption(PauseScreenOption option)
    {
        tempSettingToChange = option.GetComponent<TextMeshProUGUI>().text;
        tempLastIndex = selectedIndex;

        if (option.settingOptions.Count > 3)
        {
            for (int i = option.settingOptions.Count; i > option.settingOptions.Count - 3; i--)
            {
                SpawnSelectOption(option.settingOptions[i - 1]);
            }
        }
        else
        {
            // Shows menu
            Debug.Log("calllllll");
            settingsMenu.SetActive(true);

            int i = 0;

            foreach (string optionName in option.settingOptions)
            {
                if (i < 4)
                {
                    i++;
                    SpawnSelectOption(optionName);            
                }
            }
        }

        isSelectingSetting = true;
        selectedIndex = -1;
        plutoHead.gameObject.SetActive(false);
        plutoSelectingHead.gameObject.SetActive(true);

        StartCoroutine(MoveIndex(selectedIndex));
    }

    void SpawnSelectOption(string optionName)
    {
        GameObject spawnedOption = Instantiate(optionPrefab, optionParent);
        TextMeshProUGUI spawnedText = spawnedOption.GetComponent<TextMeshProUGUI>();
        spawnedText.text = optionName;

        if (optionName == GetComponent<PauseSetSettingValues>().SetSettingValue(tempSettingToChange))
        {
            spawnedText.color = settingDefaultColour;
        }
    }
}
