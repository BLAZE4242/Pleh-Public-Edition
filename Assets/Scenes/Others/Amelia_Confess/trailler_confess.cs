using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
// using Aura2API; // AURA HERE

public class trailler_confess : MonoBehaviour
{
    [SerializeField] PostProcessVolume vol;
    [SerializeField] PostProcessProfile prof;
    [SerializeField] GameObject screen, screenLight;
    IEnumerator Start()
    {
        yield return GeneralManager.waitForSeconds(6);
        vol.profile = prof;
        screen.SetActive(true);
        screenLight.SetActive(true);
        ColorGrading grade;
        prof.TryGetSettings(out grade);
        LerpSolution.LerpPostExposure(grade, -0.5f, 1f);
        // LerpSolution.LerpLightIntensity(screenLight.GetComponent<AuraLight>(), screenLight.GetComponent<Light>().intensity, 1, 0); // AURA HERE
        yield return GeneralManager.waitForSeconds(2);
        CameraShake.Shake(2, 0.05f);
    }
}
