using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "file", menuName = "BIOS/New File")]
public class fileExplorer_file : ScriptableObject
{
    public enum FileTypes {txt, exe, zip, mp4, sex, mp3}
    public FileTypes fileType;
    [HideInInspector] public TextAsset m_textMessage;
    [HideInInspector] public string sceneName;
    [HideInInspector] public int glotchiness;
    [HideInInspector] public bool IsMalware;
    [HideInInspector] public string password;
    [HideInInspector] public fileExplorer_folder m_zipFolder;
    [HideInInspector] public VideoClip m_videoClip;
    [HideInInspector] public AudioClip m_audioClip;

    public bool isOpened()
    {
        if (fileType != FileTypes.zip) return false;

        if (!PlayerPrefs.HasKey("zip_opened")) return false;
        string[] openedZips = PlayerPrefs.GetString("zip_opened").Split(',');
        for (int i = 0; i < openedZips.Length; i += 2)
        {
            if (openedZips[i] == name && openedZips[i + 1] == password)
            {
                return true;
            }
            else if (openedZips[i] == name && openedZips[i + 1] != password && openedZips[i] != "15cubes")
            {
                FindObjectOfType<fileExplorer_inputManager>().ConsoleWrite("CHEATER.");
            }
        }
        return false;
    }



    #if UNITY_EDITOR
    [CustomEditor(typeof(fileExplorer_file))]
    public class fileExplorer_fileEditor : Editor
    {
        SerializedProperty textMessage;
        SerializedProperty zipFolder;
        SerializedProperty videoClip;
        SerializedProperty audioClip;

        private void OnEnable()
        {
            zipFolder = serializedObject.FindProperty("m_zipFolder");
            videoClip = serializedObject.FindProperty("m_videoClip");
            textMessage = serializedObject.FindProperty("m_textMessage");
            audioClip = serializedObject.FindProperty("m_audioClip");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            fileExplorer_file _file = (fileExplorer_file)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Data");

            EditorGUI.BeginChangeCheck();
            switch(_file.fileType)
            {
                case FileTypes.txt:
                    EditorGUILayout.PropertyField(textMessage, new GUIContent("Text Message"));
                    break;
                case FileTypes.exe:
                    _file.sceneName = EditorGUILayout.TextField("Scene Name", _file.sceneName);
                    _file.glotchiness = EditorGUILayout.IntField("Glotchiness", _file.glotchiness);
                    _file.IsMalware = EditorGUILayout.Toggle("Is Malware", _file.IsMalware);
                    break;
                case FileTypes.zip:
                    _file.password = EditorGUILayout.TextField("Password", _file.password);
                    EditorGUILayout.PropertyField(zipFolder, new GUIContent("Zip Folder"));
                    break;
                case FileTypes.mp4:
                    EditorGUILayout.PropertyField(videoClip, new GUIContent("Video Clip"));
                    break;
                case FileTypes.mp3:
                    EditorGUILayout.PropertyField(audioClip, new GUIContent("Audio Clip"));
                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}