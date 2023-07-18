using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmeliaNotificationState : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] Transform startCamPos;

    Vector3 endCamPos;
    AmeliaStateManager Manager;

    void StateStart(AmeliaStateManager _manager)
    {
        endCamPos = mainCam.transform.position;
        mainCam.transform.position = startCamPos.position;
        string playerName = PlayerPrefs.GetString("playerName");
        spawnMessage("hey");
        Manager = _manager;
    }

    void spawnMessage(string content)
    {
        //StartCoroutine(FindObjectOfType<messagesSpawnerManager>().spawnMessage(new Message("Amelia", content)));
        foreach (messageManager message in FindObjectsOfType<messageManager>())
        {
            message.onClick.RemoveAllListeners();
            message.onClick.AddListener(onNotificationClick);
            Debug.Log("Not working?");
        }
    }

    public void onNotificationClick()
    {
        LerpSolution.lerpPosition(mainCam.transform, endCamPos, 1.7f);

        Manager.currentProjectState = AmeliaStateManager.AmeliaProjectStates.typing;
        FindObjectOfType<MessageConvoManager>().currentAmeliaTypeState = MessageConvoManager.AmeliaTypeState.Typing;
    }

    void StateUpdate(AmeliaStateManager _manager)
    {

    }

    bool hasStarted;
    public void CheckStateStarted(AmeliaStateManager _manager)
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
