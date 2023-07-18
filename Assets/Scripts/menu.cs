using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Rendering.PostProcessing;
using Kino;

public class menu : MonoBehaviour
{
    public TextMeshProUGUI startText;
    [SerializeField] KeyCode startAction;
    public Animator camAnim, titleAnim;
    public ScrollUV _scrollUV;
    public Camera threedcam;
    [SerializeField] PlayerController _player;
    [SerializeField] SpriteRenderer logo;
    [SerializeField] GameObject glitchOverlay;
    [SerializeField] SpriteRenderer mouse;
    [SerializeField] VoiceLine afterMessagesLine;
    [SerializeField] Transform reflectionPos;
    [SerializeField] GameObject blocker, screen, text, otherBlocker, otherOtherBlocker;
    [SerializeField] Light Light;
    [SerializeField] Animator door;
    [SerializeField] TextMeshPro doorText;
    [SerializeField] Transform spawnpos;
    [SerializeField] MeshRenderer wallRenderer;
    [SerializeField] AudioClip plehThemeCutShort;
    [SerializeField] DigitalGlitch digital;
    [SerializeField] AnalogGlitch analog;
    [SerializeField] AudioClip ding;

    [HideInInspector] public music _music;
    ScrollUV _scroll;
    AudioSource _source;
    Camera cam;
    bool hasBeenTriggered, hasMenuBeenTriggered;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("glotchiness")) PlayerPrefs.SetInt("glotchiness", 0);

        if(PlayerPrefs.GetInt("glotchiness") == 1)
        {
            Debug.Log("You've been here before, haven't you?");
        }

        if (PlayerPrefs.GetInt("glotchiness") == 0)
        {
            FindObjectOfType<AnalogGlitch>().enabled = false;
        }
    }

    private IEnumerator Start()
    {
        FindObjectOfType<DRP>().RP("");

        

        updateText(false, PlayerPrefs.GetInt("glotchiness") == 7);
        _source = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        titleAnim.SetTrigger("startFloat");
        cam = Camera.main;
        _music = GameObject.FindGameObjectWithTag("Music").GetComponent<music>();
        _scroll = FindObjectOfType<ScrollUV>();
        if(PlayerPrefs.GetInt("glotchiness") == 7)
        {
            FindObjectOfType<DRP>().RP("Goodbye.");
            otherOtherBlocker.SetActive(true);
            _player.updateCursorState(true, true);
            cam.tag = "Untagged";
            threedcam.tag = "MainCamera";
            FindObjectOfType<AudioLowPassFilter>().cutoffFrequency = 10;
            _player.lockLook = false;
            threedcam.GetComponent<PostProcessVolume>().enabled = true;
            _player.lockMovement = false;
            _player.TeleportPlayer(reflectionPos.position);
            _player.transform.rotation = reflectionPos.rotation;
            blocker.SetActive(false);
            screen.SetActive(false);
            text.SetActive(false);
            otherBlocker.SetActive(false);
            door.enabled = false;
            door.transform.rotation = Quaternion.Euler(0, 40, 0);
            Light.color = Color.white;
            _source.clip = plehThemeCutShort;
            _source.Play();

            LerpSolution.lerpCamFov(threedcam, 60, 0.3f, 0);
            ColorGrading grade;
            FindObjectOfType<PostProcessVolume>().profile.TryGetSettings(out grade);
            LerpSolution.LerpPostExposure(grade, 0, 0.2f, 10.5f);
            LerpSolution.lerpVolume(GameObject.FindGameObjectWithTag("Cube").GetComponent<AudioSource>(), 0, 0.1f);
            yield return GeneralManager.waitForSeconds(5);
        }
        else if (PlayerPrefs.GetInt("glotchiness") != 3)
        {
            bool canZoom = true;
            if (PlayerPrefs.GetString("thats it") == "no zoom")
            {
                canZoom = false;
            }
            if(canZoom) LerpSolution.lerpCamSize(cam, 10, 1.4f, 0);
        }
        else
        {
            if (PlayerPrefs.GetInt("glotchiness") == 3)
            {
                _music.source.time = 7;
                _music.source.volume -= 0.6f;
                FindObjectOfType<VoiceLineManager>().PlayLine(afterMessagesLine);
                hasMenuBeenTriggered = true;
            }

        }

        if (PlayerPrefs.GetString("thats it") == "message")
        {
            yield return GeneralManager.waitForEndOfFrame;
            FindObjectOfType<music>().source.Play();
            AsyncOperation sceneOp = SceneManager.LoadSceneAsync("New Messages");
            sceneOp.allowSceneActivation = false;
            yield return GeneralManager.waitForSeconds(4);
            _source.PlayOneShot(ding);
            LerpSolution.lerpPositionTime(FindObjectOfType<messageManager>().transform, GameObject.Find("Message pos").transform.position, 0.5f);
            yield return GeneralManager.waitForSeconds(1.5f);

            foreach (AudioSource source in FindObjectsOfType<AudioSource>())
            {
                Destroy(source);
            }
            sceneOp.allowSceneActivation = true;
        }
    }

    public void ShowReflectionText()
    {
        if (PlayerPrefs.GetInt("glotchiness") == 7)
        {
            door.enabled = true;
            door.SetTrigger("doorClose");
            doorText.gameObject.SetActive(true);
            FindObjectOfType<glitchText>().glitchGlitchedText(doorText);

            Gas.EarnAchievement("ACH_DOORbEHIND");
        }
    }

    public void PlayPlehMusic()
    {
        LerpSolution.lerpLowPass(FindObjectOfType<AudioLowPassFilter>(), 248, 0.1f);
    }

    public bool shouldAllowPleh = false;
    public void AllowPleh()
    {
        if (PlayerPrefs.GetInt("glotchiness") != 7) return; // if this wasn't here, game would be done in 10 seconds so sorry speedrunners :)
        blocker.SetActive(true);
        screen.SetActive(true);
        shouldAllowPleh = true;

        StartCoroutine(waitForPleh());
    }

    bool isAtMenuFinal = false;
    IEnumerator waitForPleh()
    {
        bool i = false;
        while (!i)
        {
            if (FindObjectOfType<menu>().shouldAllowPleh && _player.transform.rotation.y > -0.5f && _player.transform.rotation.y < 0.5f) i = true;
            yield return new WaitForEndOfFrame();
        }
        _player.lockMovement = true;
        _player.lockLook = true;
        _player.lockGravity = true;
        LerpSolution.lerpPositionTime(_player.transform, spawnpos.position, 1f);
        LerpSolution.lerpRotationTime(_player.playerCamera, spawnpos.rotation, 1f);
        LerpSolution.lerpLowPass(FindObjectOfType<AudioLowPassFilter>(), 15000, 0.5f);
        yield return GeneralManager.waitForSeconds(2);
        isAtMenuFinal = true;
    }

    private void updateText(bool deadCharacter, bool isEnd = false, bool haha = false)
    {
        if (haha)
        {
            startText.text = ("Press any key but that key to begin");
            return;
        }
        else if (isEnd)
        {
            startText.text = ("Press any key to begin");
            return;
        }

        if (deadCharacter)
        {
            startText.text = ("Press 🗅 to begin");
        }
        else
        {
            startText.text = ($"Press {startAction} to begin");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(startAction) && PlayerPrefs.GetInt("glotchiness") != 7)
        {
            Gas.EarnAchievement("ACH_BEGIN");

            switch(PlayerPrefs.GetInt("glotchiness"))
            {
                case 0:
                    if(!hasMenuBeenTriggered)
                    {
                        //StartCoroutine(startGame());
                        hasMenuBeenTriggered = true;
                        StartCoroutine(_scroll.movedown());
                    }
                    break;
                case 1:
                    doGlitchMenu();
                    break;
                case 2:
                    if (PlayerPrefs.GetString("thats it") == "message") return;
                    StartCoroutine(GetComponent<devIntro_menu>().playerTryToPlay(_source, _music, camAnim));
                    break;
                case 3:
                    if (!hasMenuBeenTriggered)
                    {
                        hasMenuBeenTriggered = true;
                        StartCoroutine(_scroll.movedown(true));
                    }
                    break;
                case 5:
                    if (!hasMenuBeenTriggered)
                    {

                        hasMenuBeenTriggered = true;
                        StartCoroutine(_scroll.movedown());
                    }
                    break;
            }
        }

        if (isAtMenuFinal && PlayerPrefs.GetInt("glotchiness") == 7 && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.H) && !hasMenuBeenTriggered)
        {
            hasMenuBeenTriggered = true;
            StartCoroutine(_scroll.movedown());
        }
        else if (isAtMenuFinal && PlayerPrefs.GetInt("glotchiness") == 7 && Input.GetKeyDown(KeyCode.H) && !hasMenuBeenTriggered)
        {
            updateText(false, true, true);
            FindObjectOfType<glitchText>().glitchGlitchedText(startText);
            Gas.EarnAchievement("ACH_H");
        }
        //StartCoroutine(startGame());
    }

    public void allowMenuLoad()
    {
        hasMenuBeenTriggered = false;
    }

    private void doGlitchMenu()
    {
        switch (startAction)
        {
            case KeyCode.H:
                //camAnim.SetInteger("glitchMenuNum", 1);
                analog.colorDrift = 0.082f;
                analog.horizontalShake = 0.04f;
                analog.scanLineJitter = 0;
                digital.intensity = 0.05f;
                StartCoroutine(pitchAudio(0.73f, 1, 0.4f, 1.07f, 4));
                StartCoroutine(zoomCamera(9.2f, 0.6f, 0, 10, 4));
                _scrollUV.isScrollingBackwards = true;
                updateText(true);
                return;
            case KeyCode.E:
                //camAnim.SetInteger("glitchMenuNum", 2);
                analog.colorDrift = 0.044f;
                analog.horizontalShake = 0.1f;
                analog.scanLineJitter = 0.015f;
                digital.intensity = 0.1f;
                _music.staticGlitch();
                logo.flipY = true;
                _source.volume = 1.2f;
                _music.switchTrack(_music.lowPitch);
                startAction = KeyCode.L;
                break;
            case KeyCode.L:
                //camAnim.SetInteger("glitchMenuNum", 3);
                analog.colorDrift = 0.079f;
                analog.horizontalShake = 0.2f;
                analog.scanLineJitter = 0.015f;
                digital.intensity = 0.3f;
                _music.staticGlitch();
                _source.volume = 1.5f;
                _source.pitch = 1.3f;
                logo.flipX = true;
                startAction = KeyCode.P;
                break;
            case KeyCode.P:
                if (!hasBeenTriggered)
                {
                    _player.updateCursorState(true, true);
                    //camAnim.SetInteger("glitchMenuNum", 4);
                    analog.enabled = false;
                    digital.enabled = false;
                    logo.GetComponent<Animator>().enabled = false;
                    _scrollUV.canMove = false;
                    glitchOverlay.SetActive(true);
                    cam.tag = "Untagged";
                    threedcam.tag = "MainCamera";
                    _source.Stop();
                    _source.PlayOneShot(_music.robot);
                    _player.lockLook = false;
                    threedcam.GetComponent<PostProcessVolume>().enabled = true;
                    hasBeenTriggered = true;
                    StartCoroutine(CountForMouse());
                }
                break;
        }
        updateText(false);
    }

    IEnumerator CountForMouse()
    {
        yield return GeneralManager.waitForSeconds(15);
        LerpSolution.lerpSpriteColour(mouse, Color.white, 0.1f);
    }

    IEnumerator pitchAudio(float pitch1, float speed1, float breakTime, float pitch2, float speed2)
    {        
        float desiredSpeed = speed1;
        float desiredPitch = pitch1;

        for(int i = 0; i < 2; i++)
        {
            LerpSolution.lerpPitch(_source, desiredPitch, desiredSpeed);
            while(_source.pitch != desiredPitch) yield return new WaitForEndOfFrame();
            if(i == 0)
            {
                desiredSpeed = speed2;
                desiredPitch = pitch2;
            }
            yield return GeneralManager.waitForSeconds(breakTime);
        }
    }

    IEnumerator zoomCamera(float zoom1, float speed1, float breakTime, float zoom2, float speed2)
    {
        float desiredSpeed = speed1;
        float desiredZoom = zoom1;

        for (int i = 0; i < 2; i++)
        {
            LerpSolution.lerpCamSize(cam, desiredZoom, desiredSpeed);
            while (cam.orthographicSize != desiredZoom) yield return new WaitForEndOfFrame();
            if (i == 0)
            {
                desiredSpeed = speed2;
                desiredZoom = zoom2;
            }
            yield return GeneralManager.waitForSeconds(breakTime);        
        }
        //this is only here because of the main menu feel free to yeet it but put it somewhere else ;)
        //shut up
        startAction = KeyCode.E;
        updateText(false);
    }

    private IEnumerator startGame()
    {
        // _cameraZoomOut.zoomOut(_cameraZoomOut.cam.orthographicSize, 2000, 0.1f);
        LerpSolution.lerpCamSize(cam, 2500, 0.08f); // TODO: make this faster now that we've zoomed out camera
        yield return GeneralManager.waitForSeconds(4);
        infoFromScene _infoFromScene = FindObjectOfType<infoFromScene>();
        if(_infoFromScene != null)
        {
            _infoFromScene.messageFromScene = "zoom in";
        }
        else
        {
            Debug.LogError("Error! The info from scene could not be found! Is this on purpose? Your going to need to add an exception here if that's the case.");
        }
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }
}
