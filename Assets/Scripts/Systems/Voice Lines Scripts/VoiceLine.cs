using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Example messages_clickMesssage_0 will play when a player clicks a message for the first time ever
[CreateAssetMenu(fileName = "section_explanation_number", menuName = "Pleh System/New Voice Line")]
public class VoiceLine : ScriptableObject
{
    [Header("Event Handling")]
    public List<UnityEvent> events = new List<UnityEvent>();
    public List<float> eventTimes = new List<float>();
    [HideInInspector] public float[] timesArray;
    public AudioClip[] clipsArray;
    [Header("Subtitles")]
    public TextAsset subtitleTranscript;
    
    // OLD
    [HideInInspector] public string[] subtitlesArray;
    [Space]
    [HideInInspector] int MinWordForSub, MaxWorkForSub;
    [HideInInspector] public bool showTime = true;
    [HideInInspector] public string Transcript;

    void Awake()
    {
        if(subtitlesArray.Length != timesArray.Length)
        {
            Debug.LogWarning("Number of subtitles does not match the number of times. Voice line cannot be instantiated and will prob create more errors now from VoiceLineManager.cs");
            return;
        }
        else if(clipsArray.Length == 0 || clipsArray[0] == null)
        {
            Debug.LogWarning("No audio clips given to play. Voice line cannot be instantiated and will prob create more errors now from VoiceLineManager.cs");
            return;
        }

        
    }

    public Dictionary<string, float> LineData()
    {
        Dictionary<string, float> LineDataTemp = new Dictionary<string, float>(); 

        for (int i = 0; i < subtitlesArray.Length; i++)
        {
            LineDataTemp.Add(subtitlesArray[i], timesArray[i]);
        }

        return LineDataTemp;
    }

    public string[] TranscriptToSubtitle(string trans)
    {
        List<string> tempSub = new List<string>();
        List<string> splitTrans = Regex.Split(trans, @"(?<=[.?!])").ToList();

        bool shouldAddToPrevious = false;

        foreach(string sentance in splitTrans)
        {
            string sentanceNew = sentance;
            if(sentance.StartsWith(" "))
            {
                sentanceNew = sentance.Remove(0, 1);
            }

            if(!shouldAddToPrevious) tempSub.Add(sentanceNew); // wtf
            else tempSub[tempSub.Count() - 1] += sentanceNew;

            int wordCount = WordCount(tempSub[tempSub.Count() - 1]); // Make the word count the entire sentance in array
            if(wordCount < MaxWorkForSub)
            {
                shouldAddToPrevious = true;
            }
            else
            {
                shouldAddToPrevious = false;
            }
        }

        tempSub.RemoveAt(tempSub.Count() - 1);

        return tempSub.ToArray();
    }

    int WordCount(string word)
    {
        return word.Split(' ').Count();
    }


    #if UNITY_EDITOR    
    [CustomEditor(typeof(VoiceLine))]
    public class VoiceLineEditor : Editor
    {

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();
            VoiceLine _VoiceLine = (VoiceLine)target;

            if (GUILayout.Button("Open transcript"))
            {
                //Application.OpenURL
            }

            #region old subtitle
            //EditorGUILayout.BeginHorizontal();
            //serializedObject.Update();

            //var ClipsProperty = serializedObject.FindProperty("clipsArray");
            //var SubtitleProperty = serializedObject.FindProperty("subtitlesArray");
            //var TimeProperty = serializedObject.FindProperty("timesArray");
            //serializedObject.Update();
            //if(_VoiceLine.showTime)
            //{
            //    EditorGUILayout.PropertyField(SubtitleProperty, true, GUILayout.MinWidth(200), GUILayout.MaxWidth(500));
            //    EditorGUILayout.PropertyField(TimeProperty, true, GUILayout.MinWidth(300), GUILayout.MaxWidth(600));
            //}
            //else
            //{
            //    EditorGUILayout.PropertyField(SubtitleProperty, true);
            //}

            //EditorGUILayout.EndHorizontal();

            //EditorGUILayout.LabelField("Transcript");
            //EditorStyles.textArea.wordWrap = true;
            //_VoiceLine.Transcript = EditorGUILayout.TextArea(_VoiceLine.Transcript, EditorStyles.textArea, GUILayout.MinHeight(50));

            //if(GUILayout.Button("Convert Transcript"))
            //{
            //    _VoiceLine.subtitlesArray = _VoiceLine.TranscriptToSubtitle(_VoiceLine.Transcript);
            //}

            //EditorGUILayout.PropertyField(ClipsProperty, true);


            //serializedObject.ApplyModifiedProperties();

            //EditorApplication.update.Invoke();
            #endregion
        }
    }
    #endif
}