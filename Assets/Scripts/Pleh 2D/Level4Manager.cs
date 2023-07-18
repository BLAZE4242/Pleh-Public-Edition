using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class Level4Manager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] confetti;
    [SerializeField] AudioClip confettiSFX;
    [SerializeField] SpriteRenderer logo;
    [SerializeField] TextMeshPro redText01;
    [SerializeField] GameObject floor;
    [SerializeField] Transform door;
    PostProcessProfile glitchProfile;
    playerMove controller;
    Rigidbody2D rb;

    private void Start()
    {
        controller = FindObjectOfType<playerMove>();
        rb = controller.GetComponent<Rigidbody2D>();
    }

    public void MoveCam()
    {
        FindObjectOfType<PipesToDoor>().canFollow = false;
        LerpSolution.lerpPosition(Camera.main.transform, GameObject.Find("Cam pos").transform.position, 4f);
    }

    public void StopPluto() { StartCoroutine(StartCutscene()); }

    IEnumerator StartCutscene()
    {
        rb.velocity = Vector3.zero;
        yield return GeneralManager.waitForSeconds(1);
        foreach (ParticleSystem part in confetti)
        {
            part.Play();
            CameraShake.Shake(0.4f, 0.2f, true, true);
        }

        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(confettiSFX);

        Gas.EarnAchievement("ACH_FAKEbEAT");

        yield return GeneralManager.waitForSeconds(4);

        float[] strobeTimes = { 0.1f, 0.1f, 0.4f, 0.05f, 0.09f, 0.2f};
        foreach (float strobe in strobeTimes)
        {
            yield return GeneralManager.waitForSeconds(strobe);
            logo.enabled = false;
            yield return GeneralManager.waitForSeconds(0.07f);
            logo.enabled = true;
        }
        logo.enabled = false;

        yield return GeneralManager.waitForSeconds(1f);

        if (checkCubes())
        {
            redText01.gameObject.SetActive(true);
            redText01.text = "You really went to all that effort...";
            GetComponent<glitchText>().glitchGlitchedText(redText01);
            yield return GeneralManager.waitForSeconds(2);
            redText01.text = "You really want to know the truth?";
            GetComponent<glitchText>().glitchGlitchedText(redText01);
            yield return GeneralManager.waitForSeconds(4);
            redText01.text = "HE CAN'T TAKE IT BACK.";
            GetComponent<glitchText>().glitchGlitchedText(redText01);
            yield return GeneralManager.waitForSeconds(4);
            redText01.text = "Solving this will fix nothing.";
            GetComponent<glitchText>().glitchGlitchedText(redText01);
            yield return GeneralManager.waitForSeconds(4);
            redText01.text = "I get it. You just want your password.";
            GetComponent<glitchText>().glitchGlitchedText(redText01);
            yield return GeneralManager.waitForSeconds(4);
            redText01.text = "whenonedoorcloses";
            GetComponent<glitchText>().glitchGlitchedText(redText01);
        }
        else
        {
            redText01.gameObject.SetActive(true);
            GetComponent<glitchText>().glitchGlitchedText(redText01);
            //int layer = LayerMask.NameToLayer("Cant Touch Player");
            //foreach (GameObject col in GameObject.FindGameObjectsWithTag("Cube"))
            //{
                //col.layer = layer;
            //}

            yield return GeneralManager.waitForSeconds(1);

            floor.SetActive(false);

            yield return GeneralManager.waitForSeconds(0.1f);

            FindObjectOfType<PipesToDoor>().canFollow = true;

            yield return GeneralManager.waitForSeconds(1);

            PostProcessVolume volume = new PostProcessVolume();
            foreach (PostProcessVolume volumeMaybe in FindObjectsOfType<PostProcessVolume>())
            {
                if (volumeMaybe.weight == 0) volume = volumeMaybe;
            }

            glitchProfile = volume.profile;

            if(volume != null) LerpSolution.LerpPPweight(volume, 1, 0.4f);
            Camera.main.GetComponent<Animator>().SetTrigger("Heavy");
            GameObject.Find("SFX").GetComponent<AudioSource>().Play();
            GameObject.Find("SFX").GetComponent<AudioSource>().loop = true;
            LerpSolution.lerpLowPass(GameObject.Find("SFX").GetComponent<AudioLowPassFilter>(), 10188, 0.4f, 10);
        }
    }

    bool checkCubes()
    {
        int amount = 0;
        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube"))
        {
            if (cube.transform.position.x >= 323.6f && cube.transform.position.x <= 356.3f && cube.transform.position.y > -2317.3f)
            {
                amount++;
            }
        }
        Debug.Log(amount);
        return amount >= 12;
    }

    public void StopPluto01() { StartCoroutine(StopPluto01Enum()); }

    IEnumerator StopPluto01Enum()
    {
        FindObjectOfType<PipesToDoor>().canFollow = false;
        rb.velocity = Vector3.zero;
        LerpSolution.lerpPosition(Camera.main.transform, GameObject.Find("Cam pos 2").transform.position, 4f);
        while (Camera.main.transform.position != GameObject.Find("Cam pos 2").transform.position) yield return new WaitForEndOfFrame();

        controller.canCamSwapScene = true;
    }

    public void activateDoorLerp() { StartCoroutine(activateDoorLerpEnum()); }

    IEnumerator activateDoorLerpEnum()
    {
        ChromaticAberration chrome;
        glitchProfile.TryGetSettings(out chrome);

        while (true)
        {
            float dist = Vector2.Distance(controller.transform.position, door.position) / 40;
            dist = 1 - dist;
            chrome.intensity.value = dist;
            if(dist >= 0) controller.moveSpeed = Mathf.Lerp(7, 1, dist +0.2f);

            yield return new WaitForEndOfFrame();
        }
    }
}
