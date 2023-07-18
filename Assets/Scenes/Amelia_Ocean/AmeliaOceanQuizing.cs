using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AmeliaOceanQuizing : MonoBehaviour
{
    quizDialogueManager diaManager;

    private void Awake()
    {
        diaManager = GetComponent<quizDialogueManager>();
    }

    #region #1
    public void SetScript()
    {
        say("{w=4}");
        //say("Event1234: OceanFinish", 2);
        say("...");
        say("Wow...");
        say("It's so nice and peaceful out here...");
        say("Away from it all.");
        say("Away from the world.");
        
        say("{w=2}");

        say("If I jumped into the ocean right now and drowned...");
        say("The waves would take me away.");
        say("No one would ever know where I went, or even cared.");
        say("Everyone would just forget about me, like I never existed.");

        say("{w=2}");

        say("At least, it feels like that sometimes.");
        say("Sometimes I just want to drown out all the background noise.");
        say("Sometimes I want to be away from everything.");
        say("Do you feel that too {name}?");

        choice("I do", Ido, "Not really", NotReally, "I don't know what I feel", IdontKnowWhatIfeel, "You're not usually like this", YoureNotUsuallyLikeThis);
        startScript();
    }

    void Ido()
    {
        say("It's normal, I suppose.");
        say("We're only human, we all feel the same thing, go through the same struggles.");
        say("Feel the same pain...");
        lostInThoughts();
    }

    void NotReally()
    {
        say("Maybe it's just me.");
        say("Maybe I'm alone and the entire world is against me.");
        say("Maybe I just hate the world that much.");
        say("Maybe I just hate myself that much...");
        lostInThoughts();
    }

    void IdontKnowWhatIfeel()
    {
        say("Emotions are hard to read sometimes.");
        say("When it's hard to collect my thoughts, I come down here.");
        say("Clear my head.");
        say("Forget about the outside.");
        lostInThoughts();
    }

    void YoureNotUsuallyLikeThis()
    {
        say("...");
        say("We haven't talked in so long {name}.");
        say("So much has changed between us...");
        say("You're beginning to see a new side of me I guess.");
        lostInThoughts();
    }

    #endregion
    #region #2
    void lostInThoughts()
    {
        say("{w=2}");

        say("You know, I was just thinking the other day about us.");
        say("I know I've said it before but, it really has been so long {name}.");
        say("I was just remembering...");
        say("That game you made.");
        say("You showed me a while ago.");
        say("It was in this whole new world... nothing like I'd ever seen before.");
        say("It was a bright pink sky...");
        say("And there were large scaled mountains...");
        say("And small little white dots floating through the air...");

        say("{w=2}");

        say("And then someone talked to me.");
        say("This person... they were very...");

        choice("Collected", quizEnd, "Mysterious", quizEnd, "Odd", quizEnd, "Judgmental", quizEnd);
        startScript();
    }
    #endregion
    #region #3
    void quizEnd()
    {
        say("Yes...");
        say("I remember it now...");
        say("I remember it all.");
        say("They asked me questions.");
        say("I thought it was funny.");
        say("I thought it was peaceful, just like now.");
        say("You wrote that... character... right?");
        say("They felt so familiar.");
        say("{w=2}");
        say("And the ending...");

        choice("What did you think of the ending?", meaningless, "It's okay if you don't want to talk about it", ItsOkayIfYouDontWantToTalkAboutIt, "The one where you fall through the ground?", IthoughtAboutYouWhenWritingIt, "I don't remember it exactly", IdontRememberItExactly);
        startScript();
    }

    void ItsOkayIfYouDontWantToTalkAboutIt()
    {
        say("No, I really do...");
        meaningless();
    }

    void IthoughtAboutYouWhenWritingIt()
    {
        say("No... I don't think that's what happened at all.");
        meaningless();
    }

    void IdontRememberItExactly()
    {
        say("I remember parts of it...");
        meaningless();
    }
    #endregion
    #region #4
    void meaningless()
    {
        say("I don't think it made the most amount of sense to me.");
        say("I think it was meant to be something symbolic.");
        say("The person came back with the quiz results.");
        say("They said... I don't remember exactly.");
        say("Something really philosophical.");
        say("And then I rose into the air.");
        say("I was way up high.");
        say("It was like I was an angel, one with the sky.");

        say("{w=2}");

        say("I think I'm making it sound a lot more surreal than it actually was...");
        say("I miss those days...");
        say("We would hang out so much more often.");
        say("And I'm tired of pretending that never happened.");
        say("I feel so much colder now...");
        say("Everything is so much more meaningless, empty...");
        say("Shallow.");
        say("Is this what they call depression?");
        say("Am I crazy?");
        say("Why did you even come here?");

        oneChoice("I'm sorry", ImSorry);
        startScript();
    }

    void ImSorry()
    {
        say("{w=2}");

        oneChoice("I have to go.", leave);
        startScript();
    }

    void leave()
    {
        diaManager.Finish();
        say("{w=2}");
        say("...");
        say("Event1234: OceanFinish", 2);
        say("Please don't leave me again.");
        startScript(true);
    }
    #endregion

    #region engine shitttttt

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

    void oneChoice(string choice1, Action choice1ac)
    {
        diaManager.oneChoice(choice1, choice1ac);
    }

    void startScript(bool noChoice = false)
    {
        StartCoroutine(diaManager.startScript(noChoice));
    }
    #endregion
}
