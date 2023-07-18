using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hub_therapyManager : MonoBehaviour
{
    [SerializeField] Transform sitPos;
    quizDialogueManager diaManager;
    PlayerController controller;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
        diaManager = GetComponent<quizDialogueManager>();
    }

    public void SitDown()
    {
        controller.lockMovement = true;
        controller.lockGravity = true;
        controller.checkForMovement = false;
        controller.TeleportPlayer(sitPos.position);
        controller.playerCamera.GetComponent<Camera>().fieldOfView = 45;
        BeginTherapy();
        //Continue();
    }

    void BeginTherapy()
    {
        say("{w=2}");
        say("So, the week is coming to an end.");
        say("Did you do anything important this week? Set any goals?");
        choice("I hung out with a friend", Friend, "I went on a walk", Walk, "I worked on Pleh!", Pleh, "I didn't get up to much", NotMuch);
        startScript();
    }

    void Friend()
    {
        say("That's great to hear!");
        say("Keeping a good social life while maintaining some \"alone time\" creates a very healthy lifestyle.");
        say("Was it this Benji guy you're friends with?");
        choice2("Yes", FriendYes, "Did I tell you about Amelia?", FriendAmelia);
        startScript();
    }

    void FriendYes()
    {
        say("Oh course, I remember you talking about him a while ago.");
        say("That's good to hear you are still in touch.");
        say("Maybe you should schedule to meet with him some other time this week?");

        ControlStress();
    }

    void FriendAmelia()
    {
        say("Of course I remember Amelia.");
        say("You two are very close.");
        say("It's always good to have close friends you can trust.");

        ControlStress();
    }

    void Walk()
    {
        say("That's very good!");
        say("Going outside is very healthy compared to being indoors for days on end.");
        say("Would you say you are satisfied with the amount of time you spent outdoors compared to indoors?");
        choice2("I'd say so", WalkAgree, "I could have gone on a few more walks", WalkMore);
        startScript();
    }

    void WalkAgree()
    {
        say("That's very good to hear!");
        say("Make sure to go on long walks as well, not just a walk outside to take out the trash.");

        ControlStress();
    }

    void WalkMore()
    {
        say("That's nothing to worry about!");
        say("You have an entire new week now to fufill your goals!");
        say("Try to set a goal, like a daily walk, or 5 walks for the week.");

        ControlStress();
    }

    void Pleh()
    {
        say("Yes, your game, no? The one with the astronaut?");
        say("You bring it up at least once a session.");
        say("Would you say you spend too much time working on the game?");
        choice2("I don't.", PlehDont, "I don't work on it enough!", PlehWorkEnough);
        startScript();
    }

    void PlehDont()
    {
        say("That's good.");
        say("This game sounds like a very healthy habit of yours.");
        say("Just remember it's never a good thing to be addicted to anything!");

        ControlStress();
    }

    void PlehWorkEnough()
    {
        say("How about we set that as one of your goals for the week!");
        say("It sounds like your overall goal is to finish the game, you can't do that without hard work!");
        say("You need to work on the game more in order to finish it sooner!");
        say("Just don't overwork yourself, that's a bad habit to get into.");

        ControlStress();
    }

    void NotMuch()
    {
        say("Well that's okay sometimes.");
        say("Sometimes we're too tired to do anything, we need a break from things.");
        say("I think you should try to do something this week though!");
        choice2("I'll try that", NotMuchTry, "I wasn't tired, I just procastinated", NotMuchProcastination);
        startScript();
    }

    void NotMuchTry()
    {
        say("I'm glad!");
        say("You should write down some goals for the next week.");
        say("You could go out for dinner, do some homework, read a book.");
        say("There are many productive fun things you can plan for this new week!");

        ControlStress();
    }

    void NotMuchProcastination()
    {
        say("That's okay too, we all procastinate sometimes.");
        say("Whenever I get home from work, I get nothing done, just sit on the couch and watch T.V");
        say("I find that setting reminders and alarms to get up and do something is a really good strategy against procastination.");

        ControlStress();
    }

    void ControlStress()
    {
        say("Now, remember what we talked about last week?");
        say("Controlling your stress.");
        say("Would you say you've been in any stressful situations in the past week?");
        choice2("Not really", NotReally, "I have been", Continue);
        startScript();
    }

    void NotReally()
    {
        say("That's good.");
        Continue();
    }

    void Continue()
    {
        say("When under a stressful situation there are techniques we can use to calm ourselves down.");
        say("Let's try this simple meditation exercise.");
        say("Close your eyes.");
        say("Event1234: CloseEyes", 0);
        say("Notice your surroundings disappear.");
        say("Everything turns into blackness.");
        say("All your worries and distractions are gone.");
        say("All that's left is the emptiness.");
        say("The calmness.");
        say("But sometimes it's scary to stay in the same place for too long.");
        say("Try imagining a new place.");
        say("Actually imagine it, don't imagine a place you've been before.");
        say("Make up some mysterious world.");

        say("Event1234: WhiteVoid", 0);
        say("{w=2}");
        say("Event1234: WhiteVoidLook", 0);
        say("Now it's just you in this new world.");
        say("Alone with your thoughts.");
        say("I want you to reach deep inside them.");
        say("Find a confession, a memory.");
        say("Anything to hold onto.");

        say("Event1234: WhiteVoidProgress", 0.3f);
        say("Consider what you're feeling right now.");

        say("Event1234: WhiteVoidProgress&", 0.3f);
        say("Cold? Hot?");

        say("Event1234: WhiteVoidProgress&&", 0.3f);
        say("Do you feel complete?");

        say("Event1234: WhiteVoidProgress&&&", 0.3f);
        say("Focus on your breathing.");

        say("Event1234: WhiteVoidProgress&&&&", 0.3f);
        say("Breath in slowly...");

        say("Event1234: WhiteVoidProgress&&&&&", 0.3f);
        say("And out slower...");

        say("Event1234: WhiteVoidProgress&&&&&&", 0.3f);
        say("In...");

        say("Event1234: WhiteVoidProgress&&&&&&&", 0.3f);
        say("And out...");

        say("Event1234: WhiteVoidProgress&&&&&&&&", 0.3f);
        say("Focus on the repetitiveness of your breathing.");

        say("Event1234: WhiteVoidProgress&&&&&&&&&", 0.3f);
        say("You are safe here.");

        say("Event1234: WhiteVoidProgress&&&&&&&&&&", 0.3f);
        say("Everything is fine.");

        say("Event1234: WhiteVoidProgress&&&&&&&&&&&", 0.3f);
        say("All your worries can be fixed.");

        say("Event1234: WhiteVoidProgresss&&&&&&&&&&&&", 0.3f);
        say("You are okay.");

        say("Now open your eyes.");

        say("Event1234: WhiteVoidEnd", 0);
        say("{w=4}");

        say("Event1234: WhiteVoidBack", 0);
        say("{w=1}");
        say("Open your eyes {name}.");
        say("{w=0.4}");
        say("Event1234: WhiteVoidRedProgress", 0.3f);
        say("What are you doing {name} open your eyes!");

        say("Event1234: WhiteVoidRedProgress&", 0.3f);
        say("Don't listen to it!");

        say("Event1234: WhiteVoidRedProgress&&", 0.3f);
        say("Don't let it take you!");

        say("Event1234: WhiteVoidRedProgress&&&", 0.3f);
        say("You can escape it!");

        say("Event1234: WhiteVoidRedProgress&&&&", 0.3f);
        say("Fight back!");

        say("Event1234: WhiteVoidEndFr", 0);
        say("COME BACK {name}!");

        startScript(true);
    }

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

    void choice2(string choice1local, Action choice1aclocal, string choice2local, Action choice2aclocal)
    {
        diaManager.choiceTwo(choice1local, choice1aclocal, choice2local, choice2aclocal);
    }

    void startScript(bool noChoice = false)
    {
        StartCoroutine(diaManager.startScript(noChoice));
    }
    #endregion
}
