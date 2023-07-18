using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class VoiceLineManager : MonoBehaviour
{
    [SerializeField] VoiceLine openingVL;
    [SerializeField] float timeUntilOpeningVL;

    AudioSource lineSource;
    TextMeshProUGUI subText;

    void Awake()
    {
        lineSource = GetComponent<AudioSource>();
        subText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private IEnumerator Start()
    {
        if (openingVL != null)
        {
            yield return GeneralManager.waitForSeconds(timeUntilOpeningVL);
            PlayLine(openingVL);
        }
    }

    public void PlayLine(VoiceLine line)
    {
        StopAllCoroutines();
        StartCoroutine(PlayAudio(line.clipsArray, null));
        StartCoroutine(WaitEvents(line));
        StartCoroutine(SubtitleSequence(line));
        // StartCoroutine(ShowLine(line.LineData(), line.clipsArray[0]));
    }

    public void PlayLine(VoiceLine line, UnityAction eventOnEnd = null)
    {
        StopAllCoroutines();
        StartCoroutine(PlayAudio(line.clipsArray, eventOnEnd));
        StartCoroutine(WaitEvents(line));
        StartCoroutine(SubtitleSequence(line));
        // StartCoroutine(ShowLine(line.LineData(), line.clipsArray[0]));
    }

    IEnumerator PlayAudio(AudioClip[] clips, UnityAction eventOnEnd)
    {
        if(clips.Length == 1)
        {
            lineSource.clip = clips[0];
            lineSource.Play();
            if (eventOnEnd != null)
            {
                yield return GeneralManager.waitForSeconds(clips[0].length);
                eventOnEnd.Invoke();
            }
            yield break;
        }

        foreach(AudioClip clip in clips)
        {
            lineSource.clip = clip;
            lineSource.Play();
            yield return GeneralManager.waitForSeconds(clip.length);
        }
    }

    IEnumerator WaitEvents(VoiceLine line)
    {
        if(line.events.Count != line.eventTimes.Count)
        {
            //Debug.LogWarning($"Warning called from {line.name}! Number of events does not match the number of times, will ignore for now but pls fix");
        }

        for(int i = 0; i<line.eventTimes.Count; i++)
        {
            

            if (i != 0)
            {
                yield return GeneralManager.waitForSeconds(line.eventTimes[i] - line.eventTimes[i - 1]);
            }
            else yield return GeneralManager.waitForSeconds(line.eventTimes[i]);
            line.events[i].Invoke();
        }


        if (line.events.Count == line.eventTimes.Count + 1) // If the amount of events to invoke is one greater than the amount of times to count for
        {
            yield return GeneralManager.waitForSeconds(lineSource.clip.length - lineSource.time);
            line.events.Last().Invoke();
        }
    }

    IEnumerator ShowLine(Dictionary<string, float> LineData, AudioClip clip)
    {
        foreach(string i in LineData.Keys)
        {
            Debug.Log(i);
        }

        // Key is the line and value is the time
        foreach(KeyValuePair<string, float> data in LineData)
        {
            string line = data.Key;
            float time = data.Value;

            subText.text = line;
            yield return GeneralManager.waitForSeconds(time);
        }

        subText.text = "";
    }

    public void StopLines()
    {
        StopAllCoroutines();
        lineSource.Stop();
    }

    IEnumerator SubtitleSequence(VoiceLine voiceLine)
    {
        Debug.Log("Hello???");
        int subtitleItteration = 0;

        while (true)
        {
            float timeToWait = GetSubtitleTime(subtitleItteration, voiceLine);
            Debug.Log(timeToWait);
            subtitleItteration++;

            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    float GetSubtitleTime(int itteration, VoiceLine vl)
    {
        return float.Parse(vl.subtitleTranscript.ToString()[0].ToString());
    }

    void ShowSubtitle(string line)
    {

    }

}
