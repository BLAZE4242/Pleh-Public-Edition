using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UTPulseSender : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float SendLightInterval = 4f;
    [SerializeField] float BuildUpTime = 6f;

    public enum PowerState {off, booting, on} // Not under the header because there's an error for some reason? idk you do you unity
    [Header("Spawning")]
    public PowerState currentPowerState;
    [SerializeField] AudioClip buildUp;
    [SerializeField] AudioClip shoot;
    public AudioSource source;
    [SerializeField] Transform LightSpawnPoint;
    [SerializeField] GameObject LightPrefab;
    public bool canShakeCamera = true;
    bool isRunningIEnumeratorBecauseIdontKnowHowToSpellCouritene = false;


    void Start()
    {

    }

    void Update()
    {
        switch(currentPowerState)
        {
            case PowerState.off:
                LerpSolution.lerpPitch(source, 0, 0.3f);
                LerpSolution.lerpVolume(source, 0, 0.3f);
                break;
            case PowerState.booting:
                bootingPulse();
                break;
            case PowerState.on:
                StartCoroutine(sendPulse());
                  break;
        }
    }

    void bootingPulse()
    {
        if(source.isPlaying) return;
        source.clip = buildUp;
        source.loop = true;
        source.Play();
        LerpSolution.lerpVolume(source, 0.4f, 0.3f, 0);
        LerpSolution.lerpPitch(source, 1f, 0.3f, 0);
    }

    IEnumerator sendPulse()
    {
        if(isRunningIEnumeratorBecauseIdontKnowHowToSpellCouritene) yield break;
        isRunningIEnumeratorBecauseIdontKnowHowToSpellCouritene = true;
        source.loop = false;
        source.clip = buildUp;
        source.Play();
        LerpSolution.lerpVolume(source, 0.4f, 0.1f, 0);
        yield return GeneralManager.waitForSeconds(BuildUpTime);
        source.Stop();
        UTPulseLight pulse = Instantiate(LightPrefab, transform.position, Quaternion.identity).GetComponent<UTPulseLight>();
        AudioSource pulseSource = pulse.GetComponent<AudioSource>();

        yield return GeneralManager.waitForEndOfFrame;
        pulseSource.PlayOneShot(shoot, 0.7f);
        pulse.canShakeCamera = canShakeCamera;
        isRunningIEnumeratorBecauseIdontKnowHowToSpellCouritene = false;
    }
}
 