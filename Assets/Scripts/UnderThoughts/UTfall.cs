using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
// using Aura2API; // AURA HERE
using TMPro;
using UnityEngine.Audio;

public class UTfall : MonoBehaviour
{
    [Header("Stair fall cutscene")]
    public AudioClip stairBreak;
    [SerializeField] AudioClip introStair;
    [SerializeField] AudioClip loopStair;
    [SerializeField] Image blackScreen;
    [SerializeField] GameObject fallingStuff;
    public Transform pushObject;
    [SerializeField] BoxCollider playerBlocker;
    [Header("Misc")]
    [SerializeField] TextMeshProUGUI tutText;

    [Header("Door cutscene")]
    [SerializeField] AudioMixerGroup shhh;
    [SerializeField] AudioClip doorMusic;
    // [SerializeField] AuraLight doorLight; // AURA HERE
    [SerializeField] Light doorLight; // AURA NOT HERE
    [SerializeField] Transform door;
    [SerializeField] Transform endPos;
    [SerializeField] PostProcessVolume currentVolume;
    [SerializeField] PostProcessProfile targetProfile;

    [HideInInspector] public bool pulseCanDestroy = false;

    CollisionManager stairCol;
    PlayerController controller;
    bool hasStarted = false;

    void StateStart(UTGameManager _manager)
    {
        LerpSolution.lerpImageColour(blackScreen, Color.clear, 0.5f);
        try
        {
            fallingStuff = GameObject.Find("Falling stuff");
            pushObject = GameObject.Find("Place to throw").transform;
            playerBlocker = GameObject.Find("Player blocker").GetComponent<BoxCollider>();
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }

        controller = FindObjectOfType<PlayerController>();
        controller.playerCamera.rotation = Quaternion.Euler(90, 0, 0);
        AudioHighPassFilter filter = FindObjectOfType<music>().GetComponent<AudioHighPassFilter>();
        LerpSolution.lerpHighPass(filter, 0, 0.2f);
        
        StartCoroutine(waitForPlayerGrounded());
    }

    IEnumerator waitForPlayerGrounded()
    {
        yield return GeneralManager.waitForSeconds(1);
        try { LerpSolution.lerpVolume(GameObject.Find("SFX").GetComponent<AudioSource>(), 0, 0.2f); }
        catch { }
        while (!controller.controller.isGrounded)
        {
            yield return new WaitForEndOfFrame();
        }
        OnStairCollide();
    }

    public void OnStairCollide()
    {
        //LerpSolution.lerpVolume(GameObject.Find("SFX").GetComponent<AudioSource>(), 0, 1);
        StartCoroutine(UnlockMovementAndLook());
    }

    IEnumerator UnlockMovementAndLook()
    {
        yield return GeneralManager.waitForSeconds(0.6f);

        Quaternion targetRot = Quaternion.Euler(0, 0, 0);
        LerpSolution.lerpLocalRotation(controller.playerCamera, targetRot, 1f);
        controller.lockMovement = false;
        while(controller.playerCamera.localRotation != targetRot) yield return new WaitForEndOfFrame();
        controller.lockLook = false;

        yield return new WaitUntil(() => GameObject.Find("SFX").GetComponent<AudioSource>().volume == 0);

        Destroy(GameObject.Find("SFX"));
    }

    public void ToggleLever()
    {
        float LeverCount = FindObjectsOfType<Lever>().Length;

        if(LeversOn() == 0)
        {
            foreach(UTPulseSender sender in FindObjectsOfType<UTPulseSender>())
            {
                sender.currentPowerState = UTPulseSender.PowerState.off;
            }
            Debug.Log("Levers Off");
        }
        else if(LeversOn() >= 1 && LeversOn() != LeverCount)
        {
            foreach(UTPulseSender sender in FindObjectsOfType<UTPulseSender>())
            {
                sender.currentPowerState = UTPulseSender.PowerState.booting;
            }
            Debug.Log("Levers Booting");
        }
        else if(LeversOn() == LeverCount)
        {
            foreach(UTPulseSender sender in FindObjectsOfType<UTPulseSender>())
            {
                sender.currentPowerState = UTPulseSender.PowerState.on;
            }
            Debug.Log("Levers On!");
        }
    }

    float LeversOn()
    {
        float tempLeversOn = 0;
        foreach(Lever lever in FindObjectsOfType<Lever>())
        {
            if(lever.currentActiveState == true) // Yes I know how to program, this just makes it easier to read, why am I not making this an enumerator instead of a boolean? Who knows! Maybe come back to this to optimize it later
            {
                tempLeversOn++;
            }
        }
        return tempLeversOn;
    }

    public void crashStairs()
    {
        Debug.Log("Should crash");
        foreach(Rigidbody rb in fallingStuff.GetComponentsInChildren<Rigidbody>())
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        AudioSource source = FindObjectOfType<music>().source;
        Invoke("allowPulseToDestroy", 1f);
        source.PlayOneShot(stairBreak, 5);
        LerpSolution.lerpVolume(source, 1, 0.3f, 0);
        source.clip = introStair;
        source.Play();
        StartCoroutine(loopMusic(source));
        // FindObjectOfType<RedTextCreature>().GetComponent<AuraLight>().enabled = true; // AURA HERE
        FindObjectOfType<RedTextCreature>().GetComponent<Light>().enabled = true; // AURA NOT HERE
    }

    IEnumerator loopMusic(AudioSource source)
    {
        yield return GeneralManager.waitForSeconds(source.clip.length - source.time);
        source.clip = loopStair;
        source.Play();
    }

    void allowPulseToDestroy()
    {
        pulseCanDestroy = true;
    }

    public void liftStuff()
    {
        for(int i = 0; i < 10; i++)
        {
            foreach (Rigidbody rb in fallingStuff.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddForce((pushObject.position - rb.position) * 500);
            }
        }
        pulseCanDestroy = false;
        playerBlocker.isTrigger = true;
    }

    public void StartCutscene()
    {
        StartCoroutine(cutsceneEnum());
    }

    IEnumerator cutsceneEnum()
    {
        controller.lockLook = true;
        controller.lockMovement = true;
        controller.lockGravity = true;
        controller.controller.enabled = false;
        AudioSource source = FindObjectOfType<music>().source;

        LerpSolution.lerpVolume(source, 0, 0.3f);

        AsyncOperation asyncHub = SceneManager.LoadSceneAsync("hub_main");
        asyncHub.allowSceneActivation = false;

        Vector3 playerTargetPos = new Vector3(door.position.x, controller.transform.position.y, controller.transform.position.z);
        LerpSolution.lerpPosition(controller.transform, playerTargetPos, 1);
        LerpSolution.lerpRotation(controller.playerCamera, Quaternion.Euler(0, 0, 0), 1);

        while(controller.transform.position != playerTargetPos)
        {
            yield return new WaitForEndOfFrame();
        }
        AudioSource mainSource = gameObject.AddComponent<AudioSource>();
        mainSource.outputAudioMixerGroup = shhh;
        mainSource.clip = doorMusic;
        mainSource.Play();
        StartCoroutine(ForcePulseDefean());
        LerpSolution.lerpPosition(controller.transform, endPos.position, 0.06f);
        yield return GeneralManager.waitForSeconds(4);

        Vignette currentVignette = currentVolume.profile.GetSetting<Vignette>();
        Vignette targetVignette = targetProfile.GetSetting<Vignette>();
        LerpSolution.lerpVignetteIntensity(currentVignette, targetVignette.intensity, 0.1f);

        ColorGrading currentColourGrading = currentVolume.profile.GetSetting<ColorGrading>();
        ColorGrading targetColourGrading = targetProfile.GetSetting<ColorGrading>();
        LerpSolution.LerpColourGradingGeneral(currentColourGrading, targetColourGrading.temperature, targetColourGrading.tint, targetColourGrading.mixerRedOutRedIn, targetColourGrading.mixerRedOutGreenIn, targetColourGrading.mixerRedOutBlueIn, 0.08f);


        yield return GeneralManager.waitForSeconds(1);
        // LerpSolution.LerpLightScattering(doorLight, 0.277f, 0.2f); // AURA HERE
        //LerpSolution.lerpCamFov(controller.playerCamera.GetComponent<Camera>(), 40, 0.1f);
        yield return GeneralManager.waitForSeconds(5.4f);
        LerpSolution.lerpRotation(controller.playerCamera, Quaternion.Euler(0, 0, 26), 0.1f);
        yield return GeneralManager.waitForSeconds(3.5f);
        foreach(UTPulseSender sender in FindObjectsOfType<UTPulseSender>())
        {
            sender.canShakeCamera = false;
        }
        CameraShake.Shake(5, 0.08f);
        
        while(controller.transform.position != endPos.position)
        {
            yield return new WaitForEndOfFrame();
        }

        PlayerPrefs.SetInt("glotchiness", 2);
        asyncHub.allowSceneActivation = true;
    }

    IEnumerator ForcePulseDefean()
    {
        while(true)
        {
            foreach(UTPulseLight source in FindObjectsOfType<UTPulseLight>())
            {
                source.GetComponent<AudioLowPassFilter>().cutoffFrequency = 614;
            }// 614
            yield return new WaitForEndOfFrame();
        }
    }


    void StateUpdate(UTGameManager _manager)
    {
        
    }

    public void CheckStateStarted(UTGameManager _manager, Image _screen)
    {
        if(!hasStarted)
        {
            StateStart(_manager);
            StateUpdate(_manager);
            hasStarted = true;
        }
        else
        {
            StateUpdate(_manager);
        }
    }

    public void OnSwitch(Light light)
    {

    }

    public void ShowTutorial()
    {
        tutText.text = "Click or Press E to interact";
        LerpSolution.lerpTextColour(tutText, Color.white, 0.2f, Color.clear);
    }
}
