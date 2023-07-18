using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//[CreateAssetMenu(fileName = "pauseMenu_options_general", menuName = "Pleh System/New Pause Menu")]
public class PauseMenu : MonoBehaviour
{
    public PauseScreenOption[] menuOptions;

    public void AssignMenuOptions()
    {
        menuOptions = GetComponentsInChildren<PauseScreenOption>();
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(PauseMenu))]
    public class PauseMenuEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PauseMenu _menu = (PauseMenu)target;

            base.OnInspectorGUI();
            bool assignMenuOptions = GUILayout.Button("Assign Menu Options");

            if (assignMenuOptions)
            {
                _menu.AssignMenuOptions();
            }
        }
    }
    #endif
}
