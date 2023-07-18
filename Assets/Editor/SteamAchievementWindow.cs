using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

public class SteamAchievementWindow : EditorWindow
{
    [MenuItem("Tools/Steam Achievement Window")]
    public static void ShowAchievementWindow()
    {
        EditorWindow wind = GetWindow<SteamAchievementWindow>();
        wind.titleContent = new GUIContent("Steam Achievement Window");

    }

    public void OnGUI()
    {
        //GUI.TextField(new Rect(10, 10, 200, 20), "Achievement to give");
    }
}
