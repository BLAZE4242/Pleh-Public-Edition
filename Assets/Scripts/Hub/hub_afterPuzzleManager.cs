using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class hub_afterPuzzleManager : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] GameObject blackRoom;
    [SerializeField] VoiceLine line;
    [SerializeField] PostProcessVolume _volume;
    [SerializeField] PostProcessProfile _profile;
    AsyncOperation sceneOp;

    void Awake()
    {
        if(FindObjectsOfType<PlayerController>().Length > 1)Destroy(controller.gameObject);
        Destroy(blackRoom);
        FindObjectOfType<DRP>().RP("What have I done?");
    }

    bool isPlaying = false;
    private void Update()
    {
        if (SceneManager.sceneCount == 1 && !isPlaying)
        {
            isPlaying = true;
            StartCoroutine(PlayLine());
        }
    }

    public IEnumerator PlayLine()
    {
        yield return GeneralManager.waitForSeconds(2);

        sceneOp = SceneManager.LoadSceneAsync("Amelia_Restaurant");
        sceneOp.allowSceneActivation = false;
        GetComponent<VoiceLineManager>().PlayLine(line);
    }

    public void Glitch()
    {
        _volume.profile = _profile;
        CameraShake.Shake(10, 0.2f);
    }

    public void LoadLevel()
    {
        sceneOp.allowSceneActivation = true;
    }
}
