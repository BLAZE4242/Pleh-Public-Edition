using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class hub_blinkManager : MonoBehaviour
{
    [Header("Blink")]
    [SerializeField] KeyCode[] CurrentBlinkKeys;
    [SerializeField] int[] KeyCountScript = { 1, 1, 1, 2, 2, 3, 4, 6, 666 };
    [SerializeField] bool isTesting = false;
    [SerializeField] Animator blinkAnim;
    [Header("Fall")]
    [SerializeField] GameObject room, brokenRoom;
    [SerializeField] Camera secondCam;
    [SerializeField] TextMeshPro redText, redTextUnderLine, amelia1, amelia2;
    [SerializeField] Transform sizeControl;
    [SerializeField] SpriteRenderer blackScreen;
    [SerializeField] GameObject waves;
    [Header("Other")]
    [SerializeField] TextMeshPro blinkText;
    [SerializeField] AudioClip click, clickReverb, reverse;
    [SerializeField] PostProcessVolume blinkVolume;
    AudioSource source;
    glitchText _gText;

    bool isEnd = false;

    KeyCode[] RandomKeys(int count)
    {
        List<KeyCode> result = new List<KeyCode>();
        List<KeyCode> candice = new List<KeyCode> { KeyCode.Q, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P, KeyCode.F, KeyCode.G, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M };

        for (int i = 0; i < count; i++)
        {
            KeyCode randomKey = candice[Random.Range(0, candice.Count - 1)];
            result.Add(randomKey);
            candice.Remove(randomKey);
        }
        return result.ToArray();
    }

    bool IsBlinking()
    {
        foreach (KeyCode BlinkKey in CurrentBlinkKeys)
        {
            if (!Input.GetKey(BlinkKey)) return false;
        }
        return true;
    }

    string BlinkTextPhrase()
    {
        string result = "Press ";
        if (CurrentBlinkKeys.Length == 1)
        {
            result += CurrentBlinkKeys[0].ToString();
        }
        else
        {
            for (int i = 0; i < CurrentBlinkKeys.Length; i++)
            {
                if (i == CurrentBlinkKeys.Length - 2)
                {
                    result += $"{CurrentBlinkKeys[i]} and {CurrentBlinkKeys[i+1]}";
                    break;
                }
                else result += $"{CurrentBlinkKeys[i]}, ";
            }
        }

        return result + " to blink";
    }

    void blinkSay(string text)
    {
        blinkText.text = text;
        source.PlayOneShot(click);
    }


    void Start()
    {
        FindObjectOfType<DRP>().RP("IT did this.");
        source = GetComponent<AudioSource>();
        _gText = GetComponent<glitchText>();
        if (PlayerPrefs.GetInt("retries") > 3) isTesting = true;
        if (isTesting) KeyCountScript = new int[] { 666 };
        StartCoroutine(blinkSequence());
    }

    IEnumerator blinkSequence()
    {
        if (PlayerPrefs.GetString("thats it") != "skip blink" && !isTesting)
        {
            yield return GeneralManager.waitForSeconds(4);
            blinkSay("Just breathe.");
            yield return GeneralManager.waitForSeconds(2);
            blinkSay("Remember to focus...");
        }
        if(!isTesting) yield return GeneralManager.waitForSeconds(2);

        foreach (int KeyCount in KeyCountScript)
        {
            source.clip = reverse;
            source.Play();

            Coroutine fallCoroutine = StartCoroutine(FallScript());

            blinkSay("");

            if (KeyCount != 666) CurrentBlinkKeys = RandomKeys(KeyCount);
            else CurrentBlinkKeys = new KeyCode[] { KeyCode.H };

            blinkSay(BlinkTextPhrase());

            Coroutine dieCoroutine = StartCoroutine(slowlyDie());

            yield return new WaitForEndOfFrame();

            if (KeyCount != 666)
            {
                yield return new WaitUntil(() => IsBlinking());

                StopCoroutine(dieCoroutine);
                StopCoroutine(fallCoroutine);
                CameraShake._instance.StopAllCoroutines();
                source.Stop();
                blinkAnim.SetTrigger("Blink");
            }
            else
            {
                isEnd = true;
            }
        }
    }

    IEnumerator slowlyDie()
    {
        LerpSolution.LerpPPweight(blinkVolume, 1, 0.14f, 0);

        yield return new WaitUntil(() => blinkVolume.weight >= 0.7);

        CameraShake.Shake(10, 0.2f);

        yield return new WaitUntil(() => blinkVolume.weight >= 1);

        if (!isEnd) Die();
        else Fall();
    }

    void Die()
    {
        StopAllCoroutines();
        source.Stop();
        GameObject.Find("Black").GetComponent<Image>().color = Color.black;
        if (PlayerPrefs.HasKey("retries")) PlayerPrefs.SetInt("retries", PlayerPrefs.GetInt("retries") + 1);
        else PlayerPrefs.SetInt("retries", 1);
        Invoke("Restart", 2);
    }

    void Fall()
    {
        blinkSay("");

        room.SetActive(false);
        brokenRoom.SetActive(true);

        foreach (Rigidbody rb in brokenRoom.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(new Vector3(0, Random.Range(0, 300), 0));
        }
    }

    void Restart()
    {
        PlayerPrefs.SetString("thats it", "skip blink");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator FallScript()
    {
        yield return GeneralManager.waitForSeconds(12.75f);

        secondCam.gameObject.SetActive(true);
        LerpSolution.lerpScale(sizeControl, new Vector3(1, 1, 1), 0.07f, new Vector3(0.8f, 0.8f, 1));
        StartCoroutine(_gText.createGlitchedText("WHY DOES HE KEEP WORKING ON THIS GAME?", redText));

        yield return GeneralManager.waitForSeconds(4);

        StartCoroutine(_gText.createGlitchedText("WHY IS HE STILL ALIVE?", redText));

        yield return GeneralManager.waitForSeconds(4);

        StartCoroutine(_gText.createGlitchedText("CAN'T HE SEE WHAT I SEE?", redText));

        yield return GeneralManager.waitForSeconds(4);

        StartCoroutine(_gText.createGlitchedText("WHY DOES HE THINK HE'S SUCH A GOOD PERSON?", redText));

        yield return GeneralManager.waitForSeconds(4.25f);

        StartCoroutine(ameliaSay("That's enough!", amelia1));

        yield return GeneralManager.waitForSeconds(0.5f);

        StartCoroutine(_gText.createGlitchedText("Wait... what was that...", redText));

        yield return GeneralManager.waitForSeconds(2f);

        StartCoroutine(ameliaSay("Leave him alone!", amelia2));
        LerpSolution.lerpSpriteColour(blackScreen, Color.black, 0.2f);
        waves.SetActive(true);
        LerpSolution.lerpVolume(waves.GetComponent<AudioSource>(), 1, 0.2f, 0);

        yield return GeneralManager.waitForSeconds(1f);

        StartCoroutine(_gText.createGlitchedText("No, that can't be...", redText));

        yield return GeneralManager.waitForSeconds(3);

        StartCoroutine(_gText.createGlitchedText("That... can't... be...", redText));

        yield return GeneralManager.waitForSeconds(1);
        waves.transform.SetParent(null);
        DontDestroyOnLoad(waves);
        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("Amelia_Ocean");
        sceneOp.allowSceneActivation = false;

        yield return GeneralManager.waitForSeconds(2);

        PlayerPrefs.DeleteKey("retries");
        sceneOp.allowSceneActivation = true;
    }

    IEnumerator ameliaSay(string text, TextMeshPro ameliaText)
    {
        source.PlayOneShot(clickReverb);
        LerpSolution.lerpScale(ameliaText.transform, ameliaText.transform.lossyScale, 2, ameliaText.transform.lossyScale - new Vector3(0.03f, 0.03f, 0.03f));
        ameliaText.text = text;
        LerpSolution.lerpTextColour(ameliaText, Color.white, 6, Color.clear);
        yield return GeneralManager.waitForSeconds(0.3f);
        LerpSolution.lerpTextColour(ameliaText, Color.clear, 6, Color.white);

    }

    private void Update()
    {
        if (redText != null)
        {
            redTextUnderLine.text = redText.text;
        }
    }
}
