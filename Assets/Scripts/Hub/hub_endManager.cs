using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.SceneManagement;

public class hub_endManager : MonoBehaviour
{
    [SerializeField] Transform playerSpawnPos;
    [SerializeField] GameObject blackRoom, door, text, whiteCube, particles, hiddenSong;
    [SerializeField] Image blackScreen;
    [SerializeField] List<VoiceLine> lines = new List<VoiceLine>();
    [SerializeField] AudioClip click1;
    [SerializeField] GameObject noose;
    [SerializeField] PostProcessVolume volumeNew, volumeOld;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip endSong, choke;
    [SerializeField] TextMeshPro[] texts;

    PlayerController controller;
    VoiceLineManager VLManager;

    private void Start()
    {
        FindObjectOfType<DRP>().RP("And now I can't take it back.");
        controller = FindObjectOfType<PlayerController>();
        VLManager = FindObjectOfType<VoiceLineManager>();

        controller.playerCamera.GetComponent<Camera>().backgroundColor = Color.black;
        controller.transform.rotation = playerSpawnPos.rotation;
        controller.TeleportPlayer(playerSpawnPos.position);
        controller.walkSpeed = 2f;
        Destroy(blackRoom);
        Destroy(text);
        Destroy(whiteCube);
        Destroy(hiddenSong);
        //Instantiate(door);
        particles.SetActive(true);

        LerpSolution.lerpImageColour(blackScreen, Color.clear, 0.5f, Color.black);
    }

    public void SayLine()
    {
        VLManager.PlayLine(lines[0]);
        lines.RemoveAt(0);
    }

    public void NooseAppear()
    {
        source.PlayOneShot(click1);
        noose.SetActive(true);
        LerpSolution.LerpPPweight(volumeNew, 0.3f, 0.5f);
        LerpSolution.LerpPPweight(volumeOld, 0.7f, 0.5f);

        // Maybe play ambience
    }

    public void EndIt() { StartCoroutine(EndItEnum()); }

    IEnumerator EndItEnum()
    {
        yield return GeneralManager.waitForSeconds(1f); 
        source.clip = endSong;
        source.Play();
        yield return GeneralManager.waitForSeconds(2f);
        LerpSolution.LerpPPweight(volumeNew, 1f, 0.06f);
        LerpSolution.LerpPPweight(volumeOld, 0f, 0.06f);
        CameraShake.Shake(50, 0.06f);
        Camera cam = controller.playerCamera.GetComponent<Camera>();
        //controller.playerCamera = null;
        GameObject controllerGo = controller.gameObject;
        Destroy(controller);
        GameObject.Find("rot").transform.LookAt(noose.transform.position);
        LerpSolution.lerpRotationTime(controllerGo.transform, GameObject.Find("rot").transform.rotation, 5f);
        LerpSolution.lerpPositionTime(controllerGo.transform, noose.transform.position, 22f);
        yield return GeneralManager.waitForSeconds(18);
        StartCoroutine(spawnText());

        yield return new WaitUntil(() => !source.isPlaying);

        source.gameObject.AddComponent<AudioDistortionFilter>();
        source.PlayOneShot(choke);
        blackScreen.color = Color.black;

        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("LostInThoughts");
        sceneOp.allowSceneActivation = false;

        yield return GeneralManager.waitForSeconds(7);

        sceneOp.allowSceneActivation = true;
    }

    IEnumerator spawnText()
    {
        glitchText gText = GetComponent<glitchText>();
        foreach (TextMeshPro redText in texts)
        {
            StartCoroutine(gText.createGlitchedText("NO ESCAPE", redText));
            yield return GeneralManager.waitForSeconds(0.03f);
        }
    }
}
