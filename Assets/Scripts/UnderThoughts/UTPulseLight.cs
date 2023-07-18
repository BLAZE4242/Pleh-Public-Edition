using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTPulseLight : MonoBehaviour
{
    [SerializeField] float Speed = 5f;
    [SerializeField] AudioClip woosh;
    [HideInInspector] public bool canShakeCamera;
    public Vector3 dir;
    UTfall stateManager;

    void Start()
    {
        stateManager = FindObjectOfType<UTfall>();
        StartCoroutine(CheckForCameraShake());
    }

    void Update()
    {
        if(dir == Vector3.zero) transform.position += Vector3.forward * Time.deltaTime * Speed;
        else transform.position += dir * Time.deltaTime * Speed;

        if(stateManager.pulseCanDestroy)
        {
            float DistanceFromWall = Mathf.Abs(transform.position.z - stateManager.pushObject.transform.position.z);
            if (DistanceFromWall < 4)
            {
                CameraShake.Shake(0.3f * 3, 0.4f * 3);
                CameraShake.Shake(0.5f * 3, 0.8f * 3);
                CameraShake.Shake(0.2f * 3, 0.4f * 3);
                FindObjectOfType<UTfall>().liftStuff();
                FindObjectOfType<AudioSource>().PlayOneShot(stateManager.stairBreak, 2);
            }
        }
    }

    IEnumerator CheckForCameraShake()
    {
        bool CanCheck = true;
        while(CanCheck)
        {
            //give player camera shake depending on how close they are to it.

            float DistanceFromPlayer = Mathf.Abs(transform.position.z - FindObjectOfType<PlayerController>().transform.position.z);
            if (DistanceFromPlayer < 4 && canShakeCamera)
            {
                GetComponent<AudioSource>().PlayOneShot(woosh, 0.5f);
                CameraShake.Shake(0.3f, 0.4f, false);
                CameraShake.Shake(0.5f, 0.8f, false);
                CameraShake.Shake(0.2f, 0.4f, false);
                CanCheck = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
