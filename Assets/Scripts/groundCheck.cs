using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    [HideInInspector] public bool isGrounded;
    playerMove _playerMove;
    int objects;

    private void Start()
    {
        _playerMove = GetComponentInParent<playerMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Cube"))
        {
            objects++;
            update();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Cube"))
        {
            objects--;
            update();
        }
    }

    void update()
    {
        if(objects > 0)
        {
            if(_playerMove != null)
                _playerMove.isGrounded = true;
            isGrounded = true;
        }
        else
        {
            if(_playerMove != null)
                _playerMove.isGrounded = false;
            isGrounded = false;
        }
    }

    public void Respawn()
    {
        objects = 0;
    }
}
