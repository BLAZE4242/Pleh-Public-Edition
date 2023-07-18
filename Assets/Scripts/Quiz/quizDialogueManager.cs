using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class quizDialogueManager : MonoBehaviour
{
    [SerializeField] bool developerControls = true;
    public bool isFirstTime;
    [SerializeField] TextMeshProUGUI tutorialText;
    bool hasSeenProgress, hasSeenChoice; // for tutorial
    [SerializeField] TextMeshPro dialogueText;
    [SerializeField] List<TextMeshPro> evilDoorTexts = new List<TextMeshPro>();
    [SerializeField] List<TextMeshPro> evilEndingTexts = new List<TextMeshPro>();
    public TextMeshPro[] dialogueOptions;
    public TextMeshPro[] secondaryDialogueOptions;
    [SerializeField] SpriteRenderer arrow;
    [SerializeField] glitchText _glitchText;
    public AudioClip click1;
    [SerializeField] AudioClip click2;
    [HideInInspector] public Dictionary<string, float> script = new Dictionary<string, float>();
    [HideInInspector] public float clickNoiseMultiplyer = 1f;
    AudioSource _sfx;
    AudioSource _music;
    string choice1, choice2, choice3, choice4;
    Coroutine lerpingFadingCouritine;
    TextMeshPro selectedTextFromPreviousQuestion;
    Action choice1ac, choice2ac, choice3ac, choice4ac;
    bool canPlayerChoose = false;
    bool playerPressSpace = false;
    LerpSolution _lerp;
    quizEvents _event;
    quizManager _manager;
    Coroutine arrowEnum;
    Camera cam;

    void Awake()
    {
        AssignVariables();
    }

    void OnLevelWasLoaded()
    {
        AssignVariables();
    }

    void AssignVariables()
    {
        _lerp = FindObjectOfType<LerpSolution>();
        _event = GetComponent<quizEvents>();
        _manager = FindObjectOfType<quizManager>();
        _sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        if (GameObject.Find("SFX") == null)
        {
            _sfx = GetComponent<AudioSource>();
        }
        try
        {
            _music = GameObject.Find("Music").GetComponent<AudioSource>();
        }
        catch
        {
            Debug.Log("Fancy resturaunt oh lala");
        }
        cam = FindObjectOfType<Camera>();
    }

    public void say(string message, float time = 2.8f) // 2.8f by default
    {
        message = message.Replace("{name}", PlayerPrefs.GetString("playerName"));

        if (script.ContainsKey(message))
        {
            script.Add(message + "6969lmao", time); // this is to stop a glitch happening if the dialouge has two of the same messages
        }
        else
        {
            script.Add(message, time);
        }
    }

    public void sayEvil(string message)
    {
        script.Add(message, 666);
    }

    public void choice(string choice1local, Action choice1aclocal, string choice2local, Action choice2aclocal,
    string choice3local, Action choice3aclocal, string choice4local, Action choice4aclocal)
    {
        choice1 = choice1local;
        choice2 = choice2local;
        choice3 = choice3local;
        choice4 = choice4local;
        choice1ac = choice1aclocal;
        choice2ac = choice2aclocal;
        choice3ac = choice3aclocal;
        choice4ac = choice4aclocal;
    }

    public void choiceTwo(string choice1local, Action choice1aclocal, string choice2local, Action choice2aclocal)
    {
        choice1 = choice1local;
        choice2 = choice2local;
        choice3 = "";
        choice4 = "";
        choice1ac = choice1aclocal;
        choice2ac = choice2aclocal;
        choice3ac = null;
        choice4ac = null;
    }

    public void oneChoice(string choice1local, Action choice1aclocal)
    {
        choice1 = choice1local;
        choice2 = "";
        choice3 = "";
        choice4 = "";
        choice1ac = choice1aclocal;
        choice2ac = null;
        choice3ac = null;
        choice4ac = null;
    }

    public IEnumerator startScript(bool noChoice = false)
    {
        TextMeshPro targetDialougeText = dialogueText;

        KeyValuePair<string, float> last = script.Last();

        bool isEvil = false;
        foreach (KeyValuePair<string, float> line in script)
        {
            if(line.Value == 666)
            {
                // Create a list with the different bloody texts along the wall, then we get the one with an index of 0, make that text equal line.Key, and then remove that text from the list, as well as maybe a camera shake or other effects you wanna add!
                isEvil = true;
                switch(_manager.currentQuizProjectState)
                {
                    case quizManager.quizProjectStates.doorEnding:
                        targetDialougeText = evilDoorTexts[0];
                        evilDoorTexts.RemoveAt(0);
                        break;
                    case quizManager.quizProjectStates.normalEnding:
                        targetDialougeText = evilEndingTexts[0];
                        evilEndingTexts.RemoveAt(0);
                        break;
                }
                CameraShake.Shake(0.2f, 0.4f);
                //script[line.Key] = 2; // Change this to suitable time
                // Maybe make the first text "Stay away from it" also be timed
            }
            //StartCoroutine(_glitchText.createGlitchedText(line.Key, dialogueText));
            if(line.Key.StartsWith("Event1234: "))
            {
                _event.eventManager(line.Key.Split(' ')[1], line.Value);
            }
            else if(line.Value != 4.2f && line.Value != 666)
            {
                targetDialougeText.text = line.Key;
                _sfx.PlayOneShot(click1, 4 * clickNoiseMultiplyer);
                yield return GeneralManager.waitForSeconds(line.Value);
            }
            else if(line.Key.StartsWith("{w="))
            {
                targetDialougeText.text = "";
                yield return GeneralManager.waitForSeconds(float.Parse(line.Key.Split('=')[1][0].ToString()));
            }
            else
            {
                if (line.Key.EndsWith("6969lmao")) targetDialougeText.text = line.Key.Replace("6969lmao", "");
                else targetDialougeText.text = line.Key;
                if (line.Value == 666) GetComponent<glitchText>().glitchGlitchedText(targetDialougeText);
                _sfx.PlayOneShot(click1, 4 * clickNoiseMultiplyer);
                if(!isEvil) yield return GeneralManager.waitForSeconds(1f);
                else
                yield return GeneralManager.waitForSeconds(2f);
                if(!line.Equals(last) && !isEvil)
                {
                    Coroutine showArrowEnum = StartCoroutine(showArrow());
                    while(!playerPressSpace) yield return new WaitForEndOfFrame();
                    if(showArrowEnum != null)StopCoroutine(showArrowEnum);
                }
                if(arrowEnum != null) StopCoroutine(arrowEnum);
                if (isFirstTime) tutorialText.text = "";
                LerpSolution.lerpSpriteColour(arrow, Color.clear, 3);
            }
        }
        script = new Dictionary<string, float>();
        if(!noChoice)
        {
            foreach (TextMeshPro dialogueOption in dialogueOptions)
            {
                dialogueOption.text = "";
                dialogueOption.color = Color.white;   
            }

            string[] choices = {choice1, choice2, choice3, choice4};

            int diaItteration = 0;
            for(int i = 0; i < 4; i++)
            {
                diaItteration++;
                if(choices[i] != "")
                {
                    dialogueOptions[i].text = $"{i + 1}. {choices[i]}";
                    dialogueOptions[i].color = Color.white;
                    if (dialogueOptions[i] == selectedTextFromPreviousQuestion)
                    {
                        StopCoroutine(lerpingFadingCouritine);
                    }

                    if (diaItteration == 3 && FindObjectOfType<Amelia_Restaurant_Cam>() != null)
                    {
                        FindObjectOfType<Amelia_Restaurant_Cam>().GoDownUnder();
                    }

                    _sfx.PlayOneShot(click2, 3.5f );
                    yield return GeneralManager.waitForSeconds(1);
                }
            }

            canPlayerChoose = true;

            if (isFirstTime && !hasSeenChoice)
            {
                Debug.Log("MAde tsdfsdfs!");
                yield return GeneralManager.waitForSeconds(8);
                tutorialText.text = "Press 1, 2, 3 or 4 to select an option from the left";
                LerpSolution.lerpTextColour(tutorialText, Color.black, 0.2f, Color.clear);
                hasSeenChoice = true;
            }
        }
    }

    IEnumerator showArrow()
    {
        yield return GeneralManager.waitForSeconds(1.5f);
        arrowEnum = StartCoroutine(_lerp.lerpSpriteColourEnum(arrow, Color.white, 1.4f));
        if (isFirstTime && !hasSeenProgress)
        {
            yield return GeneralManager.waitForSeconds(5);
            tutorialText.text = "Space/Enter";
            LerpSolution.lerpTextColour(tutorialText, Color.black, 0.2f, Color.clear);
            hasSeenProgress = true;
        }
    }

    string argQuestionOrder = "";
    int argIndex = 0;
    List<int> choiceNumbers = new List<int>();

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
        PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name} arg", argQuestionOrder);
        argIndex++;
    }

    void Update()
    {
        if (canPlayerChoose && Time.timeScale != 0)
        {
            if (isFirstTime)
            {
                if (!dialogueOptions[0].GetComponent<MeshRenderer>().IsVisibleFrom(FindObjectOfType<Camera>()))
                {
                    return;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (choice1ac == null) return;

                choiceNumbers.Add(1);

                choice1ac.Invoke();
                canPlayerChoose = false;
                AddArg(1);
                StartCoroutine(playerSelected(dialogueOptions[0]));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                if(choice2ac == null) return;

                choiceNumbers.Add(2);

                choice2ac.Invoke();
                canPlayerChoose = false;
                AddArg(2);
                StartCoroutine(playerSelected(dialogueOptions[1]));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                if(choice3ac == null) return;

                choiceNumbers.Add(3);

                choice3ac.Invoke();
                canPlayerChoose = false;
                AddArg(3);
                StartCoroutine(playerSelected(dialogueOptions[2]));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                if(choice4ac == null) return;

                choiceNumbers.Add(4);

                choice4ac.Invoke();
                canPlayerChoose = false;
                AddArg(4);
                StartCoroutine(playerSelected(dialogueOptions[3]));
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) && Time.timeScale != 0)
        {
            StartCoroutine(continueDialogue());
        }

        if (secondaryDialogueOptions.Length != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                secondaryDialogueOptions[i].color = dialogueOptions[i].color;
                secondaryDialogueOptions[i].text = dialogueOptions[i].text;
            }
        }
    }

    IEnumerator continueDialogue()
    {
        playerPressSpace = true;
        yield return new WaitForEndOfFrame();
        playerPressSpace = false;
    }

    IEnumerator playerSelected(TextMeshPro selectedOption)
    {

        if (isFirstTime) tutorialText.text = "";

        string selectedText = selectedOption.text;
        foreach(TextMeshPro dialogueOption in dialogueOptions)
        {
            if(dialogueOption != selectedOption)
            {
                dialogueOption.text = "";
            }
        }

        if (FindObjectOfType<Amelia_Restaurant_Cam>() != null)
        {
            FindObjectOfType<Amelia_Restaurant_Cam>().GoUpAbove();
        }

        yield return GeneralManager.waitForSeconds(2);
        if(selectedOption.text == selectedText)
        {
            selectedTextFromPreviousQuestion = selectedOption;
            lerpingFadingCouritine = StartCoroutine(LerpSolution.lerpTextColourEnum(selectedOption, Color.clear, 1));
        }
    }

    public void Finish()
    {
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
