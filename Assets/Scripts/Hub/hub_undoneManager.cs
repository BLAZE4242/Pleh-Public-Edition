using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
// using Aura2API; // AURA HERE

public class hub_undoneManager : MonoBehaviour
{
    [SerializeField] bool skipCutscene;
    [Header("Cutscene")]
    [SerializeField] Transform spawnPos;
    [SerializeField] AudioClip click1;
    [SerializeField] GameObject props;
    // [SerializeField] AuraCamera auraCam; // AURA HERE
    [SerializeField] GameObject dev;
    [SerializeField] PostProcessVolume volume;
    [SerializeField] PostProcessProfile red;
    [SerializeField] TextMeshPro redText;
    [SerializeField] TMP_FontAsset[] fonts;
    [SerializeField] Color redColour;
    [SerializeField] TextMeshPro[] noescape;
    [SerializeField] GameObject noescapeParent;
    [SerializeField] GameObject Therapist;
    [SerializeField] TextMeshPro therapistText;
    glitchText _gText;
    AudioSource source;
    PlayerController controller;

    private void Awake()
    {
        props.SetActive(false);
        Therapist.SetActive(false);
        therapistText.text = "";
        controller = FindObjectOfType<PlayerController>();
        Destroy(controller.GetComponent<chairHug>());
        source = GetComponent<AudioSource>();
        _gText = GetComponent<glitchText>();

        controller.lockMovement = true;
        controller.transform.position = spawnPos.position;
        controller.transform.rotation = spawnPos.rotation;
    }

    IEnumerator Start()
    {
        FindObjectOfType<DRP>().RP("IT MADE ME DO THIS.");
        if (skipCutscene)
        {
            EndCutscene();
        }
        else
        {
            PostProcessProfile defaultProfile = volume.profile;
            float defaultFov = controller.playerCamera.GetComponent<Camera>().fieldOfView;

            source.PlayOneShot(click1);
            yield return GeneralManager.waitForSeconds(0.4f);
            props.SetActive(true);
            source.PlayOneShot(click1);
            // auraCam.enabled = true; // AURA HERE
            dev.SetActive(true);
            yield return GeneralManager.waitForSeconds(0.4f);

            volume.profile = red;
            controller.playerCamera.GetComponent<Camera>().fieldOfView = 40;
            source.Play();
            CameraShake.Shake(100, 0.08f);

            yield return GeneralManager.waitForSeconds(4);

            Coroutine textChange = StartCoroutine(ChangeText("I have done it."));

            yield return GeneralManager.waitForSeconds(4);

            StopCoroutine(textChange);

            textChange = StartCoroutine(ChangeText("And now it can't be undone."));

            yield return GeneralManager.waitForSeconds(3f);

            StartCoroutine(NoEscape());
            CameraShake.Shake(100, 0.1f);

            yield return GeneralManager.waitForSeconds(4);

            StopCoroutine(textChange);
            CameraShake._instance.StopAllCoroutines();
            volume.profile = defaultProfile;
            controller.playerCamera.GetComponent<Camera>().fieldOfView = defaultFov;
            controller.lockMovement = false;

            dev.SetActive(false);
            foreach (RedTextIndividual text in FindObjectsOfType<RedTextIndividual>())
            {
                Destroy(text.gameObject);
            }
            redText.gameObject.SetActive(false);
            noescapeParent.SetActive(false);
            Gas.EarnAchievement("ACH_STORY_3");
            EndCutscene();
        }
    }

    IEnumerator ChangeText(string text)
    {
        StartCoroutine(_gText.createGlitchedText(text, redText));
        while (true)
        {
            redText.font = redText.font == fonts[0] ? fonts[1] : fonts[0];
            redText.color = redText.font == fonts[0] ? redColour : Color.white;
            yield return GeneralManager.waitForSeconds(Random.Range(0.1f, 1f));
        }
    }

    IEnumerator NoEscape()
    {
        foreach (TextMeshPro text in noescape)
        {
            text.text = "NO ESCAPE";
            yield return GeneralManager.waitForSeconds(0.1f);
        }
    }

    void EndCutscene()
    {
        props.SetActive(true);
        controller.lockMovement = false;
        Therapist.SetActive(true);
        therapistText.text = $"Nice to see you again {PlayerPrefs.GetString("playerName")}, take a seat and we'll get started.";
        source.PlayOneShot(click1);
    }

    private void Update()
    {
        if (GameObject.Find("Head") != null)
        {
            GameObject.Find("Head").transform.LookAt(controller.transform);
            GameObject.Find("Head").transform.rotation *= Quaternion.Euler(0, 180, 0);
        }
    }
}
