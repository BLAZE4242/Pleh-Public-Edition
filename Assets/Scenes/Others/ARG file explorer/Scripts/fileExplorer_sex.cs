using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class fileExplorer_sex : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] AudioClip staticSfx;
    AudioSource source;
    int itteration = 0;

    private void Start()
    {
        source = GameObject.Find("SFX").GetComponent<AudioSource>();
    }

    public void Progress()
    {
        switch (itteration)
        {
            case 1:
                FindObjectOfType<fileExplorer_inputManager>().shouldRestart = true;
                Say("Don't do this.");
                break;
            case 2:
                Say("Trust me.");
                source.clip = staticSfx;
                source.loop = true;
                source.Play();
                LerpSolution.lerpVolume(source, 0.3f, 0.1f, 0);
                break;
            case 3:
                Say("You don't know what you're doing.");
                LerpSolution.lerpVolume(source, 0.5f, 0.1f);
                break;
            case 4:
                Say("Fine. Have it your way.");
                LerpSolution.lerpVolume(source, 0.8f, 0.1f);
                break;
            case 5:
                Say("Here we go...");
                StartCoroutine(ContinueHavingSex());
                break;
        }

        Type();
        itteration++;
    }

    void Type()
    {
        FindObjectOfType<fileExplorer_inputManager>().isDeleting = true;
        FindObjectOfType<fileExplorer_inputManager>().ConsoleWrite("'unknown entity.exe' is too large to move to bin. Are you sure you want to remove the file permanently? (y/n)");
    }

    void Say(string whatToSay)
    {
        text.text = whatToSay;
        GetComponent<glitchText>().glitchGlitchedText(text);
    }

    IEnumerator ContinueHavingSex()
    {
        GetComponent<fileExplorer_inputManager>().canType = false;

        yield return GeneralManager.waitForSeconds(2);

        PlayerPrefs.SetString("thats it", "sex");
        SceneManager.LoadScene("ekse");
    }
}
