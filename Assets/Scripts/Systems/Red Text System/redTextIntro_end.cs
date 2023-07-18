using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class redTextIntro_end : MonoBehaviour
{
    [SerializeField] bool beginAtChoice = false;

    [SerializeField] GameObject[] script;
    [SerializeField] GameObject[] options;
    [SerializeField] AudioClip optionClick;
    AudioSource source;
    RedTextNarrative _narrative;
    PlayerController player;

    [SerializeField] AudioHighPassFilter highPass;
    AudioSource musicSource;
    public bool canIncreaseMusic = false;
    [SerializeField] Transform trigger;
    [SerializeField] PostProcessVolume defaultVolume;
    [SerializeField] PostProcessVolume redVolume;
    [SerializeField] AudioClip pianoSfx;

    [Header("Voice Lines")]
    [SerializeField] VoiceLine redText_intro_01;
    [SerializeField] VoiceLine redText_intro_02;
    [SerializeField] VoiceLine redText_intro_03;
    [SerializeField] VoiceLine redText_intro_04;
    [SerializeField] VoiceLine redText_intro_05;
    VoiceLineManager _VLManager;

    [Header("Other stuff for ending")]
    [SerializeField] LayerMask wallLayer;
    [SerializeField] ReflectionProbe redRoomProbe;
    [SerializeField] Image blackScreen;
    glitchText gText;
    AsyncOperation sceneOp;

    bool canChoose = false;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        _narrative = FindObjectOfType<RedTextNarrative>();
        player = FindObjectOfType<PlayerController>();
        _VLManager = FindObjectOfType<VoiceLineManager>();
        gText = FindObjectOfType<glitchText>();

        if(beginAtChoice) StartCoroutine(startSequence());
    }

    private void Update()
    {
        if (canIncreaseMusic)
        {
            highPass.cutoffFrequency = Mathf.Lerp(10, 6764, Vector3.Distance(player.transform.position, trigger.position) / 100);
            Debug.Log(Vector3.Distance(player.transform.position, trigger.position) / 100);
        }

        if (canChoose && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)))
        {
            StartCoroutine(playerChose(0, 1));
        }
        else if (canChoose && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)))

        {
            StartCoroutine(playerChose(1, 0));
        }
    }

    public void teleportPlayer()
    {
        player.TeleportPlayer(player.transform.position + new Vector3(0, 0, 20));

        StartCoroutine(startSequence());
    }

    IEnumerator startSequence()
    {
        musicSource = highPass.GetComponent<AudioSource>();
        musicSource.enabled = false;
        musicSource.clip = null;
        musicSource.playOnAwake = false;
        musicSource.enabled = true;

        foreach (GameObject Go in script)
        {
            _narrative.ShowText(Go);
            yield return GeneralManager.waitForSeconds(1.5f);
        }

        foreach (GameObject Go in options)
        {
            Go.SetActive(true);
            source.PlayOneShot(optionClick);
            yield return GeneralManager.waitForSeconds(1);
        }

        canChoose = true;
    }

    string argQuestionOrder = "";
    int argIndex = 0;
    void AddArg(int option)
    {
        if (argQuestionOrder.Length > argIndex)
        {
            char[] ch = argQuestionOrder.ToCharArray();
            ch[argIndex] = option.ToString().ToCharArray()[0];
            argQuestionOrder = ch.ArrayToString();
        }
        else
        {
            argQuestionOrder += option.ToString();
        }
        PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name} arg", argQuestionOrder);
        argIndex++;
    }

    IEnumerator playerChose(int index, int otherIndex)
    {
        AddArg(index + 1);

        redVolume.weight = 0.3f;

        musicSource.clip = pianoSfx;
        highPass.enabled = false;
        musicSource.loop = false;
        musicSource.GetComponent<AudioReverbFilter>().enabled = false;
        musicSource.Play();

        canChoose = false;

        options[otherIndex].SetActive(false);
        options[index].GetComponent<TextMeshPro>().text = index + 1 + ". I WISH HE WOULD KILL HIMSELF.";


        CameraShake.Shake(0.5f, 0.2f);

        yield return GeneralManager.waitForSeconds(0.2f);

        LerpSolution.LerpPPweight(redVolume, 0.1f, 0.5f);
        LerpSolution.lerpCamFov(player.playerCamera.GetComponent<Camera>(), 60, 0.3f);

        yield return GeneralManager.waitForSeconds(0.3f);

        CameraShake.Shake(10.751f, 0.05f);

        yield return GeneralManager.waitForSeconds(10.751f);

        foreach (TextMeshPro text in FindObjectsOfType<TextMeshPro>())
        {
            text.text = "";
        }
        _VLManager.PlayLine(redText_intro_01, RedDialogue01);
    }

    public void RedDialogue01()
    {
        StartCoroutine(RedDialogue01Enum());
    }
    IEnumerator RedDialogue01Enum()
    {
        spawnRedWallText("They hate you.");
        yield return GeneralManager.waitForSeconds(0.7f);
        _VLManager.PlayLine(redText_intro_02, RedDialogue02);
    }

    public void RedDialogue02()
    {
        StartCoroutine(RedDialogue02Enum());
    }
    IEnumerator RedDialogue02Enum()
    {
        spawnRedWallText("They do and you know it.");
        yield return GeneralManager.waitForSeconds(1.8f);
        spawnRedWallText("Why else would they want to torture you like this?");
        yield return GeneralManager.waitForSeconds(1.4f);
        _VLManager.PlayLine(redText_intro_03, RedDialogue03);
    }

    public void RedDialogue03()
    {
        StartCoroutine(RedDialogue03Enum());
    }
    IEnumerator RedDialogue03Enum()
    {
        spawnRedWallText("They downloaded this game out of pity.");
        yield return GeneralManager.waitForSeconds(1.8f);
        spawnRedWallText("Who would wilingingly play a stupid 2D game?");
        yield return GeneralManager.waitForSeconds(1.4f);
        _VLManager.PlayLine(redText_intro_04, RedDialogue04);
    }

    public void RedDialogue04()
    {
        StartCoroutine(RedDialogue04Enum());
    }
    IEnumerator RedDialogue04Enum()
    {
        spawnRedWallText("YOU ARE WORTHLESS.");
        yield return GeneralManager.waitForSeconds(0.8f);
        _VLManager.PlayLine(redText_intro_05);
    }


    void spawnRedWallText(string content)
    {
        RaycastHit hit;
        Transform hitTrans;
        if (!Physics.Raycast(player.playerCamera.position, player.playerCamera.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, wallLayer))
        {
            hitTrans = GameObject.Find("Room 5_Wall_1").transform;
        }
        else
        {
            hitTrans = hit.transform;
        }

        TextMeshPro targetText = hitTrans.GetComponentInChildren<TextMeshPro>();
        int itteration = 0;
        while (targetText.text != "")
        {
            itteration++;
            string hitName = targetText.transform.name;
            string nextHitName = hitName.Remove(hitName.Length - 1) + (float.Parse(hitName[hitName.Length - 1].ToString()) + 1);
            if (nextHitName.EndsWith("9"))
            {
                nextHitName = hitName.Remove(hitName.Length - 1) + "1";
            }
            Debug.Log(nextHitName);
            targetText = GameObject.Find(nextHitName).GetComponentInChildren<TextMeshPro>();
            if (itteration > 10) return;
        }
        StartCoroutine(gText.createGlitchedText(content, targetText));
    }

    public void StartShake(float amount)
    {
        CameraShake.Shake(10, amount);
    }

    public void LerpPPforEnd()
    {
        LerpSolution.LerpPPweight(redVolume, 1, 0.3f);
    }

    public void LerpProbeIntensity01() // this is called 01 but there's no 02 and I'm not renaming it because then I would have to reassign it in the inspector which would take about 15 seconds, god I'm lazy
    {
        LerpSolution.LerpProbeIntensity(redRoomProbe, 2, 0.3f);

        PlayerPrefs.SetInt("glotchiness", 4);
        sceneOp = SceneManager.LoadSceneAsync("jesterGames");
        sceneOp.allowSceneActivation = false;
    }

    public void EndPanicAttack()
    {
        StartCoroutine(EndPanicAttackEnum());
    }

    IEnumerator EndPanicAttackEnum()
    {
        foreach (TextMeshPro text in FindObjectsOfType<TextMeshPro>())
        {
            text.text = "";
        }

        CameraShake._instance.StopAllCoroutines();

        redVolume.weight = 0;
        redRoomProbe.intensity = 10;

        yield return GeneralManager.waitForSeconds(0.3f);

        LerpSolution.LerpProbeIntensity(redRoomProbe, 3, 0.2f);
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutEnum());
    }

    IEnumerator FadeOutEnum()
    {
        LerpSolution.lerpImageColour(blackScreen, Color.black, 5f);
        while(blackScreen.color != Color.black) yield return new WaitForEndOfFrame();
        yield return GeneralManager.waitForSeconds(0.3f);
        sceneOp.allowSceneActivation = true;
    }
}
