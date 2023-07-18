using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmeliaStateManager : MonoBehaviour
{
    public enum AmeliaProjectStates {notification, typing, reflection, se}
    public AmeliaProjectStates currentProjectState;
    [SerializeField] AmeliaNotificationState _notificationState;
    [SerializeField] AmeliaTypingState _typingState;
    [SerializeField] AmeliaSeState _seState;

    private void Start()
    {
        if (PlayerPrefs.GetString("thats it") == "glotchiness")
        {
            currentProjectState = AmeliaProjectStates.se;
            return;
        }


        if (PlayerPrefs.GetInt("glotchiness") == 7)
        {
            FindObjectOfType<DRP>().RP("IT MADE ME DO THIS.");
            currentProjectState = AmeliaProjectStates.reflection;
        }
        else
        {
            FindObjectOfType<DRP>().RP("What have I done?");
            GeneralManager.SetSceneSave();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        switch (currentProjectState)
        {
            case AmeliaProjectStates.notification:
                _notificationState.CheckStateStarted(this);
                break;
            case AmeliaProjectStates.typing:
                _typingState.CheckStateStarted(this);
                break;
            case AmeliaProjectStates.reflection:
                _typingState.CheckStateStarted(this);
                FindObjectOfType<MessageConvoManager>().currentAmeliaTypeState = MessageConvoManager.AmeliaTypeState.Typing;
                break;
            case AmeliaProjectStates.se:
                _seState.CheckStateStarted(this);
                break;
        }

        //if (Input.GetKeyDown(KeyCode.R) && Application.isEditor)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }
}
