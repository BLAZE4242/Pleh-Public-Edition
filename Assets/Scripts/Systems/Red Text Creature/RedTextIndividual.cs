using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedTextIndividual : MonoBehaviour
{
    [SerializeField] float rotOffset = 180f;
    string[] meanWordsWaaaaa = { "Depression", "Motivation", "Disappointment", "Suicide", "Afterlife", "Dullness", "Desperation", "Agony", "Anxiety", "Misery" };
    Transform playerTrans;
    [HideInInspector] public RedTextCreature host;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = FindObjectOfType<PlayerController>().transform;
        if (FindObjectOfType<RedTextLight>() != null)
        {
            StartCoroutine(checkForDestroy(FindObjectOfType<RedTextLight>().transform));
        }
    }

    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(playerTrans.position) * Quaternion.Euler(0, -90, 0);
        transform.LookAt(playerTrans);
        transform.rotation *= Quaternion.Euler(0, rotOffset, 0);
    }

    IEnumerator checkForDestroy(Transform light)
    {
        while(true)
        {
            if(Vector3.Distance(transform.position, light.position) <= 3)
            {
                LerpSolution.lerpScale(transform, Vector3.zero, 4);
                StopAllCoroutines();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void AssignMeanWord()
    {
        GetComponent<TextMeshPro>().text = meanWordsWaaaaa[Random.Range(0, meanWordsWaaaaa.Length-1)];
    }

    public void KillSelf(float timeUntil)
    {
        Destroy(gameObject, timeUntil);
    }

    public IEnumerator beatText()
    {
        LerpSolution.lerpPosition(transform, transform.position + new Vector3(3f, 0, 0), 4); // Lerp scale as well
        yield return GeneralManager.waitForSeconds(0.3f);
        LerpSolution.lerpPosition(transform, transform.position - new Vector3(3f, 0, 0), 4);
    }

    public IEnumerator fallText()
    {
        TextMeshPro text = GetComponent<TextMeshPro>();
        host.redTexts.Remove(text);
        gameObject.AddComponent<Rigidbody>();
        LerpSolution.lerpTextColour(text, Color.clear, 0.3f);
        while(text.color != Color.clear)
        {
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
