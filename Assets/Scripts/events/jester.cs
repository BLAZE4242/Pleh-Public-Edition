using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class jester : MonoBehaviour
{
    [Header("General Stuff")]
    [SerializeField] GameObject hinge;
    [SerializeField] float secondsForJesterGames = 3f;
    [SerializeField] float secondsToEnd;
    [SerializeField] Transform bg;
    [SerializeField] Animator handleAnim, boxAnim, textAnim, fadeAnim;
    [SerializeField] AudioClip windUpMusic, jesterGames, wind;

    [Header("Glitching Stuff")]
    [SerializeField] Material missingTexture;
    [SerializeField] MeshRenderer jesterRenderer;
    [SerializeField] AudioClip glitch1;
    [SerializeField] GameObject glitchOverlay;
    [SerializeField] Transform blackScreen;
    AudioSource source;
    Camera cam;
    AsyncOperation asyncSceneLoad;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<DRP>().RP("");

        cam = Camera.main;
        source = GetComponent<AudioSource>();
        
        int targetScene;

        if (PlayerPrefs.GetString("thats it") == "pleh2")
        {
            targetScene = 30;
            asyncSceneLoad = SceneManager.LoadSceneAsync(targetScene);
            asyncSceneLoad.allowSceneActivation = false;
            StartCoroutine(jesterPopUp());
            return;
        }

        switch(PlayerPrefs.GetInt("glotchiness"))
        {
            case 3:
                jesterRenderer.material = missingTexture;
                targetScene = 5;
                break;
            case 4:
                targetScene = -1;
                asyncSceneLoad = SceneManager.LoadSceneAsync("Look both ways");
                asyncSceneLoad.allowSceneActivation = false;
                FindObjectOfType<DRP>().RP("What have I done?");
                break;
            case 0:
                targetScene = SceneManager.GetActiveScene().buildIndex + 1;
                pause.canPause = false;
                break;
            default:
                targetScene = SceneManager.GetActiveScene().buildIndex + 1;
                break;
        }


        if (PlayerPrefs.GetInt("glotchiness") == 4) StartCoroutine(jesterFall());
        else
        {
            asyncSceneLoad = SceneManager.LoadSceneAsync(targetScene);
            asyncSceneLoad.allowSceneActivation = false;
            StartCoroutine(jesterPopUp());
        }

    }

    IEnumerator jesterFall()
    {
        yield return new WaitForEndOfFrame();
        cam.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
        yield return GeneralManager.waitForSeconds(6.1f);
        handleAnim.speed = 0;
        boxAnim.SetTrigger("jesterPop");
        //CameraShake.Shake(0.3f, 0.1f);
        FindObjectOfType<SpringJoint>().GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        FindObjectOfType<SpringJoint>().GetComponent<Rigidbody>().velocity = new Vector3(0, -10, 0);

        yield return GeneralManager.waitForSeconds(2f);
        source.clip = wind;
        source.Play();
        LerpSolution.lerpVolume(source, 1, 0.4f, 0);
        hinge.SetActive(false);
        Invoke("puzzle", 3);

        while (true)
        {
            if (cam.transform.position.y < -360f)
            {
                cam.transform.position = new Vector3(cam.transform.position.x, -35f, cam.transform.position.z);
            }
            yield return new WaitForEndOfFrame();
        }
        //CameraShake.Shake(0.3f, 0.06f);
    }

    void puzzle()
    {
        StartCoroutine(puzzleEnum());
    }

    private IEnumerator puzzleEnum()
    {
        LerpSolution.lerpVolume(source, 0, 0.8f);
        LerpSolution.lerpPosition(blackScreen, new Vector3(0, 0, blackScreen.localPosition.z), 4.5f, true);
        yield return GeneralManager.waitForSeconds(1f);
        asyncSceneLoad.allowSceneActivation = true;
    }

    IEnumerator jesterPopUp()
    {
        yield return GeneralManager.waitForSeconds(6.1f);
        handleAnim.speed = 0;
        boxAnim.SetTrigger("jesterPop");
        CameraShake.Shake(0.2f, 0.08f);
        yield return GeneralManager.waitForSeconds(secondsForJesterGames);
        source.PlayOneShot(jesterGames);
        yield return GeneralManager.waitForSeconds(0.8f);
        textAnim.SetTrigger("text");
        yield return GeneralManager.waitForSeconds(secondsToEnd);

        if(PlayerPrefs.GetInt("glotchiness") == 3)
        {
            yield return GeneralManager.waitForSeconds(2);
            glitchOverlay.SetActive(true);
            source.PlayOneShot(glitch1); //Maybe don't do one shot?
            yield return GeneralManager.waitForSeconds(glitch1.length);
        }
        else
        {
            fadeAnim.Play("FadeOut");
            yield return GeneralManager.waitForSeconds(2);
        }
        asyncSceneLoad.allowSceneActivation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //bgFitCam();
    }

    void bgFitCam()
    {
        bg.localScale = new Vector3(cam.orthographicSize / 5, cam.orthographicSize / 5, 1);
    }

    private void OnDisable()
    {
        pause.canPause = true;
    }
}
