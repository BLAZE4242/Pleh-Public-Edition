using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldGlitch : MonoBehaviour
{
    [SerializeField] Material glitchMat;
    Material defaultMat;
    MeshRenderer _renderer;
    [Header("Random Variables")]
    public Vector2 waitTime;
    public Vector2 glitchTime;
    [SerializeField] Vector2 itterationTime;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        defaultMat = _renderer.material;
        StartCoroutine(glitchStarfield());
    }

    IEnumerator glitchStarfield()
    {
        while (true)
        {
            yield return GeneralManager.waitForSeconds(Random.Range(waitTime.x, waitTime.y));

            _renderer.material = glitchMat;

            yield return GeneralManager.waitForSeconds(Random.Range(glitchTime.x, glitchTime.y));

            _renderer.material = defaultMat;
        }
    }
}
