using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hub_manager : MonoBehaviour
{
    public enum HubStates { intro, afterPuzzle, blink, undone, kys, credits, glitchCredits }
    public HubStates currentState;
    [SerializeField] pause Pause;
    [Header("State References")]
    [SerializeField] hub_introManager introState;
    [SerializeField] hub_afterPuzzleManager afterPuzzleState;
    [SerializeField] hub_blinkManager blinkState;
    [SerializeField] hub_undoneManager undoneState;
    [SerializeField] hub_endManager endState;
    [SerializeField] hub_creditsManager creditsState;
    [SerializeField] hub_glitchCredits glitchCreditsState;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("glotchiness") == 4)
        {
            currentState = HubStates.afterPuzzle;
            if (FindObjectsOfType<pause>().Length > 1) Destroy(Pause.gameObject);
        }
        else if(PlayerPrefs.GetInt("glotchiness") == 5)
        {
            currentState = HubStates.blink;
        }
        else if(PlayerPrefs.GetInt("glotchiness") == 6)
        {
            currentState = HubStates.undone;
        }
        else if(PlayerPrefs.GetInt("glotchiness") == 7)
        {
            currentState = HubStates.kys;
        }
        else if(PlayerPrefs.GetInt("glotchiness") == 8)
        {
            currentState = HubStates.credits;
        }
        if (PlayerPrefs.GetString("Backup") == "yep")
        {
            currentState = HubStates.glitchCredits;
        }

        switch (currentState)
        {
            case HubStates.intro:
                introState.gameObject.SetActive(true);
                break;
            case HubStates.afterPuzzle:
                afterPuzzleState.gameObject.SetActive(true);
                break;
            case HubStates.blink:
                blinkState.gameObject.SetActive(true);
                break;
            case HubStates.undone:
                undoneState.gameObject.SetActive(true);
                break;
            case HubStates.kys:
                endState.gameObject.SetActive(true);
                break;
            case HubStates.credits:
                creditsState.gameObject.SetActive(true);
                break;
            case HubStates.glitchCredits:
                creditsState.gameObject.SetActive(true);
                glitchCreditsState.gameObject.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        GeneralManager.SetSceneSave();
    }
}
