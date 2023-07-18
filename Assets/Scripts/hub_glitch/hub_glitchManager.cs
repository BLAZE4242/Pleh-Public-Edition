using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class hub_glitchManager : MonoBehaviour
{
    [Header("Light Move")]
    [SerializeField] Transform[] MoveablePositions;
    [SerializeField] UnityEvent[] MoveableEvents;
    [SerializeField] AudioClip touch;

    [Header("Flashbang")]
    [SerializeField] AudioLowPassFilter musicLowPass;
    PostProcessVolume _volume;
    ColorGrading _grading;
    [SerializeField] Image whiteImage;

    int touchItteration = 0;
    bool canMove = true;

    private void Start()
    {
        FindObjectOfType<DRP>().RP("Error: Cannot locate player");

        _volume = FindObjectOfType<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _grading);
    }

    public void moveLight(Transform objectTrans)
    {
        foreach (RedTextIndividual redText in FindObjectsOfType<RedTextIndividual>())
        {
            Destroy(redText.gameObject);
        }

        if (canMove)
        {
            LerpSolution.lerpPosition(objectTrans, MoveablePositions[touchItteration].position, 2);
        }
        
        MoveableEvents[touchItteration].Invoke();
        Debug.Log(touchItteration);
        touchItteration++;

        GetComponent<AudioSource>().PlayOneShot(touch);
    }

    public void changeMoveableState(bool desiredCanMove)
    {
        canMove = desiredCanMove;
    }

    public void triggerFlashBang()
    {
        StartCoroutine(triggerFlashBangEnum());
    }

    IEnumerator triggerFlashBangEnum()
    {
        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("LostInThoughts");
        sceneOp.allowSceneActivation = false;

        LerpSolution.lerpLowPass(musicLowPass, 10, 0.2f);
        LerpSolution.LerpPostExposure(_grading, 20, 0.5f);

        yield return GeneralManager.waitForSeconds(2f);

        LerpSolution.lerpImageColour(whiteImage, Color.white, 0.5f);

        yield return GeneralManager.waitForSeconds(4);
        sceneOp.allowSceneActivation = true;
    }
}
