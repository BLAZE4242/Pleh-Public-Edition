using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//this was meant to be called chair hub but due to a typo it's now chair hug
public class chairHug : MonoBehaviour
{
    [Header("Chair Variables")]
    public float pushPower;
    [SerializeField] LayerMask noPlayerCollisionLayer;
    [SerializeField] bool canLockYpos;

    [Header("Cabinet Variables")]
    [SerializeField] Camera playerCam;
    [SerializeField] float range = 4f;
    [SerializeField] Transform cabinetToOpen;
    isTouchingCabinet _touchingCabinet;
    PlayerController _controller;
    bool cabinetIsOpen;
    bool cabinetFullyOpen;
    bool transcriptCollected;
    float startYpos;
    bool canPush = true;

    [Header("God")]
    [SerializeField] AudioClip song;
    [SerializeField] Image whiteScreen;
    [SerializeField] AudioClip winSfx;

    void Start()
    {
        startYpos = transform.position.y;
        _touchingCabinet = FindObjectOfType<isTouchingCabinet>();
        _controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            StartCoroutine(moveCabinet());
        }

        if (playerCam == null)
        {
            playerCam = FindObjectOfType<Camera>();
        }
    }

    IEnumerator moveCabinet()
    {

        RaycastHit hit;
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.forward), out hit, range) && hit.transform == cabinetToOpen)
        {
            if (!cabinetIsOpen)
            {
                FindObjectOfType<Transcript>().currentState = Transcript.transcriptStates.notIdle;
                LerpSolution.StopCoroutines();

                Vector3 targetCabinetPos = cabinetToOpen.position + new Vector3(0.6f, 0, 0);
                LerpSolution.lerpPosition(cabinetToOpen, targetCabinetPos, 1.85f);
                cabinetIsOpen = true;
                while(cabinetToOpen.position != targetCabinetPos)
                {
                    if(isPlayerTouching())
                    {
                        transform.parent = cabinetToOpen; //Makes the player a child of the drawer so they can move with it instead of physics going crazy
                        _controller.lockMovement = true;
                    }
                    yield return new WaitForEndOfFrame();
                }
                transform.parent = null;
                _controller.lockMovement = false;

                cabinetFullyOpen = true;

                FindObjectOfType<Transcript>().currentState = Transcript.transcriptStates.idle;
                StartCoroutine(FindObjectOfType<Transcript>().TranscriptIdle());
            }
            else if(cabinetFullyOpen && !transcriptCollected)
            {
                transcriptCollected = true;
                FindObjectOfType<Transcript>().Collect();
            }
        }
    }

    bool isPlayerTouching()
    {
        return _touchingCabinet.isPlayerTouching;
    }

    void FixedUpdate()
    {
        if(canLockYpos)
        {
            transform.position = new Vector3(transform.position.x, startYpos, transform.position.z);
        }
    }

    IEnumerator closeGame() { yield return GeneralManager.waitForSeconds(3); Application.Quit(); }

    IEnumerator hasPushed(GameObject chair)
    {
        

        bool shouldCrash = chair.name.EndsWith("Death");
        if (chair.name.EndsWith("Win"))
        {
            chair.name = "Chair";
            FindObjectOfType<AudioSource>().PlayOneShot(winSfx);
            StartCoroutine(closeGame());
        }

        if (!shouldCrash)
        {
            if (chairStat() == 1499)
            {
                PrepareArt();
            }
            canPush = false;
            LayerMask defaultLayer = chair.layer;
            chair.layer = LayerMask.NameToLayer("Cant touch player");
            yield return GeneralManager.waitForSeconds(0.3f);
            while (isSamePosWithCube(chair.transform))
            {
                yield return GeneralManager.waitForEndOfFrame;
            }
            canPush = true;
            chair.layer = defaultLayer;
        }

        if (shouldCrash)
        {
            yield return GeneralManager.waitForSeconds(0.5f);
            PlayerPrefs.DeleteKey("thats it");
            if (!Application.isEditor)
            {
                int i = 0;
                while (true)
                {
                    i += i + 9;
                }
            }
            else Debug.Log("Crashed!");
        }

    }

    int chairStat()
    {
        PlayerPrefs.SetInt("chair", PlayerPrefs.GetInt("chair") + 1);
        
        if (FindObjectOfType<devroom_manager>() != null)
        {
            FindObjectOfType<devroom_manager>().UpdateChairText();
        }

        switch (PlayerPrefs.GetInt("chair"))
        {
            case (10):
                Gas.EarnAchievement("ACH_CHAIR_10");
                break;
            case (50):
                Gas.EarnAchievement("ACH_CHAIR_50");
                break;
            case (100):
                Gas.EarnAchievement("ACH_CHAIR_100");
                break;
            case (500):
                Gas.EarnAchievement("ACH_CHAIR_500");
                break;
            case (1500):
                Gas.EarnAchievement("ACH_CHAIR_1500");
                break;
        }

        Gas.SetStat("STAT_CHAIR", PlayerPrefs.GetInt("chair"));

        return PlayerPrefs.GetInt("chair");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.CompareTag("Chair") && canPush)
        {
            CameraShake.Shake(0.2f, 0.06f, false);
            Rigidbody rb = hit.collider.attachedRigidbody;
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.AddForce(pushDir * pushPower + new Vector3(0, Random.Range(-30, 30), 0), ForceMode.Impulse);
            
            StartCoroutine(hasPushed(hit.gameObject));
        }
    }

    bool isSamePosWithCube(Transform cube)
    {
        Vector2 cubePos = new Vector2(cube.position.x, cube.position.z);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        if(Vector3.Distance(cubePos, playerPos) < 0.9f) return true;
        else return false;
    }

    void PrepareArt()
    {
        canPush = false;
        if (!PlayerPrefs.GetString("hs").Contains("4"))
        {
            PlayerPrefs.SetString("hs", PlayerPrefs.GetString("hs") + "4");
        }

        foreach (MonoBehaviour go in FindObjectsOfType<MonoBehaviour>())
        {
            go.StopAllCoroutines();
        }

        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            source.Stop();
        }

        _controller.lockMovement = true;

        StartCoroutine(DoChairEnd());
    }

    IEnumerator DoChairEnd()
    {
        //AsyncOperation sceneOp = SceneManager.LoadSceneAsync("Quiz End");
        //sceneOp.allowSceneActivation = false;

        AudioSource source = GameObject.Find("SFX").GetComponent<AudioSource>();
        source.clip = song;
        source.Play();
        
        yield return GeneralManager.waitForSeconds(6);

        LerpSolution.lerpImageColourTime(whiteScreen, Color.white, 9);

        yield return GeneralManager.waitForSeconds(12);

        DontDestroyOnLoad(source);
        PlayerPrefs.SetString("thats it", "chair");
        SceneManager.LoadScene("Quiz End");
    }
}
