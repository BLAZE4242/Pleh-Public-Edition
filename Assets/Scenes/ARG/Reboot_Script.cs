using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Reboot_Script : MonoBehaviour
{
    [SerializeField] Image blackScreen;
    [SerializeField] List<TextMeshPro> texts = new List<TextMeshPro>();
    [SerializeField] SpriteRenderer blackSprite;
    [SerializeField] TextMeshPro directoryText;
    [SerializeField] TMP_FontAsset redTextFont;
    [SerializeField] Color redTextColour;
    PlayerController controller;
    glitchText gText;
    AudioSource source;

    private void Start()
    {
        FindObjectOfType<DRP>().RP("Error: Cannot locate player");
        GeneralManager.SetSceneSave();
        controller = FindObjectOfType<PlayerController>();
        gText = GetComponent<glitchText>();
        source = GetComponent<AudioSource>();
        foreach (TextMeshPro text in texts)
        {
            text.gameObject.SetActive(false);
        }

        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
        blackScreen.color = Color.black;
        controller.lockLook = true;
        controller.lockMovement = true;

        yield return GeneralManager.waitForSeconds(2);

        blackScreen.color = Color.clear;
        controller.lockLook = false;
        controller.lockMovement = false;

        yield return GeneralManager.waitForSeconds(0.5f);

        NextText();
        source.Play();
    }

    public void NextText()
    {
        texts[0].gameObject.SetActive(true);
        gText.glitchGlitchedText(texts[0]);
        texts.RemoveAt(0);
    }

    public void MovePlayer()
    {
        controller.TeleportPlayer(controller.transform.position + new Vector3(-10, 0, 0));
    }

    public void AfterCollect() { StartCoroutine(AfterCollectEnum()); }

    IEnumerator AfterCollectEnum()
    {
        Vector3 defaultScale = directoryText.transform.localScale;

        GetComponent<AudioSource>().Stop();
        blackSprite.color = Color.black;
        directoryText.font = redTextFont;
        directoryText.color = redTextColour;


        StartCoroutine(gText.createGlitchedText("This is the last of 10.", directoryText));
        LerpSolution.lerpScale(directoryText.transform, defaultScale, 0.1f, defaultScale - new Vector3(0.01f, 0.01f, 0.01f));

        yield return GeneralManager.waitForSeconds(4);


        StartCoroutine(gText.createGlitchedText("Just like this game, this one has it's secrets.", directoryText));
        LerpSolution.lerpScale(directoryText.transform, defaultScale, 0.1f, defaultScale - new Vector3(0.01f, 0.01f, 0.01f));

        yield return GeneralManager.waitForSeconds(5);


        StartCoroutine(gText.createGlitchedText("Uncover them, then look for the rest.", directoryText));
        LerpSolution.lerpScale(directoryText.transform, defaultScale, 0.1f, defaultScale - new Vector3(0.01f, 0.01f, 0.01f));

        yield return GeneralManager.waitForSeconds(5);


        StartCoroutine(gText.createGlitchedText("I will reset the game now, it will be like none of this ever happened.", directoryText));
        LerpSolution.lerpScale(directoryText.transform, defaultScale, 0.1f, defaultScale - new Vector3(0.01f, 0.01f, 0.01f));

        yield return GeneralManager.waitForSeconds(6);


        StartCoroutine(gText.createGlitchedText("Good luck.", directoryText));
        LerpSolution.lerpScale(directoryText.transform, defaultScale, 0.1f, defaultScale - new Vector3(0.01f, 0.01f, 0.01f));

        yield return GeneralManager.waitForSeconds(4);

        blackScreen.color = Color.black;
        directoryText.text = "";

        yield return GeneralManager.waitForSeconds(2);

        if (PlayerPrefs.HasKey("PlaythroughCount")) PlayerPrefs.SetInt("PlaythroughCount", PlayerPrefs.GetInt("PlaythoughCount") + 1);
        else PlayerPrefs.SetInt("PlaythroughCount", 1);
        PlayerPrefs.DeleteKey("glotchiness");
        PlayerPrefs.DeleteKey("Scene Save");
        PlayerPrefs.DeleteKey("Streamer Mode");
        PlayerPrefs.DeleteKey("playerName");

        string SecondDir = Application.dataPath;
        List<string> SecondDirOperation = new List<string>(SecondDir.Split('/'));
        SecondDirOperation.RemoveAt(SecondDirOperation.Count - 1);
        SecondDir = string.Join("/", SecondDirOperation);
        Application.OpenURL("file:///" + SecondDir + "/Transcripts");

        //SceneManager.LoadScene(0);

        Application.Quit();
    }
}
