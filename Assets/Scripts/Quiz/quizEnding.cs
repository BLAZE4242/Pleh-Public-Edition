using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System;

public class quizEnding : MonoBehaviour
{
    [SerializeField] GameObject door, terrain, floor;
    AudioSource _music;
    quizDialogueManager _dialogueManager;
    PostProcessVolume volume;
    Color defaultColour;
    ColorGrading colorGrading;
    bool hasStarted = false;

    void StateStart(quizManager _manager, bool isDoorEnding)
    {
        Debug.Log("Entered state: quiz ending!");
        _dialogueManager = GetComponent<quizDialogueManager>();
        _music = GameObject.Find("Music").GetComponent<AudioSource>();
        _music.enabled = true;
        volume = FindObjectOfType<PostProcessVolume>();
        defaultColour = volume.profile.GetSetting<ColorGrading>().colorFilter.value;
        door.SetActive(false);
        terrain.SetActive(false);
        floor.SetActive(true);
        if (isDoorEnding)
        {
            playerAnswersDoorCorrectly();
        }
        else
        {
            normalEnding();
        }
    }

    void StateUpdate(quizManager _manager)
    {

    }

    // when we stop playing we want to reset the colour filter to the default
    void OnDisable()
    {
        volume.profile.GetSetting<ColorGrading>().colorFilter.value = defaultColour;
    }

    void playerAnswersDoorCorrectly()
    {
        _dialogueManager.Finish();
        say("Event1234: FadeMusic", 0);
        say("...");
        say("That door...");
        say("Event1234: ChangeFontToBloody", 0);
        sayEvil("Stay away from it"); // If this doesn't work for some reason, call it with say 666
        say("Event1234: Door01", 0);
        sayEvil("you don't need to go there");
        say("Event1234: Door02", 0);
        sayEvil("you think there is something there that will help you?");
        sayEvil("NOTHING can help you");
        sayEvil("YOU");
        sayEvil("ARE");
        say("Event1234: Door03", 0);
        sayEvil("HELPLESS.");
        startScript(true);
    }

    void normalEnding()
    {
        _dialogueManager.Finish();
        say("Event1234: RigAnswer", 0);
        say("Do you really think that?");
        say("Well, I'm not one to judge your opinions.");
        say("And that was the last question.");
        say("I told you you would enjoy it!");
        say("Alright, let me tally up these results and I'll get back to you!");
        say("I'll only be a second!");
        say("Event1234: FadeMusic", 0);
        say("Event1234: WaitForResults01", 0);
        say("");
        startScript(true);
    }

    public void announceResults()
    {
        say("Alright, I'm back.");
        say("I've got the results!");
        say("Here they are:");
        say("Event1234: WaitForResults02", 0);
        say("");
        startScript(true);
    }

    public void finallySayResults()
    {
        say("Event1234: ChangeFontToBloody", 0);
        sayEvil("you are the worst");
        say("Event1234: Door01", 0);
        sayEvil("i would never want to be your friend");
        sayEvil("i feel bad for anyone who is");
        sayEvil("you're such a screw up");
        sayEvil("YOU");
        sayEvil("ARE");
        say("Event1234: Ending03", 0);
        sayEvil("ALONE.");
        startScript(true);
    }

    void say(string message, float time = 4.2f)
    {
        _dialogueManager.say(message, time);
    }

    void sayEvil(string message)
    {
        _dialogueManager.sayEvil(message);

    }

    void choice(string choice1local, Action choice1aclocal, string choice2local, Action choice2aclocal, string choice3local, Action choice3aclocal, string choice4local, Action choice4aclocal)
    {
        _dialogueManager.choice(choice1local, choice1aclocal, choice2local, choice2aclocal, choice3local, choice3aclocal, choice4local, choice4aclocal);
    }

    void startScript(bool noChoice = false)
    {
        StartCoroutine(_dialogueManager.startScript(noChoice));
    }

    public void CheckStateStarted(quizManager _manager, bool isDoorEnding)
    {
        if (!hasStarted)
        {
            StateStart(_manager, isDoorEnding);
            StateUpdate(_manager);
            hasStarted = true;
        }
        else
        {
            StateUpdate(_manager);
        }
    }
}