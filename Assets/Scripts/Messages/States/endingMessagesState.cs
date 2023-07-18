using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class endingMessagesState : messagesState
{
    bool hasBeenRun;
    [SerializeField] PostProcessVolume volume;
    [SerializeField] PostProcessProfile winProfile;
    [SerializeField] Camera mainCam, gameCam;
    [SerializeField] GameObject camStuff;
    [SerializeField] Transform camWinPos;
    [SerializeField] Color bgGlitchColour2;
    [SerializeField] Image black;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip distortLoudLoop;
    [SerializeField] GameObject noEscape;
    [SerializeField] GameObject noEscape64;
    playerControllerMessages _controller;
    musicManagerMessages _musicManager;
    messagesEvent _event;
    [HideInInspector] public bool canEndWithText;

    void startCurrentState()
    {
        if (hasBeenRun) return;
        
        _controller = FindObjectOfType<playerControllerMessages>();
        _musicManager = FindObjectOfType<musicManagerMessages>();
        _event = GetComponent<messagesEvent>();
        StartCoroutine(endingSequence());

        hasBeenRun = true;
    }

    public override messagesState runCurrentState()
    {
        startCurrentState();
        //Update code here
        return this;
    }

    IEnumerator endingSequence()
    {
        Debug.LogError("We're starting the end!");
        volume.profile = winProfile;
        Vignette vig;
        winProfile.TryGetSettings(out vig);
        float currentVigIntesnse = vig.intensity.value;
        vig.intensity.value = 0;

        mainCam.backgroundColor = bgGlitchColour2;
        mainCam.GetComponent<Animator>().Play("end");
        black.enabled = false;
        _controller.canCameraFollow = false;
        gameCam.transform.position = camWinPos.position;
        camStuff.SetActive(false);

        if (_source == null)
        {
            _source = FindObjectOfType<musicManagerMessages>().GetComponent<AudioSource>();
        }

        _source.Stop(); //no idea if this needs to be here but it's here ig
        _source.clip = distortLoudLoop;
        _musicManager.smallGlitch(1, 0.7f);
        //show no escape background, glitch a bit and flash the bas64 for a few frames
        noEscape.SetActive(true);
        CameraShake.Shake(0.580f, 0.5f);
        noEscape64.SetActive(true);
        yield return GeneralManager.waitForSeconds(0.193f);
        _event._mainAnim.Play("Light1");
        yield return GeneralManager.waitForSeconds(0.193f);
        noEscape64.transform.position += new Vector3(4, 2, 0);
        yield return GeneralManager.waitForSeconds(0.193f);
        noEscape64.transform.position += new Vector3(6, -3, 0);
        _event._mainAnim.Play("Digi0");
        noEscape.SetActive(false);
        noEscape64.SetActive(false);

        vig.intensity.value = currentVigIntesnse;
        _source.Play();
        _source.pitch = 1.3f;
        _source.time = 4;

        canEndWithText = true;
    }

    public IEnumerator doEndRedText()
    {
        if(canEndWithText)
        {
            AsyncOperation sceneOp = SceneManager.LoadSceneAsync("Menu 1");
            sceneOp.allowSceneActivation = false;

            canEndWithText = false;
            TextMeshPro redText = GameObject.Find("Red Text").GetComponent<TextMeshPro>();
            glitchText _gText = GetComponent<glitchText>();
            StartCoroutine(_gText.createGlitchedText("Do you get it now?", redText));
            _musicManager.doSnap();
            yield return GeneralManager.waitForSeconds(3);
            StartCoroutine(_gText.createGlitchedText("He MADE this", redText));
            _musicManager.doSnap();
            yield return GeneralManager.waitForSeconds(3);
            StartCoroutine(_gText.createGlitchedText("Poor him, begging for help...", redText));
            _musicManager.doSnap();
            yield return GeneralManager.waitForSeconds(4);
            StartCoroutine(_gText.createGlitchedText("Too bad he won't get any.", redText));
            _musicManager.doSnap();
            yield return GeneralManager.waitForSeconds(4);
            CameraShake.Shake(5f, 0.1f);
            _musicManager._source.PlayOneShot(_musicManager.reversePiano, 5);
            yield return GeneralManager.waitForSeconds(2);
            CameraShake.Shake(5f, 0.5f);
            yield return GeneralManager.waitForSeconds(_musicManager.reversePiano.length - _musicManager._source.time);

            Destroy(_musicManager.gameObject);
            FindObjectOfType<infoFromScene>().messageFromScene = "messages";
            PlayerPrefs.SetInt("glotchiness", 3);
            sceneOp.allowSceneActivation = true;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(doEndRedText());
        }
    }
}
