using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UTGameManager : MonoBehaviour
{
    [SerializeField] UTintro _intro;
    [SerializeField] UTfall _fall;
    public enum UTProjectStates {intro, fall}
    public UTProjectStates currentUTProjectState;
    [SerializeField] int[] bannedScenes;

    [HideInInspector] public Image blackScreen;

    void Awake()
    {
        FindObjectOfType<DRP>().RP("Can you hear me?");
        GeneralManager.SetSceneSave();
    }

    void OnLevelWasLoaded(int level)
    {
        if(bannedScenes.Contains(level))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        switch(currentUTProjectState)
        {
            case UTProjectStates.intro:
                _intro.CheckStateStarted(this);
                break;
            case UTProjectStates.fall:
                _fall.CheckStateStarted(this, blackScreen);
                break;
        }

        //if(Input.GetKeyDown(KeyCode.R) && Application.isEditor)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(UTGameManager))]
    public class UTGameManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            GUILayout.BeginHorizontal();
                bool StartAtFall = GUILayout.Button("Start At Fall");
                bool StartOnStair = GUILayout.Button("Start On Stair");
                bool StartAtPulse = GUILayout.Button("Start At Pulse");
            GUILayout.EndHorizontal();

            if(StartAtFall)
            {
                PlayerController controller = FindObjectOfType<PlayerController>();
                Transform playerPos = childTrans("Fall");

                controller.transform.position = playerPos.position;
                controller.transform.rotation = playerPos.rotation;

                controller.lockMovement = true;
                controller.lockLook = true;                
            }
            else if(StartOnStair)
            {
                PlayerController controller = FindObjectOfType<PlayerController>();
                Transform playerPos = childTrans("Stair");

                controller.transform.position = playerPos.position;
                controller.transform.rotation = playerPos.rotation;

                controller.lockMovement = false;
                controller.lockLook = false;
            }
            else if(StartAtPulse)
            {
                PlayerController controller = FindObjectOfType<PlayerController>();
                Transform playerPos = childTrans("Pulse");

                controller.transform.position = playerPos.position;
                controller.transform.rotation = playerPos.rotation;

                controller.lockMovement = false;
                controller.lockLook = false;
            }
        }

        Transform childTrans(string name)
        {
            UTGameManager manager = (UTGameManager)target;
            foreach(Transform trans in manager.GetComponentsInChildren<Transform>())
            {
                if(trans.name.ToLower() == name.ToLower()) return trans;
            }
            
            return null;
        }
    }
    #endif
}
