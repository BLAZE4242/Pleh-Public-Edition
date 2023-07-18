using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LBW_manager : MonoBehaviour
{
    public enum LBWstates { intro, puzzle}

    [Header("States")]
    public LBWstates currentState;
    [SerializeField] LBW_intro introState;
    [SerializeField] LBW_puzzle puzzleState;

    [Header("Fall")]
    [SerializeField] TextMeshPro devText;
    [SerializeField] AudioClip click;

    [Header("Other Variables")]
    [SerializeField] Image blackImage;
    [SerializeField] Transform spawnPos;
    PlayerController controller;

    public AsyncOperation sceneOp;

    private void Start()
    {
        FindObjectOfType<DRP>().RP("What have I done?");
        GeneralManager.SetSceneSave();

        sceneOp = SceneManager.LoadSceneAsync("hub_main", LoadSceneMode.Additive);
        sceneOp.allowSceneActivation = false;
        runCurrentState();
        controller = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        runCurrentState();
        checkForDeath();
    }

    void runCurrentState()
    {
        switch (currentState)
        {
            case LBWstates.intro:
                introState.runState();
                break;
            case LBWstates.puzzle:
                puzzleState.runState();
                break;
        }
    }

    bool isDying = false;
    void checkForDeath()
    {
        if (controller.transform.position.y < -6f && !isDying)
        {
            puzzleState.isWalking = false;
            isDying = true;
            StartCoroutine(resetScene());
        }
    }

    string[] script = { "Keep your focus on the door.", "Hear out for my cue.", "Don't look away from the door.", "You need to listen.", "You'll make it.", "Keep on focusing." };
    int scriptI = 0;
    IEnumerator resetScene()
    {
        foreach (CollisionManager col in FindObjectsOfType<CollisionManager>())
        {
            col.ResetIfTriggered();
        }
        LerpSolution.lerpImageColour(blackImage, Color.black, 2f);
        yield return GeneralManager.waitForSeconds(2f);

        devText.text = "";
        LerpSolution.lerpImageColour(blackImage, Color.clear, 2f);
        controller.TeleportPlayer(spawnPos.position);

        isDying = false;
        puzzleState.floor.SetActive(true);
        puzzleState.hasHeardAudio = false;

        yield return GeneralManager.waitForSeconds(1);

        devText.text = script[scriptI];
        scriptI++;

        if (scriptI == script.Length) scriptI = 0;

        GetComponent<AudioSource>().PlayOneShot(click);
    }
}
