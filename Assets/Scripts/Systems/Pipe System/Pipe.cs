using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Pipe : MonoBehaviour
{
    public PipesCreator.PipeDirection CurrentDirection;
    

    void Awake()
    {
        if(SceneManager.GetActiveScene().name != "Level 1_G") DontDestroyOnLoad(gameObject);
    }

    void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            Destroy(gameObject);
        }
    }

    public Transform TargetNextPipe()
    {
        foreach(Transform trans in GetComponentsInChildren<Transform>())
        {
            if(trans.name.ToLower().Contains("target")) return trans;
        }

        return null;
    }

    public Transform PullOrbit()
    {
        foreach(Transform trans in GetComponentInChildren<Transform>())
        {
            if(trans.name.ToLower().Contains("orbit"))
            {
                return trans;
            }
        }

        return null;
    }

    public Pipe[] PipeChildren()
    {
        if(GetComponent<PipesCreator>() != null)
        {
            return GetComponentsInChildren<Pipe>();
        }
        else
        {
            Debug.LogWarning("Could not return PipeChildren because target pipe is not master pipe.");
            return null;
        }
    }
}
