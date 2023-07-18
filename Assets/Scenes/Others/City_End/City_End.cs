using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DigitalRuby.RainMaker;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class City_End : MonoBehaviour
{
    [SerializeField] PostProcessVolume profile;
    [SerializeField] AudioClip LightningSfx;
    [SerializeField] AudioClip elevatorDoorSfx;
    [SerializeField] Transform elevatorDoor;
    [SerializeField] GameObject elevatorCol;
    [SerializeField] AudioSource sourceWithoutLowpass;
    [SerializeField] AudioClip elevatorMoveSfx;
    [SerializeField] Transform Light;
    [SerializeField] AudioClip elevatorStopSfx;
    [SerializeField] List<VoiceLine> script = new List<VoiceLine>();
    [SerializeField] Image blackScreen;
    [SerializeField] TextMeshPro argText;
    ColorGrading _grade;
    VoiceLineManager VLManager;
    bool canLightning = true;
    Coroutine lightning;
    bool hasBeenUp = false;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<DRP>().RP("And now I can't take it back.");
        GeneralManager.SetSceneSave();
        profile.profile.TryGetSettings(out _grade);
        VLManager = FindObjectOfType<VoiceLineManager>();
        lightning = StartCoroutine(LightningSequence());

        CheckArg();
    }

    void CheckArg()
    {
        string final = arg("LostInThoughts") + arg("New Messages") + arg("RedText Intro") + arg("Amelia_Restaurant") + arg("Amelia_Ocean") + arg("hub_main") + arg("Dev_Confess") + arg("New MessagesConfess");
        if (final == "4322122232142333122312134211321134244444")
        {
            argText.text = "itstoolateformenow";
        }
    }

    string arg(string sceneName)
    {
        return PlayerPrefs.GetString(sceneName + " arg");
    }

    IEnumerator LightningSequence()
    {
        StartCoroutine(Lightning(true));
        while (canLightning)
        {
            yield return GeneralManager.waitForSeconds(Random.Range(5, 20));
            StartCoroutine(Lightning());
        }
    }

    IEnumerator Lightning(bool instant = false)
    {
        if (instant)
        {
            LerpSolution.lerpCamFov(FindObjectOfType<Camera>(), FindObjectOfType<Camera>().fieldOfView, 3f, 1);

            GetComponent<AudioLowPassFilter>().enabled = false;
            gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = 0.7f;
            GetComponent<AudioSource>().PlayOneShot(LightningSfx, 0.6f);
        }
        LerpSolution.LerpPostExposure(_grade, 3.5f, 2f, 0);
        yield return GeneralManager.waitForSeconds(0.2f);
        LerpSolution.LerpPostExposure(_grade, 0f, 1f, 3.5f);
        yield return GeneralManager.waitForSeconds(0.3f);

        LerpSolution.LerpPostExposure(_grade, 3.5f, 2f, 0);
        yield return GeneralManager.waitForSeconds(0.2f);
        LerpSolution.LerpPostExposure(_grade, 0f, 0.4f, 3.5f);
        yield return GeneralManager.waitForSeconds(0.3f);

        if (!instant)
        {
            yield return GeneralManager.waitForSeconds(Random.Range(2, 4));
            GetComponent<AudioLowPassFilter>().cutoffFrequency = Random.Range(600, 2000);
            GetComponent<AudioSource>().PlayOneShot(LightningSfx);
        }
        else
        {
            yield return GeneralManager.waitForSeconds(5f);
            GetComponent<AudioLowPassFilter>().enabled = true;
            Destroy(GetComponent<AudioDistortionFilter>());
        }
    }

    public void GoInside(bool stopRain)
    {
        FindObjectOfType<RainScript>().FollowCamera = stopRain;
        if (stopRain)
        {

            foreach (ParticleSystem trans in FindObjectOfType<RainScript>().GetComponentsInChildren<ParticleSystem>())
            {
                trans.transform.position = Vector3.zero;
            }
        }
    }

    int i = 0;
    public void PlayNextLine()
    {
        if (i == 0) StartCoroutine(softElevator());
        VLManager.PlayLine(script[0]);
        script.RemoveAt(0);
        i++;
    }

    IEnumerator softElevator()
    {
        LerpSolution.lerpVolume(sourceWithoutLowpass, 0.5f, 2);
        yield return GeneralManager.waitForSeconds(1);
        LerpSolution.lerpVolume(sourceWithoutLowpass, 1, 1);
    }

    bool isRiding = false;
    bool hasHeardFirst = false;
    public void ElevatorCloseDoors(bool goingUp)
    {
        if (isRiding) return;
        isRiding = true;
        if (goingUp) StopCoroutine(lightning);
        if (!goingUp) StartCoroutine(LightningSequence());
        LerpSolution.LerpRainIntensity(FindObjectOfType<RainScript>(), 0.266f, 0.4f);
        GetComponent<AudioSource>().PlayOneShot(elevatorDoorSfx);
        elevatorCol.SetActive(true);
        LerpSolution.lerpPosition(elevatorDoor, new Vector3(-37.45f, elevatorDoor.position.y, elevatorDoor.position.z), 1);
        if (goingUp) StartCoroutine(ElevatorUp());
        else StartCoroutine(ElevatorDown());
    }

    IEnumerator ElevatorUp()
    {
        hasBeenUp = true;
        sourceWithoutLowpass.loop = true;
        sourceWithoutLowpass.clip = elevatorMoveSfx;
        sourceWithoutLowpass.Play();
        yield return GeneralManager.waitForSeconds(1.5f);
        CameraShake.Shake(1, 0.1f);
        yield return GeneralManager.waitForSeconds(1);
        CameraShake.Shake(50, 0.02f);
        for (int i = 8; i > 0; i--)
        {
            if (i == 7 && !hasHeardFirst)
            {
                PlayNextLine();
            }

            Light.localPosition = new Vector3(Light.localPosition.x, 7.5f, Light.localPosition.z);
            LerpSolution.lerpPosition(Light, new Vector3(Light.localPosition.x, -3, Light.localPosition.z), 1, true);
            yield return GeneralManager.waitForSeconds(1.3f);
        }
        FindObjectOfType<PlayerController>().walkSpeed /= 2;
        Transform controller = FindObjectOfType<PlayerController>().transform;

        FindObjectOfType<PlayerController>().TeleportPlayer(new Vector3(controller.position.x - 10, controller.position.y, controller.position.z));
        sourceWithoutLowpass.Stop();
        sourceWithoutLowpass.PlayOneShot(elevatorStopSfx);
        CameraShake.Shake(1f, 0.1f);
        yield return GeneralManager.waitForSeconds(4);
        GetComponent<AudioSource>().PlayOneShot(elevatorDoorSfx);
        elevatorCol.SetActive(false);
        LerpSolution.lerpPosition(elevatorDoor, new Vector3(-39.09f, elevatorDoor.position.y, elevatorDoor.position.z), 1);
        yield return GeneralManager.waitForSeconds(1);
        isRiding = false;
        if (!hasHeardFirst)
        {
            PlayNextLine();
            hasHeardFirst = true;
        }
    }

    IEnumerator ElevatorDown()
    {
        sourceWithoutLowpass.loop = true;
        sourceWithoutLowpass.clip = elevatorMoveSfx;
        sourceWithoutLowpass.Play();
        yield return GeneralManager.waitForSeconds(1.5f);
        CameraShake.Shake(1, 0.1f);
        yield return GeneralManager.waitForSeconds(1);
        CameraShake.Shake(50, 0.02f);
        for (int i = 8; i > 0; i--)
        {
            Light.localPosition = new Vector3(Light.localPosition.x, -3, Light.localPosition.z);
            LerpSolution.lerpPosition(Light, new Vector3(Light.localPosition.x, 7.5f, Light.localPosition.z), 1, true);
            yield return GeneralManager.waitForSeconds(1.3f);
        }
        FindObjectOfType<PlayerController>().walkSpeed *= 2;
        Transform controller = FindObjectOfType<PlayerController>().transform;

        FindObjectOfType<PlayerController>().TeleportPlayer(new Vector3(controller.position.x + 10, controller.position.y, controller.position.z));
        sourceWithoutLowpass.Stop();
        sourceWithoutLowpass.PlayOneShot(elevatorStopSfx);
        CameraShake.Shake(1f, 0.1f);
        yield return GeneralManager.waitForSeconds(4);
        GetComponent<AudioSource>().PlayOneShot(elevatorDoorSfx);
        elevatorCol.SetActive(false);
        LerpSolution.lerpPosition(elevatorDoor, new Vector3(-39.09f, elevatorDoor.position.y, elevatorDoor.position.z), 1);
        yield return GeneralManager.waitForSeconds(1);
        isRiding = false;
    }

    public void end_section() { StartCoroutine(end_sectionEnum()); }

    IEnumerator end_sectionEnum()
    {
        PlayerPrefs.SetInt("glotchiness", 7);
        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("hub_main");
        sceneOp.allowSceneActivation = false;
        LerpSolution.lerpImageColour(blackScreen, Color.black, 0.6f);
        LerpSolution.LerpRainIntensity(FindObjectOfType<RainScript>(), 0, 1);
        yield return GeneralManager.waitForSeconds(1);
        sceneOp.allowSceneActivation = true;
    }

    public void GoBack()
    {
        if (hasBeenUp)
        {
            StartCoroutine(GoBackEnum());
        }
    }

    IEnumerator GoBackEnum()
    {
        blackScreen.color = Color.black;
        AudioListener.pause = true;
        Gas.EarnAchievement("ACH_TRIGGERING");

        yield return GeneralManager.waitForSeconds(3);

        PlayerPrefs.SetInt("glotchiness", 7);
        AudioListener.pause = false;
        SceneManager.LoadScene("LostInThoughts");
    }
}
