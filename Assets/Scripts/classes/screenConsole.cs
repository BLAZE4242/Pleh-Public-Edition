using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class screenConsole : MonoBehaviour
{
    [SerializeField] GameObject console;

    [SerializeField] TextMeshProUGUI inputText, consoleHistory;
    [SerializeField] string writtenText;
    [SerializeField] float blinkTime;
    [SerializeField] float typeTime;
    bool isWriting;

    // Start is called before the first frame update
    void Start()
    {
        //writeConsole("Load_Game(Pleh)", "Loading Pleh...", 2, 2);
        console.SetActive(false);
    }

    void Update()
    {
        inputText.text = writtenText;
        // if(Input.GetKeyDown(KeyCode.Y))
        // {
        //     console.SetActive(true);
        //     writeConsole("Application.Restart();", ">Application.Restart();\n<color=#FFC107>Warning: Unknown entity \"🗅🗅🗅🗅🗅🗅🗅.exe\" has attempted to access and modify program files. According to the default project settings, Unknown entity \"🗅🗅🗅🗅🗅🗅🗅.exe\" has been blocked. By restarting the program you are saving changes to the firewall to allow Unknown entity \"🗅🗅🗅🗅🗅🗅🗅.exe\" to have permission to access and modify program files. Are you sure you want to command this action? (y/n)</color>", 2, 0.5f);

        // }
    }

    public void writeConsole(string command, string print, UnityEvent onFinish, float waitBefore = 1, float waitAfter = 1)
    {
        console.SetActive(true);
        StartCoroutine(writeConsoleReal(command, print, onFinish, waitBefore, waitAfter));
    }

    IEnumerator writeConsoleReal(string command, string print, UnityEvent onFinish, float waitBefore, float waitAfter)
    {
        Coroutine last = StartCoroutine(underscoreBlink());
        yield return GeneralManager.waitForSeconds(waitBefore);
        StopCoroutine(last);
        writtenText = "_";


        char[] commandChar = command.ToCharArray();
        foreach (char character in commandChar)
        {
            writtenText = writtenText.Insert(writtenText.Length - 1, character.ToString());
            yield return GeneralManager.waitForSeconds(typeTime);
        }
        writtenText = writtenText.Remove(writtenText.Length - 1);
        last = StartCoroutine(underscoreBlink());
        yield return GeneralManager.waitForSeconds(waitAfter);

        writtenText = "";
        consoleHistory.text = consoleHistory.text.Insert(consoleHistory.text.Length, $"\n{print}");
        onFinish.Invoke();
    }



    IEnumerator underscoreBlink()
    {
        while(true)
        {
            writtenText = writtenText.Insert(writtenText.Length, "_");
            yield return GeneralManager.waitForSeconds(blinkTime);
            writtenText = writtenText.Remove(writtenText.Length - 1);
            yield return GeneralManager.waitForSeconds(blinkTime);
        }
    }
}
