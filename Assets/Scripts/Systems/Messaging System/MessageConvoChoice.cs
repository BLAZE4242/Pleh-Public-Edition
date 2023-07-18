using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class MessageConvoChoice : MonoBehaviour
{
    [HideInInspector] public int playerChosenTimes;

    [SerializeField] AudioSource SFX;
    [SerializeField] AudioClip msgSFX;
    [SerializeField] Camera phoneCamera;
    [SerializeField] SpriteRenderer sendBubble, sendButton;
    [SerializeField] TextMeshPro[] textOptions = new TextMeshPro[4];
    [SerializeField] Color notSelectable, selectable;
    MessageConvoManager _messageConvoManager;
    bool isInChoosingMenu = false;

    [Header("Glitch shit")]
    [SerializeField] AudioClip click1;
    [SerializeField] AudioClip itllBeAlright;
    [SerializeField] Image blackScreen;
    [SerializeField] PostProcessVolume volume;
    [SerializeField] PostProcessProfile profile;
    [SerializeField] Animator glitchAnim;
    bool isGlitched = false;

    MessageConvoManager.MessageConvoState lastState;
    private void Start()
    {
        _messageConvoManager = FindObjectOfType<MessageConvoManager>();
        lastState = _messageConvoManager.currentMessagingState;
    }

    void Update()
    {
        if(lastState != _messageConvoManager.currentMessagingState)
        {
            UpdateSendBubbleState(_messageConvoManager.currentMessagingState == MessageConvoManager.MessageConvoState.PlayerChoosing);
            lastState = _messageConvoManager.currentMessagingState;
            playerChosenTimes++;
        }

        
        CheckForChoiceInput();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) OnMessageSendBubbleSelect();
    }

    public void CheckForChoiceInput(int choice = 0)
    {
        if (!isInChoosingMenu)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1) || choice == 1)
        {
            StartCoroutine(PlayerChosen(_messageConvoManager.currentConvo.messageConvos[0], _messageConvoManager.currentConvo.responses[0], 1));
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2) || choice == 2)
        {
            StartCoroutine(PlayerChosen(_messageConvoManager.currentConvo.messageConvos[1], _messageConvoManager.currentConvo.responses[1], 2));
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3) || choice == 3)
        {
            StartCoroutine(PlayerChosen(_messageConvoManager.currentConvo.messageConvos[2], _messageConvoManager.currentConvo.responses[2], 3));
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4) || choice == 4)
        {
            StartCoroutine(PlayerChosen(_messageConvoManager.currentConvo.messageConvos[3], _messageConvoManager.currentConvo.responses[3], 4));
        }
    }

    List<int> choiceNumbers = new List<int>();
    IEnumerator PlayerChosen(MessageConvo chosenConvo, string chosenText, int optionNumber)
    {
        SFX.PlayOneShot(msgSFX);

        AddArg(optionNumber);
        bool isForced = _messageConvoManager.currentConvo.IsForced;

        if (isForced && optionNumber != 4 && textOptions[optionNumber - 1].text != textOptions[3].text)
        {
            StartCoroutine(GetComponent<glitchText>().createGlitchedText(textOptions[3].text, textOptions[optionNumber - 1]));
        }
        else
        {
            //if (chosenText == "Worlds whitest sneakers") Gas.EarnAchievement("ACH_CHILIS");

            _messageConvoManager.currentMessagingState = MessageConvoManager.MessageConvoState.AmeliaTexting;
            isInChoosingMenu = false;
            LerpSolution.lerpPosition(phoneCamera.transform, new Vector3(phoneCamera.transform.position.x, phoneCamera.transform.position.y + 5, phoneCamera.transform.position.z), 6f);
            _messageConvoManager.currentConvo = chosenConvo;

            if (chosenConvo.name.ToLower().EndsWith("end"))
            {
                Finish();
            }

            choiceNumbers.Add(optionNumber);

            if (!isForced) _messageConvoManager.SpawnMessage(chosenText, true);
            else _messageConvoManager.SpawnMessage(textOptions[3].text, true);
            if (isForced)
            {
                selectable = Color.white;
                glitchAnim.SetTrigger("glitchUp");
            }
            if (!chosenConvo.messages[0].StartsWith("{nw}"))
            {
                yield return GeneralManager.waitForSeconds(0.5f);
            }
            StartCoroutine(_messageConvoManager.messageSequence(chosenConvo));

            if (isForced && !isGlitched)
            {
                isGlitched = true;
                StartCoroutine(GlitchFuck());
            }
            else if (isForced)
            {
                glitchAnim.SetTrigger("glitchUp");
            }
        }

    }

    string argQuestionOrder = "";
    int argIndex = 0;
    void AddArg(int option)
    {
        if (argQuestionOrder.Length > argIndex)
        {
            char[] ch = argQuestionOrder.ToCharArray();
            ch[argIndex] = option.ToString().ToCharArray()[0];
            argQuestionOrder = ch.ArrayToString();
        }
        else
        {
            argQuestionOrder += option.ToString();
        }
        if (PlayerPrefs.GetInt("glotchiness") == 6)
        {
            PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name}Confess arg", argQuestionOrder);
        }
        else
        {
            PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name} arg", argQuestionOrder);
        }
            
        argIndex++;
    }

    IEnumerator GlitchFuck()
    {
        SFX.PlayOneShot(click1);
        yield return GeneralManager.waitForSeconds(0.1f);
        blackScreen.color = Color.black;
        GetComponent<AudioSource>().clip = itllBeAlright;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
        volume.profile = profile;
        FindObjectOfType<Kino.AnalogGlitch>().enabled = true;
        glitchAnim.SetTrigger("glitch");
        CameraShake.Shake(500, 0.07f);

        yield return GeneralManager.waitForSeconds(0.2f);
        LerpSolution.lerpImageColour(blackScreen, Color.clear, 0.1f);
    }


    public void UpdateSendBubbleState(bool isSelectable)
    {
        // Off: 828282, On: 91B5CC
        if (isSelectable)
        {
            Debug.Log("Making selectable");
            LerpSolution.lerpSpriteColour(sendBubble, selectable, 2f);
            LerpSolution.lerpSpriteColour(sendButton, Color.white, 2f);

            UpdateChoices();
        }
        else
        {
            Debug.Log("Making unselectable");
            LerpSolution.lerpSpriteColour(sendBubble, notSelectable, 2f);
            LerpSolution.lerpSpriteColour(sendButton, notSelectable, 2f);
        }
    }

    bool canEnd;
    public void UpdateChoices()
    {
        try
        {
            for (int i = 0; i < 4; i++)
            {
                textOptions[i].text = _messageConvoManager.currentConvo.responses[i];
            }
        }
        catch
        {
            textOptions[0].text = "I'm so sorry...";
            textOptions[1].text = "";
            textOptions[2].text = "";
            textOptions[3].text = "";

            canEnd = true;
        }
    }

    //IEnumerator finishSection()
    //{
    //    yield return GeneralManager.waitForSeconds(0.5f);
    //    blackScreen.color = Color.black;
    //    SceneManager.LoadScene("dev_angry");
    //}

    public void OnMessageSendBubbleSelect() // gets invoked from collision manager
    {
        if (isInChoosingMenu || _messageConvoManager.currentMessagingState != MessageConvoManager.MessageConvoState.PlayerChoosing) return;
        else StartCoroutine(pullUpSelectMenu());
    }

    IEnumerator pullUpSelectMenu()
    {
        isInChoosingMenu = true;
        Vector3 targetCamPos = new Vector3(phoneCamera.transform.position.x, phoneCamera.transform.position.y - 5, phoneCamera.transform.position.z);
        LerpSolution.lerpPosition(phoneCamera.transform, targetCamPos, 6f);

        while(phoneCamera.transform.position != targetCamPos)
        {
            yield return GeneralManager.waitForEndOfFrame;
        }

        if (canEnd)
        {
            yield return GeneralManager.waitForSeconds(0.5f);
            blackScreen.color = Color.black;
            SceneManager.LoadScene("dev_angry");
        }
    }

    public void Finish()
    {
        //Debug.LogError("test");
        int lastChoice = 0;
        foreach (int i in choiceNumbers)
        {
            if (lastChoice != 0)
            {
                if (lastChoice != i) return;
            }
            lastChoice = i;
        }

        Gas.EarnAchievement("ACH_MATH");
    }
}
