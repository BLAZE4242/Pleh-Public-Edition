using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class quizCheckForRise : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip mainSong;
    [SerializeField] float flySpeed = 1f;
    PlayerController controller;
    [SerializeField] Image whiteScreen;
    bool canFly = false;
    AsyncOperation sceneOp;
    bool hasStarted;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    source.enabled = true;
        //    GetComponent<quizManager>().currentQuizProjectState = quizManager.quizProjectStates.checkForRise;
        //    source.Stop();
        //}
    }

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
    }

    void StateStart(quizManager _manager)
    {
        StartCoroutine(Rise());
    }

    private IEnumerator Rise()
    {
        sceneOp = SceneManager.LoadSceneAsync("Quiz End");
        sceneOp.allowSceneActivation = false;
        Debug.Log("waiting...");
        yield return new WaitUntil(() => !source.isPlaying);

        Debug.Log("Flying!");

        // rise my child
        canFly = true;
        source.clip = mainSong;
        source.Play();
        controller.lockMovement = true;
        controller.checkForMovement = false;
        controller.controller.enabled = false;

        yield return GeneralManager.waitForSeconds(10);
        LerpSolution.lerpImageColour(whiteScreen, Color.white, 0.2f, Color.clear);
        yield return GeneralManager.waitForSeconds(8);

        DontDestroyOnLoad(source);
        sceneOp.allowSceneActivation = true;
    }

    void StateUpdate(quizManager _manager)
    {
        if(canFly) controller.transform.position += new Vector3(0, 1, 0) * flySpeed * Time.deltaTime;
    }

    public void CheckStateStarted(quizManager _manager)
    {
        if (!hasStarted)
        {
            StateStart(_manager);
            StateUpdate(_manager);
            hasStarted = true;
        }
        else
        {
            StateUpdate(_manager);
        }
    }
}
