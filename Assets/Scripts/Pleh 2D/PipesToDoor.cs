using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PipesToDoor : MonoBehaviour
{
    [SerializeField] AudioClip glitch01;
    [SerializeField] TextMeshPro cubeGlitchText;
    [Header("Objects for glitch")]
    [SerializeField] GameObject cubes;
    [SerializeField] GameObject text;
    [SerializeField] GameObject sidewall;
    [SerializeField] RuntimeAnimatorController glitchAnim;
    [Header("Glitch 1")]
    [SerializeField] TextMeshPro text1;
    [SerializeField] Transform exit1, exitGlitched;
    [Header("Glitch 2")]
    [SerializeField] GameObject cube;
    [Header("Glitch 3")]
    [SerializeField] GameObject invisWalls;
    [SerializeField] Transform floor;
    [SerializeField] Transform camFallPos;
    playerMove controller;
    Rigidbody2D controllerRb;

    private void Start()
    {
        controller = FindObjectOfType<playerMove>();
        controllerRb = controller.GetComponent<Rigidbody2D>();
    }

    public void fuckTheCube()
    {
        cubeGlitchText.gameObject.SetActive(true);
    }

    public void makePipesInvis()
    {
        StartCoroutine(makePipesInvisEnum());
    }

    IEnumerator makePipesInvisEnum()
    {
        yield return GeneralManager.waitForSeconds(2f);

        foreach (SpriteRenderer pipe in GetComponentsInChildren<SpriteRenderer>())
        {
            pipe.enabled = false;
        }
    }

    public void sike()
    {
        StartCoroutine(sikeEnum());
    }

    bool isGlitchin = false;

    IEnumerator sikeEnum()
    {
        GameObject.Find("Camera Final Pos").transform.position = GameObject.Find("Camera Final Pos 2").transform.position;

        Time.timeScale = 0f;
        StartCoroutine(FindObjectOfType<music>().musicGetGlitch(4, MusicGetUnGlitch));
        isGlitchin = true;

        while (isGlitchin) yield return new WaitForEndOfFrame();
        Time.timeScale = 1f;

        smallGlitch();

        cubes.SetActive(true);
        text.SetActive(true);
        GetComponent<glitchText>().glitchGlitchedText(text.GetComponent<TextMeshPro>());
        sidewall.SetActive(false);

        foreach (SpriteRenderer pipe in GetComponentsInChildren<SpriteRenderer>())
        {
            pipe.enabled = true;
        }

        LerpSolution.lerpPitch(GameObject.Find("Music").GetComponent<AudioSource>(), 1.1f, 2f);
        while (GameObject.Find("Music").GetComponent<AudioSource>().pitch != 1.1f) yield return new WaitForEndOfFrame();
        yield return GeneralManager.waitForSeconds(0.4f);
        LerpSolution.lerpPitch(GameObject.Find("Music").GetComponent<AudioSource>(), 0.98f, 0.3f);

    }

    private void smallGlitch()
    {
        try
        {
            FindObjectOfType<Camera>().GetComponent<Animator>().runtimeAnimatorController = glitchAnim;
            FindObjectOfType<Camera>().GetComponent<Animator>().SetTrigger("Light");
        }
        catch
        {
            Debug.Log("Couldn't find animator");
        }
        FindObjectOfType<music>().staticGlitch();
    }

    public void MusicGetUnGlitch()
    {
        isGlitchin = false;
    }

    public void SkipPuzzle01() { StartCoroutine(SkipPuzzle01Enum()); }
    public void SkipPuzzle02() { StartCoroutine(SkipPuzzle02Enum()); }
    public void SkipPuzzle03() { StartCoroutine(SkipPuzzle03Enum()); }
    public void FallSequence() { StartCoroutine(FallSequenceEnum()); }

    IEnumerator SkipPuzzle01Enum()
    {
        controller.lockRestart = true;

        yield return GeneralManager.waitForSeconds(2);

        text1.gameObject.SetActive(true);
        GetComponent<glitchText>().glitchGlitchedText(text1);

        yield return GeneralManager.waitForSeconds(1f);

        LerpSolution.lerpPosition(exit1, exitGlitched.position, 4f);
        LerpSolution.lerpRotation(exit1, exitGlitched.rotation, 4f);
        LerpSolution.lerpScale(exit1, exitGlitched.lossyScale, 4f);
        smallGlitch();
        CameraShake.Shake(0.3f, 0.2f, true, true);

        yield return GeneralManager.waitForSeconds(0.3f);

        exit1.gameObject.SetActive(false);
    }

    IEnumerator SkipPuzzle02Enum()
    {
        yield return GeneralManager.waitForSeconds(1f);

        for (int i = 1; i <= 4; i++)
        {
            Instantiate(cube, GameObject.Find("Cube spawn_" + i).transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector3.down * 10;
            smallGlitch();
            CameraShake.Shake(0.3f, 0.2f, true, true);
            yield return GeneralManager.waitForSeconds(0.3f);
        }
    }

    IEnumerator SkipPuzzle03Enum()
    {
        Vector3 defaultPos = floor.position;
        Quaternion defaultRot = floor.rotation;
        Vector3 defaultScale = floor.lossyScale;

        CameraShake.Shake(0.6f, 0.4f, true, true);
        smallGlitch();
        LerpSolution.lerpPosition(floor, GameObject.Find("Floor Glitch 1").transform.position, 6);
        LerpSolution.lerpRotation(floor, GameObject.Find("Floor Glitch 1").transform.rotation, 6);
        LerpSolution.lerpScale(floor, GameObject.Find("Floor Glitch 1").transform.lossyScale, 6);
        
        yield return GeneralManager.waitForSeconds(0.3f);
        smallGlitch();
        floor.position = defaultPos;
        floor.rotation = defaultRot;
        floor.localScale = defaultScale;

        LerpSolution.lerpPosition(floor, GameObject.Find("Floor Glitch 2").transform.position, 6);
        LerpSolution.lerpRotation(floor, GameObject.Find("Floor Glitch 2").transform.rotation, 6);
        LerpSolution.lerpScale(floor, GameObject.Find("Floor Glitch 2").transform.lossyScale, 6);

        yield return GeneralManager.waitForSeconds(0.3f);
        smallGlitch();

        floor.gameObject.SetActive(false);
        controllerRb.velocity = Vector3.zero;
    }

    public bool canFollow = false;
    private void FixedUpdate()
    {
        if (canFollow)
        {
            Transform cam = Camera.main.transform;
            cam.position = new Vector3(cam.position.x, controller.transform.position.y, cam.position.z);

            //if (controllerRb.velocity.y < -150)
            //{
            //    controllerRb.velocity = new Vector3(controllerRb.velocity.x, -150);
            //}

        }
    }

    public IEnumerator FallSequenceEnum()
    {
        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube"))
        {
            DontDestroyOnLoad(cube);
        }
        invisWalls.SetActive(true);
        Fall();
        yield return GeneralManager.waitForSeconds(0.3f);
        LerpSolution.lerpLowPass(GameObject.Find("Music").GetComponent<AudioLowPassFilter>(), 789, 0.4f);
    }

    private void Fall()
    {
        controller.canCamSwapScene = false;
        canFollow = true;
    }
}
