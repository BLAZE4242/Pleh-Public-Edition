using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionLookAway : MonoBehaviour
{
    [SerializeField] lookAwayJumpScare _script;
    void OnTriggerEnter(Collider col)
    {
        _script.doorStep++;
        Destroy(gameObject);
    }
}
