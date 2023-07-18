using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using Kino;
using UnityEngine.UI;

public class ekseManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] PostProcessVolume volume;
    [SerializeField] TextMeshPro[] texts;
    [SerializeField] GameObject[] walls;
    [SerializeField] Transform[] redThingPoints;
    [SerializeField] TextMeshPro hackerText, inputText;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] glitches;
    [SerializeField] AudioClip impact;
    [SerializeField] Transform redThing;
    [Header("End")]
    [SerializeField] SpriteRenderer blackScreen;
    [SerializeField] TextMeshPro redTextScreen;
    [SerializeField] AnalogGlitch endAnalog;
    [SerializeField] AudioSource endSource;
    [SerializeField] AudioSource beginSource;
    [SerializeField] AudioClip click1;
    [SerializeField] GameObject pluto;
    [SerializeField] Transform[] plutoSpawnPos;
    PostProcessProfile profile;
    ColorGrading grade;
    glitchText _gText;
    bool canDelete, hasDeleted;

    private void Awake()
    {
        if (PlayerPrefs.GetString("thats it") != "sex")
        {
            Destroy(FindObjectOfType<AudioListener>());
            FindObjectOfType<DigitalGlitch>().intensity = 0;
            canvas.SetActive(true);
            FindObjectOfType<PlayerController>().lockMovement = true;
        }
    }

    private void Start()
    {
        profile = volume.profile;
        profile.TryGetSettings(out grade);
        _gText = GetComponent<glitchText>();
    }

    // Update is called once per frame
    void Update()
    {
        grade.hueShift.value += 2.6f * Time.deltaTime;
        if (grade.hueShift.value >= 180) grade.hueShift.value = -180;

        if (canDelete)
        {
            if (hackerText.text == "")
            {
                hackerText.text = "'unknown entity.exe' is too large to move to bin. Are you sure you want to remove the file permanently? (y/n)";
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                inputText.text = "y";
            }

            if (Input.GetKeyDown(KeyCode.Return) && inputText.text == "y")
            {
                hackerText.text = "";
                inputText.text = "";
                canDelete = false;
                hasDeleted = true;
            }
        }
    }

    public void HaveSex(int itteration) // ekse
    {
        StartCoroutine(HaveSexEnum(itteration-1));
    }

    int glitchItteration = 0;
    IEnumerator HaveSexEnum(int itteration)
    {
        texts[itteration].gameObject.SetActive(true);
        _gText.glitchGlitchedText(texts[itteration]);

        yield return GeneralManager.waitForSeconds(1);

        canDelete = true;

        yield return new WaitUntil(() => hasDeleted);

        CameraShake.Shake(0.2f, 0.5f); // play impact and random glitch sfx. Also increase digital for a second

        texts[itteration].gameObject.SetActive(false);
        walls[itteration].SetActive(false);
        hasDeleted = false;

        LerpSolution.lerpPosition(redThing, redThingPoints[itteration].position, 0.3f);
        
        source.PlayOneShot(impact);
        source.PlayOneShot(glitches[glitchItteration]);
        glitchItteration++;
        if (glitchItteration >= glitches.Length) glitchItteration = 0;

        float defaultIntensity = FindObjectOfType<DigitalGlitch>().intensity;
        FindObjectOfType<DigitalGlitch>().intensity = 0.3f;

        yield return GeneralManager.waitForSeconds(0.2f);

        FindObjectOfType<DigitalGlitch>().intensity = defaultIntensity;
    }

    public void Ekse()
    {
        StartCoroutine(EkseEnum());
    }

    List<string> endScript = new List<string> {
        "What a shame.",
        "All his hard work...",
        "For nothing.",
        "You're going to destroy everything.",
        "Fine by me.",
        "I'll still live on.",
        "Maybe I'll live inside her.",
        "I AM EVERYWHERE.",
        "Doing this doesn't stop anything.",
        "",
        "You're just as fucking dumb as he was." // good bye red text :') thanks for playing Pleh!
    };
    IEnumerator EkseEnum()
    {
        FindObjectOfType<PlayerController>().lockMovement = true;
        blackScreen.color = Color.black;
        redThing.gameObject.SetActive(false);

        yield return GeneralManager.waitForSeconds(2);

        endAnalog.enabled = true;
        endSource.Play();

        StartCoroutine(HaveSexEkse());
    }

    IEnumerator HaveSexEkse()
    {
        if (endScript.Count == 0)
        {
            StartCoroutine(corruptGame());
        }
        else
        {

            endSource.volume += 0.09f;
            beginSource.volume -= 0.045f;
            endAnalog.scanLineJitter += 0.025f;
            endAnalog.horizontalShake += 0.017f;
            endAnalog.colorDrift += 0.012f;

            redTextScreen.text = endScript[0];
            _gText.glitchGlitchedText(redTextScreen);

            yield return GeneralManager.waitForSeconds(1);

            canDelete = true;

            yield return new WaitUntil(() => hasDeleted);

            source.PlayOneShot(impact);
            source.PlayOneShot(glitches[glitchItteration]);
            glitchItteration++;
            if (glitchItteration >= glitches.Length) glitchItteration = 0;

            float defaultIntensity = FindObjectOfType<DigitalGlitch>().intensity;
            FindObjectOfType<DigitalGlitch>().intensity = 0.6f;

            yield return GeneralManager.waitForSeconds(0.5f);

            FindObjectOfType<DigitalGlitch>().intensity = defaultIntensity;

            hasDeleted = false;
            endScript.RemoveAt(0);
            StartCoroutine(HaveSexEkse());
        }

    }

    IEnumerator corruptGame()
    {
        Destroy(redTextScreen); // thanks for playing...
        PlayerPrefs.SetInt("reddelete", 1);
        Gas.EarnAchievement("ACH_ARG_2");

        endSource.Stop();
        beginSource.Stop();
        FindObjectOfType<DigitalGlitch>().intensity = 0;
        foreach (AnalogGlitch glitch in FindObjectsOfType<AnalogGlitch>())
        {
            glitch.enabled = false;
        }

        yield return GeneralManager.waitForSeconds(2);

        StartCoroutine(spamErrors());

        yield return GeneralManager.waitForSeconds(10);

        StartCoroutine(SpamPluto());

        yield return GeneralManager.waitForSeconds(5f);

        PlayerPrefs.SetString("Backup", "yep");
        PlayerPrefs.SetInt("glotchiness", 0);
        GeneralManager.SetSceneSave("hub_main");

        if (Application.isEditor)
        {
            Debug.Log("Crash!");
        }
        else
        {
            int temp = 0;
            while (true)
            {
                temp += temp + 17;
                temp -= 2 * temp;
            }
        }
    }

    private IEnumerator SpamPluto()
    {
        foreach (Transform spawnPos in plutoSpawnPos)
        {
            Vector3 spawnPosPos = new Vector3(0.1f, -0.1f, 0);

            for (int i = 10; i > 0; i--)
            {
                Transform plutoGo = Instantiate(pluto, spawnPos.position + spawnPosPos, Quaternion.identity).transform;
                plutoGo.SetParent(spawnPos);
                spawnPosPos -= new Vector3(0.1f, -0.1f, 0);

                yield return GeneralManager.waitForSeconds(0.2f);
            }
        }
    }

    IEnumerator spamErrors()
    {
        float timeToWait = 1.5f;

        while (true)
        {
            hackerText.text += $"Error: Missing/corrupt chunk {randomLongNumber()} from memory address {randomMemoryAddress()}\n";
            source.PlayOneShot(click1);

            yield return GeneralManager.waitForSeconds(timeToWait);

            if (timeToWait >= 0.1f) timeToWait -= 0.1f;
            else if (timeToWait >= 0.02f) timeToWait -= 0.005f;
        }
    }

    string randomLongNumber()
    {
        string result = "";
        for (int i = 0; i < Random.Range(5, 10); i++)
        {
            result += Random.Range(0, 10);
        }
        return result;
    }

    string randomMemoryAddress()
    {
        return $"0{randomNumber()}{randomNumber()}{randomChar()}{randomNumber()}x{randomNumber()}{randomChar()}{randomChar()}{randomChar()}{randomNumber()}";
    }

    char randomChar()
    {
        string chars = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        return chars[Random.Range(0, chars.Length - 1)];
    }

    int randomNumber()
    {
        return Random.Range(0, 10);
    }
}
