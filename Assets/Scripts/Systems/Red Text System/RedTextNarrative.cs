using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedTextNarrative : MonoBehaviour
{
    [SerializeField] TextMeshPro[] redTexts;
    glitchText _gText;

    private void Awake()
    {
        _gText = GetComponent<glitchText>();

        foreach (TextMeshPro text in redTexts)
        {
            text.gameObject.SetActive(false);
        }
    }

    public void ShowText(GameObject Go)
    {
        Go.SetActive(true);

        TextMeshPro text = Go.GetComponent<TextMeshPro>();
        StartCoroutine(_gText.createGlitchedText(text.text, text));
    }

    public void PullText(GameObject Go)
    {
        StartCoroutine(PullTextEnum(Go));
    }

    IEnumerator PullTextEnum(GameObject Go)
    {
        yield return GeneralManager.waitForSeconds(0.8f);
        LerpSolution.lerpPosition(Go.transform, FindObjectOfType<RedTextCreature>().transform.position, 3f);
        LerpSolution.lerpRotation(Go.transform, RandRot(), 2f);
    }

    Quaternion RandRot()
    {
        return Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
    }
}
