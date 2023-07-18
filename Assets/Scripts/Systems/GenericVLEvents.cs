using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericVLEvents : MonoBehaviour
{
    AsyncOperation sceneOp;

    #region SceneLoading
    public void LoadSceneStringAsync(string sceneIndex)
    {
        sceneOp = SceneManager.LoadSceneAsync(sceneIndex);
        sceneOp.allowSceneActivation = false;
    }

    public void LoadSceneStringAsyncImmediate(string sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void LoadSceneString(string sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }



    public void LoadSceneIntAsync(int sceneIndex)
    {
        sceneOp = SceneManager.LoadSceneAsync(sceneIndex);
        sceneOp.allowSceneActivation = false;
    }

    public void LoadSceneIntAsyncImmediate(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void LoadSceneInt(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ActivateAsyncSceneLoad()
    {
        sceneOp.allowSceneActivation = true;
    }
    #endregion
}
