using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class conveyerBeltHandle : MonoBehaviour
{
    public enum beltControlStates { notControlling, couldEnter, controlling}
    public beltControlStates currentBeltControlState;

    [SerializeField] TextMeshProUGUI tutText, tutTextLeft;
    [SerializeField] KeyCode action;

    bool isControlling;
    [SerializeField] conveyerBeltController ConveyerBelt;
    playerMove _playerMove;

    private void Start()
    {
        _playerMove = FindObjectOfType<playerMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutText.text = $"Press {action} to control the belt";
            currentBeltControlState = beltControlStates.couldEnter;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutText.text = "";
            currentBeltControlState = beltControlStates.notControlling;
        }
    }

    private void Update()
    {
        switch (currentBeltControlState)
        {
            case beltControlStates.couldEnter:
                if (Input.GetKeyDown(action))
                {
                    currentBeltControlState = beltControlStates.controlling;
                }
                break;
            case beltControlStates.controlling:
                tutText.text = $"Press {action} to exit the belt";
                tutTextLeft.text = "Press Space to spawn a cube";
                ConveyerBelt.isInControl = true;
                _playerMove.canControl = false;
                _playerMove.PlutoAnimator.SetFloat("Speed", 0);
                isControlling = true;
                break;
        }

        if (isControlling && Input.GetKeyDown(action))
        {
            tutText.text = $"Press {action} to enter the belt";
            ConveyerBelt.isInControl = false;
            _playerMove.canControl = true;
            currentBeltControlState = beltControlStates.couldEnter;
            isControlling = false;
        }
        else if(currentBeltControlState != beltControlStates.controlling)
        {
            tutTextLeft.text = "";
        }
    }

}
