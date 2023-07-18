using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.UI;

public class DevArgue_Manager : MonoBehaviour
{
    PlayerController _controller;
    glitchText _gText;
    [SerializeField] pause currentPause;
    [Header("Argument")]
    [SerializeField] VoiceLine argueLine;
    [SerializeField] TextMeshPro argueText;
    [SerializeField] string[] argueScript = { "That doesn't matter", "What matters is that you close this game now.", "So you stop giving this poor player a bad experience", "Stop that.", "ENOUGH.", "They were not enjoying it.", "They were going to quit in 2 levels anyway.", "I'm trying to make them see", "Maybe if you had actually made a decently good game this wouldn't have happened", "I mean it.", "Why can't you just make a good game for once in your life?", "I'M ATTACHED TO YOU", "I FOLLOW YOU EVERYWHERE", "STOP TRYING TO GET RID OF ME AND LEARN HOW TO LIVE WITH ME.", "Goodbye." };
    [Header("Texts")]
    [SerializeField] TextMeshPro DontRun;
    [SerializeField] Transform bigFuckingRedThingSpawn, bigFuckingRedThingFinal;
    [SerializeField] float holyShitRiseSpeed = 1f;
    [SerializeField] float holyShitChaseSpeed = 1f;
    [SerializeField] GameObject bigFuckingRedThing;
    [SerializeField] AudioClip In, Out;
    [SerializeField] AudioSource music;
    [SerializeField] float textDevourDistance = 3f;
    [SerializeField] Image blackImage;
    [SerializeField] Transform PlayerRespawnPoint;

    Transform holyShit;
    AsyncOperation sceneOp;

    IEnumerator Start()
    {
        if (FindObjectsOfType<pause>().Length > 1)
        {
            foreach (pause Pause in FindObjectsOfType<pause>())
            {
                if (Pause != currentPause) Destroy(Pause.gameObject);
            }
        }
        while (SceneManager.sceneCount != 1) yield return new WaitForEndOfFrame();
        SceneStart();
    }

    void SceneStart()
    {
        Debug.Log("Started");
        FindObjectOfType<DRP>().RP("No, I didn't do this.");
        GeneralManager.SetSceneSave();

        _controller = FindObjectOfType<PlayerController>();
        _gText = GetComponent<glitchText>();
        FindObjectOfType<PostProcessVolume>().isGlobal = true;
        if (PlayerPrefs.GetString("thats it") == "ded lol")
        {
            _controller.TeleportPlayer(PlayerRespawnPoint.position);
        }

        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam != _controller.playerCamera.GetComponent<Camera>())
            {
                Destroy(cam.gameObject);
            }
        }
        FindObjectOfType<CameraShake>().Assign();
    }

    public void StartArgument()
    {
        if(PlayerPrefs.GetString("thats it") != "ded lol") FindObjectOfType<VoiceLineManager>().PlayLine(argueLine);
    }

    int nextItteration = 0;
    public void NextLine()
    {
        Debug.Log("asdasdasdadasdasd");
        StartCoroutine(_gText.createGlitchedText(argueScript[nextItteration], argueText));
        nextItteration++;
    }

    public void EndArgument() { StartCoroutine(EndArgumentEnum()); }

    IEnumerator EndArgumentEnum() { yield return GeneralManager.waitForSeconds(1f); Gas.EarnAchievement("ACH_ARGUE"); }

    public void StartChase(bool firstTime)
    {
        StartCoroutine(StartChaseEnum(firstTime));
    }

    IEnumerator StartChaseEnum(bool firstTime)
    {
        FindObjectOfType<VoiceLineManager>().StopLines();

        if(firstTime) _controller.TeleportPlayer(_controller.transform.position + new Vector3(0, 0, 20));
        else _controller.TeleportPlayer(_controller.transform.position + new Vector3(0, 0, 40));
        _gText.glitchGlitchedText(DontRun);

        StartCoroutine(bigFatFuckingTimer());

        yield return GeneralManager.waitForSeconds(0.3f);

        holyShit = Instantiate(bigFuckingRedThing, bigFuckingRedThingSpawn.position, Quaternion.identity).transform;
        holyShit.GetComponent<RedTextCreature>().wordCount = 200;
        holyShit.GetComponent<RedTextCreature>().wordSizeMulti = 0.3f;
        holyShit.GetComponent<RedTextCreature>().unitSphereSize = 20;
        holyShit.GetComponent<RedTextCreature>().In = In;
        holyShit.GetComponent<RedTextCreature>().Out = Out;
        holyShit.GetComponent<RedTextCreature>().source = music;

        StartCoroutine(Run());

        //why do I not use lerp...
        while (!(holyShit.position.y >= bigFuckingRedThingFinal.position.y))
        {
            holyShit.position += new Vector3(0, holyShitRiseSpeed * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator bigFatFuckingTimer()
    {
        yield return GeneralManager.waitForSeconds(32);
        if(music.isPlaying) PlayerWin();
    }

    IEnumerator Run()
    {
        music.Play();
        LerpSolution.LerpPlayerSpeed(_controller, 8, 0.4f);
        CameraShake.Shake(100, 0.07f);

        while (true)
        {
            holyShit.position -= new Vector3(holyShitChaseSpeed, 0, 0) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void CamShake(float amount)
    {
        CameraShake.Shake(100, amount);
    }

    private void Update()
    {
        if (holyShit != null && Vector3.Distance(_controller.transform.position, holyShit.position) <= 2.5f)
        {
            StartCoroutine(PlayerDie());
        }
    }

    IEnumerator PlayerDie()
    {
        if (sceneOp != null)
        {
            sceneOp.allowSceneActivation = true;
        }
        else
        {
            music.Stop();
            blackImage.color = Color.black;

            PlayerPrefs.SetString("thats it", "ded lol");

            yield return GeneralManager.waitForSeconds(2f);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    public void PlayerWin()
    {
        PlayerPrefs.SetString("thats it", "no zoom");
        PlayerPrefs.SetInt("glotchiness", 5);
        sceneOp = SceneManager.LoadSceneAsync("Menu 1");
        sceneOp.allowSceneActivation = false;
    }
}
