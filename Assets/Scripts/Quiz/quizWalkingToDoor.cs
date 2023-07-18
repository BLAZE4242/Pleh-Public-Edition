using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class quizWalkingToDoor : MonoBehaviour
{
    [SerializeField] Transform door;
    [SerializeField] Transform playerTrans;
    [SerializeField] Transcript transcript;
    PostProcessVolume _volume;
    ColorGrading _grading;
    bool hasStarted = false;

    void StateStart(quizManager _manager)
    {
        _volume = FindObjectOfType<PostProcessVolume>();
        _volume.profile.TryGetSettings(out _grading);
        if(PlayerPrefs.GetInt("glotchiness") != 7) LerpSolution.LerpPostExposure(_grading, 0, 0.2f, 4f);
        else
        {
            transcript.gameObject.SetActive(false);
            GetComponent<quizDialogueManager>().isFirstTime = false;
        }
    }

    void StateUpdate(quizManager _manager)
    {
        if(Vector3.Distance(playerTrans.position, door.position) <= 8)
        {
            _manager.currentQuizProjectState = quizManager.quizProjectStates.quizing;
        }
    }

    public void CheckStateStarted(quizManager _manager)
    {
        if (!hasStarted)
        {
            StateStart(_manager);
            StateUpdate(_manager);
            hasStarted = true;
        }
        else
        {
            StateUpdate(_manager);
        }
    }

}
