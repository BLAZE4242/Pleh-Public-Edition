using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlehGlitchedLevel : MonoBehaviour
{
    [SerializeField] string LevelName;
    public float delayTime = 0f;
    public float spawnWaitTime = 0.2f;
    public PlehGlitchedLevel(TextMeshPro[] _texts, GameObject[] _platformers)
    {
        texts = _texts;
        platforms = _platformers;
    }

    public TextMeshPro[] texts;
    public GameObject[] platforms;
}
