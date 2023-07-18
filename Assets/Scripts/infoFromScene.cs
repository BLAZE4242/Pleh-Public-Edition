using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class infoFromScene : MonoBehaviour
{
    public string messageFromScene;
    [HideInInspector] public Message messageArgument;
    private void Awake()
    {
        infoFromScene[] objs = FindObjectsOfType<infoFromScene>();
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void OnLevelWasLoaded()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0)
        {
            Destroy(gameObject);
        }
    }
}
