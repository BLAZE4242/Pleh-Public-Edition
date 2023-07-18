using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool isDisabled = false;

    public Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    public bool invertMouse;
    public float walkSpeed = 6.0f;
    private float speedMultiplier;
    public float gravity = -13.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    public bool lockCursor = true, showCursor = false;
    public bool lockMovement;
    public bool checkForMovement = true;
    public bool lockGravity;
    public bool lockY;
    public bool lockLook;
    [SerializeField] bool useDeltaTime = true;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    [HideInInspector] public CharacterController controller = null;

    [HideInInspector] public Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    bool noclip = false;

    bool invertX, invertY;



    private void Awake()
    {
        PlayerController[] playerController = FindObjectsOfType<PlayerController>();
        if (playerController.Length > 1)
        {
            Destroy(gameObject);
        }
        previousY = transform.position.y;
        controller = GetComponent<CharacterController>();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "ekse")
        {
            speedMultiplier = 1;
            return;
        }
        speedMultiplier = PlayerPrefs.GetFloat("speed", 1);
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        updateCursorState(lockCursor, showCursor);
        
    }

    public void updateCursorState(bool shouldLockCursor, bool shouldShowCursor)
    {
        if (shouldLockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if(shouldLockCursor)
        {
            Cursor.visible = false;
        }
    }

    float previousY = 0;
    Vector3 localCampos;
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse3))
        //{
        //    noclip = !noclip;

        //    if (noclip)
        //    {
        //        localCampos = playerCamera.localPosition;

        //        controller.enabled = false;
        //        lockMovement = true;
        //        lockLook = true;
        //        playerCamera.gameObject.AddComponent<Aura2API.AuraFreeCamera>().freeLookEnabled = true;
        //    }
        //    else
        //    {
        //        Destroy(GetComponentInChildren<Aura2API.AuraFreeCamera>());

        //        playerCamera.localPosition = localCampos;
        //        controller.enabled = true;
        //        lockMovement = false;
        //        lockLook = false;
        //    }
        //}

        if (!isDisabled)
        {
            if(!lockLook && pause.currentPauseState == pause.pausedState.playing)
                UpdateMouseLook();
            if(checkForMovement)
                UpdateMovement();
            // Usually we lock movement here but we need gravity to apply so we don't do that and instead call it in UpdateMovement()
        }

        //if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L) && Application.isEditor)
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        //}

        if (lockY)
        {
            transform.position = new Vector3(transform.position.x, previousY, transform.position.z);
        }
        else
        {
            previousY = transform.position.y;
        }

        // CheckForSceneChange();

        if(!lockMovement) CheckForBackwardsAchievement();
    }

    float backwardsInterval = 10;

    void CheckForBackwardsAchievement()
    {
        if (backwardsInterval <= 0)
        {
            Gas.EarnAchievement("ACH_BEHIND");
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            backwardsInterval -= Time.deltaTime;
        }
        else
        {
            backwardsInterval = 10;
        }
    }

    void UpdateMouseLook()
    {
        invertX = PlayerPrefs.GetString("pmurgs").Contains("X");
        invertY = PlayerPrefs.GetString("pmurgs").Contains("Y");

        Vector2 targetMouseDelta = new Vector2();
        /*if(!invertMouse)*/ targetMouseDelta = new Vector2(invertX ? -Input.GetAxis("Mouse X") : Input.GetAxis("Mouse X"), invertY ? -Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y"));
        //if(invertMouse) targetMouseDelta = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2();
        if(!lockMovement)
        {
            string[] inputAxes = { "Horizontal", "Vertical" };
            string[] glitchedAxes = { "Glitch Horizontal", "Glitch Vertical" };
            if(!invertMouse) targetDir = new Vector2(Input.GetAxisRaw(inputAxes[0]), Input.GetAxisRaw(inputAxes[1]));
            if(invertMouse) targetDir = new Vector2(Input.GetAxisRaw(glitchedAxes[0]), Input.GetAxisRaw(glitchedAxes[1]));
            if(Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1))
            {
                if(invertMouse)
                {
                    targetDir = new Vector2(-1, 0);
                }
                else
                {
                    targetDir = new Vector2(0, 1);
                }
            }
        }

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);


        if (controller.isGrounded)
            velocityY = 0.0f;

        float gravityToUse = gravity;
        if (lockGravity) gravityToUse = 0;

        if(useDeltaTime)
            velocityY += gravityToUse * Time.deltaTime;
        else
            velocityY += gravityToUse;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * (walkSpeed * speedMultiplier) + Vector3.up * velocityY;
        if (useDeltaTime && controller.enabled)
            controller.Move(velocity * Time.deltaTime);
        else if(!useDeltaTime && controller.enabled)
            controller.Move(velocity);
    }

    public void ChangeSpeed(float desiredSpeed)
    {
        walkSpeed = desiredSpeed;
    }

    public void toggleMovementLock(bool canMove)
    {
        lockMovement = !canMove;
        checkForMovement = canMove;
    }

    public void TeleportPlayer(Vector3 newPos)
    {
        StartCoroutine(TeleportPlayerEnum(newPos));
    }

    IEnumerator TeleportPlayerEnum(Vector3 newPos)
    {
        transform.position = newPos;
        //player.isDisabled = false;

        controller.enabled = false;

        yield return new WaitForEndOfFrame();

        controller.enabled = true;
    }

    void CheckForSceneChange()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("null");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LostInThoughts");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("UnderThoughts Part 2");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("hub_main");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("New Messages");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("RedText Intro");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Look both ways");
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                PlayerPrefs.DeleteKey("glotchiness");
                PlayerPrefs.DeleteKey("Scene Save");
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                PlayerPrefs.SetInt("glotchiness", 1);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                PlayerPrefs.SetInt("glotchiness", 2);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerPrefs.SetInt("glotchiness", 3);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerPrefs.SetInt("glotchiness", 4);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                PlayerPrefs.SetInt("glotchiness", 5);
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                if(SteamManager.Initialized)
                    Steamworks.SteamUserStats.ResetAllStats(true);
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                PlayerPrefs.SetInt("glotchiness", 5);
                PlayerPrefs.SetString("Scene Save", "Amelia_Restaurant");
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("thats it");
    }
}