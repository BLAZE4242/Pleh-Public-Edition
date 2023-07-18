using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tapToPlay : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] Transform posOne, posTwo;
    void Start()
    {
        StartCoroutine(animate());
    }

    IEnumerator animate()
    {
        while(true)
        {
            transform.position = posOne.position;
            transform.localRotation = posOne.rotation;
            yield return GeneralManager.waitForSeconds(time);
            transform.position = posTwo.position;
            transform.localRotation = posTwo.rotation;
            yield return GeneralManager.waitForSeconds(time);
        }
    }
        
}
