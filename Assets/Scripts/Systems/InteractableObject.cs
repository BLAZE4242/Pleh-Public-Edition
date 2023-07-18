using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public UnityEvent onInteract;
    public float range = 2.5f;
    public MeshCollider colliderToScan;
    [SerializeField] BoxCollider OtherColliderToScan;

    Transform cam;

    void Start()
    {
        asssignCam();
    }

    void asssignCam()
    {
        cam = FindObjectOfType<PlayerController>().playerCamera;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E))
        {
            onRaycast();
        }
    }

    void onRaycast()
    {
        if(cam == null) asssignCam();
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, range) && (hit.collider == colliderToScan || hit.collider == OtherColliderToScan))
        {
            Debug.Log("We hit a lever!");
            onInteract.Invoke();
        }
    }
}
