using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
// using Aura2API; // AURA HERE
using TMPro;

public class Amelia_Restaurant_Script : MonoBehaviour
{
    [SerializeField] PlayerController _controller;
    [Header("Reflection state")]
    [SerializeField] GameObject annoyingLight;
    [SerializeField] TextMeshPro Amelia, oldAmelia;
    // [SerializeField] AuraLight ameliaLight; // AURA HERE
    [SerializeField] Light ameliaLight; // AURA NOT HERE
    [SerializeField] AudioSource music, sfx;
    [SerializeField] AudioClip click1;
    quizDialogueManager diaManager;

    private void Start()
    {
        if (PlayerPrefs.GetInt("glotchiness") == 7)
        {
            StartCoroutine(ReflectionState());
            return;
        }

        FindObjectOfType<DRP>().RP("No, I didn't do this.");
        GeneralManager.SetSceneSave();
        Gas.EarnAchievement("ACH_STORY_2");

        diaManager = FindObjectOfType<quizDialogueManager>();
        switch (PlayerPrefs.GetInt("AmeliaRes_Save"))
        {
            case 1:
                ruok1();
                break;
            case 2:
                ruok2();
                break;
            case 3:
                TrustYou();
                break;
            default:
                SetScript();
                //TrustYou();
                break;

        }
    }

    IEnumerator ReflectionState()
    {
        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("New Messages");
        sceneOp.allowSceneActivation = false;

        _controller.GetComponent<PlayerController>().lockMovement = true;
        _controller.GetComponent<PlayerController>().lockLook = true;
        music.Stop();
        sfx.Stop();
        LerpSolution.LerpLightIntensity(ameliaLight, 0, 0.17f);
        annoyingLight.SetActive(false);
        oldAmelia.text = "";

        Amelia.text = "I need to move on from a lot more.";
        music.PlayOneShot(click1);
        yield return GeneralManager.waitForSeconds(4);
        Amelia.text = "He needed to move on, but he couldn't.";
        music.PlayOneShot(click1);
        yield return GeneralManager.waitForSeconds(4);
        sceneOp.allowSceneActivation = true;
    }

    void Save(int data)
    {
        if (data == 0) PlayerPrefs.DeleteKey("AmeliaRes_Save");
        else PlayerPrefs.SetInt("AmeliaRes_Save", data);
    }
    
    void SetScript()
    {
        //say("Event1234: LoadArgueRes", 0); //

        say("Hey! Thanks for meeting me here!");
        say("It's so nice here {name}, really cosy...");
        //say("Event1234: EndRes", 0); //
        say("Remember when our families would always come and eat here? Those were good times...");
        say("Anyway, how have you been?");
        choice("I've been good", BeenGood, "Same as always", SameAsAlways, "Fantastic", Fantastic, "I'm managing", Managing);
        startScript();
    }

    #region How have you been?
    void BeenGood()
    {
        say("That's good!");
        say("That's also the same answer you would always give me everytime I used to ask you.");
        say("Have you really never been sad once in your entire joyful life?");
        say("Cheery old {name}, always feeling happy!");
        TalkAboutSchool();
    }

    void SameAsAlways()
    {
        say("Oh yeah? How's that?");
        say("Like, good, bad?");
        say("You always seem cheery but, I dunno sometimes.");
        TalkAboutSchool();
    }

    void Fantastic()
    {
        say("Yay! That's great!");
        say("Step aside people, {name} is coming through!");
        say("Look at you, being all optimistic and witty!");
        say("Wish I was as cool as you...");
        TalkAboutSchool();
    }

    void Managing()
    {
        say("Aren't we all?");
        say("I might as well be holding on for dear life.");
        say("Fake it 'till you make it, aint that right?");
        say("Works very well in my experience, trust me.");
        TalkAboutSchool();
    }
    #endregion

    #region School
    void TalkAboutSchool()
    {
        say("On a more specific note... what about school?");
        say("How's that treating you?");
        choice("Pretty well", PrettyWell, "Not so good", NotSoGood, "It's boring", ItsBoring, "I'm going to drop out", ImGoingToDropOut);
        startScript();
    }

    void PrettyWell()
    {
        say("That's also good!");
        say("You used to hate school, glad to see your attitude is changing!");
        say("Only got a few more years to go!");
        say("Time flies by too quickly, isn't that right?");
        AskIfHungry();
    }

    void NotSoGood()
    {
        say("Oh, sorry to hear that.");
        say("Bad grades? Not paying attention?");
        say("If that's the case, you know you're always free to meet with me after school!");
        say("I'd be very happy to help you study! You can count on me!");
        AskIfHungry();
    }

    void ItsBoring()
    {
        say("Tough luck bud.");
        say("Still got a few years, hope you'll survive...");
        say("If not, guess you'll just end up on the streets, rotting away with no education.");
        say("Use that as motivation to not find school boring!");
        AskIfHungry();
    }

    void ImGoingToDropOut()
    {
        say("You can't do that!");
        say("What about me? Who am I going to annoy every lunchtime?");
        say("I mean, we don't really hang out as much anymore but...");
        say("I'd still miss you...");
        say("Just don't go... okay?");
        AskIfHungry();
    }
    #endregion

    #region Hungry?
    void AskIfHungry()
    {
        say("Soooo...");
        say("You hungry?");
        choice("Starving", Starving, "I can wait a bit", IcanWaitAbit, "Depends if you are", DependsIfYouAre, "The food here will probably suck anyway", TheFoodHereWillProbablySuckAnyway);
        startScript();
    }

    void Starving()
    {
        say("Oh okay, we should order soon then.");
        say("We'll wait for the waiters to come around.");
        say("Wait for the waiters... heh.");
        say("Sounds like a tongue twister!");
        say("Wouldn't you agree {name}?");
        ruok1();
    }

    void IcanWaitAbit()
    {
        say("Well I can't!");
        say("Trust me, by the time we place our orders and the food will take a while to come...");
        say("You'll be practically drooling!");
        say("The food is also really good here, don't you remember it?");
        ruok1();
    }

    void DependsIfYouAre()
    {
        say("Well maybe, but it shouldn't matter if I am!");
        say("Don't let my appetite determine your appetite.");
        say("Stand up for yourself! Be your own person who won't let other people step over you!");
        say("Come on, say it with me: I matter! I matter!");
        ruok1();
    }

    void TheFoodHereWillProbablySuckAnyway()
    {
        say("What the hell are you talking about?");
        say("Are you not the same {name} I've known for years?");
        say("We've been here so many times, you love this place!");
        say("You didn't just hit your head really hard on the way here and forgot everything...");
        say("...did you?");
        ruok1();
    }
    #endregion

    #region ruok 1

    void ruok1()
    {
        Save(1);

        say("...");
        say("Well um...");
        say("Haha...");
        say("...");
        say("You really have changed a lot {name}.");
        say("You're all quiet and keep to yourself now.");
        say("Are you okay?");
        choice("I'm okay", ImOkay, "Sorry, just tired", SorryJustTired, "Don't worry about me", DontWorryAboutMe, "I'm not okay.", Crash);
        startScript();
    }

    void ImOkay()
    {
        say("You always say that though.");
        say("I don't know what to believe...");
        ruok2();
    }

    void SorryJustTired()
    {
        say("Like, a physical \"not enough sleep\" tiredness?");
        say("Or a mental tiredness?");
        ruok2();
    }

    void DontWorryAboutMe()
    {
        say("No, it's my duty as your friend to make sure you're okay.");
        say("I'm allowed to be worried about you, please let me.");
        ruok2();
    }

    #endregion

    #region ruok 2

    void ruok2()
    {
        Save(2);

        say("Are you really sure you're okay?");
        choice("Yes! I'm really fine!", TrustYou, "You don't have to worry about me.", TrustYou, "Actually you're right, I need help.", Crash, "Please stop doing this...", Crash);
        startScript();
    }

    void TrustYou()
    {
        Save(3);

        say("...");
        say("But...");
        say("...");
        say("Okay, fine.");
        say("I trust you.");
        choice("You have nothing to worry about.", IcareForYou, "Please tell me everything will be okay...", Crash, "I want to die.", Crash, "LETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUTLETMEOUT", Crash);
        startScript();
    }

    void IcareForYou()
    {
        diaManager.Finish();
        Save(0);
        say("Event1234: LoadArgueRes", 0);

        say("I know...");
        say("It's just...");
        say("I care for you, yknow?");
        say("And as your friend, sometimes I can get really worried...");
        say("But you would tell me if something was wrong wouldn't you?");
        say("Wouldn't you, {name}.", 1f);
        say("Event1234: EndRes", 0);
        startScript(true);
    }

    #endregion

    #region engine stuff

    void say(string message, float time = 4.2f)
    {
        diaManager.say(message, time);
    }

    void sayEvil(string message)
    {
        diaManager.sayEvil(message);

    }

    void choice(string choice1local, Action choice1aclocal, string choice2local, Action choice2aclocal, string choice3local, Action choice3aclocal, string choice4local, Action choice4aclocal)
    {
        diaManager.choice(choice1local, choice1aclocal, choice2local, choice2aclocal, choice3local, choice3aclocal, choice4local, choice4aclocal);
    }

    void startScript(bool noChoice = false)
    {
        StartCoroutine(diaManager.startScript(noChoice));
    }
    #endregion

    void Crash()
    {
        Application.Quit();
    }

    public void Deload()
    {
        SceneManager.MoveGameObjectToScene(_controller.gameObject, SceneManager.GetSceneByName("DevArgue"));
        _controller.TeleportPlayer(_controller.transform.position + new Vector3(-20, 20, 0));
        SceneManager.UnloadSceneAsync("Amelia_Restaurant");
    }
}
