using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TMPro;

public class lookAwayJumpScare : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject wall;
    [SerializeField] Animator doorAnim;
    [SerializeField] Texture2D targetMaterial;
    [SerializeField] AudioSource jazzSource, doorSource;
    [SerializeField] GameObject returnToGameText, blockPlayer;
    [SerializeField] RawImage blackScreen;
    [SerializeField] Camera otherCam;
    PostProcessVolume _post;
    ColorGrading _colorGrading;
    Bloom _bloom;
    LensDistortion _lensDistort;
    ChromaticAberration _chromaticAberration;
    //these refrences
    public int doorStep;
    Camera cam;
    PlayerController _PlayerController;
    MeshRenderer textRenderer;
    [HideInInspector] public MeshRenderer wallRenderer;
    music _music;
    Animator camAnim;


    void Start()
    {
        textRenderer = GetComponent<MeshRenderer>();
        wallRenderer = wall.GetComponent<MeshRenderer>();
        cam = player.GetComponentInChildren<Camera>();
        _PlayerController = player.GetComponent<PlayerController>();
        _music = FindObjectOfType<music>();
        _post = cam.GetComponent<PostProcessVolume>();
        _post.profile.TryGetSettings(out _colorGrading);
        _post.profile.TryGetSettings(out _bloom);
        _post.profile.TryGetSettings(out _lensDistort);
        _post.profile.TryGetSettings(out _chromaticAberration);
    }

    void Update()
    {
        if(wall != null && textRenderer.IsVisibleFrom(cam) && !wallRenderer.IsVisibleFrom(cam))
        {
           wall.SetActive(false);
           StartCoroutine(checkWhenLookBack());
        }

        switch (doorStep)
        {
            case 1:
                doorAnim.SetTrigger("doorOpen");
                doorSource.enabled = true;
                break;
            case 2:
                doorAnim.SetTrigger("doorClose");
                StartCoroutine(jumpScareNoise());
                doorStep++;
                break;
        }
    }

    IEnumerator checkWhenLookBack()
    {
        bool isFinished = false;
        while(!isFinished)
        {
            if(wallRenderer.IsVisibleFrom(cam))
            {
                yield return GeneralManager.waitForSeconds(1.2f);
                jazzSource.enabled = true;
                _PlayerController.lockMovement = false;
                _PlayerController.lockGravity = false;
                isFinished = true;
            }
            yield return new WaitForEndOfFrame();
        }
        Destroy(otherCam.gameObject);
    }

    IEnumerator jumpScareNoise()
    {
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync("devIntro");
        sceneAsync.allowSceneActivation = false;
        doorSource.PlayOneShot(_music.jumpScare0);
        StartCoroutine(jumpScareCommit());
        yield return GeneralManager.waitForSeconds(_music.jumpScare0.length - 5);
        LerpSolution.lerpCamFov(cam, 100, 0.2f);
        LerpSolution.lerpLensDistortion(_lensDistort, -50, 0.3f);
        LerpSolution.lerpChromaticAberration(_chromaticAberration, 0.4f, 0.3f);
        CameraShake.Shake(2, 0.02f);
        yield return GeneralManager.waitForSeconds(2);
        CameraShake.Shake(2, 0.05f);
        yield return GeneralManager.waitForSeconds(2);
        CameraShake.Shake(2, 0.2f);
        yield return GeneralManager.waitForSeconds(1);
        blackScreen.color = Color.black;
        sceneAsync.allowSceneActivation = true;
    }

    IEnumerator jumpScareCommit()
    {
        blockPlayer.SetActive(true);
        //intensity of bloom to 0
        //saturation on colour grading to -35
        //contrast on colour grading to 52
        //red 167, green -120, blue 0
        _bloom.intensity.value = 0;
        _colorGrading.saturation.value = -35;
        _colorGrading.contrast.value = 52;
        _colorGrading.mixerRedOutRedIn.value = 167;
        _colorGrading.mixerRedOutGreenIn.value = -120;
        CameraShake.Shake(0.5f, 0.1f);

        glitchText _gText = GetComponent<glitchText>();

        while(true)
        {
            TextMeshPro text = Instantiate(returnToGameText, randomPos(), Quaternion.Euler(randomEuler())).GetComponent<TextMeshPro>();
            _gText.glitchGlitchedText(text);
            yield return GeneralManager.waitForSeconds(0.06f);
        }
        
    }

    Vector3 randomPos()
    {
        Transform zDoor = doorAnim.GetComponent<Transform>();
        return new Vector3(Random.Range(-28, -22), Random.Range(0, 3), Random.Range(zDoor.position.z, 45));
    }

    Vector3 randomEuler()
    {
        return new Vector3(0, 0, Random.Range(0, 100));
    }
}