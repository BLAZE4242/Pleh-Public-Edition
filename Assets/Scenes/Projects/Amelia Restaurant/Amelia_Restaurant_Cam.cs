using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
// using Aura2API; // AURA HERE

public class Amelia_Restaurant_Cam : MonoBehaviour
{
    [SerializeField] Transform mainCam;

    [Header("Camera")]
    //[SerializeField] AuraLight ameliaLight; // AURA HERE
    [SerializeField] Light ameliaLight; // AURA NOT HERE
    [SerializeField] Transform choicePos;

    [Header("Post Processing")]
    [SerializeField] PostProcessVolume _volume;
    [SerializeField] PostProcessProfile choiceProfile;

    [Header("Audio Clips")]
    [SerializeField] AudioSource _sfx;
    [SerializeField] AudioClip whisperClip;
    AudioClip defaultClip;

    [Header("Audio Effects")]
    [SerializeField] AudioLowPassFilter _lowPass;
    [SerializeField] AudioReverbFilter _reverb;

    PostProcessProfile defaultProfile;
    Vector3 defaultLightPos;
    float defaultLightStrength;

    private void Start()
    {
        defaultProfile = _volume.profile;

        defaultLightPos = ameliaLight.transform.position;
        // defaultLightStrength = ameliaLight.strength; // AURA HERE

        defaultClip = _sfx.clip;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCam.rotation;
    }

    public void GoDownUnder()
    {
        mainCam.GetComponentInParent<Transform>().position -= new Vector3(0, 10, 0);
        // mainCam.GetComponent<AuraCamera>().frustumSettings.baseSettings.density = 0; // AURA HERE
        ameliaLight.transform.position = choicePos.transform.position;
        // ameliaLight.strength = 40; // AURA HERE

        _volume.profile = choiceProfile;

        _lowPass.enabled = true;
        _reverb.enabled = true;

        _sfx.clip = whisperClip;
        _sfx.Play();
    }

    public void GoUpAbove()
    {
        mainCam.GetComponentInParent<Transform>().position += new Vector3(0, 10, 0);
        // mainCam.GetComponent<AuraCamera>().frustumSettings.baseSettings.density = 0.01f; // AURA HERE
        ameliaLight.transform.position = defaultLightPos;
        // ameliaLight.strength = defaultLightStrength; // AURA HERE

        _volume.profile = defaultProfile;

        _lowPass.enabled = false;
        _reverb.enabled = false;

        _sfx.clip = defaultClip;
        _sfx.Play();
    }
}
