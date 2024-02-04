using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GeneralManager : MonoBehaviour
{
    public bool speedrun = false;

    public AudioMixer mixer;

    char[] cheatCode = new char[] { 'b', 'l', 'a', 'z', 'e', 's', 'e', 'n', 't', 'm', 'e' };
    int cheatCodeIndex = 0;

    char[] devRoomCode = new char[] { '1', '0', '.', '1', '7' };
    int devRoomIndex = 0;

    public static Dictionary<float, WaitForSeconds> waitForSecondsPool = new Dictionary<float, WaitForSeconds>()
    {
        { 1, new WaitForSeconds(1) },
        { 2, new WaitForSeconds(2) },
        { 3, new WaitForSeconds(3) },
        { 4, new WaitForSeconds(4) },
        { 5, new WaitForSeconds(5) },
        { 6, new WaitForSeconds(6) },
        { 7, new WaitForSeconds(7) }
    };

    public static WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public static WaitForSeconds waitForSeconds(float time)
    {
        if (waitForSecondsPool.TryGetValue(time, out WaitForSeconds waitForSecondsMatch))
        {
            return waitForSecondsMatch;
        }
        else
        {
            WaitForSeconds newWaitForSeconds = new WaitForSeconds(time);
            waitForSecondsPool.Add(time, newWaitForSeconds);
            Debug.Log("Added to pool");
            return newWaitForSeconds;
        }
    }

    private void Start()
    {
#if UNITY_EDITOR // For the open source pleh
        if (!PlayerPrefs.HasKey("Volume"))
        {
            Debug.LogWarning("No volume thing has been created yet... creating one for you ;)");
            PlayerPrefs.SetString("Volume", "-16.25962|0.1538222");
        }
#endif
        if (PlayerPrefs.GetString("Volume").Split('|')[0] == "NaN") mixer.SetFloat("Volume", -80);
        else if (PlayerPrefs.HasKey("Volume"))
        {
            mixer.SetFloat("Volume", float.Parse(PlayerPrefs.GetString("Volume").Split('|')[0]));
        }
    }

    public static void SetSceneSave(string scene = "")
    {
        PlayerPrefs.SetString("Scene Save", scene == "" ? SceneManager.GetActiveScene().name : scene);
    }

    float timeForBIOS = 0f;
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            CheckForCheatCode();
            CheckForDevRoom();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            speedrun = !speedrun;
        }

        CheckForBios();
    }

    private void CheckForBios()
    {
        if (Input.GetKey(KeyCode.F2))
        {
            timeForBIOS += Time.deltaTime;
            if (timeForBIOS >= 1)
            {
                CheckForBIOS();
                timeForBIOS = 0;
            }
        }
        else
        {
            timeForBIOS = 0;
        }
    }

    private void CheckForCheatCode()
    {
        if (Input.GetKeyDown(cheatCode[cheatCodeIndex].ToString()))
        {
            cheatCodeIndex++;
            if (cheatCodeIndex == cheatCode.Length)
            {
                Gas.EarnAchievement("ACH_READcREDITS");
                cheatCodeIndex = 0;
            }

        }
        else
        {
            cheatCodeIndex = 0;
        }
    }

    private void CheckForDevRoom()
    {
        if (Input.GetKeyDown(devRoomCode[devRoomIndex].ToString()))
        {
            devRoomIndex++;
            Debug.Log("Did that right!");
            if (devRoomIndex == devRoomCode.Length)
            {
                string sceneName = SceneManager.GetActiveScene().name;
                if (sceneName == "hub_bingus")
                {
                    PlayerPrefs.SetString("inception", "yeppers");
                }

                foreach (GameObject obj in FindObjectsOfType<GameObject>())
                {
                    if (obj != gameObject && obj.GetComponent<Gas>() == null && obj.GetComponent<Canvas>() == null && obj.GetComponentInParent<Canvas>() == null)
                    {
                        Destroy(obj);
                    }
                }
                SceneManager.LoadScene("hub_bingus");
                devRoomIndex = 0;
            }

        }
        else
        {
            devRoomIndex = 0;
        }
    }

    private void CheckForBIOS()
    {
        string SecondDir = Application.dataPath;
        List<string> SecondDirOperation = new List<string>(SecondDir.Split('/'));
        SecondDirOperation.RemoveAt(SecondDirOperation.Count - 1);
        SecondDir = string.Join("\\", SecondDirOperation);

        if (Application.isEditor)
        {
            SecondDir += "\\Assets\\Scenes\\ARG\\BIOSener.config";
        }
        else
        {
            SecondDir += "\\BIOSener.config";
        }

        string sceneName = SceneManager.GetActiveScene().name;
        if (File.Exists(SecondDir) && File.ReadAllLines(SecondDir)[0][0] == ',' && File.ReadAllLines(SecondDir)[0][4] == ',' && sceneName != "file demo explorer" && sceneName != "ekse" && PlayerPrefs.GetString("Backup") != "yepp")
        {
            if (sceneName == "hub_main" && PlayerPrefs.GetString("Backup") == "yep") return;
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                if (obj != gameObject && obj.GetComponent<Gas>() == null && obj.GetComponent<Canvas>() == null && obj.GetComponentInParent<Canvas>() == null)
                {
                    Destroy(obj);
                }
            }
            SceneManager.LoadScene("file demo explorer");
        }
        else
        {
            Debug.Log("Not success!");
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "file demo explorer")
        {
            try { GetComponentInChildren<Image>().enabled = true; } catch { }
        }
        else
        {
            try { GetComponentInChildren<Image>().enabled = false; } catch { }
        }
    }
}
