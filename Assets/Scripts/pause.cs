using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class pause : MonoBehaviour
{
    [SerializeField] GameObject pausePrefab;
    public static bool canPause = true;
    public enum pausedState { playing, paused }
    public static pausedState currentPauseState;

    [SerializeField] KeyCode pauseKey = KeyCode.Escape;
    PostProcessVolume _volume;
    //DepthOfField _dof;

    bool dofDefaultOverrideState;
    float dofDefaultFocalLength;


    void OnLevelWasLoaded()
    {
        defineVariables();
    }

    bool wasCursorLocked, wasCursorVisible;


    void Start()
    {
        defineVariables();
    }

    void defineVariables()
    {
        wasCursorLocked = Cursor.lockState == CursorLockMode.Locked;
        wasCursorVisible = Cursor.visible;
        _volume = FindObjectOfType<PostProcessVolume>();
        //if (!_volume.profile.TryGetSettings(out _dof))
        //{
        //    _volume.profile.AddSettings<DepthOfField>();
        //    _volume.profile.TryGetSettings(out _dof);
        //}

        //if (!_dof.focalLength.overrideState)
        //{
        //    _dof.focalLength.overrideState = true;
        //    _dof.focalLength.value = 1;
        //}
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey) && canPause)
        {
            switch (currentPauseState)
            {
                case pausedState.playing:
                    Debug.Log("Should pause");
                    PauseGame();
                    break;
                case pausedState.paused:
                    Debug.Log("Should unpause");
                    UnPauseGame();
                    break;
            }
        }

        

    }

    GameObject currentPause;
    void PauseGame()
    {
        wasCursorLocked = Cursor.lockState == CursorLockMode.Locked;
        wasCursorVisible = Cursor.visible;
        currentPauseState = pausedState.paused;
        //SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
        currentPause = Instantiate(pausePrefab);

        Time.timeScale = 0;
        AudioListener.pause = true;

        foreach (VideoPlayer player in FindObjectsOfType<VideoPlayer>())
        {
            player.Pause();
        }

        //dofDefaultOverrideState = _dof.focalLength.overrideState;
        //dofDefaultFocalLength = _dof.focalLength.value;

        //_dof.focalLength.overrideState = true;
        //_dof.focalLength.value = 200;
    }

    public void UnPauseGame()
    {
        Cursor.lockState = wasCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = wasCursorVisible;

        currentPauseState = pausedState.playing;
        //SceneManager.UnloadSceneAsync("Pause");
        Destroy(currentPause);
        Time.timeScale = 1;
        AudioListener.pause = false;

        foreach (VideoPlayer player in FindObjectsOfType<VideoPlayer>())
        {
            player.Play();
        }

        //_dof.focalLength.overrideState = dofDefaultOverrideState;
        //_dof.focalLength.value = dofDefaultFocalLength;
    }
}
