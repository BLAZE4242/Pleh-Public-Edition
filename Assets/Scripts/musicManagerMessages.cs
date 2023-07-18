using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class musicManagerMessages : MonoBehaviour
{
    public AudioClip intro, loop, staticSfx, omoriSfx, snap, reversePiano;
    [SerializeField] bool startInLoop;
    [HideInInspector] public AudioSource _source;
    public bool hasGoneLoop;
    public AudioClip[] glitchClips;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        _source = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        StartCoroutine(startMusic());
    }

    void OnLevelWasLoaded(int level)
    {
        _source.DOFade(1, 1);
        if(hasGoneLoop) changeTrack(loop);
        _source.DOPitch(1, 1);
        if(level == 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator startMusic()
    {
        if(startInLoop)
        {
            _source.clip = loop;
            _source.loop = true;
            _source.Play();
            hasGoneLoop = true;
        }
        else
        {
            yield return GeneralManager.waitForSeconds(intro.length - _source.time - 2.5f);
            _source.clip = loop;
            _source.loop = true;
            _source.Play();
            hasGoneLoop = true;
        }
    }

    public void doSnap()
    {
        _source.PlayOneShot(snap, 4);
    }

    public void changeTrack(AudioClip targetTrack, bool fade = false)
    {
        float currentTimeStamp = _source.time;
        _source.clip = targetTrack;
        _source.Play();
        _source.time = currentTimeStamp;
    }

    public IEnumerator pitchAudio(float pitch1, float speed1, float breakTime, float pitch2, float speed2)
    {
        float changePercentage = 0f;
        float currentPitch = _source.pitch;

        while (changePercentage < 1f)
        {
            changePercentage += Time.deltaTime * speed1;
            _source.pitch = Mathf.Lerp(currentPitch, pitch1, changePercentage);
            yield return new WaitForEndOfFrame();
        }

        yield return GeneralManager.waitForSeconds(breakTime);

        changePercentage = 0f;
        currentPitch = _source.pitch;

        while (changePercentage < 1f)
        {
            changePercentage += Time.deltaTime * speed2;
            _source.pitch = Mathf.Lerp(currentPitch, pitch2, changePercentage);
            yield return new WaitForEndOfFrame();
        }
    }

    public void staticGlitch(float volume = 1)
    {
        _source.PlayOneShot(staticSfx, volume);
    }

    public void omoriGlitch(float volume = 1)
    {
        _source.PlayOneShot(omoriSfx, volume);
    }

    public void smallGlitch(int clipIndex, float volume = 1f)
    {
        foreach (AudioClip clip in glitchClips)
        {
            if (clip.name == "glitch" + clipIndex) _source.PlayOneShot(clip, volume);
        }
    }
}
