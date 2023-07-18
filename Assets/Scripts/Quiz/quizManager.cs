using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quizManager : MonoBehaviour
{
    [SerializeField] GameObject walls;
    [SerializeField] quizWalkingToDoor _walkingToDoor;
    [SerializeField] quizQuizing _quizQuizing;
    [SerializeField] quizEnding _quizEnding;
    [SerializeField] quizCheckForRise _quizCheckForRise;
    [SerializeField] AudioSource _music, _sfx;
    PlayerController _playerController;
    public enum quizProjectStates {walkingToDoor, quizing, doorEnding, normalEnding, checkForRise};
    public quizProjectStates currentQuizProjectState;

    private void Start()
    {
        FindObjectOfType<DRP>().RP("Can you hear me?");
        GeneralManager.SetSceneSave();
        Gas.EarnAchievement("ACH_STORY_1");
    }

    void Update()
    {
        switch(currentQuizProjectState)
        {
            case quizProjectStates.walkingToDoor:
                _walkingToDoor.CheckStateStarted(this);
                break;
            case quizProjectStates.quizing:
                _quizQuizing.CheckStateStarted(this);
                break;
            case quizProjectStates.doorEnding:
                _quizEnding.CheckStateStarted(this, true);
                break;
            case quizProjectStates.normalEnding:
                _quizEnding.CheckStateStarted(this, false);
                break;
            case quizProjectStates.checkForRise:
                _quizCheckForRise.CheckStateStarted(this);
                break;
        }
    }
}
