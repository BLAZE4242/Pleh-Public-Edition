using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Mum : MonoBehaviour
{
    [SerializeField] AudioClip toiletFlush, sinkOn;
    [SerializeField] Animator SinkAnim;
    
    public enum dayCycle { breakfast, plehMorn, lunch, plehArvo, dinner, plehNight, bed, death }
    [Header("Misc")]
    [SerializeField] GameObject TSPdoor;
    [SerializeField] Material tvStatic;
    [Header("Cycle")]
    [SerializeField] dayCycle currentTask = dayCycle.plehNight;
    [SerializeField] int dayCount = 1;
    [Space]
    [SerializeField] MeshRenderer[] Monitor;
    [SerializeField] Material itt0, itt1, itt2, itt3;
    [SerializeField] TextMeshPro studyText;
    [SerializeField] TextMeshPro BedroomText;
    [SerializeField] TextMeshPro LivingroomText;
    [SerializeField] GameObject couch;
    [SerializeField] Transform playerWakePos;
    [SerializeField] Image blackScreen;
    [SerializeField] GameObject[] Breakfast, Lunch, Dinner;
    [SerializeField] CollisionManager doorTrigger;
    [SerializeField] GameObject table;
    [SerializeField] GameObject doorAtTable;
    [SerializeField] GameObject distraction;
    [SerializeField] AudioClip click1, clickReverb;
    [Header("Craziness insues")]
    [SerializeField] GameObject day3;
    [SerializeField] GameObject[] day4;
    [SerializeField] GameObject day5;
    PlayerController controller;
    [Space]
    [Header("Death")]
    [SerializeField] MeshRenderer computerScreen;
    [SerializeField] Material deathScreen;
    [SerializeField] AudioClip deathMusic;
    [SerializeField] AudioClip loopMusic;
    [SerializeField] TextMeshPro[] DeathTexts;
    [SerializeField] AudioClip gateSlam;
    [SerializeField] PostProcessVolume hubVolume, redVolume;
    VoiceLineManager VLManager;
    [SerializeField] AudioSource AmeliaBg;
    [SerializeField] List<VoiceLine> AmeliaLines = new List<VoiceLine>();
    [SerializeField] DoorBehavior door1;
    glitchText gText;

    bool toiletFlushed = false;
    bool sinkSinked = false;

    private void Start()
    {
        foreach (music Music in FindObjectsOfType<music>())
        {
            Destroy(Music.gameObject);
        }
        FindObjectOfType<DRP>().RP("IT did this.");
        GeneralManager.SetSceneSave();
        controller = FindObjectOfType<PlayerController>();
        dayItterations();
        VLManager = FindObjectOfType<VoiceLineManager>();
        gText = FindObjectOfType<glitchText>();
    }

    int TSPcount = 0;
    public void OnTSPclick()
    {
        TSPcount++;
        if (TSPcount >= 5)
        {
            TSPdoor.SetActive(false);
            GetComponent<AudioSource>().PlayOneShot(click1);
        }
    }

    public void OnTvPress(MeshRenderer renderer)
    {
        if (renderer.materials[1] != tvStatic) GetComponent<AudioSource>().PlayOneShot(click1);

        var tvMats = renderer.materials;
        tvMats[1] = tvStatic;
        renderer.materials = tvMats;
    }

    #region loop
    public void OnWorkPleh()
    {
        var dayMats = Monitor[0].materials;

        switch (currentTask)
        {
            case dayCycle.plehMorn:
                currentTask++;
                if(dayCount < 4)
                {
                    dayMats[2] = itt1;
                    ChangeText(studyText, "I'm quite hungry, I should have lunch.");
                    LivingroomText.text = "Click to eat.";
                    foreach (GameObject lunches in Lunch)
                    {
                        lunches.SetActive(true);
                    }
                }
                else if(dayCount == 4)
                {
                    currentTask = dayCycle.plehArvo;
                    OnWorkPleh();
                    return;
                }
                else
                {
                    currentTask = dayCycle.plehNight;
                    OnWorkPleh();
                    return;
                }
                if (dayCount == 3) // Door at dinner event
                {
                    foreach (MeshRenderer renderer in table.GetComponentsInChildren<MeshRenderer>())
                    {
                        if (renderer.GetComponent<InteractableObject>() == null)
                        {
                            renderer.enabled = !renderer.enabled;
                        }
                    }

                    doorAtTable.SetActive(true);
                }
                break;
            case dayCycle.plehArvo:
                currentTask++;
                dayMats[2] = itt2;
                ChangeText(studyText, "It's starting to get dark, I should have some dinner.");
                LivingroomText.text = "Click to eat.";
                foreach (GameObject dinners in Dinner)
                {
                    dinners.SetActive(true);
                }
                break;
            case dayCycle.plehNight:
                currentTask++;
                dayMats[2] = itt3;
                ChangeText(studyText, "When did it get so late? I should go to bed.");
                LivingroomText.text = "";
                BedroomText.text = "Click to go to sleep.";
                controller.lockMovement = false;

                break;
        }

        foreach (MeshRenderer monitor in Monitor)
        {
            monitor.materials = dayMats;
        }
    }

    public void DoorEvent() { if(!table.GetComponentInChildren<MeshRenderer>().enabled)StartCoroutine(DoorEventEnum()); }

    IEnumerator DoorEventEnum()
    {
        doorTrigger.enabled = false;
        yield return GeneralManager.waitForSeconds(0.2f);
        doorAtTable.SetActive(false);
        foreach (MeshRenderer renderer in table.GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.GetComponent<InteractableObject>() == null)
            {
                renderer.enabled = !renderer.enabled;
            }
        }
    }

    public void OnSleep()
    {
        if (currentTask == dayCycle.bed)
        {
            currentTask = 0;
            StartCoroutine(EndDay());
        }
    }

    IEnumerator EndDay()
    {
        GetComponent<AudioSource>().volume = 0;
        foreach (CollisionManager manager in FindObjectsOfType<CollisionManager>())
        {
            manager.ResetIfTriggered();
        }
        controller.lockLook = true;
        controller.lockMovement = true;
        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(click1);
        blackScreen.color = Color.black;
        yield return GeneralManager.waitForSeconds(1);

        dayCount++;
        dayItterations();

        controller.lockLook = false;
        controller.lockMovement = false;
        blackScreen.color = Color.clear;
        ChangeText(BedroomText, "I had a good sleep. I should have breakfast.");
        LivingroomText.text = "Click to eat.";
        studyText.text = "";
        foreach (GameObject breakfasts in Breakfast)
        {
            breakfasts.SetActive(true);
        }

        foreach (MeshRenderer monitor in Monitor)
        {
            var mats = monitor.materials;
            mats[2] = itt0;
            monitor.materials = mats;
        }

        toiletFlushed = false;
        sinkSinked = false;
        if(dayCount < 7) GetComponent<AudioSource>().volume = 1;
    }

    void dayItterations()
    {
        TSPcount = 0;
        switch (dayCount)
        {
            case 3:
                day3.SetActive(true);
                break;
            case 4:
                foreach (GameObject blocker in day4)
                {
                    blocker.SetActive(true);
                }
                couch.SetActive(false);
                break;
            case 5:
                controller.lockMovement = false;
                controller.TeleportPlayer(controller.transform.position - new Vector3(0, 10, 10));
                currentTask = dayCycle.breakfast;
                break;
            case var expression when (dayCount > 5 && dayCount != 9):
                distraction.SetActive(true);
                currentTask = dayCycle.plehMorn;
                break;
            case 9:
                currentTask = dayCycle.death;

                break;
        }
    }

    public void OnEat()
    {
        switch (currentTask)
        {
            case dayCycle.breakfast:
                currentTask++;
                foreach (GameObject breakfasts in Breakfast)
                {
                    breakfasts.SetActive(false);
                }
                ChangeText(LivingroomText, "I should start my day by working on Pleh!");
                BedroomText.text = "";
                studyText.text = "Click to work on Pleh!";
                break;
            case dayCycle.lunch:
                currentTask++;
                foreach (GameObject lunches in Lunch)
                {
                    lunches.SetActive(false);
                }
                ChangeText(LivingroomText, "I should keep working on Pleh!");
                studyText.text = "Click to work on Pleh!";
                break;
            case dayCycle.dinner:
                currentTask++;
                foreach (GameObject dinners in Dinner)
                {
                    dinners.SetActive(false);
                }
                ChangeText(LivingroomText, "I'll put in a few more hours working on Pleh!");
                studyText.text = "Click to work on Pleh!";
                break;
        }
    }
    #endregion

    public void OnDeath()
    {
        if (currentTask == dayCycle.death)
        {
            StartCoroutine(StartDeath());
        }
    }

    string[] DeathScript = { "I can't do this anymore.", "I need help."};
    IEnumerator StartDeath()
    {
        controller.lockMovement = true;

        var screenMaterials = computerScreen.materials;
        screenMaterials[2] = deathScreen;
        computerScreen.materials = screenMaterials;

        StartCoroutine(StartMusic());

        for (int i = 0; i < 2; i++)
        {
            yield return GeneralManager.waitForSeconds(3);
            ChangeText(DeathTexts[i], DeathScript[i]);
        }
        yield return GeneralManager.waitForSeconds(0.4f);
        controller.lockMovement = false;
        controller.ChangeSpeed(1.7f);
        // open door
        door1.OpenDoor(0.08f);
        door1.GetComponentInChildren<BoxCollider>().enabled = false;
        LerpSolution.LerpPPweight(hubVolume, 0, 0.3f);
        LerpSolution.LerpPPweight(redVolume, 1, 0.3f);
        StartCoroutine(checkForRed());
    }

    IEnumerator StartMusic()
    {

        GetComponent<AudioSource>().volume = 1;
        GetComponent<AudioSource>().clip = deathMusic;
        GetComponent<AudioSource>().Play();

        yield return GeneralManager.waitForSeconds(deathMusic.length);

        GetComponent<AudioSource>().clip = loopMusic;
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = true;
    }

    IEnumerator checkForRed()
    {
        while (true)
        {
            foreach (GameObject text in GameObject.FindGameObjectsWithTag("Exit"))
            {
                if (Vector3.Distance(text.transform.position, controller.transform.position) < 7f)
                {

                    StartCoroutine(gText.createGlitchedText("No Escape", text.GetComponent<TextMeshPro>()));
                    text.transform.tag = "Untagged";
                }
            }
            yield return GeneralManager.waitForSeconds(0.6f);
        }
    }

    bool followText = false;
    string textText;
    public void AmeliaLine(GameObject AmeliaText)
    {
        StartCoroutine(AmeliaLineEnum(AmeliaText));
    }

    private IEnumerator AmeliaLineEnum(GameObject AmeliaText)
    {
        AmeliaBg.Stop();
        AmeliaBg.Play();
        StartCoroutine(FollowAmeliaText(AmeliaText));
        StartCoroutine(ShowAmeliaText(AmeliaText));

        yield return GeneralManager.waitForSeconds(3);

        VLManager.PlayLine(AmeliaLines[0]);
        AmeliaLines.RemoveAt(0);

        yield return GeneralManager.waitForSeconds(1f);

        GetComponent<AudioSource>().PlayOneShot(gateSlam);
        Transform gate = AmeliaText.GetComponentsInChildren<Transform>()[1];

        gate.gameObject.SetActive(true);
        LerpSolution.lerpPositionTime(gate, gate.position - new Vector3(0, 5), 0.3f);
    }

    IEnumerator FollowAmeliaText(GameObject AmeliaText)
    {
        followText = true;
        AmeliaText.SetActive(true);
        textText = AmeliaText.GetComponent<TextMeshPro>().text;
        AmeliaText.GetComponent<TextMeshPro>().text = "";
        float difference = AmeliaText.transform.position.x - controller.transform.position.x;
        while (followText)
        {
            AmeliaText.transform.position = new Vector3(controller.transform.position.x + difference, AmeliaText.transform.position.y, AmeliaText.transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ShowAmeliaText(GameObject AmeliaText)
    {
        yield return GeneralManager.waitForSeconds(3);
        followText = false;
        AmeliaText.GetComponent<TextMeshPro>().text = textText;
        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(clickReverb, 5);
    }

    AsyncOperation sceneOp;
    public void LoadHub()
    {
        sceneOp = SceneManager.LoadSceneAsync("hub_main");
        sceneOp.allowSceneActivation = false;
    }

    public void EndSequence()
    {
        StartCoroutine(EndSequenceEnum());
    }

    private IEnumerator EndSequenceEnum()
    {
        blackScreen.color = Color.black;
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(click1);
        PlayerPrefs.SetInt("glotchiness", 6);
        yield return GeneralManager.waitForSeconds(0.4f);
        if (sceneOp != null) sceneOp.allowSceneActivation = true;
        else SceneManager.LoadScene("hub_main");
    }

    private void ChangeText(TextMeshPro tmp, string text)
    {
        tmp.text = text;
        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(click1);
    }

    #region Misc
    public void OnToilet()
    {
        if (toiletFlushed) return;
        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(toiletFlush);
        Gas.EarnAchievement("ACH_BATHROOM");
        toiletFlushed = true;
    }

    public void OnSink()
    {
        if (!toiletFlushed || sinkSinked) return;
        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(sinkOn);
        SinkAnim.SetTrigger("Turn on");
        Gas.EarnAchievement("ACH_HYGIENE");
        sinkSinked = true;
    }

    public void OnWTDC()
    {
        Gas.EarnAchievement("ACH_WTDC");
        Debug.LogWarning("Achievement earned!");
    }
    #endregion
}
