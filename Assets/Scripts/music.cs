using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Audio;

public class music : MonoBehaviour
{
    public AudioClip main, lowPitch, staticSfx, robot, jumpScare0, longStatic;
    [SerializeField] float stampMin, stampMax;
    [SerializeField] int[] bannedScenes;
    [SerializeField] string[] bannedScenesName;
    [HideInInspector] public AudioSource source;
    [SerializeField] bool CanBeMultiple;
    [SerializeField] AudioMixerGroup shhhh;
    
    bool isGlitching;

    void Awake()
    {
        music[] objs = FindObjectsOfType<music>();
        if (objs.Length > 1 && !CanBeMultiple)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
    }

    void OnLevelWasLoaded(int level)
    {
        foreach (int bannedLevel in bannedScenes)
        {
            if(level == bannedLevel) Destroy(gameObject);
        }
        
        foreach(string bannedLevel in bannedScenesName)
        {
            if (SceneManager.GetActiveScene().name == bannedLevel) Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "Level 1_G") Destroy(GetComponent<AudioLowPassFilter>());
    }

    public void staticGlitch()
    {
        source.PlayOneShot(staticSfx, 3.4f);
    }

    public void loopSfx(AudioClip sfx, float volume)
    {
        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = shhhh;
        sfxSource.volume = volume;
        sfxSource.clip = sfx;
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public IEnumerator musicGetGlitch(int repeatTimes = 0, UnityAction eventWhenDone = null)
    {
        Debug.Log("Glitch go overload...");
        if(repeatTimes > 0)
        {
            for(int i = repeatTimes; i > 0; i--)
            {
                stampMin = source.time;
                yield return new WaitForSecondsRealtime(0.26f);
                source.time = stampMin;
            }
        }
        else
        {
            isGlitching = true;
            while (isGlitching)
            {
                stampMin = source.time;
                yield return new WaitForSecondsRealtime(0.26f);
                source.time = stampMin;
            }
        }

        if(eventWhenDone != null) eventWhenDone.Invoke();
    }

    public void switchTrack(AudioClip targetTrack)
    {
        float currentTimeStamp = source.time;
        source.clip = targetTrack;
        source.Play();
        source.time = currentTimeStamp;
    }

    public void musicGetUnglitch()
    {
        isGlitching = false;
    }

    private void Update()
    {
        // Check if we pres G and if the game is running in editor
    }
}
