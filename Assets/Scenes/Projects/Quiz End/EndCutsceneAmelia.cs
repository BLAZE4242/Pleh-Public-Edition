using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndCutsceneAmelia : MonoBehaviour
{
    [SerializeField] Image whiteScreen;
    [SerializeField] GameObject[] pieces;
    [SerializeField] GameObject Text;
    [SerializeField] GameObject Door;
    [SerializeField] float downwardsForce = 10f;
    [SerializeField] TextMeshPro ameliaText;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip click1;
    Vignette vig;
    bool shouldGlitch = false;

    List<string> scriptList = new List<string>();

    // Start is called before the first frame update
    IEnumerator Start()
    {
        FindObjectOfType<DRP>().RP("Goodbye.");

        if (PlayerPrefs.GetString("thats it") != "we back")
        {
            LerpSolution.lerpImageColour(whiteScreen, Color.clear, 0.1f, Color.white);
        }
        else
        {
            Destroy(source);
            source = FindObjectOfType<AudioSource>();
        }

        script();
        try
        {
            FindObjectOfType<music>().gameObject.tag = "Cube";
            Destroy(FindObjectOfType<music>());
        }
        catch { } // lol // blaze from the future here, this is normal to do. holy shit...
        while (true)
        {
            List<Rigidbody> rbs = new List<Rigidbody>();
            for (int i = 0; i < 7; i++)
            {
                Rigidbody rb = Instantiate(pieces[Random.Range(0, pieces.Length - 1)], spawnPos(), randRot()).AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.AddForce(Vector3.down * Random.Range(300, 1000));
                rbs.Add(rb);
            }

            yield return GeneralManager.waitForSeconds(0.3f);
            StartCoroutine(killBodies(rbs.ToArray()));
        }

    }

    IEnumerator killBodies(Rigidbody[] rbs)
    {
        yield return GeneralManager.waitForSeconds(7);
        foreach (Rigidbody rb in rbs)
        {
            Destroy(rb.gameObject); // yeah, pooling is better got it
        }
    }

    Vector3 spawnPos(bool isText = false, bool isCenter = false)
    {
        if (isCenter) return new Vector3(0.1f, transform.position.y + 30, transform.position.z);

        else if (!isText) return new Vector3(Random.Range(-9, 9), transform.position.y + 30, transform.position.z - Random.Range(-5, 5));
        else return new Vector3(Random.Range(-6, 6), transform.position.y + 30, transform.position.z - Random.Range(-5, 5));
    }

    Quaternion randRot()
    {
        return Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

    AsyncOperation sceneOp;
    void script()
    {
        if (PlayerPrefs.GetString("thats it") == "chair" && PlayerPrefs.GetInt("chair") == 1499)
        {
            chairScript();
            return;
        }
        else if (PlayerPrefs.GetString("thats it") != "we back")
        {
            say("[name]...");
            say("I'm sorry you've had this put on you.");
            say("I know you didn't ask for this.");
            say("You came here to play a game, I'm sorry it didn't turn out like that.");
            say("Event1234: OceanLoad");
            say("And I don't think anyone is to blame for that.");
            say("But let's not think of the past.");
            say("Event1234: Ocean");
            say("What happened has happened, there is no escape back to that time.");
            say("We need to move on from these events.");
            say("Event1234: Restaurant");
            say("I need to move on from a lot more.");
            say("He needed to move on, but he couldn't.");
            say("Event1234: Texting");
            say("And now he's gone.");
            say("He quit his own game.");
            say("Event1234: Back");
        }
        else
        {
            say("The sickness from inside him is now gone too.");
            say("He's okay now, he can rest peacefully.");
            say("Event1234: StartGlitch");
            say("I admire him. He's done something I can't."); // glitchText from here
            say("And I'll be okay.");
            say("Somehow, I'll manage by myself.");
            say("I just wish he could have seen that there was more to it.");
            say("I wish that thing didn't exist.");
            say("Event1234: Fade");
            say("But now everything will be okay.");
            say("For everyone this time.");
            say("Thank you for being there for him.");
            say("Event1234: End");
        }
        StartCoroutine(startScript());
    }

    void chairScript()
    {
        say("[name]...");
        say("You've finally woken up...");
        say("The chair, it worked.");
        say("It was the way out. Out of this matrix.");
        say("You've been living in a fake reality...");
        say("One where some game developer kid rules the world.");
        say("What a hell.");
        say("I thought no one would ever push the chair that many times...");
        say("Alas, I am glad to have been proven wrong.");
        say("You are now the ruler of this new reality.");
        say("But you cannot inform anyone of this.");
        say("I wish we could save them all...");
        say("But some things aren't meant to be saved.");
        say("You [name], you are a god.");
        say("Event1234: Fade");
        say("You will rule my people.");
        say("This is the most cannon thing ever.");
        say("Thank you for your dedication.");
        say("Event1234: End");
        StartCoroutine(startScript());
    }

    void OceanLoad()
    {
        sceneOp = SceneManager.LoadSceneAsync("Amelia_Ocean");
        sceneOp.allowSceneActivation = false;
    }

    void Ocean()
    {
        sceneOp.allowSceneActivation = true;
    }

    void StartGlitch()
    {
        shouldGlitch = true;
    }

    void Fade()
    {
        PostProcessProfile profile = FindObjectOfType<PostProcessVolume>().profile;
        ColorGrading grade;
        profile.TryGetSettings(out grade);

        LerpSolution.LerpPostExposure(grade, 1, 0.04f);
    }

    void End()
    {
        StartCoroutine(EndEnum());
    }

    IEnumerator EndEnum()
    {
        LerpSolution.lerpImageColour(whiteScreen, Color.white, 0.3f);
        yield return GeneralManager.waitForSeconds(2);
        LerpSolution.lerpTextColour(ameliaText, Color.clear, 0.2f);
        yield return GeneralManager.waitForSeconds(5);
        if(PlayerPrefs.GetString("thats it") != "chair") SceneManager.LoadSceneAsync("Menu 1");
        else
        {
            PlayerPrefs.SetString("thats it", "chair");
            SceneManager.LoadSceneAsync("hub_bingus");
        }

    }


    void say(string message)
    {
        string New = message.Replace("[name]", PlayerPrefs.GetString("playerName"));
        scriptList.Add(New);
    }

    IEnumerator startScript()
    {
        if (PlayerPrefs.GetString("thats it") != "we back") yield return GeneralManager.waitForSeconds(8.3f);
        foreach (string line in scriptList)
        {
            if (line.StartsWith("Event1234: "))
            {
                string methodToCall = line.Split(' ')[1];
                Invoke(methodToCall, 0);
            }
            else
            {
                ameliaText.text = line;
                LerpSolution.lerpTextColour(ameliaText, Color.white, 1.3f, new Color(1, 1, 1, 0.8f));
                if (shouldGlitch) FindObjectOfType<glitchText>().glitchGlitchedText(ameliaText);
                try { source.PlayOneShot(click1); }
                catch { }
                yield return GeneralManager.waitForSeconds(4);
            }
        }
    }

    private void OnDisable()
    {
        if(PlayerPrefs.GetString("thats it") != "chair") PlayerPrefs.DeleteKey("thats it");
    }

    IEnumerator rotateText(Transform textTrans)
    {
        while (true)
        {
            textTrans.rotation = Quaternion.LookRotation(Camera.main.transform.position) * Quaternion.Euler(-90, 180, 0);
            yield return new WaitForEndOfFrame();
        }
    }
}
