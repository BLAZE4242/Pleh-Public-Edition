using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
// using Aura2API; // AURA HERE
using TMPro;
using DigitalRuby.RainMaker;
using Kino;

public class LerpSolution : MonoBehaviour
{
    #region Instance Instantiation
    public static LerpSolution instance = null;
    public static LerpSolution Instance
    {
        get
        {
            if (instance != null)
            {
                instance = FindObjectOfType<LerpSolution>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "LerpSolution";
                    instance = go.AddComponent<LerpSolution>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        StopAllCoroutines();
    }

    public static void StopCoroutines()
    {
        if (instance != null)
        {
            instance.StopAllCoroutines();
        }
    }

    #endregion
    #region Methods
    public static void lerpVolume(AudioSource source, float targetVolume, float speed)
    {
        instance.StartCoroutine(instance.lerpVolumeEnum(source, targetVolume, speed));
    }

    public static void lerpVolume(AudioSource source, float targetVolume, float speed, float startVolume)
    {
        instance.StartCoroutine(instance.lerpVolumeEnum(source, targetVolume, speed, startVolume));
    }

    public static void lerpPitch(AudioSource source, float targetPitch, float speed)
    {
        instance.StartCoroutine(instance.lerpPitchEnum(source, targetPitch, speed));
    }

    public static void lerpPitch(AudioSource source, float targetPitch, float speed, float startPitch)
    {
        instance.StartCoroutine(instance.lerpPitchEnum(source, targetPitch, speed, startPitch));
    }

    public static void lerpLowPass(AudioLowPassFilter source, float targetCutoff, float speed)
    {
        instance.StartCoroutine(instance.lerpLowPassEnum(source, targetCutoff, speed));
    }

    public static void lerpLowPass(AudioLowPassFilter source, float targetCutoff, float speed, float startCutoff)
    {
        instance.StartCoroutine(instance.lerpLowPassEnum(source, targetCutoff, speed, startCutoff));
    }

    public static void lerpHighPass(AudioHighPassFilter source, float targetCutoff, float speed)
    {
        instance.StartCoroutine(instance.lerpHighPassEnum(source, targetCutoff, speed));
    }

    public static void lerpHighPass(AudioHighPassFilter source, float targetCutoff, float speed, float startCutoff)
    {
        instance.StartCoroutine(instance.lerpHighPassEnum(source, targetCutoff, speed, startCutoff));
    }

    public static void lerpPosition(Transform modelTransform, Vector3 targetPosition, float speed, bool useLocalPosition = false)
    {
        instance.StartCoroutine(instance.lerpPositionEnum(modelTransform, targetPosition, speed, useLocalPosition));
    }

    public static void lerpPosition(Transform modelTransform, Vector3 targetPosition, float speed, Vector3 startPosition)
    {
        instance.StartCoroutine(instance.lerpPositionEnum(modelTransform, targetPosition, speed, startPosition));
    }

    public static void lerpPositionTime(Transform modelTransform, Vector3 targetPosition, float speed, bool useLocalPosition = false)
    {
        instance.StartCoroutine(instance.lerpPositionTimeEnum(modelTransform, targetPosition, speed, useLocalPosition));
    }

    public static void lerpRotation(Transform modelTransform, Quaternion targetRotation, float speed)
    {
        instance.StartCoroutine(instance.lerpRotationEnum(modelTransform, targetRotation, speed));
    }

    public static void lerpRotationTime(Transform modelTransform, Quaternion targetRotation, float speed)
    {
        instance.StartCoroutine(instance.lerpRotationTimeEnum(modelTransform, targetRotation, speed));
    }

    public static void lerpRotation(Transform modelTransform, Quaternion targetRotation, float speed, Quaternion startRotation)
    {
        instance.StartCoroutine(instance.lerpRotationEnum(modelTransform, targetRotation, speed, startRotation));
    }

    public static void lerpLocalRotation(Transform modelTransform, Quaternion targetRotation, float speed)
    {
        instance.StartCoroutine(instance.lerpLocalRotationEnum(modelTransform, targetRotation, speed));
    }

    public static void lerpLocalRotation(Transform modelTransform, Quaternion targetRotation, float speed, Quaternion startRotation)
    {
        instance.StartCoroutine(instance.lerpLocalRotationEnum(modelTransform, targetRotation, speed, startRotation));
    }

    public static void lerpScale(Transform modelTransform, Vector3 targetScale, float speed)
    {
        instance.StartCoroutine(instance.lerpScaleEnum(modelTransform, targetScale, speed));
    }

    public static void lerpScale(Transform modelTransform, Vector3 targetScale, float speed, Vector3 startScale)
    {
        instance.StartCoroutine(instance.lerpScaleEnum(modelTransform, targetScale, speed, startScale));
    }

    public static void lerpMaterialColour(Material material, Color targetColour, float speed)
    {
        instance.StartCoroutine(instance.lerpMaterialColourEnum(material, targetColour, speed));
    }

    public static void lerpMaterialColour(Material material, Color targetColour, float speed, Color startColour)
    {
        instance.StartCoroutine(instance.lerpMaterialColourEnum(material, targetColour, speed, startColour));
    }

    public static void lerpSpriteColour(SpriteRenderer spriteRenderer, Color targetColour, float speed)
    {
        instance.StartCoroutine(instance.lerpSpriteColourEnum(spriteRenderer, targetColour, speed));
    }

    public static void lerpSpriteColour(SpriteRenderer spriteRenderer, Color targetColour, float speed, Color startColour)
    {
        instance.StartCoroutine(instance.lerpSpriteColourEnum(spriteRenderer, targetColour, speed, startColour));
    }

    public static void lerpImageColour(Image image, Color targetColour, float speed)
    {
        instance.StartCoroutine(instance.lerpImageColourEnum(image, targetColour, speed));
    }

    public static void lerpImageColour(Image image, Color targetColour, float speed, Color startColour)
    {
        instance.StartCoroutine(instance.lerpImageColourEnum(image, targetColour, speed, startColour));
    }

    public static void lerpImageColourTime(Image image, Color targetColour, float speed)
    {
        instance.StartCoroutine(instance.lerpImageColourTimeEnum(image, targetColour, speed));
    }

    public static void lerpImageColourTime(Image image, Color targetColour, float speed, Color startColour)
    {
        instance.StartCoroutine(instance.lerpImageColourTimeEnum(image, targetColour, speed, startColour));
    }

    public static void lerpTextColour(TextMeshProUGUI text, Color targetColour, float speed)
    {
        instance.StartCoroutine(instance.lerpTextColourEnum(text, targetColour, speed));
    }

    public static void lerpTextColour(TextMeshProUGUI text, Color targetColour, float speed, Color startColour)
    {
        instance.StartCoroutine(instance.lerpTextColourEnum(text, targetColour, speed, startColour));
    }

    public static void lerpTextColour(TextMeshPro text, Color targetColour, float speed)
    {
        instance.StartCoroutine(LerpSolution.lerpTextColourEnum(text, targetColour, speed));
    }

    public static void lerpTextColour(TextMeshPro text, Color targetColour, float speed, Color startColour)
    {
        instance.StartCoroutine(instance.lerpTextColourEnum(text, targetColour, speed, startColour));
    }

    public static void lerpCamColour(Camera camera, Color targetColour, float speed)
    {
        instance.StartCoroutine(instance.lerpCamColourEnum(camera, targetColour, speed));
    }

    public static void lerpCamColour(Camera camera, Color targetColour, float speed, Color startColour)
    {
        instance.StartCoroutine(instance.lerpCamColourEnum(camera, targetColour, speed, startColour));
    }

    public static void lerpCamSize(Camera camera, float targetSize, float speed)
    {
        instance.StartCoroutine(instance.lerpCamSizeEnum(camera, targetSize, speed));
    }

    public static void lerpCamSize(Camera camera, float targetSize, float speed, float startSize)
    {
        instance.StartCoroutine(instance.lerpCamSizeEnum(camera, targetSize, speed, startSize));
    }

    public static void lerpCamSizeTime(Camera camera, float targetSize, float speed)
    {
        instance.StartCoroutine(instance.lerpCamSizeTimeEnum(camera, targetSize, speed));
    }

    public static void lerpCamSizeTime(Camera camera, float targetSize, float speed, float startSize)
    {
        instance.StartCoroutine(instance.lerpCamSizeTimeEnum(camera, targetSize, speed, startSize));
    }

    public static void lerpCamFov(Camera camera, float targetFov, float speed)
    {
        instance.StartCoroutine(instance.lerpCamFovEnum(camera, targetFov, speed));
    }

    public static void lerpCamFov(Camera camera, float targetFov, float speed, float startFov)
    {
        instance.StartCoroutine(instance.lerpCamFovEnum(camera, targetFov, speed, startFov));
    }

    public static void lerpVignetteIntensity(Vignette vignette, float targetIntensity, float speed)
    {
        instance.StartCoroutine(instance.lerpVignetteIntensityEnum(vignette, targetIntensity, speed));
    }

    public static void lerpVignetteIntensity(Vignette vignette, float targetIntensity, float speed, float startIntensity)
    {
        instance.StartCoroutine(instance.lerpVignetteIntensityEnum(vignette, targetIntensity, speed, startIntensity));
    }

    public static void lerpLensDistortion(LensDistortion lensDistort, float targetDistort, float speed)
    {
        instance.StartCoroutine(instance.lerpLensDistortionEnum(lensDistort, targetDistort, speed));
    }

    public static void lerpLensDistortion(LensDistortion lensDistort, float targetDistort, float speed, float startDistort)
    {
        instance.StartCoroutine(instance.lerpLensDistortionEnum(lensDistort, targetDistort, speed, startDistort));
    }

    public static void lerpChromaticAberration(ChromaticAberration chromaticAbberation, float targetIntensity, float speed)
    {
        instance.StartCoroutine(instance.lerpChromaticAberrationEnum(chromaticAbberation, targetIntensity, speed));
    }

    public static void lerpChromaticAberration(ChromaticAberration chromIntensity, float targetIntensity, float speed, float startIntensity)
    {
        instance.StartCoroutine(instance.lerpChromaticAberrationEnum(chromIntensity, targetIntensity, speed, startIntensity));
    }

    public static void lerpDOFdistance(DepthOfField dof, float targetDistance, float speed)
    {
        instance.StartCoroutine(instance.lerpDOFdistanceEnum(dof, targetDistance, speed));
    }

    public static void lerpDOFdistance(DepthOfField dof, float targetDistance, float speed, float startDistance)
    {
        instance.StartCoroutine(instance.lerpDOFdistanceEnum(dof, targetDistance, speed, startDistance));
    }

    public static void lerpBloomIntensity(Bloom bloom, float targetIntensity, float speed)
    {
        instance.StartCoroutine(instance.lerpBloomIntensityEnum(bloom, targetIntensity, speed));
    }

    public static void lerpBloomIntensity(Bloom bloom, float targetIntensity, float speed, float startIntensity)
    {
        instance.StartCoroutine(instance.lerpBloomIntensityEnum(bloom, targetIntensity, speed, startIntensity));
    }

    public static void LerpColourFilter(ColorGrading colorGrading, Color targetColour, float speed)
    {
        instance.StartCoroutine(instance.lerpColourFilterEnum(colorGrading, targetColour, speed));
    }

    public static void LerpColourFilter(ColorGrading colorGrading, Color targetColour, float speed, Color startColour)
    {
        instance.StartCoroutine(instance.lerpColourFilterEnum(colorGrading, targetColour, speed, startColour));
    }

    public static void LerpSaturation(ColorGrading colorGrading, float targetSaturation, float speed)
    {
        instance.StartCoroutine(instance.lerpSaturationEnum(colorGrading, targetSaturation, speed));
    }

    public static void LerpSaturation(ColorGrading colorGrading, float targetSaturation, float speed, float startSaturation)
    {
        instance.StartCoroutine(instance.lerpSaturationEnum(colorGrading, targetSaturation, speed, startSaturation));
    }

    public static void LerpColourGradingGeneral(ColorGrading colorGrading, float targetTemperature, float targetTint, float targetRedMix, float targetGreenMix, float targetBlueMix, float speed)
    {
        instance.StartCoroutine(instance.lerpColourGradingGeneralEnum(colorGrading, targetTemperature, targetTint, targetRedMix, targetGreenMix, targetBlueMix, speed));
    }

    public static void LerpPostExposure(ColorGrading colorGrading, float targetExposure, float speed)
    {
        instance.StartCoroutine(instance.lerpPostExposure(colorGrading, targetExposure, speed));
    }

    public static void LerpPostExposure(ColorGrading colorGrading, float targetExposure, float speed, float startExposure)
    {
        instance.StartCoroutine(instance.lerpPostExposure(colorGrading, targetExposure, speed, startExposure));
    }

    public static void LerpLightRange(Light light, float targetRange, float speed)
    {
        instance.StartCoroutine(instance.lerpLightRange(light, targetRange, speed));
    }
    public static void LerpLightRange(Light light, float targetRange, float speed, float startRange)
    {
        instance.StartCoroutine(instance.lerpLightRange(light, targetRange, speed, startRange));
    }

    //public static void LerpLightScattering(AuraLight light, float targetScatter, float speed) // AURA HERE
    //{ // AURA HERE
    //    instance.StartCoroutine(instance.lerpLightScattering(light, targetScatter, speed)); // AURA HERE
    //} // AURA HERE

    //public static void LerpLightScattering(AuraLight light, float targetScatter, float speed, float startScatter) // AURA HERE
    //{ // AURA HERE
    //    instance.StartCoroutine(instance.lerpLightScattering(light, targetScatter, speed, startScatter)); // AURA HERE
    //} // AURA HERE

    public static void LerpLightIntensity(Light light, float targetIntensity, float speed)
    {
        instance.StartCoroutine(instance.lerpLightIntensity(light, targetIntensity, speed));
    }

    public static void LerpLightIntensity(Light light, float targetIntensity, float speed, float startIntensity)
    {
        instance.StartCoroutine(instance.lerpLightIntensity(light, targetIntensity, speed, startIntensity));
    }

    //public static void LerpLightIntensity(AuraLight light, float targetIntensity, float speed) // AURA HERE
    //{ // AURA HERE
    //    instance.StartCoroutine(instance.lerpLightIntensity(light, targetIntensity, speed)); // AURA HERE
    //} // AURA HERE

    //public static void LerpLightIntensity(AuraLight light, float targetIntensity, float speed, float startIntensity) // AURA HERE
    //{ // AURA HERE
    //    instance.StartCoroutine(instance.lerpLightIntensity(light, targetIntensity, speed, startIntensity)); // AURA HERE
    //} // AURA HERE

    public static void LerpGetOutCraziness(GetOut getOut, float targetCraziness, float speed)
    {
        instance.StartCoroutine(instance.lerpGetOutCrazinessEnum(getOut, targetCraziness, speed));
    }

    public static void LerpGetOutCraziness(GetOut getOut, float targetCraziness, float speed, float startCraziness)
    {
        instance.StartCoroutine(instance.lerpGetOutCrazinessEnum(getOut, targetCraziness, speed, startCraziness));
    }

    public static void LerpPPweight(PostProcessVolume volume, float targetWeight, float speed)
    {
        instance.StartCoroutine(instance.lerpPPweightEnum(volume, targetWeight, speed));
    }

    public static void LerpPPweight(PostProcessVolume volume, float targetWeight, float speed, float startWeight)
    {
        instance.StartCoroutine(instance.lerpPPweightEnum(volume, targetWeight, speed, startWeight));
    }

    public static void LerpProbeIntensity(ReflectionProbe probe, float targetIntensity, float speed)
    {
        instance.StartCoroutine(instance.lerpProbeIntensityEnum(probe, targetIntensity, speed));
    }

    public static void LerpProbeIntensity(ReflectionProbe probe, float targetIntensity, float speed, float startIntensity)
    {
        instance.StartCoroutine(instance.lerpProbeIntensityEnum(probe, targetIntensity, speed, startIntensity));
    }

    public static void LerpPlayerSpeed(PlayerController controller, float targetSpeed, float speed)
    {
        instance.StartCoroutine(instance.lerpPlayerSpeedEnum(controller, targetSpeed, speed));
    }

    public static void LerpPlayerSpeed(PlayerController controller, float targetSpeed, float speed, float startSpeed)
    {
        instance.StartCoroutine(instance.lerpPlayerSpeedEnum(controller, targetSpeed, speed, startSpeed));
    }

    public static void LerpBlendValue(Material mat, float targetBlend, float speed)
    {
        instance.StartCoroutine(instance.lerpBlendValueEnum(mat, targetBlend, speed));
    }

    public static void LerpBlendValue(Material mat, float targetBlend, float speed, float startBlend)
    {
        instance.StartCoroutine(instance.lerpBlendValueEnum(mat, targetBlend, speed, startBlend));
    }

    public static void LerpRainIntensity(RainScript script, float targetIntensity, float speed)
    {
        instance.StartCoroutine(instance.LerpRainIntensityEnum(script, targetIntensity, speed));
    }

    public static void LerpRainIntensity(RainScript script, float targetIntensity, float speed, float startIntensity)
    {
        instance.StartCoroutine(instance.LerpRainIntensityEnum(script, targetIntensity, speed, startIntensity));
    }

    #endregion
    #region Coroutines

    IEnumerator lerpVolumeEnum(AudioSource source, float targetVolume, float speed)
    {
        float currentVolume = source.volume;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            source.volume = Mathf.Lerp(currentVolume, targetVolume, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpVolumeEnum(AudioSource source, float targetVolume, float speed, float startVolume)
    {
        float currentVolume = startVolume;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            source.volume = Mathf.Lerp(currentVolume, targetVolume, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpPitchEnum(AudioSource source, float targetPitch, float speed)
    {
        float currentPitch = source.pitch;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            source.pitch = Mathf.Lerp(currentPitch, targetPitch, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }
    
    IEnumerator lerpPitchEnum(AudioSource source, float targetPitch, float speed, float startPitch)
    {
        float currentPitch = startPitch;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            source.pitch = Mathf.Lerp(currentPitch, targetPitch, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpLowPassEnum(AudioLowPassFilter lowPass, float targetCutoff, float speed)
    {
        float currentCutoff = lowPass.cutoffFrequency;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            lowPass.cutoffFrequency = Mathf.Lerp(currentCutoff, targetCutoff, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpLowPassEnum(AudioLowPassFilter lowPass, float targetCutoff, float speed, float startCutoff)
    {
        float currentCutoff = startCutoff;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            lowPass.cutoffFrequency = Mathf.Lerp(currentCutoff, targetCutoff, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpHighPassEnum(AudioHighPassFilter highPass, float targetCutoff, float speed)
    {
        float currentCutoff = highPass.cutoffFrequency;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            highPass.cutoffFrequency = Mathf.Lerp(currentCutoff, targetCutoff, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpHighPassEnum(AudioHighPassFilter highPass, float targetCutoff, float speed, float startCutoff)
    {
        float currentCutoff = startCutoff;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            highPass.cutoffFrequency = Mathf.Lerp(currentCutoff, targetCutoff, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpPositionEnum(Transform modelTransform, Vector3 targetPosition, float speed, bool useLocalPosition)
    {
        Vector3 currentPosition = modelTransform.position;
        if(useLocalPosition) currentPosition = modelTransform.localPosition;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            if(useLocalPosition) modelTransform.localPosition = Vector3.Lerp(currentPosition, targetPosition, dist);
            else modelTransform.position = Vector3.Lerp(currentPosition, targetPosition, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpPositionEnum(Transform modelTransform, Vector3 targetPosition, float speed, Vector3 startPosition)
    {
        Vector3 currentPosition = startPosition;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            modelTransform.position = Vector3.Lerp(currentPosition, targetPosition, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpPositionTimeEnum(Transform modelTransform, Vector3 targetPosition, float speed, bool useLocalPosition)
    {
        Vector3 currentPosition = modelTransform.position;
        if (useLocalPosition) currentPosition = modelTransform.localPosition;
        float dist = 0;
        while (dist < speed)
        {
            dist += Time.deltaTime;
            if (useLocalPosition) modelTransform.localPosition = Vector3.Lerp(currentPosition, targetPosition, dist / speed);
            else modelTransform.position = Vector3.Lerp(currentPosition, targetPosition, dist / speed);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpRotationEnum(Transform modelTransform, Quaternion targetRotation, float speed)
    {
        Quaternion currentRotation = modelTransform.rotation;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            modelTransform.rotation = Quaternion.Lerp(currentRotation, targetRotation, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpRotationEnum(Transform modelTransform, Quaternion targetRotation, float speed, Quaternion startRotation)
    {
        Quaternion currentRotation = startRotation;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            modelTransform.rotation = Quaternion.Lerp(currentRotation, targetRotation, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpRotationTimeEnum(Transform modelTransform, Quaternion targetRotation, float speed)
    {
        Quaternion currentRotation = modelTransform.rotation;
        float dist = 0;
        while (dist < speed)
        {
            dist += Time.deltaTime;
            modelTransform.rotation = Quaternion.Lerp(currentRotation, targetRotation, dist / speed);
            //valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpLocalRotationEnum(Transform modelTransform, Quaternion targetRotation, float speed)
    {
        Quaternion currentRotation = modelTransform.localRotation;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            modelTransform.localRotation = Quaternion.Lerp(currentRotation, targetRotation, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpLocalRotationEnum(Transform modelTransform, Quaternion targetRotation, float speed, Quaternion startRotation)
    {
        Quaternion currentRotation = startRotation;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            modelTransform.localRotation = Quaternion.Lerp(currentRotation, targetRotation, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpScaleEnum(Transform modelTransform, Vector3 targetScale, float speed)
    {
        Vector3 currentScale = modelTransform.localScale;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            modelTransform.localScale = Vector3.Lerp(currentScale, targetScale, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpScaleEnum(Transform modelTransform, Vector3 targetScale, float speed, Vector3 startScale)
    {
        Vector3 currentScale = startScale;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            modelTransform.localScale = Vector3.Lerp(currentScale, targetScale, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpMaterialColourEnum(Material material, Color targetColour, float speed)
    {
        Color currentColour = material.color;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            material.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpMaterialColourEnum(Material material, Color targetColour, float speed, Color startColour)
    {
        Color currentColour = startColour;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            material.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    public IEnumerator lerpSpriteColourEnum(SpriteRenderer spriteRenderer, Color targetColour, float speed)
    {
        Color currentColour = spriteRenderer.color;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            spriteRenderer.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpSpriteColourEnum(SpriteRenderer spriteRenderer, Color targetColour, float speed, Color startColour)
    {
        Color currentColour = startColour;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            spriteRenderer.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpImageColourEnum(Image image, Color targetColour, float speed)
    {
        Color currentColour = image.color;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            image.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpImageColourEnum(Image image, Color targetColour, float speed, Color startColour)
    {
        Color currentColour = startColour;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            image.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpImageColourTimeEnum(Image image, Color targetColour, float speed)
    {
        Color currentColour = image.color;
        float dist = 0;
        while (dist < speed)
        {
            dist += Time.deltaTime;
            image.color = Color.Lerp(currentColour, targetColour, dist / speed);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpImageColourTimeEnum(Image image, Color targetColour, float speed, Color startColour)
    {
        Color currentColour = startColour;
        float dist = 0;
        while (dist < speed)
        {
            dist += Time.deltaTime;
            image.color = Color.Lerp(currentColour, targetColour, dist / speed);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpTextColourEnum(TextMeshProUGUI text, Color targetColour, float speed)
    {
        Color currentColour = text.color;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            text.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpTextColourEnum(TextMeshProUGUI text, Color targetColour, float speed, Color startColour)
    {
        Color currentColour = startColour;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            text.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    public static IEnumerator lerpTextColourEnum(TextMeshPro text, Color targetColour, float speed)
    {
        Color currentColour = text.color;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            text.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpTextColourEnum(TextMeshPro text, Color targetColour, float speed, Color startColour)
    {
        Color currentColour = startColour;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            text.color = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpCamColourEnum(Camera camera, Color targetColour, float speed)
    {
        Color currentColour = camera.backgroundColor;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            camera.backgroundColor = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpCamColourEnum(Camera camera, Color targetColour, float speed, Color startColour)
    {
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            camera.backgroundColor = Color.Lerp(startColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpCamSizeEnum(Camera camera, float targetSize, float speed)
    {
        float currentSize = camera.orthographicSize;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            camera.orthographicSize = Mathf.Lerp(currentSize, targetSize, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpCamSizeEnum(Camera camera, float targetSize, float speed, float startSize)
    {
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            camera.orthographicSize = Mathf.Lerp(startSize, targetSize, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpCamSizeTimeEnum(Camera camera, float targetSize, float speed)
    {
        float currentSize = camera.orthographicSize;
        float dist = 0;
        while (dist < speed)
        {
            dist += Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(currentSize, targetSize, dist / speed);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpCamSizeTimeEnum(Camera camera, float targetSize, float speed, float startSize)
    {
        float dist = 0;
        while (dist < speed)
        {
            dist += Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(startSize, targetSize, dist / speed);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpCamFovEnum(Camera camera, float targetFov, float speed)
    {
        float currentFov = camera.fieldOfView;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            camera.fieldOfView = Mathf.Lerp(currentFov, targetFov, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpCamFovEnum(Camera camera, float targetFov, float speed, float startFov)
    {
        float currentFov = startFov;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            camera.fieldOfView = Mathf.Lerp(currentFov, targetFov, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpVignetteIntensityEnum(Vignette vignette, float targetIntensity, float speed)
    {
        float currentIntensity = vignette.intensity.value;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            vignette.intensity.value = Mathf.Lerp(currentIntensity, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpVignetteIntensityEnum(Vignette vignette, float targetIntensity, float speed, float startIntensity)
    {
        float currentIntensity = startIntensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            vignette.intensity.value = Mathf.Lerp(currentIntensity, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpLensDistortionEnum(LensDistortion lensDistort, float targetDistort, float speed)
    {
        float currentDistort = lensDistort.intensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            lensDistort.intensity.value = Mathf.Lerp(currentDistort, targetDistort, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpLensDistortionEnum(LensDistortion lensDistort, float targetDistort, float speed, float startDistort)
    {
        float currentDistort = startDistort;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            lensDistort.intensity.value = Mathf.Lerp(currentDistort, targetDistort, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpChromaticAberrationEnum(ChromaticAberration chromaticAberration, float targetIntensity, float speed)
    {
        float currentDistort = chromaticAberration.intensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            chromaticAberration.intensity.value = Mathf.Lerp(currentDistort, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpChromaticAberrationEnum(ChromaticAberration chromaticAberration, float targetIntensity, float speed, float startIntensity)
    {
        float currentDistort = startIntensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            chromaticAberration.intensity.value = Mathf.Lerp(currentDistort, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpDOFdistanceEnum(DepthOfField dof, float targetDistance, float speed)
    {
        float currentDistort = dof.focusDistance;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            dof.focusDistance.value = Mathf.Lerp(currentDistort, targetDistance, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpDOFdistanceEnum(DepthOfField dof, float targetDistance, float speed, float startDistance)
    {
        float currentDistort = startDistance;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            dof.focusDistance.value = Mathf.Lerp(currentDistort, targetDistance, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpBloomIntensityEnum(Bloom bloom, float targetIntensity, float speed)
    {
        float currentDistort = bloom.intensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            bloom.intensity.value = Mathf.Lerp(currentDistort, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpBloomIntensityEnum(Bloom bloom, float targetIntensity, float speed, float startIntensity)
    {
        float currentDistort = startIntensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            bloom.intensity.value = Mathf.Lerp(currentDistort, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpColourFilterEnum(ColorGrading colorGrading, Color targetColour, float speed)
    {
        Color currentColour = colorGrading.colorFilter.value;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.colorFilter.value = (Color.Lerp(currentColour, targetColour, dist));
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpColourFilterEnum(ColorGrading colorGrading, Color targetColour, float speed, Color startColour)
    {
        Color currentColour = startColour;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.colorFilter.value = Color.Lerp(currentColour, targetColour, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpSaturationEnum(ColorGrading colorGrading, float targetSaturation, float speed)
    {
        float currentSaturation = colorGrading.saturation;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.saturation.value = Mathf.Lerp(currentSaturation, targetSaturation, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpSaturationEnum(ColorGrading colorGrading, float targetSaturation, float speed, float startSaturation)
    {
        float currentSaturation = startSaturation;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.saturation.value = Mathf.Lerp(currentSaturation, targetSaturation, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpColourGradingGeneralEnum(ColorGrading colorGrading, float targetTemperature, float targetTint, float targetRedMix, float targetGreenMix, float targetBlueMix, float speed)
    {
        instance.StartCoroutine(instance.lerpTemperature(colorGrading, targetTemperature, speed));
        instance.StartCoroutine(instance.lerpTint(colorGrading, targetTint, speed));
        instance.StartCoroutine(instance.lerpRedMix(colorGrading, targetRedMix, speed));
        instance.StartCoroutine(instance.lerpGreenMix(colorGrading, targetGreenMix, speed));
        instance.StartCoroutine(instance.lerpBlueMix(colorGrading, targetBlueMix, speed));

        yield return GeneralManager.waitForEndOfFrame;
    }

    IEnumerator lerpPostExposure(ColorGrading colorGrading, float targetExposure, float speed)
    {
        float currentExposure = colorGrading.postExposure;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.postExposure.value = Mathf.Lerp(currentExposure, targetExposure, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpPostExposure(ColorGrading colorGrading, float targetExposure, float speed, float startExposure)
    {
        float currentExposure = startExposure;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.postExposure.value = Mathf.Lerp(currentExposure, targetExposure, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpTemperature(ColorGrading colorGrading, float targetTemperature, float speed)
    {
        float currentTemp = colorGrading.temperature;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.temperature.value = Mathf.Lerp(currentTemp, targetTemperature, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpTint(ColorGrading colorGrading, float targetTint, float speed)
    {
        float currentTint = colorGrading.tint;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.tint.value = Mathf.Lerp(currentTint, targetTint, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpRedMix(ColorGrading colorGrading, float targetMix, float speed)
    {
        float currentMix = colorGrading.mixerRedOutRedIn;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.mixerRedOutRedIn.value = Mathf.Lerp(currentMix, targetMix, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpGreenMix(ColorGrading colorGrading, float targetMix, float speed)
    {
        float currentMix = colorGrading.mixerRedOutGreenIn;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.mixerRedOutGreenIn.value = Mathf.Lerp(currentMix, targetMix, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpBlueMix(ColorGrading colorGrading, float targetMix, float speed)
    {
        float currentMix = colorGrading.mixerRedOutBlueIn;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            colorGrading.mixerRedOutBlueIn.value = Mathf.Lerp(currentMix, targetMix, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpLightRange(Light light, float targetRange, float speed)
    {
        float currentRange = light.range;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            light.range = (Mathf.Lerp(currentRange, targetRange, dist));
            yield return GeneralManager.waitForEndOfFrame;
        }
    }
    IEnumerator lerpLightRange(Light light, float targetRange, float speed, float startRange)
    {
        float currentRange = startRange;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            light.range = (Mathf.Lerp(currentRange, targetRange, dist));
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    //IEnumerator lerpLightScattering(AuraLight light, float targetScatter, float speed) // AURA HERE
    //{ // AURA HERE
    //    float currentScattering = light.overridingScattering; // AURA HERE
    //    float dist = 0; // AURA HERE
    //    while (dist < 1) // AURA HERE
    //    { // AURA HERE
    //        dist += Time.deltaTime * speed; // AURA HERE
    //        light.overridingScattering = (Mathf.Lerp(currentScattering, targetScatter, dist)); // AURA HERE
    //        yield return GeneralManager.waitForEndOfFrame; // AURA HERE
    //    } // AURA HERE
    //} // AURA HERE
    //IEnumerator lerpLightScattering(AuraLight light, float targetScatter, float speed, float startScatter) // AURA HERE
    //{ // AURA HERE
    //    float currentScattering = startScatter; // AURA HERE
    //    float dist = 0; // AURA HERE
    //    while (dist < 1) // AURA HERE
    //    { // AURA HERE
    //        dist += Time.deltaTime * speed; // AURA HERE
    //        light.overridingScattering = (Mathf.Lerp(currentScattering, targetScatter, dist)); // AURA HERE
    //        yield return GeneralManager.waitForEndOfFrame; // AURA HERE
    //    } // AURA HERE
    //} // AURA HERE

    IEnumerator lerpLightIntensity(Light light, float targetIntensity, float speed)
    {
        float currentScattering = light.intensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            light.intensity = (Mathf.Lerp(currentScattering, targetIntensity, dist));
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpLightIntensity(Light light, float targetIntensity, float speed, float startIntensity)
    {
        float currentScattering = startIntensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            light.intensity = (Mathf.Lerp(currentScattering, targetIntensity, dist));
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    //IEnumerator lerpLightIntensity(AuraLight light, float targetIntensity, float speed) // AURA HERE
    //{ // AURA HERE
    //    float currentScattering = light.strength; // AURA HERE
    //    float dist = 0; // AURA HERE
    //    while (dist < 1) // AURA HERE
    //    { // AURA HERE
    //        dist += Time.deltaTime * speed; // AURA HERE
    //        light.strength = (Mathf.Lerp(currentScattering, targetIntensity, dist)); // AURA HERE
    //        yield return GeneralManager.waitForEndOfFrame; // AURA HERE
    //    } // AURA HERE
    //} // AURA HERE

    //IEnumerator lerpLightIntensity(AuraLight light, float targetIntensity, float speed, float startIntensity) // AURA HERE
    //{ // AURA HERE
    //    float currentScattering = startIntensity; // AURA HERE
    //    float dist = 0; // AURA HERE
    //    while (dist < 1) // AURA HERE
    //    { // AURA HERE
    //        dist += Time.deltaTime * speed; // AURA HERE
    //        light.strength = (Mathf.Lerp(currentScattering, targetIntensity, dist)); // AURA HERE
    //        yield return GeneralManager.waitForEndOfFrame; // AURA HERE
    //    } // AURA HERE
    //} // AURA HERE

    IEnumerator lerpGetOutCrazinessEnum(GetOut getOut, float targetCraziness, float speed)
    {
        float currentCraziness = getOut.craziness;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            getOut.craziness = Mathf.Lerp(currentCraziness, targetCraziness, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpGetOutCrazinessEnum(GetOut getOut, float targetCraziness, float speed, float startCraziness)
    {
        float currentCraziness = startCraziness;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            getOut.craziness = Mathf.Lerp(currentCraziness, targetCraziness, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpPPweightEnum(PostProcessVolume volume, float targetWeight, float speed)
    {
        float currentWeight = volume.weight;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            volume.weight = Mathf.Lerp(currentWeight, targetWeight, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpPPweightEnum(PostProcessVolume volume, float targetWeight, float speed, float startWeight)
    {
        float currentWeight = startWeight;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            volume.weight = Mathf.Lerp(currentWeight, targetWeight, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpProbeIntensityEnum(ReflectionProbe probe, float targetIntensity, float speed)
    {
        float currentIntensity = probe.intensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            probe.intensity = Mathf.Lerp(currentIntensity, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpProbeIntensityEnum(ReflectionProbe probe, float targetIntensity, float speed, float startIntensity)
    {
        float currentIntensity = startIntensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            probe.intensity = Mathf.Lerp(currentIntensity, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpPlayerSpeedEnum(PlayerController controller, float targetSpeed, float speed)
    {
        float currentSpeed = controller.walkSpeed;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            controller.walkSpeed = Mathf.Lerp(currentSpeed, targetSpeed, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpPlayerSpeedEnum(PlayerController controller, float targetSpeed, float speed, float startSpeed)
    {
        float currentSpeed = startSpeed;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            controller.walkSpeed = Mathf.Lerp(currentSpeed, targetSpeed, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpBlendValueEnum(Material mat, float targetBlend, float speed)
    {
        float currentBlend = mat.GetFloat("_Blend");
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            mat.SetFloat("_Blend", Mathf.Lerp(currentBlend, targetBlend, dist));
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator lerpBlendValueEnum(Material mat, float targetBlend, float speed, float startBlend)
    {
        float currentBlend = startBlend;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            mat.SetFloat("_Blend", Mathf.Lerp(currentBlend, targetBlend, dist));
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator LerpRainIntensityEnum(RainScript script, float targetIntensity, float speed)
    {
        float currentIntensity = script.RainIntensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            script.RainIntensity = Mathf.Lerp(currentIntensity, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    IEnumerator LerpRainIntensityEnum(RainScript script, float targetIntensity, float speed, float startIntensity)
    {
        float currentIntensity = startIntensity;
        float dist = 0;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            script.RainIntensity = Mathf.Lerp(currentIntensity, targetIntensity, dist);
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    #endregion
}