using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTlights : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(LerpRotation());
        StartCoroutine(LerpPosition());
    }

    IEnumerator LerpRotation()
    {
        while(true)
        {
            Quaternion targetRot = randomRotBelow180();
            Quaternion currentRotation = transform.rotation;
            float dist = 0;
            while (dist < 1)
            {
                dist += Time.deltaTime * 0.1f;
                transform.rotation = Quaternion.Lerp(currentRotation, targetRot, dist);
                yield return new WaitForEndOfFrame();
            }

            yield return GeneralManager.waitForSeconds(Random.Range(0, 0.6f));
        }
    }
    
    IEnumerator LerpPosition()
    {
        while(true)
        {
            Vector3 targetPos = randPos(transform.position.y);
            Vector3 currentPos = transform.position;
            float dist = 0;
            while (dist < 1)
            {
                dist += Time.deltaTime * 0.1f;
                transform.position = Vector3.Lerp(currentPos, targetPos, dist);
                yield return new WaitForEndOfFrame();
            }

            yield return GeneralManager.waitForSeconds(Random.Range(0, 0.6f));
        }
    }

    Quaternion randomRotBelow180()
    {
        return Quaternion.Euler(Random.Range(50, 130), Random.Range(0, 360), 0);
    }

    Vector3 randPos(float yPos)
    {
        return new Vector3(Random.Range(-50, 40), yPos, Random.Range(12, 126));
    }
}
