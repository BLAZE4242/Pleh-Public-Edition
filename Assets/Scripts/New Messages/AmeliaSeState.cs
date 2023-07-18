using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmeliaSeState : MonoBehaviour
{
    [SerializeField] MessageConvo message;
    [SerializeField] TextMeshPro onlineStatus;
    [SerializeField] AudioClip song;
    [SerializeField] GameObject Creator, Jester, Cast, Playtesters, Playtesters1, Support, Support1, Support2, SpecialThanks, Unity, Help, Final;
    [SerializeField] TextMeshPro nameText;
    bool hasStarted = false;
    bool canQuit;

    void StateStart(AmeliaStateManager _manager)
    {
        FindObjectOfType<MessageConvoManager>().currentConvo = message;
        FindObjectOfType<MessageConvoManager>().currentAmeliaTypeState = MessageConvoManager.AmeliaTypeState.Typing;
    }

    public IEnumerator credits()
    {
        yield return GeneralManager.waitForSeconds(2);

        onlineStatus.text = "Offline"; // Poor Amelia... I actually feel kinda guilty. Sorry...
        FindObjectOfType<glitchText>().glitchGlitchedText(onlineStatus);
        Gas.EarnAchievement("ACH_ARG_1");

        yield return GeneralManager.waitForSeconds(4);

        Destroy(FindObjectOfType<AudioDistortionFilter>());
        GetComponent<AudioSource>().clip = song;
        GetComponent<AudioSource>().Play();
        StartCoroutine(CreditsRoll());
        if (!PlayerPrefs.GetString("hs").Contains("2"))
        {
            PlayerPrefs.SetString("hs", PlayerPrefs.GetString("hs") + "2");
        }
    }

    IEnumerator CreditsRoll()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Creator.SetActive(true);

        yield return GeneralManager.waitForSeconds(6);

        Creator.SetActive(false);
        Jester.SetActive(true);

        yield return GeneralManager.waitForSeconds(6);

        Jester.SetActive(false);
        Cast.SetActive(true);

        yield return GeneralManager.waitForSeconds(6);

        Cast.SetActive(false);
        Playtesters.SetActive(true);

        yield return GeneralManager.waitForSeconds(9);


        Playtesters.SetActive(false);
        Playtesters1.SetActive(true);

        yield return GeneralManager.waitForSeconds(9);

        Playtesters1.SetActive(false);
        Support.SetActive(true);

        yield return GeneralManager.waitForSeconds(9);

        Support.SetActive(false);
        Support1.SetActive(true);

        yield return GeneralManager.waitForSeconds(9);

        Support1.SetActive(false);
        Support2.SetActive(true);

        yield return GeneralManager.waitForSeconds(9);

        Support2.SetActive(false);
        SpecialThanks.SetActive(true);
        nameText.text = PlayerPrefs.GetString("playerName");

        yield return GeneralManager.waitForSeconds(15);

        SpecialThanks.SetActive(false);
        Unity.SetActive(true);

        yield return GeneralManager.waitForSeconds(6);

        Unity.SetActive(false);
        Help.SetActive(true);

        yield return GeneralManager.waitForSeconds(10);

        Help.SetActive(false);
        Gas.EarnAchievement("ACH_CREDITS");
        Final.SetActive(true);

        PlayerPrefs.DeleteKey("thats it");
        PlayerPrefs.DeleteKey("Scene Save");
        PlayerPrefs.DeleteKey("glotchiness");
        canQuit = true;

    }

    public void OnLink(bool donate)
    {
        if (!donate) Application.OpenURL("https://discord.gg/7JW9R4waEg");
        else Application.OpenURL("https://buymeacoffee.com/blaze42");
    }

    public void CheckStateStarted(AmeliaStateManager _manager)
    {
        if (!hasStarted)
        {
            StateStart(_manager);
            StateUpdate(_manager);
            hasStarted = true;
        }
        else
        {
            StateUpdate(_manager);
        }
    }

    void StateUpdate(AmeliaStateManager ameliaStateManager)
    {
        if (Input.GetKeyDown(KeyCode.H) && canQuit)
        {
            Application.Quit();
        }
    }
}
