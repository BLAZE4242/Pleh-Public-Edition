using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using Aura2API; AURA HERE
using UnityEngine.SceneManagement;
using Kino;

public class LBW_puzzle : MonoBehaviour
{
    LBW_intro introState;

    [Header("Turning on light")]
    [SerializeField] AudioClip turnOnSFX;
    [SerializeField] GameObject walll;
    [Header("Puzzle")]
    [SerializeField] AudioClip beepSFX;
    [SerializeField] MeshRenderer doorFrame;
    [SerializeField] Camera playerCam;
    public GameObject floor;
    [SerializeField] AudioClip echoClick;
    public bool isWalking;
    [Header("Arg")]
    [SerializeField] TextMeshPro argText;
    [SerializeField] AudioClip glitchClip;
    [SerializeField] DigitalGlitch digitalGlitch;
    [Header("After Puzzle")]
    [SerializeField] Transform door;
    [SerializeField] Transform backDoor;
    [SerializeField] Transform blackBarrier;
    [SerializeField] DoorBehavior _door;
    [SerializeField] AudioSource ambie, music;

    // AuraLight spotLight; // AURA HERE
    Light spotLight; // AURA NOT HERE
    TextMeshPro directionText;

    bool hasRun = false;

    public void stateStart()
    {
        Debug.Log("Forward, left, forward, right, forward, left, forward, forward, right, forward, forward");

        introState = GetComponent<LBW_intro>();
        spotLight = introState.spotLight;
        directionText = introState.directionText;

        // spotLight.strength = introState.defaultLightStrength; // AURA HERE
        GetComponent<AudioSource>().PlayOneShot(turnOnSFX);
        directionText.color = introState.defaultDirectionColour;

        walll.SetActive(true);
    }

    [HideInInspector] public bool hasHeardAudio = false;
    public void stateUpdate()
    {
        if (isWalking && !doorFrame.IsVisibleFrom(playerCam))
        {
            if (!hasHeardAudio)
            {
                floor.SetActive(false);
                GetComponent<AudioSource>().PlayOneShot(echoClick, 0.5f);
                hasHeardAudio = true;
            }
        }
    }

    public void StartWalking(bool value)
    {
        isWalking = value;
    }

    public void playBeep()
    {
        GetComponent<AudioSource>().PlayOneShot(beepSFX);
    }

    public void finishPuzzle()
    {
        StartCoroutine(loadHub());
    }

    IEnumerator loadHub()
    {
        Debug.Log("asfsdf");
        LerpSolution.lerpPosition(blackBarrier, blackBarrier.position + new Vector3(0, 16, 0), 3f);
        //LerpSolution.lerpPosition(door, finalDoorPos.position, 3f);
        yield return GeneralManager.waitForSeconds(2f);
        GetComponent<LBW_manager>().sceneOp.allowSceneActivation = true;
        while (!GetComponent<LBW_manager>().sceneOp.isDone) yield return new WaitForEndOfFrame();
        FindObjectOfType<CharacterController>().GetComponentInChildren<Camera>().backgroundColor = Color.white;
        FindObjectOfType<CharacterController>().gameObject.AddComponent<chairHug>().pushPower = 10;
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("hub_main"));
    }

    public void OpenDoorToHub()
    {
        FindObjectOfType<PlayerController>().lockY = true;
        backDoor.SetParent(null);
        _door.OpenDoor(1f);

    }

    public void CloseDoorToHub()
    {

        FindObjectOfType<PlayerController>().ChangeSpeed(5);

        _door.CloseDoor(1f);
        SceneManager.MoveGameObjectToScene(FindObjectOfType<PlayerController>().gameObject, SceneManager.GetSceneByName("hub_main"));
        SceneManager.MoveGameObjectToScene(_door.gameObject, SceneManager.GetSceneByName("hub_main"));
        SceneManager.MoveGameObjectToScene(music.gameObject, SceneManager.GetSceneByName("hub_main"));
        SceneManager.MoveGameObjectToScene(ambie.gameObject, SceneManager.GetSceneByName("hub_main"));

        SceneManager.UnloadSceneAsync("Look both ways");
        LerpSolution.lerpVolume(ambie, 0, 0.4f);
        LerpSolution.lerpVolume(music, 0, 0.4f);


    }

    public void ARGglitch() { StartCoroutine(ARGglitchEnum()); }

    IEnumerator ARGglitchEnum()
    {
        argText.text = "th3 D0or st4anDsSS";
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().clip = glitchClip;
        GetComponent<AudioSource>().Play();

        yield return GeneralManager.waitForSeconds(0.8f);
        digitalGlitch.intensity = 0.4f;

        yield return GeneralManager.waitForSeconds(0.4f);
        argText.text = "";
        Application.Quit();
    }

    public void runState()
    {
        if (!hasRun)
        {
            hasRun = true;
            stateStart();
        }
        else stateUpdate();
    }
}
