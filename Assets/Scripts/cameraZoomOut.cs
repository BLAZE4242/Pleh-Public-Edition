using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]

public class cameraZoomOut : MonoBehaviour
{
    public float zoomOutSpeed;
    [SerializeField] public Camera cam;
    [SerializeField] float colorChangeSpeed;

    public void zoomIn(float endZoom, float zoomOutSpeedLocal)
    {
        Debug.Log("Zooming in...");
        LerpSolution.lerpCamColour(cam, cam.backgroundColor, colorChangeSpeed, Color.black);
        LerpSolution.lerpCamSize(cam, endZoom, zoomOutSpeedLocal, 2000);
    }

    IEnumerator colorChange()
    {
        Color targetColor = cam.backgroundColor;
        float changePercentage = 0f;
        while(changePercentage < 1f)
        {
            changePercentage += Time.deltaTime * colorChangeSpeed;
            cam.backgroundColor = Color.Lerp(Color.black, targetColor, changePercentage);
            yield return new WaitForEndOfFrame();
        }
    }
}
