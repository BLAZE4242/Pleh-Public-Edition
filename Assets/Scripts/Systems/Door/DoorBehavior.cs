using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{

    void Update()
    {

    }

    public void OpenDoor(float openSpeed)
    {
        StartCoroutine(ToggleDoorEnum(openSpeed, true));
    }

    public void CloseDoor(float closeSpeed)
    {
        StartCoroutine(ToggleDoorEnum(closeSpeed, false));
    }

    float tempSpeed = 0f;
    IEnumerator ToggleDoorEnum(float openSpeed, bool isOpening = true)
    {
        Debug.Log("Opening!");
        float t = 0f;
        Quaternion targetQuat;
        if (isOpening)
        {
            targetQuat = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(0, 134.082f, 0));
        }
        else
        {
            targetQuat = Quaternion.Euler(transform.localRotation.eulerAngles - new Vector3(0, 134.082f, 0));

        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Look both ways")
        {
            StartCoroutine(ChangeActiveScene());
        }

        // lerp the temp speed up to open speed in a seperate coroutine

        Coroutine lerpSpeedIEnum = StartCoroutine(LerpSpeed(openSpeed));
        while (t < 1)
        {
            t += Time.deltaTime * tempSpeed;
            // transform.rotation = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), t);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetQuat, t);
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(lerpSpeedIEnum);
    }

    IEnumerator ChangeActiveScene()
    {
        yield return GeneralManager.waitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName("hub_main"));
    }

    IEnumerator LerpSpeed(float openSpeed)
    {
        float t = 0;
        while (tempSpeed < openSpeed)
        {
            tempSpeed = Mathf.Lerp(tempSpeed, openSpeed, t);
            t += Time.deltaTime * 0.001f;
            yield return new WaitForEndOfFrame();
        }
    }
}
