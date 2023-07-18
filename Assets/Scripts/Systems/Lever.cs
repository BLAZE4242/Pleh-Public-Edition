using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;


[RequireComponent(typeof(InteractableObject))]
public class Lever : MonoBehaviour
{
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;
    public bool defaultActiveState = false;
    public bool currentActiveState = false;
    [SerializeField] GameObject leverObject;
    [HideInInspector] InteractableObject _interact;
    [SerializeField] TextMeshProUGUI tutText;

    bool isMoving = false;
    float OnRotation = -205;
    float OffRotation = -160;


    public void OnLeverInteract()
    {
        if (isMoving) return;

        Debug.Log("We're interacting!");

        Quaternion OffToOnRotation = Quaternion.Euler(-205, 0, 0);
        Quaternion OnToOffRotation = Quaternion.Euler(-160, 0, 0);
        switch(currentActiveState)
        {
            case false:
                LerpSolution.lerpRotation(leverObject.transform, OffToOnRotation, 2f);
                StartCoroutine(CheckLeverProgress(OffToOnRotation));
                break;
            case true:
                LerpSolution.lerpRotation(leverObject.transform, OnToOffRotation, 2f);
                StartCoroutine(CheckLeverProgress(OnToOffRotation));
                break;
        }

        tutText.gameObject.SetActive(false);
    }

    IEnumerator CheckLeverProgress(Quaternion targetQuat)
    {
        isMoving = true;

        while(leverObject.transform.rotation != targetQuat)
        {
            yield return new WaitForEndOfFrame();
        }

        currentActiveState = !currentActiveState;
        isMoving = false;

        if(currentActiveState == true)
        {
            onActivate.Invoke();
        }
        else
        {
            onDeactivate.Invoke();
        }
    }


    #if UNITY_EDITOR
    [CustomEditor(typeof(Lever))]
    public class LeverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Lever _lever = (Lever)target;

            base.OnInspectorGUI();
            bool DefaultOnButton = GUILayout.Button("Lever Default On");
            bool DefaultOffButton = GUILayout.Button("Lever Default Off");

            if(DefaultOnButton)
            {
                _lever.leverObject.transform.rotation = Quaternion.Euler(_lever.OnRotation, 0, 0);
                _lever.defaultActiveState = true;
                _lever.currentActiveState = true;
            }
            else if(DefaultOffButton)
            {
                _lever.leverObject.transform.rotation = Quaternion.Euler(_lever.OffRotation, 0, 0);
                _lever.defaultActiveState = false;
                _lever.currentActiveState = false;
            }
        }
    }
    #endif
}
