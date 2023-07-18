using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using Aura2API; // AURA HERE

public class quizEvents : MonoBehaviour
{
    [SerializeField] quizDialogueManager _dialogueManager;
    [SerializeField] quizQuizing _quizingState;
    [SerializeField] AudioSource _source;
    [SerializeField] PlayerController _playerController;

    [Header("In Your Head Glitch")]
    [SerializeField] Animator _camAnim;
    [SerializeField] PostProcessVolume _volume;
    [SerializeField] PostProcessProfile glitch1profile;
    [SerializeField] AudioClip glitchSFX1;
    [SerializeField] Transform playerTransform, playerAfterTransform;

    [Header("Guessed Door Glitch")]
    [SerializeField] TMP_FontAsset bloodyFont;
    [SerializeField] Color bloodColor;
    [SerializeField] TextMeshPro mainTalkingText;
    [SerializeField] AudioClip endDramaSong;
    [SerializeField] Color endDramaColour;
    [SerializeField] PostProcessProfile redProfile;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject crackedFloor;
    [SerializeField] Transform walls;
    [SerializeField] Transform playerFeet;
    [Header("Random Positions")]
    [SerializeField] Transform[] wallPositions; // Why is this here when the variable walls exists? Who knows?
    [SerializeField] float offsetSpawn;
    [SerializeField] TextMeshPro textToSpawn;
    [SerializeField] AudioClip Ground_Breaking_SFX, Ground_smash_SFX;
    [Header("Ending")]
    [SerializeField] Image blackScreen;
    AudioSource SFX;
    quizEnding _endingState;
    AsyncOperation loadSceneOperation;
    float defaultVolume = 1;
    bool helplessEnd = false;

    void Start()
    {
        _endingState = GetComponent<quizEnding>();
        SFX = GameObject.Find("SFX").GetComponent<AudioSource>();
    }

    public void eventManager(string eventToInvoke, float timeToInvoke = 0)
    {
        string Event = eventToInvoke.Replace("&", "");
        Invoke("Event" + Event, timeToInvoke);
    }

    #region Quiz

    public void EventInYourHead()
    {
        StartCoroutine(EnumInYourHead());
    }
     
    IEnumerator EnumInYourHead()
    {
        PostProcessProfile defaultProfile = _volume.profile;
        _volume.profile = glitch1profile;
        AudioClip defaultClip = _source.clip;
        float defaultVolume = _source.volume;

        _playerController.invertMouse = !_playerController.invertMouse;
        CameraShake.Shake(0.8f, 0.4f);
        _source.clip = glitchSFX1;
        _source.Play();
        _source.volume = 0.9f;
        _camAnim.SetTrigger("glitch1");
        yield return GeneralManager.waitForSeconds(0.8f);

        _playerController.invertMouse = !_playerController.invertMouse;
        _playerController.lockMovement = true;
        _playerController.lockLook = true;
        playerTransform.position = playerAfterTransform.position;
        playerTransform.rotation = playerAfterTransform.rotation;
        _playerController.lockMovement = false;
        _playerController.lockLook = false;

        _source.clip = defaultClip;
        _source.time = 16;
        _source.volume = defaultVolume;
        _source.Play(); // Maybe put this on line 38?
        _volume.profile = defaultProfile;
        _dialogueManager.script = new Dictionary<string, float>();

        Gas.EarnAchievement("ACH_MUTUAL");

        _quizingState.askPlayerIfLikeTheView();
    }

    void EventFakeName01()
    {
        defaultVolume = _source.volume;
        _source.Stop();
    }

    void EventFakeName02()
    {
        // Get the vigentte from the profile and save it as a new type vignette
        Vignette _vignette = _volume.profile.GetSetting<Vignette>();
        ColorGrading _grade = _volume.profile.GetSetting<ColorGrading>();
        LerpSolution.lerpVignetteIntensity(_vignette, 0.58f, 0.2f);
        LerpSolution.LerpSaturation(_grade, -35, 0.2f);
    }

    void EventFakeName03()
    {
        Vignette _vignette = _volume.profile.GetSetting<Vignette>();
        ColorGrading _grade = _volume.profile.GetSetting<ColorGrading>();
        if(_vignette.intensity > 0.1f)
        {
            _source.Play();
            _source.time = 20;
        }
        _vignette.intensity.value = 0;
        _grade.saturation.value = 36;
    }

    void EventFadeMusic()
    {
        LerpSolution.lerpVolume(_source, 0, 0.4f);
    }

    void EventChangeFontToBloody()
    {
        CameraShake.Shake(0.2f, 0.4f);
        mainTalkingText.font = bloodyFont;
        mainTalkingText.color = bloodColor;
        _dialogueManager.clickNoiseMultiplyer = 1.6f;
        _source.clip = endDramaSong;
        StartCoroutine(makeVolume1Again());
        _source.Play();
        _source.time = 0;
        _source.loop = false;
        // int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        // loadSceneOperation = SceneManager.LoadSceneAsync(sceneIndex + 1);
        // loadSceneOperation.allowSceneActivation = false;
    }

    IEnumerator makeVolume1Again()
    {
        while(_source.volume != 1)
        {
            _source.volume = 1;
            yield return new WaitForEndOfFrame();
        }
    }

    void EventDoor01()
    {
        Debug.Log("WHY AM I GETTING CALLED?????");
        ColorGrading colorGrading;
        _volume.profile.TryGetSettings(out colorGrading);
        LerpSolution.LerpColourFilter(colorGrading, endDramaColour, 0.3f);
    }

    void EventDoor02()
    {
        //_volume.profile = redProfile;
        //loadSceneOperation.allowSceneActivation = true;
    }

    void EventDoor03()
    {
        helplessEnd = true;
        StartCoroutine(spawnText("HELPLESS"));
        Invoke("CrackFloor", 7);
    }

    void EventEnding03()
    {
        StartCoroutine(spawnText("ALONE"));
        Invoke("CrackFloor", 7);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        //    _source.enabled = true;
        //    _source.clip = endDramaSong;
        //    _source.Play();
        //    StartCoroutine(Fade());
        //}
    }

    IEnumerator Fade()
    {
        yield return GeneralManager.waitForSeconds(4);
        int TargetScene = SceneManager.GetActiveScene().buildIndex + 1;
        loadSceneOperation = SceneManager.LoadSceneAsync(TargetScene);
        loadSceneOperation.allowSceneActivation = false;
        LerpSolution.lerpImageColour(blackScreen, Color.black, 0.4f, Color.clear);
        yield return GeneralManager.waitForSeconds(3f);
        if(helplessEnd) Gas.EarnAchievement("ACH_HELPLESS");
        loadSceneOperation.allowSceneActivation = true;
    }

    bool canSpawnText = true;

    void StartCrackSFX()
    {
        SFX.clip = Ground_Breaking_SFX;
        SFX.Play();
        LerpSolution.lerpVolume(SFX, 1, 1, 0);
    }

    IEnumerator spawnText(string text)
    {
        Invoke("StartCrackSFX", 0.5f);

        List<TextMeshPro> Texts = GameObject.FindGameObjectsWithTag("Text").Select(x => x.GetComponent<TextMeshPro>()).ToList();
        //Random.State originalRandomState = Random.state;
        //Random.InitState(1234);
        glitchText _gText = GetComponent<glitchText>();

        while(canSpawnText)
        {
            int randomText = Random.Range(0, Texts.Count - 1);
            Texts[randomText].text = text;
            _gText.glitchGlitchedText(Texts[randomText]);
            Texts.RemoveAt(randomText);
            yield return GeneralManager.waitForSeconds(0.06f);
        }

        //Random.state = originalRandomState;
    }

    private void CrackFloor()
    {
        StartCoroutine(Fade());
        floor.SetActive(false);
        crackedFloor.SetActive(true);
        canSpawnText = false;
        SFX.Stop();
        SFX.PlayOneShot(Ground_smash_SFX, 3.4f);
    }

    void EventRigAnswer()
    {
        foreach(TextMeshPro dialogueOption in _dialogueManager.dialogueOptions)
        {
            if(dialogueOption.text != "")
            {
                string numberChoice = dialogueOption.text.Split('.')[0];
                dialogueOption.text = numberChoice + ". There is no meaning to life.";
                StartCoroutine(fadeNoMeaningToLife(dialogueOption));
                Debug.Log("Should be first thing!");
                return;
            }
        }
    }

    IEnumerator fadeNoMeaningToLife(TextMeshPro selectedOption)
    {
        yield return GeneralManager.waitForSeconds(6);
        selectedOption.text = "";
    }

    void EventWaitForResults01()
    {
        StartCoroutine(EnumWaitForResults01());
    }

    IEnumerator EnumWaitForResults01()
    {
        yield return GeneralManager.waitForSeconds(4);
        _endingState.announceResults();
    }

    void EventWaitForResults02()
    {
        StartCoroutine(EnumWaitForResults02());
    }

    IEnumerator EnumWaitForResults02()
    {
        yield return GeneralManager.waitForSeconds(2);
        _endingState.finallySayResults();
    }

    void EventBrightnessEnd() { StartCoroutine(EventBrightnessEndEnum()); }

    IEnumerator EventBrightnessEndEnum()
    {
        yield return GeneralManager.waitForSeconds(0.5f);

        PostProcessProfile defaultProfile = _volume.profile;
        _volume.profile = glitch1profile;
        AudioClip defaultClip = _source.clip;
        float defaultVolume = _source.volume;

        _playerController.invertMouse = !_playerController.invertMouse;
        CameraShake.Shake(0.8f, 0.4f);
        _source.clip = glitchSFX1;
        _source.Play();
        _source.volume = 0.9f;
        _camAnim.SetTrigger("glitch1");

        yield return GeneralManager.waitForSeconds(0.8f);

        Application.Quit();
    }

    #endregion

    #region Restaurant

    [Header("Restaurant References")]
    [SerializeField] CharacterController cubeCommunity;

    public void EventLoadArgueRes()
    {
        SceneManager.LoadSceneAsync("DevArgue", LoadSceneMode.Additive);
    }

    public void EventEndRes()
    {
        _playerController.transform.position += new Vector3(0, 10, 0);

        // foreach (AuraCamera cam in FindObjectsOfType<AuraCamera>()) // AURA HERE
        // { // AURA HERE
            // cam.enabled = false; // AURA HERE
        // } // AURA HERE

        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam != FindObjectOfType<PlayerController>().playerCamera.GetComponent<Camera>())
            {
                Destroy(cam.gameObject);
            }
        }

        _playerController.lockGravity = false;
        cubeCommunity.enabled = true;

        _source.Stop();
        SFX.Stop();
    }

    #endregion

    #region Ocean
    [Header("Ocean References")]
    [SerializeField] Transform EndPos;
    [SerializeField] GameObject lookAhead;
    [SerializeField] GameObject playerBlock;
    [HideInInspector] public AsyncOperation sceneOp;

    public void EventOceanFinish()
    {
        sceneOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        sceneOp.allowSceneActivation = false;

        lookAhead.SetActive(false);
        playerBlock.SetActive(true);

        PlayerController controller = FindObjectOfType<PlayerController>();
        controller.ChangeSpeed(1.5f);
        controller.TeleportPlayer(EndPos.position);
        controller.lockGravity = false;
        controller.checkForMovement = true;

        GetComponent<AmeliaOceanManager>().CanShowAhead = false;
    }
    #endregion

    #region Therapy
    [Header("Therapy References")]
    [SerializeField] PostProcessProfile WhiteSpaceProfile;
    [SerializeField] Animator eyeAnim;
    [SerializeField] TextMeshPro therapistText;
    [SerializeField] AudioClip Northen;
    [SerializeField] AudioClip Thud;
    [SerializeField] TextMeshPro[] thoughts;
    [SerializeField] TMP_FontAsset redTextFont;
    [SerializeField] TextMeshPro[] Redthoughts;
    [SerializeField] GameObject hallway;
    [SerializeField] AudioClip veryShortGlitch;
    PostProcessProfile HubProfile;

    public void EventCloseEyes()
    {
        eyeAnim.SetTrigger("Close");
    }

    public void EventWhiteVoid()
    {
        HubProfile = _volume.profile;
        _volume.profile = WhiteSpaceProfile;
        eyeAnim.SetTrigger("OpenInstant");
        FindObjectOfType<PlayerController>().lockLook = true;
        Transform player = FindObjectOfType<PlayerController>().transform;
        FindObjectOfType<PlayerController>().TeleportPlayer(player.position + new Vector3(31.675f, 0, 0));
        therapistText.transform.position += new Vector3(31.675f, 0, 0);
        therapistText.color = Color.black;

        GetComponent<AudioSource>().clip = Northen;
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = true;
    }

    public void EventWhiteVoidLook()
    {
        FindObjectOfType<PlayerController>().lockLook = false;
    }

    public void EventWhiteVoidProgress()
    {
        Debug.Log("Done!");
        foreach (TextMeshPro text in GetThoughts())
        {
            text.gameObject.SetActive(true);
        }
        CameraShake.Shake(0.3f, 0.1f);

        SFX.PlayOneShot(Thud);
    }

    public void EventWhiteVoidProgresss()
    {
        Debug.Log("Done!");
        foreach (TextMeshPro text in thoughts)
        {
            text.gameObject.SetActive(true);
        }
        CameraShake.Shake(0.3f, 0.1f);

        SFX.PlayOneShot(Thud);
    }

    TextMeshPro[] GetThoughts()
    {
        Debug.LogWarning("This might crash if there isn't exactly 120 texts");
        List<TextMeshPro> result = new List<TextMeshPro>();
        for (int i = 0; i < 7; i++)
        {
            bool canMoveOn = false;
            while (!canMoveOn)
            {
                TextMeshPro subject = thoughts[Random.Range(0, thoughts.Length - 1)];
                if (!subject.gameObject.activeInHierarchy && !result.Contains(subject))
                {
                    result.Add(subject);
                    canMoveOn = true;
                }
            }
        }

        return result.ToArray();
    }

    public void EventWhiteVoidEnd()
    {
        eyeAnim.SetTrigger("Open");
        _volume.profile = HubProfile;
        foreach (TextMeshPro text in thoughts) text.gameObject.SetActive(false);
        Transform player = FindObjectOfType<PlayerController>().transform;
        FindObjectOfType<PlayerController>().TeleportPlayer(player.position - new Vector3(31.675f, 0, 0));
        GetComponent<AudioSource>().volume = 0;
    }

    public void EventWhiteVoidBack()
    {
        sceneOp = SceneManager.LoadSceneAsync("Dev_Confess");
        sceneOp.allowSceneActivation = false;

        _volume.profile = WhiteSpaceProfile;
        foreach (TextMeshPro text in thoughts) text.gameObject.SetActive(true);
        Transform player = FindObjectOfType<PlayerController>().transform;
        FindObjectOfType<PlayerController>().TeleportPlayer(player.position + new Vector3(31.675f, 0, 0));
        GetComponent<AudioSource>().volume = 1;
        GetComponent<AudioSource>().GetComponent<AudioDistortionFilter>().distortionLevel = 0.6f;
    }

    int itteration = 0;
    string[] redTextScript = { "You will stay here.", "Don't give in.", "I WILL NEVER LEAVE YOU ALONE", "I LIVE INSIDE YOU", "YOU CANNOT ESCAPE FROM ME" };
    public void EventWhiteVoidRedProgress()
    {
        Redthoughts[itteration].font = redTextFont;
        Redthoughts[itteration].color = new Color(87/255, 4/255, 4/255);
        StartCoroutine(GetComponent<glitchText>().createGlitchedText(redTextScript[itteration], Redthoughts[itteration]));
        CameraShake.Shake(0.3f, 0.2f);
        GetComponent<AudioSource>().PlayOneShot(Thud);
        itteration++;
    }

    public void EventWhiteVoidEndFr()
    {
        StartCoroutine(WhiteVoidEndFr());
    }

    IEnumerator WhiteVoidEndFr()
    {
        yield return GeneralManager.waitForSeconds(0.3f);
        hallway.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(veryShortGlitch);
        yield return GeneralManager.waitForSeconds(0.17f);
        sceneOp.allowSceneActivation = true;
    }
    #endregion

    #region End quiz
    [Header("End quiz references")]
    [SerializeField] AudioClip SongIntro;

    public void EventEndMusic()
    {
        _source.enabled = true;
        _source.clip = SongIntro;
        _source.Play();
        _source.loop = false;
    }

    public void EventEndRiseCheck()
    {
        GetComponent<quizManager>().currentQuizProjectState = quizManager.quizProjectStates.checkForRise;
    }

    #endregion
}
