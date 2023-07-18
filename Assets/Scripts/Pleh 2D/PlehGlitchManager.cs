using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PlehGlitchManager : MonoBehaviour
{
    [SerializeField] Animator camAnim;
    playerMove pluto;

    [Header("Cube Glitch")]
    [SerializeField] SpriteRenderer doorRenderer;
    Coroutine doorGlitch;

    [Header("Fall Glitch")]
    [SerializeField] SpriteRenderer[] spritesToGlitch;
    [SerializeField] Sprite glitchSprite;
    [SerializeField] Transform[] CamCutRightPos;

    [Header("Door preview")]
    [SerializeField] Transform doorCamPos;
    [SerializeField] TextMeshPro doorRedText;
    [SerializeField] string[] redTextDoorScript = { "He can't make the game crash anymore", "I made sure of it", "Let's try this again." };
    Camera cam;
    glitchText _gText;
    [HideInInspector] public bool overrideCamSwap = false;

    [Header("Level 3")]
    [SerializeField] Transform platform;
    [SerializeField] Transform platform1;
    [SerializeField] Transform platform2;
    [SerializeField] Transform nextPlatform;
    [SerializeField] float platformFallSpeed, platformYend, platformYend2;

    [Header("Level 5")]
    [SerializeField] BoxCollider2D platform5;

    [Header("Fuck you I'm done labelling code")]
    [SerializeField] Transform camPosfuckyou;
    [SerializeField] ParticleSystem[] confetti;
    [SerializeField] AudioClip confettiSFX;
    [SerializeField] Transform[] flickerObjects;
    [SerializeField] TextMeshPro[] redTextsBeatGame;
    [SerializeField] AudioClip click;
    [SerializeField] GameObject cubesAtBeat;
    [SerializeField] Animator doorAtBeat;
    [SerializeField] PostProcessProfile glitchProfile;
    [SerializeField] GameObject goInside;
    [SerializeField] AudioClip glitchSong;
    [SerializeField] AudioClip staticSound;
    public AsyncOperation sceneOp;

    private IEnumerator Start()
    {
        GeneralManager.SetSceneSave("Menu 1");
        pluto = FindObjectOfType<playerMove>();
        cam = camAnim.GetComponent<Camera>();
        _gText = GetComponent<glitchText>();

        yield return GeneralManager.waitForSeconds(5);
        Destroy(FindObjectOfType<ScrollUV>().gameObject);
    }

    public void ToggleLeftCam(bool canUse)
    {
        overrideCamSwap = true;
        pluto.canCamSwapLeft = canUse;
    }
    public void ToggleRightCam(bool canUse)
    {
        overrideCamSwap = true;
        pluto.canCamSwapRight = canUse;
    }

    #region Cube Glitch
    public void GlitchCube(Rigidbody2D cubeRb) { StartCoroutine(GlitchCubeEnum(cubeRb)); }

    IEnumerator GlitchCubeEnum(Rigidbody2D cubeRb)
    {
        yield return GeneralManager.waitForSeconds(1f);

        camAnim.SetTrigger("Light");
        FindObjectOfType<music>().staticGlitch();

        yield return GeneralManager.waitForSeconds(0.2f);

        pluto.DropCube();
        cubeRb.gravityScale = -5;

        doorGlitch = StartCoroutine(GlitchDoor());
    }

    IEnumerator GlitchDoor()
    {
        Vector2 waitTime = FindObjectOfType<StarfieldGlitch>().waitTime;
        Vector2 glitchTime = FindObjectOfType<StarfieldGlitch>().glitchTime;

        doorRenderer.GetComponent<BoxCollider2D>().enabled = false;

        while (true)
        {
            yield return GeneralManager.waitForSeconds(Random.Range(waitTime.x, waitTime.y));

            doorRenderer.enabled = false;

            yield return GeneralManager.waitForSeconds(Random.Range(glitchTime.x, glitchTime.y));

            doorRenderer.enabled = true;
        }
    }

    #endregion

    #region Glitch Textures

    public void GlitchTextures() { StartCoroutine(GlitchTexturesEnum()); }

    IEnumerator GlitchTexturesEnum()
    {
        foreach (SpriteRenderer renderer in spritesToGlitch)
        {
            renderer.sprite = glitchSprite;
            yield return GeneralManager.waitForSeconds(0.2f);
        }
    }

    #endregion

    #region Door Preview

    public void DoorPreview() { StartCoroutine(DoorPreviewEnum()); }

    IEnumerator DoorPreviewEnum()
    {
        if(doorGlitch != null) StopCoroutine(doorGlitch);

        AudioLowPassFilter lowPass = FindObjectOfType<music>().gameObject.AddComponent<AudioLowPassFilter>();
        lowPass.cutoffFrequency = 350;

        foreach (SpriteRenderer renderer in spritesToGlitch)
        {
            renderer.enabled = false;
        }

        pluto.canControl = false;
        pluto.canCamSwapScene = false;

        cam.orthographicSize = 5;
        Vector3 defaultCamPos = cam.transform.position;
        cam.transform.position = doorCamPos.position;

        yield return GeneralManager.waitForSeconds(3);

        foreach(string line in redTextDoorScript)
        {
            StartCoroutine(_gText.createGlitchedText(line, doorRedText));
            yield return GeneralManager.waitForSeconds(2.5f);
        }

        yield return GeneralManager.waitForSeconds(1);

        Destroy(lowPass);

        foreach (SpriteRenderer renderer in spritesToGlitch)
        {
            renderer.enabled = true;
        }

        cam.orthographicSize = 10;
        cam.transform.position = defaultCamPos;
        pluto.canControl = true;
        pluto.canCamSwapScene = true;
    }

    #endregion

    #region Glitched Levels

    public void SpawnGlitchLevel(PlehGlitchedLevel level) { StartCoroutine(SpawnGlitchLevelEnum(level)); }

    IEnumerator SpawnGlitchLevelEnum(PlehGlitchedLevel level)
    {
        yield return GeneralManager.waitForSeconds(level.delayTime);

        foreach (TextMeshPro text in level.texts)
        {
            text.gameObject.SetActive(true);
            yield return GeneralManager.waitForSeconds(level.spawnWaitTime);
        }

        for (int i = 0; i < level.platforms.Length; i++)
        {
            level.platforms[i].SetActive(true);
            level.texts[i].gameObject.SetActive(false);
            yield return GeneralManager.waitForSeconds(level.spawnWaitTime);
        }
    }

    bool hasTriggered = false;

    public void TriggerEndPuzzle3(doorTrigger2D door) 
    {
        pluto.camKill = false;
        if (door.transform.localPosition.y != 0 && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(TriggerEndPuzzle3Enum());
        }
        else Debug.Log(door.transform.localPosition.y);
    }

    IEnumerator TriggerEndPuzzle3Enum()
    {
        Debug.Log("WTFFFF");
        pluto.canCamSwapScene = false;

        Transform targetPlatform = null;
        float targetYend = platformYend;

        if (platform != null)
        {
            targetPlatform = platform;
        }
        else if(platform1 != null)
        {
            targetPlatform = platform1;
            targetYend = platformYend2;
            nextPlatform = platform2;

            platform5.enabled = true;
        }
        else if(platform2 != null)
        {
            StartCoroutine(holyShitJustFuckOff());
        }

        Debug.Log(nextPlatform.name);

        foreach (BoxCollider2D barrier in targetPlatform.GetComponentsInChildren<BoxCollider2D>())
        {
            if (barrier.transform.name == "Barrier") barrier.enabled = true;
        }

        yield return GeneralManager.waitForSeconds(1);

        cam.transform.SetParent(targetPlatform);
        pluto.transform.SetParent(targetPlatform);

        Vector3 targetPos = new Vector3(targetPlatform.position.x, targetYend, targetPlatform.position.z);
        LerpSolution.lerpPosition(targetPlatform, targetPos, 0.2f);

        while (targetPlatform.position != targetPos) yield return new WaitForEndOfFrame();

        pluto.transform.SetParent(null);
        cam.transform.SetParent(null);
        pluto.canCamSwapScene = true;

        foreach (BoxCollider2D barrier in targetPlatform.GetComponentsInChildren<BoxCollider2D>())
        {
            if (barrier.transform.name == "Barrier") barrier.enabled = false;
        }

        if (pluto.transform.position.y > targetPlatform.position.y + 10) pluto.transform.position = new Vector3(pluto.transform.position.x, targetPlatform.position.y + 3, pluto.transform.position.z);
        yield return GeneralManager.waitForEndOfFrame;
        Destroy(targetPlatform.gameObject);
        nextPlatform.gameObject.SetActive(true);

        hasTriggered = false;

        if (platform1 == null)
        {
            platform5.enabled = true;
        }

    }

    public void fuckYou() { StartCoroutine(holyShitJustFuckOff()); }

    IEnumerator holyShitJustFuckOff()
    {
        pluto.canCamSwapScene = false;

        yield return GeneralManager.waitForSeconds(1);
        camAnim.SetTrigger("Light");
        AudioLowPassFilter lowPass = FindObjectOfType<music>().gameObject.AddComponent<AudioLowPassFilter>();
        lowPass.cutoffFrequency = 350;

        Vector3 camPosBefore = cam.transform.position;
        cam.transform.position = camPosfuckyou.position;
        Vector3 difference = camPosBefore - camPosfuckyou.position;
        pluto.transform.position -= difference;

        yield return GeneralManager.waitForSeconds(1);

        foreach (ParticleSystem part in confetti)
        {
            part.Play();
            CameraShake.Shake(0.4f, 0.2f, true, true);
        }

        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(confettiSFX);

        yield return GeneralManager.waitForSeconds(4);


        float[] strobeTimes = { 0.1f, 0.1f, 0.4f, 0.05f, 0.09f, 0.2f };

        sceneOp = SceneManager.LoadSceneAsync("hub_main");
        sceneOp.allowSceneActivation = false;

        for (int i = 0; i < flickerObjects.Length; i++)
        {
            Vector3 defaultPos = flickerObjects[i].position;
            foreach (float strobe in strobeTimes)
            {
                yield return GeneralManager.waitForSeconds(strobe);
                flickerObjects[i].position = Vector3.zero;
                yield return GeneralManager.waitForSeconds(0.07f);
                flickerObjects[i].position = defaultPos;
            }
            Destroy(flickerObjects[i].gameObject);
            redTextsBeatGame[i].gameObject.SetActive(true);
            _gText.glitchGlitchedText(redTextsBeatGame[i]);

            yield return GeneralManager.waitForSeconds(2);
        }

        yield return GeneralManager.waitForSeconds(2);

        AudioSource sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        pluto._music.source.Stop();
        sfx.PlayOneShot(click);
        foreach (TextMeshPro text in redTextsBeatGame)
        {
            text.gameObject.SetActive(false);
        }

        yield return GeneralManager.waitForSeconds(0.3f);
        sfx.PlayOneShot(click);
        cubesAtBeat.SetActive(false);
        yield return GeneralManager.waitForSeconds(0.3f);
        sfx.PlayOneShot(click);
        doorAtBeat.gameObject.SetActive(true);
        yield return GeneralManager.waitForSeconds(0.3f);
        sfx.PlayOneShot(click);
        camAnim.SetTrigger("!Heavy");
        cam.GetComponent<PostProcessVolume>().profile = glitchProfile;
        goInside.SetActive(true);
        _gText.glitchGlitchedText(goInside.GetComponent<TextMeshPro>());

        Destroy(lowPass);
        pluto._music.source.clip = glitchSong;
        pluto._music.source.Play();
        sfx.loop = true;
        sfx.clip = staticSound;
        sfx.volume = 0.3f;
        sfx.Play();
    }



    #endregion
}
