using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messagesStateManager : MonoBehaviour
{
    public messagesState currentState;
    public messagesState menuState;
    infoFromScene _info;
    AudioSource source;

    void Start()
    {
        _info = FindObjectOfType<infoFromScene>();
        source = GetComponent<AudioSource>();
        if(_info.messageFromScene == "dead" || _info.messageFromScene == "message dead")
            currentState = menuState;
        else
            source.Play();
    }

    void Update()
    {
        runStateMachine();
    }

    private void runStateMachine()
    {
        messagesState nextState = currentState?.runCurrentState();

        if(nextState != null)
        {
            switchToNextState(nextState);
        }
    }

    private void switchToNextState(messagesState nextState)
    {
        currentState = nextState;
    }
}
