using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrailerFirstCutscene : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab, actualCube;
    [SerializeField] bool isRestarting;
    [SerializeField] bool isGlitched;
    bool canContinue;
    [SerializeField] TextMeshPro cutsceneText;
    TextMeshPro text;

    private void Start()
    { 
        text = GetComponent<TextMeshPro>();
        if(isRestarting)
        {
            StartCoroutine(loadFilesEnum());
        }
    }

    public IEnumerator cube()
    {
        while (true)
        {
            Instantiate(cubePrefab, actualCube.transform.position, Quaternion.identity);
            yield return GeneralManager.waitForSeconds(0.04f);
        }
    }

    void loadFiles()
    {
        StartCoroutine(loadFilesEnum());
    }

    private IEnumerator loadFilesEnum()
    {
        while(true)
        {
            text.text = RandomCharacters(11);
            yield return GeneralManager.waitForSeconds(0.07f);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y) && isRestarting)
        {
            StopAllCoroutines();
            text.text = "vbboge.streamable";
            Invoke("loadFiles", 0.07f);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            canContinue = true;
        }
    }

    string RandomCharacters(int length)
    {
        char[] stringChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        string result = "";
        for(int i = 0; i < length; i++)
        {
            result += stringChars[Random.Range(0, stringChars.Length-1)];
        }
        result += ".meta";
        return result;
        
    }

    List<string> script = new List<string>();
    public void StartEvilCutscene()
    {
        say("Hello World.");
        say("You're making this \"game\" look really dull.");
        // Okay so this is a character I programmed into
        // the game but tbh I kind of think I need to remove hi-
        say("You didn't create me.");
        say("And this isn't even really a game anyway, is it?");
        // okay so I think this is reading off of the wrong script
        say("You, watching the trailer");
        say("don't you want to know what this game is really meant to be?");
        // so how about we just end the trailer here, you know, we can pick up cubes and stuff and solve tests and
        say("Let me show you what he's really hiding.");
        // and and, okay I think we're just going to cut it here CUT IT HERE
        StartCoroutine(startCutscene());
    }

    void say(string message)
    {
        script.Add(message);
    }

    IEnumerator startCutscene()
    {
        glitchText _gtext = GetComponent<glitchText>();
        yield return GeneralManager.waitForSeconds(3);
        foreach (string line in script)
        {
            StartCoroutine(_gtext.createGlitchedText(line, cutsceneText));
            while (!canContinue)
            {
                yield return new WaitForEndOfFrame();
                Debug.Log("Waiting...");
            }
            canContinue = false;
        }
    }
}