using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RedTextLight : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] UnityEvent onTouch;
    [SerializeField] float touchCooldown = 1f;

    Transform playerTrans;
    bool isOnCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = FindObjectOfType<PlayerController>().transform;
        StartCoroutine(spawnText());
    }

    IEnumerator spawnText()
    {
        while(true)
        {
            for(int i = 0; i < 4; i++)
            {
                GameObject obj = Instantiate(prefab, RandPosition(), Quaternion.identity, transform);
                assignScale(obj, Vector3.one * 0.3f);
                RedTextIndividual indiv = obj.GetComponent<RedTextIndividual>();
                indiv.AssignMeanWord();
                indiv.KillSelf(5f);
                LerpSolution.lerpPosition(obj.transform, transform.position + Random.insideUnitSphere, 1f);
            }
            yield return GeneralManager.waitForSeconds(0.3f);
        }
    }

    void assignScale(GameObject obj, Vector3 scale)
    {
        obj.transform.SetParent(null);
        obj.transform.localScale = scale;
        obj.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(playerTrans.position, transform.position) < 2f && !isOnCooldown)
        { 
            StartCoroutine(lightTouched());
        }
    }

    IEnumerator lightTouched()
    {
        isOnCooldown = true;
        onTouch.Invoke();
        yield return GeneralManager.waitForSeconds(touchCooldown);
        isOnCooldown = false;
    }

    public Vector3 RandPosition()
    {
        return transform.position + Random.insideUnitSphere + new Vector3(Random.Range(-60, 60), Random.Range(-60, 60), Random.Range(-60, 60));
    }
}
