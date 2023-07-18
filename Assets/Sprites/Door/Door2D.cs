using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Door2D : MonoBehaviour
{
    Animator doorAnim;
    [SerializeField] AudioClip reversePiano;
    [SerializeField] bool isFinal;
    [SerializeField] TextMeshProUGUI tutText;
    [SerializeField] KeyCode actionKey = KeyCode.LeftShift;
    bool canEnter;
    [SerializeField] Transform plutoPos1, plutoPos2;
    [SerializeField] float slowDownSpeed;
    [SerializeField] SpriteRenderer barrier;
    [SerializeField] SpriteRenderer frame;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFinal) return;

        canEnter = true;
        tutText.text = $"Press {actionKey} to enter the door";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isFinal) return;

        canEnter = false;
        tutText.text = "";
    }

    private void Start()
    {
        doorAnim = GetComponent<Animator>();
    }

    public void DoorOpen()
    {
        doorAnim.SetTrigger("DoorOpen");
    }

    public void CamZoom() { StartCoroutine(CamZoomEnum()); }

    IEnumerator CamZoomEnum()
    {
        FindObjectOfType<playerMove>().canCamSwapScene = false;
        LerpSolution.lerpPosition(Camera.main.transform, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0.3f);
        LerpSolution.lerpCamSize(Camera.main.GetComponent<Camera>(), 2.5f, 0.3f);

        GetComponent<AudioSource>().clip = reversePiano;
        GetComponent<AudioSource>().Play();
        yield return GeneralManager.waitForSeconds(3.7f);

        CameraShake.Shake(10, 0.5f, true, true);

        while (GetComponent<AudioSource>().isPlaying) yield return new WaitForEndOfFrame();

        PlayerPrefs.SetInt("glotchiness", 1);
        GeneralManager.SetSceneSave("jesterGames");
        Application.Quit();
        Debug.LogError("AND QUIT");
    }

    private void Update()
    {
        if (canEnter && isFinal && Input.GetKeyDown(actionKey))
        {
            isFinal = false;
            tutText.text = "";
            StartCoroutine(EnterDoor());
        }
    }

    IEnumerator EnterDoor()
    {
        playerMove pluto = FindObjectOfType<playerMove>();
        pluto.canControl = false;

        while (Vector2.Distance(pluto.transform.position, plutoPos1.position) > 1f) // vector2.range (or whatever says how far away they are from eachother
        {
            pluto.scaleAccordingly(2);
            pluto.transform.position += new Vector3(pluto.transform.position.x < plutoPos1.position.x ? 1 : -1, 0) * pluto.moveSpeed * Time.deltaTime;
            pluto.PlutoAnimator.SetFloat("Speed", Mathf.Abs(pluto.transform.position.x < plutoPos1.position.x ? 1 : -1));
            Debug.Log(Vector2.Distance(pluto.transform.position, plutoPos1.position));
            yield return new WaitForEndOfFrame();
        }

        pluto.PlutoAnimator.SetFloat("Speed", 0);
        pluto.scaleAccordingly(1);

        yield return GeneralManager.waitForSeconds(3f);
        //transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        DoorOpen();
        barrier.enabled = true;
        yield return GeneralManager.waitForSeconds(0.5f);

        StartCoroutine(closeDoor());

        while (Vector2.Distance(pluto.transform.position, plutoPos2.position) > 1f) // vector2.range (or whatever says how far away they are from eachother
        {
            pluto.scaleAccordingly(1);
            pluto.transform.position += new Vector3(pluto.transform.position.x < plutoPos2.position.x ? 1 : -1, 0) * pluto.moveSpeed * Time.deltaTime;
            pluto.PlutoAnimator.SetFloat("Speed", Mathf.Abs(pluto.transform.position.x < plutoPos2.position.x ? 1 : -1));
            Debug.Log(Vector2.Distance(pluto.transform.position, plutoPos2.position));

            pluto.moveSpeed = 2;
            pluto.PlutoAnimator.speed = 0.285f;

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Reached");
    }

    IEnumerator closeDoor()
    {
        yield return GeneralManager.waitForSeconds(7);
        doorAnim.SetTrigger("DoorClose");
        yield return GeneralManager.waitForSeconds(3);
        Destroy(FindObjectOfType<music>().gameObject);
        FindObjectOfType<PlehGlitchManager>().sceneOp.allowSceneActivation = true;
    }

    public void TrySpawnFrame()
    {
        if (frame != null)
        {
            frame.enabled = true;
        }
    }

    public void TryKillFrame()
    {
        if (frame != null)
        {
            frame.enabled = false;
        }
    }
}
