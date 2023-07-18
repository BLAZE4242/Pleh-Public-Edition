using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messagesJumpController : MonoBehaviour
{
    playerControllerMessages _controller;

    void Start()
    {
        _controller = FindObjectOfType<playerControllerMessages>();
    }

    void OnMouseDown()
    {
        if(_controller._groundCheck.isGrounded && _controller.canJump) _controller.jump();
    }
}
