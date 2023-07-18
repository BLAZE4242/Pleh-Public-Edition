using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using System;
using UnityEngine.UI;

public class quizQuizing : MonoBehaviour
{
    [SerializeField] bool canBegin = false;

    [SerializeField] string nameOfArgProgram = "BIOSener.config";
    [SerializeField] GameObject door, walls, terrain, floor;
    [SerializeField] ParticleSystem particles;
    [SerializeField] PostProcessVolume _volume;
    [SerializeField] PostProcessProfile _profile;
    DRP _discordManager;
    quizManager _manager;
    AudioSource _music;
    quizDialogueManager _dialogueManager;
    bool hasStarted = false;

    #region story variables
    bool playerCameToSeeInterviewer;
    bool playerSawFromAdvert;
    #endregion

    void Start()
    {
        if (canBegin)
        {
            _dialogueManager = FindObjectOfType<quizDialogueManager>();
            SetScript();
        }
        else AssignVariables();
    }

    void OnLevelWasLoaded()
    {
        AssignVariables();
    }

    void AssignVariables()
    {
        _discordManager = FindObjectOfType<DRP>();
        _manager = FindObjectOfType<quizManager>();
        _music = GameObject.Find("Music").GetComponent<AudioSource>();
        _dialogueManager = GetComponent<quizDialogueManager>();
    }

    void StateStart()
    {
        Debug.Log("Entered state: quizing!");
        door.SetActive(false);
        walls.SetActive(true);
        terrain.SetActive(false);
        floor.SetActive(true);
        //particles.Stop();
        _volume.profile = _profile;
        if (PlayerPrefs.GetInt("glotchiness") != 7)
        {
            _music.enabled = true;
            SetScript();
            //question7();
        }
        else SetScriptEndGame();
    }

    void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && Application.isEditor) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SetScript()
    {
        // Currently this script is for testing, please change it... (and make sure to be listening to music
        // when you're writing it ;)  )
        say("Oh, it's a person!");
        say("I usually never see anyone come to this area.");
        say("What brings you here?");
        choice("I thought I saw something here", playerThoughtSawSomething, "I wanted to see you", playerWantedToSex, "I was told to come here", playerWasToldToComeHere, "I was just wandering around", playerWasWonderingAround);
        startScript();
    }

    void SetScriptEndGame()
    {
        say("Alright, I'm back.");
        say("I've got the results!");
        say("Here they are:");
        say("{w=2}");
        say("Event1234: EndMusic", 0);
        say("You're a nice person.");
        say("You care so much about others.");
        say("And in return they care for you.");
        say("You have so many people who want to see you succeed and achieve your goals.");
        say("And though you may come across challenges...");
        say("Those people who want you to succeed can help you.");
        say("People love you.");
        say("And everything will be okay in the end.");
        say("Event1234: EndRiseCheck", 0);
        say("Everything will be okay.");
        startScript(true);
    }

    #region askPlayerWhyTheyCameHere

    void playerThoughtSawSomething()
    {
        say("Around here? No!");
        say("There's nothing interesting here that you'd want to see.");
        say("Perhaps you were mistaken, or fooled by the light."); // Maybe get rid of this because this doesn't contribute to the plot
        say("Some who come here say the light can make you hallucinate.");
        askPlayerIfLikeTheView();
    }

    void playerWantedToSex()
    {
        playerCameToSeeInterviewer = true;
        say("There aren't many people who live around this area, you must have travelled for quite a while to get here.");
        say("I don't actually remember how long it took me to get here.");
        say("All I remember is that I existed and then...I was here.");
        say("It's almost like it's my purpose to be here, so that's why I live here really.");
        askPlayerIfLikeTheView();
    }

    void playerWasToldToComeHere()
    {
        say("That's weird.");
        say("Who told you?");
        choice("An advertisement", playerWasToldToComeByAdvert, "Something in my head", playerWasToldToComeBySomethingInHead, "My friend", playerWasToldToComeByFriend, "It doesn't matter, you wouldn't get it", playerDismissesFirstTime);
        startScript();
    }

    void playerWasWonderingAround()
    {
        say("What were you doing wondering around here?");
        say("People come here for very specific reasons, usually when they need something.");
        say("Maybe you needed to see me?");
        say("Maybe what's happening now, us conversing, is needing to be happening."); //Grammar :yikes:
        say("...");
        say("That was a bit...cheesy.");
        say("But I think you understand what I'm trying to say.");
        askPlayerIfLikeTheView();
    }

    #endregion

    #region someone told player to come here
    void playerWasToldToComeByAdvert()
    {
        say("Oh, so you actually saw those?");
        say("I did that as a joke actually, didn't realise anyone would see them.");
        say("At the time I thought that maybe people would just enjoy the atmosphere of this place.");
        say("That was dumb.");
        say("This place is nice though.");
        askPlayerIfLikeTheView();
    }

    void playerWasToldToComeBySomethingInHead()
    {
        say("In your head...?");
        say("You don't mean...");
        say("...");
        say("Event1234: InYourHead", 0.5f);
        say("It doesn't talk to you too, does it?"); // Include secret here
        startScript(true);
    }

    void playerWasToldToComeByFriend()
    {
        say("Perhaps I've talked to your friend before.");
        say("Hundreds of people have come to me over the years, and all of them have something unique about themselves.");
        say("I wonder what's so special about you..?");
        say("I suppose we'll find out.");
        askPlayerIfLikeTheView();
    }

    void playerDismissesFirstTime()
    {
        say("...");
        say("I guess I can understand if it's private.");
        say("I'm sorry, I just wasn't expecting to be dismissed.");
        say("I mean, you have every right to not answer, I just think it feels kind of weird.");
        say("Nevermind, I'm being rude now.");
        say("Anyways...");
        askPlayerIfLikeTheView();
    }

    #endregion

    #region ask player about view

    public void askPlayerIfLikeTheView()
    {
        say("I like to spend my time gazing into the bright pink sky.");
        say("Sometimes I see clouds or even rain, but it's very rare.");
        say("Do you like this place?");
        choice("I like the atmosphere", playerLikesAtmosphere, "It's too bright", playerThinksTooBright, "I'd rather be at home", playerRatherBeHome, "I hate pink", playerHatesPink);
        startScript();
    }

    void playerLikesAtmosphere()
    {
        say("Of course you do, it's so pretty!");
        say("Shame there's no rain today, it makes everything look so shiny and beautiful.");
        informPlayerOfQuiz();
    }

    void playerThinksTooBright()
    {
        if (FindObjectOfType<GeneralManager>().GetComponentInChildren<Image>() != null && FindObjectOfType<GeneralManager>().GetComponentInChildren<Image>().color.a < 1)
        {
            say("You know you can just turn your brightness down right?");
            say($"Well it's not in settings, I think there's an option in {nameOfArgProgram} if you have that insta-", 0.4f);
            say("Event1234: FadeMusic", 0);
            say("Wait...");
            say("You... already turned the brightness down...");
            say("...");
            say("Why... why are you doing this?");
            say("You don't want to go through with this.");
            say("Please, you don't want to know the truth.");
            say("PLEASE STOP NOW");
            say("Event1234: BrightnessEnd", 0);
            say("turnAway-n0w");
            startScript(true);
        }
        else
        {
            say("You know you can just turn your brightness down right?");
            say($"Well it's not in settings, I think there's an option in {nameOfArgProgram} if you have that insta-", 0.4f);
            say("Actually, nevermind.");
            say("You'll just have to deal with it.");
            say("That's why I always bring sunglasses wherever I go!");
            informPlayerOfQuiz();
        }
    }

    void playerRatherBeHome()
    {
        say("Well, sometimes it's good to go outside and do stuff!");
        say("What would you do if you were home anyway?");
        bool recording = false;
        System.Diagnostics.Process[] localByName = System.Diagnostics.Process.GetProcessesByName("obs64");
        if (localByName.Length > 0) recording = true;
        else localByName = System.Diagnostics.Process.GetProcessesByName("obs32");
        if (localByName.Length > 0) recording = true;
        else localByName = System.Diagnostics.Process.GetProcessesByName("Streamlabs OBS");
        if (localByName.Length > 0) recording = true;

        if (recording)
        {
            say("Record your screen playing video games with the intention of who knows what?");
            say("What are you doing anyways, streaming? Doing a let's play?");
            say("Are there others watching right now?");
            say("I don't really like being on camera so I'd appreciate you turning off the software thanks...");
            say("But I'm not going to force you, I'd just appreciate you being considerate.");
        }
        else say("Sit in front of a screen playing video games?");
        informPlayerOfQuiz();
    }

    void playerHatesPink()
    {
        say("I almost find that offensive, pink is my favourite colour.");
        say("You'd better get used to it though, the nearest town without pink skies is quite a while away.");
        say("Even when I close my eyes, I can still see pink.");
        informPlayerOfQuiz();
    }

    #endregion

    #region tell player about the quiz

    void informPlayerOfQuiz()
    {
        say("Anyways, I'm sure you're wondering who I actually am.");
        say("I don't really have a name, but I'm known for my famously accurate personality quiz!");
        say("I know that's a weird thing to be known for, but people love it.");
        say("Since you're here, would you like me to ask you some questions? I'll tell you things that not even you knew about yourself!");
        choice("I'll take the quiz!", playerWantsToDoQuiz, "Why can't I just do one online?", playerWantsToDoOnlineQuiz, "I don't think that I'll enjoy this", playerDoesntThinkTheyllEnjoy, "Why should I trust you?", playerDoesntTrust);
        startScript();
    }

    void playerWantsToDoQuiz()
    {
        say("Wonderful! I knew you'd be enthusiastic.");
        explainationAndQuestion1();
    }

    void playerWantsToDoOnlineQuiz()
    {
        say("Because those quizzes are full of lies!");
        say("This one is the real deal.");
        say("News flash, you can't determine your sexuality from your favourite ice cream flavour!"); //chocolate = lesbian | -Liam
        explainationAndQuestion1();
    }

    void playerDoesntThinkTheyllEnjoy()
    {
        say("Not with that attitude you won't!");
        say("I have never done this quiz on a person who wasn't shocked by their results.");
        say("If you give me your full, you will really enjoy it! Trust me!");
        explainationAndQuestion1();
    }

    void playerDoesntTrust()
    {
        say("Trust me?");
        say("What am I gonna do, hurt you?");
        say("You overthink things sometimes, this is just a personality quiz! There's nothing that I'm hiding from you!");
        explainationAndQuestion1();
    }

    #endregion

    #region The actual quiz

    void explainationAndQuestion1()
    {
        say("Now, how about we start things off with something simple!");
        say("This one should be easy, what's your name?"); // Polly says hi
        choice("Sarah", wrongName, "Preston", wrongName, PlayerPrefs.GetString("playerName"), question2, "Eden", wrongName);
        startScript();
    }

    void wrongName()
    {
        say("Event1234: FakeName01", 0);
        say("...");
        say("Is that really your name?");
        choice("No", playerConfessToWrongName, "No", playerConfessToWrongName, "No", playerConfessToWrongName, "No", playerConfessToWrongName); //Changed them all from 'No' to other versions of no, you can change back if you want | -Liam | thanks liam I did :) | -Blaze
        startScript();
    }

    void playerConfessToWrongName()
    {
        say("Event1234: FakeName02", 0);
        say("...");
        say("What is your real name?");
        choice(PlayerPrefs.GetString("playerName"), question2, PlayerPrefs.GetString("playerName"), question2, PlayerPrefs.GetString("playerName"), question2, PlayerPrefs.GetString("playerName"), question2); // once again, added unique punctuation, you can change back if you want | -Liam | I feel bad for getting rid of it but, oh well... | -Blaze
        startScript();
    }

    void question2()
    {
        say("Event1234: FakeName03", 0);
        say(PlayerPrefs.GetString("playerName") + ", that's a nice name.");
        say("Alright, next question!");
        say("You're at a party and don't know anyone there. What do you do?");
        choice("Try to make some new friends", question3, "Leave the party", question3, "Be antisocial", question3, "Call an uninvited friend to come", question3);
        startScript();
    }

    void question3()
    {
        say("Interesting.");
        say("How many hours of sleep do you get on average?");
        choice("1-5", question4, "6-10", question4, "11-24", question4, "0", question4);
        startScript();
    }

    void question4()
    {
        say("I didn't expect that from you, I would have guessed something different.");
        say("How does your body smell?");
        choice("Like I need a shower", question5, "Like roses", question5, "Like a rotting corpse", question5, "How does this question relate to my personality?", question4playerQuestioning);
        startScript();
    }

    void question4playerQuestioning()
    {
        say("Are you doubting my quiz?");
        say("I promise you, every question that I ask you is relevant.");
        question5();
    }

    void question5()
    {
        say("Now, answer carefully.");
        say("If you look behind you right now, what do you see?");
        choice("A wall", question6, "A door", playerSeesDoor, "A window", question6, "You're making up questions now, aren't you?", playerAsksIfMakingUpQuestions); //you forgot to make the 4th answer link up to playerAsksIfMakingUpQuestions, dw i gotchu | -Liam
        startScript();
    }

    void playerAsksIfMakingUpQuestions()
    {
        say("These questions were written ages ago by me.");
        say("I didn't just steal them off the internet.");
        say("Now, answer carefully:");
        say("If you look behind you right now, what do you see?");
        choice("A wall", question6, "A door", playerSeesDoor, "A window", question6, "A poster", question6);
        startScript();
    }

    void playerSeesDoor()
    {
        say("What colour is the door?");
        choice("White", playerAnswersDoorWrong, "#e610a2", playerAnswersDoorWrong, "Grey", playerAnswersDoorWrong, "Brown", playerColourDoorCorrect);
        startScript();
    }

    void playerColourDoorCorrect()
    {
        say("And this door...");
        say("What kind of door is it?");
        choice("Just a wooden door", playerAnswersDoorCorrectly, "A sliding door", playerAnswersDoorWrong, "Tinted glass", playerAnswersDoorWrong, "It's just a door, why do you need to know this?", playerQuestionsDoor);
        startScript();
    }

    void playerQuestionsDoor()
    {
        say("I ask the questions here, not you.");
        say("What kind of door is it?");
        choice("Just a wooden door", playerAnswersDoorCorrectly, "A sliding door", playerAnswersDoorWrong, "Tinted glass", playerAnswersDoorWrong, "Just a normal door...", playerFedUpWithDoor);
        startScript();
    }

    void playerAnswersDoorCorrectly()
    {
        _manager.currentQuizProjectState = quizManager.quizProjectStates.doorEnding;
    }

    void playerFedUpWithDoor()
    {
        say("...");
        say("I'm...");
        say("I'm sorry, I think I just lost my temper a little bit.");
        say("Let me just continue with the questions, we're almost done.");
        question6();
    }

    void playerAnswersDoorWrong()
    {
        say("Oh...");
        say("I thought...");
        say("Nevermind.");
        say("Anyways...");
        question6();
    }

    void question6()
    {
        say("I've still got some more questions for you.");
        say("What are you feeling now?");
        choice("Happy", question7, "Angry", question7, "Confusion", question7, "Sadness", question7);
        startScript();
    }

    public void question7()
    {
        say("This next question is a bit controversial.");
        say("I just want to say that I'll respect your opinion, no matter what you say.");
        say("What do you think is the meaning of life?");
        choice("Fame", finishedQuiz, "Companionship", finishedQuiz, "Happiness", finishedQuiz, "Creation", finishedQuiz);
        startScript();
    }

    void finishedQuiz()
    {
        _manager.currentQuizProjectState = quizManager.quizProjectStates.normalEnding;
    }

    #endregion

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

    public void CheckStateStarted(quizManager _manager)
    {
        if (!hasStarted)
        {
            StateStart();
            StateUpdate();
            hasStarted = true;
        }
        else
        {
            StateUpdate();
        }
    }
}