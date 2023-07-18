using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class messagesSpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject messagePrefab;
    [SerializeField] Transform spawningPoint, endPoint;
    [SerializeField] Message[] messageScript;
    [SerializeField] Message[] tempScript;
    [SerializeField] string[] spamScript;
    [SerializeField] PostProcessVolume volume;
    [SerializeField] float vignetteFadeTime;
    [SerializeField] Image black;
    [SerializeField] Color bgGlitchColour, bgGlitchColour2;
    [SerializeField] Camera mainCam, gameCam;
    [SerializeField] AudioClip ping;
    [HideInInspector] public Message recentMessage;
    List<Message> finalMessagingScript = new List<Message>(); 
    AudioSource _source;
    musicManagerMessages _musicManager;
    infoFromScene _info;
    playerControllerMessages _controller;
    Vignette vig;
    messagesEvent _event;

    void Start()
    {
        _event = GetComponent<messagesEvent>();
        _controller = FindObjectOfType<playerControllerMessages>();
        _info = FindObjectOfType<infoFromScene>();
        _source = FindObjectOfType<musicManagerMessages>().GetComponent<AudioSource>();
    }

    IEnumerator getMusicManager()
    {
        while(_musicManager == null)
        {
            _musicManager = FindObjectOfType<musicManagerMessages>();
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator spawningCycle()
    {
        finalMessagingScript = messageScript.ToList<Message>(); // System.Linq my beloved <3
        if(_info.messageArgument != null)
        {
            yield return GeneralManager.waitForSeconds(finalMessagingScript[0].timeAfterPreviousMessage);
            StartCoroutine(spawnMessage(_info.messageArgument));
            finalMessagingScript.RemoveRange(0, finalMessagingScript.IndexOf(_info.messageArgument) + 1);
        }
        foreach(Message message in finalMessagingScript)
        {
            yield return GeneralManager.waitForSeconds(message.timeAfterPreviousMessage);
            StartCoroutine(spawnMessage(message));
        }
    }

    public IEnumerator startSpam()
    {
        FindObjectOfType<gameMessagesState>().canDie = false;
        mainCam.backgroundColor = bgGlitchColour; //140, 41, 41

        StartCoroutine(spamMessages());
        volume.profile.TryGetSettings(out vig);
        vig.rounded.value = true;
        yield return GeneralManager.waitForSeconds(15); //keep this at 15 unless testing

        bool hasBlacked = false;
        float travelPercent = 0f;
        float currentIntensity = vig.intensity.value;
        float currentSmoothness = vig.smoothness.value;
        while(travelPercent < 1)
        {
            travelPercent += Time.deltaTime * vignetteFadeTime;
            vig.intensity.value = Mathf.Lerp(currentIntensity, 1, travelPercent);
            vig.smoothness.value = Mathf.Lerp(currentSmoothness, 1, travelPercent);
            if(travelPercent >= 0.7f && !hasBlacked)
            {
                hasBlacked = true;
                LerpSolution.lerpImageColour(black, new Color(0, 0, 0, 1), 0.7f, Color.clear);
            }
            yield return new WaitForEndOfFrame();
        }

        yield return GeneralManager.waitForSeconds(3); //2 is 2 short :D 
        GetComponent<gameMessagesState>().isDone = true;
        #region ending
        // volume.profile = winProfile;
        // mainCam.backgroundColor = bgGlitchColour2;
        // mainCam.GetComponent<Animator>().Play("end");
        // black.enabled = false;
        // _controller.canCameraFollow = false;
        // gameCam.transform.position = camWinPos.position;
        // camStuff.SetActive(false);

        // if(_source == null)
        // {
        //     _source = FindObjectOfType<musicManagerMessages>().GetComponent<AudioSource>();
        // }

        // _source.Stop(); //no idea if this needs to be here but it's here ig
        // _source.clip = distortLoudLoop;
        // _musicManager.smallGlitch(1, 0.7f);
        // //show no escape background, glitch a bit and flash the bas64 for a few frames
        // noEscape.SetActive(true);
        // CameraShake.Shake(0.580f, 0.5f);
        // noEscape64.SetActive(true);
        // yield return GeneralManager.waitForSeconds(0.193f);
        // _event._mainAnim.Play("Light1");
        // yield return GeneralManager.waitForSeconds(0.193f);
        // noEscape64.transform.position += new Vector3(4, 2, 0);
        // yield return GeneralManager.waitForSeconds(0.193f);
        // noEscape64.transform.position += new Vector3(6, -3, 0);
        // _event._mainAnim.Play("Digi0");
        // noEscape.SetActive(false);
        // noEscape64.SetActive(false);
        // _source.Play();
        // _source.pitch = 1.3f;
        // _source.time = 4;
        #endregion
    }

    IEnumerator spamMessages()
    {
        //this is different to startSpam which is the main hub of spamming. 
        //this is just for sending the messages.
        while (true)
        {
            yield return GeneralManager.waitForSeconds(0.3f);
            StartCoroutine(spawnMessage(new Message("Unknown", spamScript[Random.Range(0, spamScript.Length)], true, 5)));
        }
    }

    public IEnumerator spawnMessage(Message message)
    {
        try
        {
            if(_source == null) _source = FindObjectOfType<musicManagerMessages>().GetComponent<AudioSource>();
        }
        catch
        {
            _source = FindObjectOfType<AudioSource>();
        }
        _source.PlayOneShot(ping);
        recentMessage = message;
        moveMessages(FindObjectsOfType<messageManager>());
        messageManager messageSent = Instantiate(messagePrefab, spawningPoint.position, Quaternion.identity).GetComponent<messageManager>();
        messageSent.updateInfo(message);
        if (message.methodToCall != "" || message.coroutineToCall != "")
        {
            yield return GeneralManager.waitForSeconds(message.timeToWaitForCall);
            if (message.methodToCall != "") _event.Invoke(message.methodToCall, 0);
            if (message.coroutineToCall != "") _event.StartCoroutine(message.coroutineToCall);
        }
    }

    void moveMessages(messageManager[] messages)
    {
        foreach (messageManager message in messages)
        {
            //maybe get rid of?
            message.moveDown();
        }
    }
}
