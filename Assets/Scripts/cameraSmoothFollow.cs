using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSmoothFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 i = Vector3.zero;
    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref i, 0.125f);
    }
}
