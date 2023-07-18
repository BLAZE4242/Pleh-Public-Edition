 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DataMenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI disclaimerText;
    [SerializeField] Image streamerImage;
    [SerializeField] TextMeshProUGUI streamerText;
    [SerializeField] GameObject streamerModeAdvanced;
    [SerializeField] Toggle normal;
    [SerializeField] Button[] menuButtons;
    [SerializeField] TextMeshProUGUI[] buttonTexts;
    [SerializeField] AudioClip click;
    [SerializeField] TextMeshProUGUI disclaimerDepressionText;
    [SerializeField] TMP_FontAsset devFont;
    [SerializeField] TextMeshProUGUI devText;
    [SerializeField] Image blackScreen;
    [SerializeField] Canvas bootCanvas;
    [SerializeField] TextMeshProUGUI bootText;
    Color streamerImageColour, streamerTextColour, buttonColour, textColour;
    Toggle streamer;
    AudioSource _source;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Set's streamer mode on INIT
        if (!PlayerPrefs.HasKey("Streamer Mode")) PlayerPrefs.SetInt("Streamer Mode", 0);

        if (PlayerPrefs.GetString("Backup") == "yepp")
        {
            bootCanvas.gameObject.SetActive(true);
            return;
        }

        // Amelia save
        if (PlayerPrefs.GetInt("AmeliaRes_Save") != 0)
        {
            SceneManager.LoadScene("Amelia_Restaurant");
            return;
        }


        // Red text
        //if (PlayerPrefs.GetInt("glotchiness") == 4)
        //{
        //    SceneManager.LoadScene("RedText Intro");
        //    return;
        //}
        //else if (PlayerPrefs.GetInt("glotchiness") == 5)
        //{
        //    SceneManager.LoadScene("Menu 1");
        //    return;
        //}

        // Any other save
        if (PlayerPrefs.HasKey("Scene Save"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("Scene Save"));
            return;
        }

        // else
        if (PlayerPrefs.HasKey("glotchiness"))
        {
            SceneManager.LoadScene(1);
        }

        if (!PlayerPrefs.HasKey("Volume")) PlayerPrefs.SetString("Volume", "-16.25962|0.1538222");
    }

    void CheckForSceneChange()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneManager.LoadScene("null");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SceneManager.LoadScene("LostInThoughts");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SceneManager.LoadScene("UnderThoughts Part 2");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SceneManager.LoadScene("hub_main");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SceneManager.LoadScene("New Messages");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SceneManager.LoadScene("RedText Intro");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SceneManager.LoadScene("Lock both ways");
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerPrefs.SetInt("glotchiness", 1);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                PlayerPrefs.SetInt("glotchiness", 2);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerPrefs.SetInt("glotchiness", 3);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerPrefs.SetInt("glotchiness", 4);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                PlayerPrefs.SetInt("glotchiness", 5);
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                if (SteamManager.Initialized)
                    Steamworks.SteamUserStats.ResetAllStats(true);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        streamer = FindObjectOfType<Toggle>();
        _source = GetComponent<AudioSource>();
        if (PlayerPrefs.GetString("Backup") == "yepp") return;
        StartCoroutine(waitToShowButtons());

        if (PlayerPrefs.GetInt("Streamer Mode") == 1 || PlayerPrefs.GetInt("Streamer Mode") == 2)
        {
            streamer.isOn = true;
            streamerText.text = streamerText.text.Replace("disabled", "enabled");
        }
        else
        {
            streamer.isOn = false;
            streamerText.text = streamerText.text.Replace("enabled", "disabled");
        }
    }

    void makeButtonsInvisible(bool kill = false)
    {
        if (kill)
        {
            foreach (Button button in menuButtons)
            {
                button.gameObject.SetActive(false);
            }
            streamer.gameObject.SetActive(false);
        }
        else
        {
            foreach (Button button in menuButtons)
            {
                button.interactable = false;
                button.GetComponent<Image>().color = Color.clear;
            }
            foreach (TextMeshProUGUI text in buttonTexts)
            {
                text.color = Color.clear;
            }

            streamer.interactable = false;
            streamerText.color = Color.clear;
            streamerImage.color = Color.clear;
        }
    }

    IEnumerator waitToShowButtons()
    {
        LerpSolution.lerpTextColour(disclaimerText, Color.white, 0.6f, Color.clear);

        streamerImageColour = streamerImage.color;
        streamerTextColour = streamerText.color;

        buttonColour = menuButtons[0].GetComponent<Image>().color;
        textColour = buttonTexts[0].color;

        makeButtonsInvisible();

        yield return GeneralManager.waitForSeconds(2.5f);

        LerpSolution.lerpImageColour(streamerImage, streamerImageColour, 1f, Color.clear);
        LerpSolution.lerpTextColour(streamerText, streamerTextColour, 1f, Color.clear);
        streamer.interactable = true;

        yield return GeneralManager.waitForSeconds(1.5f);

        for(int i = 0; i < menuButtons.Length; i++)
        {
            Image img = menuButtons[i].GetComponent<Image>();
            TextMeshProUGUI text = buttonTexts[i];
            LerpSolution.lerpImageColour(img, buttonColour, 1f, Color.clear);
            LerpSolution.lerpTextColour(text, textColour, 1f, Color.clear);
            menuButtons[i].interactable = true;
            yield return GeneralManager.waitForSeconds(2);
        }
    }

    public void OnStreamerToggle(Toggle value)
    {
        Debug.Log("wtf???");
        if (value.isOn)
        {
            PlayerPrefs.SetInt("Streamer Mode", 1);
            streamerText.text = streamerText.text.Replace("disabled", "enabled");
        }
        else
        {
            PlayerPrefs.SetInt("Streamer Mode", 0);
            streamerText.text = streamerText.text.Replace("enabled", "disabled");
        }
    }

    public void OnAdvancedStreamerToggle(Toggle value)
    {
        if (value == normal && value.isOn)
        {
            PlayerPrefs.SetInt("Streamer Mode", 1);
        }
        else if(!normal.isOn)
        {
            PlayerPrefs.SetInt("Streamer Mode", 2);
        }
    }

    public void onContinuePress()
    {
        _source.PlayOneShot(click, 4);
        if (streamerModeAdvanced.activeInHierarchy)
        {
            streamerModeAdvanced.SetActive(false);
            disclaimerDepressionText.gameObject.SetActive(true);
            return;
        }
        if (disclaimerText.text != "")
        {
            disclaimerText.text = "";
            streamer.gameObject.SetActive(false);
            streamerText.gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("Streamer Mode") == 1)
            {
                streamerModeAdvanced.SetActive(true);
            }
            else
            {
                disclaimerDepressionText.gameObject.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(beginGame());
        }
    }

    IEnumerator beginGame()
    {
        disclaimerDepressionText.text = "";
        makeButtonsInvisible(true);

        AsyncOperation sceneOp;
        if(SceneManager.GetSceneByName("jesterGames").IsValid())
        {
            sceneOp = SceneManager.LoadSceneAsync("jesterGames");
        }
        else
        {
            sceneOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        sceneOp.allowSceneActivation = false;
        yield return GeneralManager.waitForSeconds(3);

        devText.font = devFont;
        devText.text = "Okay, here we go.";
        LerpSolution.lerpImageColour(blackScreen, Color.black, 0.1f);        
        yield return GeneralManager.waitForSeconds(3);
        devText.text = "It can't hurt anyone in here, it's just a game.";
        yield return GeneralManager.waitForSeconds(5);
        devText.text = "Let's begin.";
        yield return GeneralManager.waitForSeconds(4);
        LerpSolution.lerpTextColour(devText, Color.clear, 0.2f);
        yield return GeneralManager.waitForSeconds(5);
        sceneOp.allowSceneActivation = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerPrefs.GetString("Backup") == "yepp")
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && PlayerPrefs.GetString("Backup") == "yepp")
        {
            bootText.text = "Restoring most recent backup...\n\nThis could take up to several minutes";
            Invoke("Restore", 5);
        }
    }

    void Restore()
    {
        PlayerPrefs.DeleteKey("Backup");
        PlayerPrefs.DeleteKey("Scene Save");
        PlayerPrefs.SetInt("glotchiness", 0);
        SceneManager.LoadScene(0);
    }

    public void onExitPress()
    {
        StartCoroutine(onExitPressEnum());
    }

    IEnumerator onExitPressEnum()
    {
        try
        {
            Gas.EarnAchievement("ACH_QUIT");
            _source.PlayOneShot(click, 4);
            disclaimerText.text = "";
            disclaimerDepressionText.text = "";
            makeButtonsInvisible(true);
            
        }
        catch
        {
            Debug.LogError("No idea why that didn't work, but it didn't...");
        }
        yield return GeneralManager.waitForSeconds(2f);
        Application.Quit();
    }
}
