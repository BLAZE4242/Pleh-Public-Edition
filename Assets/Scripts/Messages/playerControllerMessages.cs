using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerMessages : MonoBehaviour
{
    [Header("Camera Following")]
    [SerializeField] Camera cam;
    [SerializeField] Vector2 offset;
    [HideInInspector] public bool canCameraFollow = true;
    
    [Header("Player Controller")]
    [HideInInspector] public bool canJump, hasJumped;
    public float playerSpeed;
    [SerializeField] float jumpHeight;
    Rigidbody2D rb;
    [HideInInspector] public groundCheck _groundCheck;
    gameMessagesState _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _groundCheck = FindObjectOfType<groundCheck>();
        _gameManager = FindObjectOfType<gameMessagesState>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMove();
        if(canJump) checkForJump();
        checkForDeath();
    }

    private void checkForDeath()
    {
        if (transform.position.y <= 14)
        {
            _gameManager.die(false);
        }
    }

    private void checkForJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _groundCheck.isGrounded)
        {
            jump();
        }
    }

    public void jump()
    {
        rb.velocity = new Vector3(0, jumpHeight, 0);
        hasJumped = true;
    }

    void LateUpdate()
    {
        if(canCameraFollow) updateCamPos();
    }

    private void playerMove()
    {
        transform.position += new Vector3(playerSpeed, 0, 0) * Time.deltaTime;
    }

    private void updateCamPos()
    {
        cam.transform.position = new Vector3(transform.position.x + offset.x, offset.y, -10);
    }
}
