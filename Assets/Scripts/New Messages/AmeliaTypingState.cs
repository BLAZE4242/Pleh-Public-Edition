using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using TMPro;

public class AmeliaTypingState : MonoBehaviour
{
    [SerializeField] VoiceLine newMessage_01, newMessage_02;
    [SerializeField] PostProcessVolume _volume;
    [SerializeField] GameObject[] preMessages;
    [SerializeField] TextMeshPro Amelia;
    [SerializeField] AudioClip click1;
    VoiceLineManager voiceLineManager;
    int lastNum;
    bool hasStarted;

    void StateStart(AmeliaStateManager _manager)
    {
        if (PlayerPrefs.GetInt("glotchiness") == 7 && PlayerPrefs.GetString("thats it") != "glotchiness")
        {
            foreach (GameObject message in preMessages)
            {
                message.SetActive(!message.activeInHierarchy);
            }
            StartCoroutine(ReflectionState());
        }

        foreach (messageManager _message in GameObject.FindObjectsOfType<messageManager>())
        {
            SpriteRenderer renderer = _message.GetComponent<SpriteRenderer>();
            LerpSolution.lerpSpriteColour(renderer, Color.clear, 2.7f);
        }
        lastNum = FindObjectOfType<MessageConvoChoice>().playerChosenTimes;
        voiceLineManager = FindObjectOfType<VoiceLineManager>();
    }

    IEnumerator ReflectionState()
    {
        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("Quiz End");
        sceneOp.allowSceneActivation = false;

        Amelia.text = "And now he's gone.";
        GetComponent<AudioSource>().PlayOneShot(click1);
        yield return GeneralManager.waitForSeconds(6);
        Amelia.text = "He quit his own game.";
        GetComponent<AudioSource>().PlayOneShot(click1);
        yield return GeneralManager.waitForSeconds(6);
        PlayerPrefs.SetString("thats it", "we back");
        sceneOp.allowSceneActivation = true;
    }

    void StateUpdate(AmeliaStateManager _manager)
    {
        if (lastNum != FindObjectOfType<MessageConvoChoice>().playerChosenTimes)
        {
            lastNum = FindObjectOfType<MessageConvoChoice>().playerChosenTimes;
            //checkWhichVoiceLine();
        }
    }

    void checkWhichVoiceLine()
    {
        if (MessageConvoManager.currentStoryState == MessageConvoManager.AmeliaStoryState.Confess) return;

        switch (lastNum)
        {
            case 3:
                voiceLineManager.PlayLine(newMessage_01);
                break;
            case 6:
                voiceLineManager.PlayLine(newMessage_02);
                break;
        }
    }

    public void CheckStateStarted(AmeliaStateManager _manager)
    {
        if(!hasStarted)
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

    public void getOutSuspense()
    {
        ColorGrading grade;
        DepthOfField dof;
        _volume.profile.TryGetSettings(out grade);
        _volume.profile.TryGetSettings(out dof);
        StartCoroutine(getOutSuspenseEnum(grade, dof));
    }

    IEnumerator getOutSuspenseEnum(ColorGrading grade, DepthOfField dof)
    {
        CameraShake.Shake(10, 0.01f);
        yield return GeneralManager.waitForSeconds(1);
        LerpSolution.LerpPostExposure(grade, 3, 0.1f);
        LerpSolution.lerpDOFdistance(dof, 1, 0.3f);
        LerpSolution.LerpGetOutCraziness(FindObjectOfType<GetOut>(), 1, 0.1f);
        CameraShake.Shake(10, 0.05f);
        yield return GeneralManager.waitForSeconds(8);
        CameraShake.Shake(10, 0.1f);
        yield return GeneralManager.waitForSeconds(1);
        FindObjectOfType<GenericVLEvents>().ActivateAsyncSceneLoad();
    }
}
