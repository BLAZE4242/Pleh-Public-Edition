using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kino;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.Events;
using DigitalRuby.RainMaker;
using System.IO;

public class Transcript : MonoBehaviour
{
    public enum transcriptStates {idle, notIdle }
    public transcriptStates currentState;
    [SerializeField] int TranscriptNumber = 0;
    [SerializeField] bool shouldUnmute = true;
    [SerializeField] bool shouldRevertPP = true;
    [Header("Collect Scene")]
    [SerializeField] SpriteRenderer blackScreen;
    [SerializeField] Animator anim;
    [SerializeField] Image fileScreen;
    [SerializeField] AudioSource[] sourcesToDissable;
    [SerializeField] AudioClip glitchClip, staticClip;
    [SerializeField] TextMeshPro directoryText;
    [SerializeField] Transform playerSpawnAfter;
    [SerializeField] GameObject[] objectsAfter;
    [SerializeField] UnityEvent eventForAfter;
    PlayerController controller;
    playerMove controller2d;
    Dictionary<PostProcessVolume, bool> allVolumesActive = new Dictionary<PostProcessVolume, bool>();
    AudioSource source;
    Coroutine TranscriptIdleEnum;
    

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Gas.GetStat("STAT_TRANS"));

        controller = FindObjectOfType<PlayerController>();
        controller2d = FindObjectOfType<playerMove>();
        source = GetComponent<AudioSource>();
        defaultPos = transform.position;

        if (currentState == transcriptStates.idle)
        {
            TranscriptIdleEnum = StartCoroutine(TranscriptIdle());
        }

        if (source == null) source = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (currentState != transcriptStates.idle && TranscriptIdleEnum != null)
        {
            StopCoroutine(TranscriptIdleEnum);
        }
    }

    Vector3 defaultPos;
    Vector3 startPos;
    public IEnumerator TranscriptIdle()
    {
        startPos = transform.position;
        while (true)
        {
            LerpSolution.lerpPositionTime(transform, new Vector3(startPos.x, defaultPos.y + 0.2f, startPos.z), 2f);
            yield return GeneralManager.waitForSeconds(2f);
            LerpSolution.lerpPositionTime(transform, new Vector3(startPos.x, defaultPos.y - 0.2f, startPos.z), 2f);
            yield return GeneralManager.waitForSeconds(2f);
        }
    }

    public void Collect()
    {
        // play glitch effect and black screen, wait a second, slowly fade in transcript as ear piercing noise plays, then show file directory for a second (shorter if streamer mode) and glitch then return, maybe reset player pos unless null
        StartCoroutine(CollectEnum());
    }

    bool didCameraHaveGlitch = false;
    IEnumerator CollectEnum()
    {
        // make screen black
        blackScreen.color = Color.black;
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }

        foreach (SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.enabled = false;
        }

        if (controller != null)
        {
            controller.lockMovement = true;
            controller.lockLook = true;
        }

        // deactivate all post processing
        PostProcessVolume[] volumes = FindObjectsOfType<PostProcessVolume>();
        foreach (PostProcessVolume volume in volumes)
        {
            allVolumesActive.Add(volume, volume.isActiveAndEnabled);
            volume.enabled = false;
        }

        // dissable all audio
        foreach (AudioSource source in sourcesToDissable)
        {
            try { if (source.isPlaying) source.Pause(); }
            catch { foreach (music sources in GameObject.FindObjectsOfType<music>()) sources.GetComponent<AudioSource>().Pause(); }

        }

        foreach (RainScript rain in FindObjectsOfType<RainScript>())
        {
            foreach (AudioSource rainSource in rain.GetComponents<AudioSource>())
            {
                rainSource.Pause();
            }
        }

            // glitch effect
            DigitalGlitch digitalGlitch = FindObjectOfType<DigitalGlitch>();
        if (digitalGlitch == null)
        {
            Debug.Log("has!");
            digitalGlitch = controller.playerCamera.gameObject.AddComponent<DigitalGlitch>();
            didCameraHaveGlitch = false;
        }
        else
        {
            Debug.Log("Doesn't have");
            didCameraHaveGlitch = true;
        }

        if (anim != null) anim.enabled = false;
        digitalGlitch.intensity = 0.3f;
        source.PlayOneShot(glitchClip);

        yield return GeneralManager.waitForSeconds(glitchClip.length);

        digitalGlitch.intensity = 0;

        yield return GeneralManager.waitForSeconds(2);

        source.clip = staticClip;
        source.Play();
        LerpSolution.lerpVolume(source, 1, 0.1f, 0);

        // source.Play(); // for when glitch noise
        LerpSolution.lerpImageColourTime(fileScreen, Color.white, 5);

        yield return GeneralManager.waitForSeconds(5.5f);

        source.Stop();
        digitalGlitch.intensity = 0.3f;
        source.PlayOneShot(glitchClip);
        fileScreen.color = Color.clear;

        if (TranscriptNumber != 11)
        {

            string achName = "ACH_TRANS_" + TranscriptNumber;

            if (TranscriptNumber < 10)
            {
                achName = "ACH_TRANS_0" + TranscriptNumber;
            }

            if (!Gas.HasAchievement(achName))
            {
                Gas.SetStat("STAT_TRANS", Gas.GetStat("STAT_TRANS") + 1);
                Debug.Log("Success!");
            }
            Gas.EarnAchievement(achName);

            if (Gas.GetStat("STAT_TRANS") >= 10)
            {
                Gas.EarnAchievement("ACH_DETECTIVE");
                Debug.Log("sherlok holesdfsdf");
            }

            MoveFile();

            if(PlayerPrefs.GetInt("Streamer Mode") == 1 || PlayerPrefs.GetInt("Streamer Mode") == 2) directoryText.text = "*GameDirectory*/Pleh!/Transcripts/";
            else
            {
                string SecondDir = Application.dataPath;
                List<string> SecondDirOperation = new List<string>(SecondDir.Split('/'));
                SecondDirOperation.RemoveAt(SecondDirOperation.Count - 1);
                SecondDir = string.Join("/", SecondDirOperation);

                directoryText.text = SecondDir + "/Transcripts/";
            }
            Debug.Log(Application.dataPath);
        }
        else
        {
            directoryText.text = "most real thing ever";
        }

        yield return GeneralManager.waitForSeconds(glitchClip.length);

        digitalGlitch.intensity = 0;
        if (anim != null) anim.enabled = true;
        directoryText.text = "";

        yield return GeneralManager.waitForSeconds(2);

        if (shouldRevertPP)
        {
            foreach (KeyValuePair<PostProcessVolume, bool> dict in allVolumesActive)
            {
                dict.Key.enabled = dict.Value;
            }
        }

        if (shouldUnmute)
        {
            foreach (AudioSource source in sourcesToDissable)
            {
                try { source.Play(); }
                catch { foreach (music sources in FindObjectsOfType<music>()) sources.GetComponent<AudioSource>().Play(); }
            }
        }

        foreach (RainScript rain in FindObjectsOfType<RainScript>())
        {
            foreach (AudioSource rainSource in rain.GetComponents<AudioSource>())
            {
                rainSource.Play();
            }
        }

        blackScreen.color = Color.clear;

        if (controller != null)
        {
            controller.lockMovement = false;
            controller.lockLook = false;
        }

        eventForAfter.Invoke();

        if (playerSpawnAfter != null)
        {
            controller.TeleportPlayer(playerSpawnAfter.position);
            controller.transform.rotation = playerSpawnAfter.rotation;
        }

        foreach (GameObject go in objectsAfter)
        {
            go.SetActive(!go.activeInHierarchy);
        }

        if (!didCameraHaveGlitch) Destroy(digitalGlitch); 
    }

    void MoveFile()
    {
        if (Application.isEditor) return;
        string fileLocation = "";

        string SecondDir = Application.dataPath;
        List<string> SecondDirOperation = new List<string>(SecondDir.Split('/'));
        SecondDirOperation.RemoveAt(SecondDirOperation.Count - 1);
        SecondDir = string.Join("\\", SecondDirOperation);

        switch (TranscriptNumber)
        {
            case 1:
                fileLocation = SecondDir + "\\Pleh!_Data\\gamereader_additional.tech";
                break;
            case 2:
                fileLocation = SecondDir + "\\MonoBleedingEdge\\etc\\mono\\lab\\write.in";
                break;
            case 3:
                fileLocation = SecondDir + "\\Pleh!_Data\\Gl\\level.manager";
                break;
            case 4:
                fileLocation = SecondDir + "\\Pleh!_Data\\Resources\\raider_modified.ext";
                break;
            case 5:
                fileLocation = SecondDir + "\\MonoBleedingEdge\\EmbedRuntime\\dopple.da";
                break;
            case 6:
                fileLocation = SecondDir + "\\MonoBleedingEdge\\EmbedRuntime\\demo\\rawr.rawr";
                break;
            case 7:
                fileLocation = SecondDir + "\\Pleh!_Data\\care.fol";
                break;
            case 8:
                fileLocation = SecondDir + "\\Pleh!_Data\\Resources\\un";
                break;
            case 9:
                fileLocation = SecondDir + "\\MonoBleedingEdge\\etc\\mono\\4.5\\analyse.tio";
                break;
            case 10:
                fileLocation = SecondDir + "\\Pleh!_Data\\gamereader.tech";
                Debug.Log(fileLocation);
                break;
            default:
                Debug.Log("No file given");
                return;
        }

        string strNumber = TranscriptNumber.ToString();
        if (TranscriptNumber < 10) strNumber = "0" + TranscriptNumber.ToString();

        Directory.CreateDirectory(SecondDir + "\\Transcripts");
        File.Copy(fileLocation, SecondDir + $"\\Transcripts\\Transcript#{strNumber}.pdf", true);
    }
}
