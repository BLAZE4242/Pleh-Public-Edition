using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isTouchingCabinet : MonoBehaviour
{
    public bool isPlayerTouching;
    
    void OnTriggerEnter(Collider col)
    {
        isPlayerTouching = true;
    }

    void OnTriggerExit(Collider col)
    {
        isPlayerTouching = false;
    }
}
