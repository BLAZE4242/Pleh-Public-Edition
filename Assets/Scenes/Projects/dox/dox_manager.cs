using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Kino;

public class dox_manager : MonoBehaviour
{
    [SerializeField] TextMeshPro redText;
    glitchText _gText;
    [SerializeField] GameObject camStopWall;
    [SerializeField] GameObject plutoWall;
    [SerializeField] AudioClip click1;
    [SerializeField] Transform centreWall;
    [SerializeField] List<TextMeshPro> texts = new List<TextMeshPro>();
    [SerializeField] AudioSource endSource;
    [SerializeField] AudioClip endSong;
    [SerializeField] AudioClip glitch;
    [Header("Loop")]
    [SerializeField] ButtonHandle button;
    [SerializeField] Transform cube;
    [SerializeField] floorButton2D floorButton;
    [SerializeField] doorTrigger2D door;
    [SerializeField] Transform cubeSpawnPos;
    [SerializeField] CollisionManager redTextCol;
    playerMove controller;
    bool wallsFollow = true;
    Vector3 difference;

    Coroutine friendName;
    private void Start()
    {
        FindObjectOfType<DRP>().RP("Exploring an empty NULL");
        GeneralManager.SetSceneSave();

        controller = FindObjectOfType<playerMove>();
        _gText = FindObjectOfType<glitchText>();
        friendName = StartCoroutine(FindObjectOfType<DRP>().GetFriendNameReady());

        difference = controller.transform.position - plutoWall.transform.position;

        controller.canControl = false;
        controller.PlutoAnimator.SetTrigger("Get up");
    }

    public void teleportPlayer()
    {
        controller.transform.position += new Vector3(-60, 0);
        Camera.main.transform.position += new Vector3(-60, 0);

        button.isTriggered = false;
        button.whatif.color = button.notTriggeredColour;
        cube.position = cubeSpawnPos.position;
        floorButton.cubes = new List<GameObject>();
        floorButton.numOfTrigger = 0;
        floorButton.isWorking = true;
        redText.text = "";
        redTextCol.ResetIfTriggered();
    }

    public void RedTextLine() { StartCoroutine(RedTextLineEnum()); }


    string[] script = new string[] {
        "You should quit",
        "There is nothing interesting to play here",
        "Don't give it any of your attention",
        "Go talk to {friend} instead, they would be more interesting", // replace this with "Go outside, that would be more interesting" if can't get name
        "Just quit now.",
        "Quit"
    };

    int i = -1; // -1
    IEnumerator RedTextLineEnum()
    {
        if(i >= 0)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
                LerpSolution.lerpVolume(GetComponent<AudioSource>(), 0.7f, 0.1f, 0);
            }

            if (script[i] == "Quit") // same as here
            {
                camStopWall.SetActive(true);
                controller.canCamSwapLeft = false;
                controller.canCamSwapRight = false; // should be impossible but still just in case
            }

            yield return GeneralManager.waitForSeconds(script[i] == "Quit" ? 1 : Random.Range(1, 5)); // and here

            string targetText = script[i];
            if (targetText.Contains("{friend}"))
            {
                if (FindObjectOfType<DRP>().friendName != "" && PlayerPrefs.GetInt("Streamer Mode") != 2)
                {
                    targetText = targetText.Replace("{friend}", FindObjectOfType<DRP>().friendName);
                }
                else
                {
                    targetText = "Go outside, that would be more interesting";
                }
            }

            StartCoroutine(_gText.createGlitchedText(targetText, redText));

            if (targetText == "Quit") // should just be Quit
            {
                FindObjectOfType<floorButton2D>().acceptCubes = false;
                StartCoroutine(EndScene());
            }
        }
        i++;
    }

    IEnumerator EndScene()
    {
        LerpSolution.lerpVolume(GetComponent<AudioSource>(), 0, 0.7f);
        yield return GeneralManager.waitForSeconds(2);
        yield return new WaitUntil(() => controller.isGrounded);

        wallsFollow = false;

        // make everything invis
        foreach (SpriteRenderer renderer in FindObjectsOfType<SpriteRenderer>())
        {
            if (renderer != controller.GetComponent<SpriteRenderer>())
            {
                renderer.enabled = false;
                try { renderer.GetComponent<BoxCollider2D>().enabled = false; } catch { }
            }
        }

        button.gameObject.SetActive(false);

        plutoWall.SetActive(true);

        foreach (SpriteRenderer renderer in plutoWall.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = true;
        }

        endSource.PlayOneShot(click1);

        endSource.Play();
        LerpSolution.lerpVolume(endSource, 1, 0.3f, 0);
        yield return GeneralManager.waitForSeconds(1);
        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("Menu 1");
        sceneOp.allowSceneActivation = false;

        LerpSolution.lerpCamSizeTime(Camera.main, 2.75f, 5);
        LerpSolution.lerpPositionTime(Camera.main.transform, centreWall.position, 5);
        LerpSolution.lerpCamColour(Camera.main, Color.black, 0.1f);

        yield return GeneralManager.waitForSeconds(7);
        StartCoroutine(spamQuit());

        LerpSolution.lerpCamSizeTime(Camera.main, 17f, 20);

        yield return GeneralManager.waitForSeconds(22-1.28f);

        LerpSolution.lerpCamSizeTime(Camera.main, 2.75f, 1);
        FindObjectOfType<DigitalGlitch>().intensity = 0.1f;
        PlayerPrefs.SetInt("glotchiness", 3);
        yield return new WaitUntil(() => Camera.main.orthographicSize <= 15f);
        endSource.PlayOneShot(glitch);
        yield return new WaitUntil(() => Camera.main.orthographicSize <= 3f);
        Destroy(FindObjectOfType<playerMove>().gameObject);
        sceneOp.allowSceneActivation = true;
    }

    IEnumerator spamQuit()
    {
        _gText.clickSfx = null;
        while (true)
        {
            TextMeshPro targetText = texts[Random.Range(0, texts.Count - 1)];
            StartCoroutine(_gText.createGlitchedText("Quit", targetText));
            targetText.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-25, 25)));

            texts.Remove(targetText);

            yield return new WaitForSeconds(0.06f);
        }
    }

    private void Update()
    {
        if(wallsFollow) plutoWall.transform.position = controller.transform.position + difference;
    }
}
