using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetOut : MonoBehaviour
{
    [SerializeField] AudioClip[] getOutClips;

    float lowMinVolume = 0f, lowMaxVolume = 0.3f;
    float lowMinTimeWait = 0f, lowMaxTimeWait = 1f;

    float highMinVolume = 0.5f, highMaxVolume = 3f;
    float highMinTimeWait = 0.08f, highMaxTimeWait = 0.7f;

    float actualMinVolume, actualMaxVolume;
    float actualMinTimeWait, actualMaxTimeWait;

    AudioLowPassFilter lowPass;

    [Range(0, 1)] public float craziness;

    AudioSource source;

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
        lowPass = GetComponent<AudioLowPassFilter>();
    }

    private void OnLevelWasLoaded(int level)
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    private void Update()
    {
        actualMinVolume = Mathf.Lerp(lowMinVolume, highMinVolume, craziness);
        actualMaxVolume = Mathf.Lerp(lowMaxVolume, highMaxVolume, craziness);
        actualMinTimeWait = Mathf.Lerp(lowMinTimeWait, highMinTimeWait, craziness);
        actualMaxTimeWait = Mathf.Lerp(lowMaxTimeWait, highMaxTimeWait, craziness);
        lowPass.cutoffFrequency = Mathf.Lerp(727, 2000, craziness);
    }

    public void startGetOut()
    {
        StartCoroutine(startGetOutEnum());
    }

    IEnumerator startGetOutEnum()
    {
        while (true)
        {
            source.PlayOneShot(getOutClips[Random.Range(0, getOutClips.Length - 1)], Random.Range(actualMinVolume, actualMaxVolume));
            yield return GeneralManager.waitForSeconds(Random.Range(actualMinTimeWait, actualMaxTimeWait));
        }
    }
}
