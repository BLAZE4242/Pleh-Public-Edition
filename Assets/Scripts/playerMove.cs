using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class playerMove : MonoBehaviour
{
    VoiceLineManager vlManager;
    [SerializeField] VoiceLine pickUpVoiceLine;
    public float moveSpeed;
    [SerializeField] float jumpForce;
    Camera cam;
    [SerializeField] Vector3 offset;
    public Transform camSpawnPos, camEndPos;
    public Animator PlutoAnimator;
    [Header("Pick Up Variables")]
    [SerializeField] AudioClip cubeSfx;
    [SerializeField] Transform cubePickUpTrans;
    [SerializeField] BoxCollider2D cubeHitbox;
    [SerializeField] float CubeRotateSpeed = 1f;
    BoxCollider2D col;
    public Animator fadeAnim;
    public bool canControl = true;
    bool cubePickedUp;
    [HideInInspector] public Transform pickedUpCube;
    public bool isGrounded;
    [SerializeField] TextMeshProUGUI tutText;

    [Header("Camera Movement")]
    public bool canCamSwapScene = true;
    public bool canCamSwapLeft = true;
    public bool canCamSwapRight = true;
    public enum CameraDirection {left, right, up, down}
    [HideInInspector] public bool camKill;

    [Header("Spinnn")]
    [SerializeField] float spinnnSpeed;

    [HideInInspector] public infoFromScene _infoFromScene;
    [HideInInspector] public music _music;
    public bool lockRestart;
    conveyerBeltHandle _handle;
    ScrollUV starfield;
    [HideInInspector] public Vector3 defaultScale;
    Rigidbody2D rb;
    Vector3 movement;
    bool isCredits;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        playerMove[] plutoClones = FindObjectsOfType<playerMove>();
        if (plutoClones.Length > 1) Destroy(gameObject);

        isCredits = SceneManager.GetActiveScene().name == "hub_main";

        if (!isCredits)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AllowIt()
    {
        canControl = true;
    }

    void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            Destroy(cam.gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator Start()
    {
        //References of things I don't care about
        cam = FindObjectOfType<Camera>();
        _infoFromScene = FindObjectOfType<infoFromScene>();
        _music = FindObjectOfType<music>();
        _handle = FindObjectOfType<conveyerBeltHandle>();
        rb = GetComponent<Rigidbody2D>();
        starfield = FindObjectOfType<ScrollUV>();
        vlManager = FindObjectOfType<VoiceLineManager>();
        col = GetComponent<BoxCollider2D>();
    
        defaultScale = transform.lossyScale;

        ScrollUV _scroll;

        switch(_infoFromScene.messageFromScene)
        {
            case "zoom in":
                GameObject playerBlocker = GameObject.Find("blocker");
                BoxCollider2D blockCol = playerBlocker.GetComponent<BoxCollider2D>();
                blockCol.enabled = true;
                LerpSolution.lerpCamColour(cam, cam.backgroundColor, 0.7f, Color.black);
                LerpSolution.lerpCamSize(cam, 10, 0.4f, 2000);
                yield return GeneralManager.waitForSeconds(2.3f);
                blockCol.enabled = false;
                break;
            case "fade in":
                fadeAnim.SetTrigger("fadeIn");
                _scroll = FindObjectOfType<ScrollUV>();
                _scroll.transform.SetParent(null);
                break;
            case "oh yea baby time to go big glitch":
                lockRestart = true;
                fadeAnim.SetTrigger("fadeIn");
                Debug.Log("We're glitchin baby");
                break;
            case "scroll down":
                canCamSwapScene = false;
                scrollDown();
                yield return GeneralManager.waitForSeconds(2);
                if(transform.parent == null) canCamSwapScene = true;
                break;
            case "first time lets goo":
                scrollDown();
                yield return GeneralManager.waitForSeconds(2);
                // wasd
                break;
            default:
                fadeAnim.SetTrigger("fadeIn");
                _scroll = FindObjectOfType<ScrollUV>();
                _scroll.transform.SetParent(null);
                break;
        }
    }

    void scrollDown(bool canInvertCamStatus = false)
    {
        ScrollUV _scroll = FindObjectOfType<ScrollUV>();
        _scroll.transform.SetParent(cam.transform);
        cam.transform.position = camSpawnPos.position;
        LerpSolution.lerpPosition(cam.transform, camEndPos.position, 0.7f);
        StartCoroutine(_scroll.reachTarget(0.4f, canInvertCamStatus));
    }

    void Update()
    {
        if (canControl)
        {
            //Input stored into a vector 2 (Or 3 idk it was 2am when I wrote this)
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
            //Animation variables so player can walk
            //anim.SetFloat("InputHorizontal", Mathf.Abs(movement.x));

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                isGrounded = false;
            }
            scaleAccordingly();
        }
        else
        {
            movement = Vector3.zero;
        }

        checkForGrab();

        if (Input.GetKeyDown(KeyCode.R) && !lockRestart)
        {
            StartCoroutine(restart());
        }

        if(canControl) PlutoAnimator.SetFloat("Speed", Mathf.Abs(movement.x));
        PlutoAnimator.SetFloat("Jump Speed", Mathf.Abs(rb.velocity.y));

        if (_glitch == null) canCamSwapLeft = true;
        try
        {

            if (GameObject.Find("Camera Final Pos").CompareTag("Finish") && cam.transform.position == GameObject.Find("Camera Final Pos").transform.position)
            {
                canCamSwapLeft = false;
            }
        }
        catch { }
    }


    void FixedUpdate()
    {
        if(canControl)
        {
            if(!isCredits) transform.position += movement * moveSpeed * Time.deltaTime;
            else transform.localPosition += movement * moveSpeed * Time.deltaTime;
        }

        if(cubePickedUp)
        {
            LerpSolution.lerpPosition(pickedUpCube, cubePickUpTrans.position, 9);
        }
    }

    IEnumerator randomSpin(Transform cube)
    {
        while(true)
        {
            cube.Rotate(new Vector3(0, 0, cube.rotation.z + 1) * CubeRotateSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    Quaternion randomRotationForCube()
    {
        float x = Random.Range(0, 360);
        return new Quaternion(x, 0, 0, 0);
    }
    
    void checkForGrab()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //vlManager.PlayLine(pickUpVoiceLine);
            try { if (tutText.text == "Press E to pick up") tutText.text = ""; } catch { }

            if (!pickedUpCube)
            {
                // Find the closest game object with the tag "Cube"
                GameObject closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = cubePickUpTrans.position;
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Cube"))
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance && curDistance < 5)
                    {
                        closest = go;
                        distance = curDistance;
                    }
                }

                if(closest != null)
                {
                    PickUpCube(closest);
                }
            }
            else
            {
                DropCube();
            }

            PlutoAnimator.SetTrigger("Grab Trigger");
        }
    }

    PlehGlitchManager _glitch;
    void PickUpCube(GameObject cube)
    {
        try {FindObjectOfType<music>().source.PlayOneShot(cubeSfx); }
        catch
        {
            try
            {
                GetComponent<AudioSource>().PlayOneShot(cubeSfx);
            }
            catch { }
        }
        cube.transform.SetParent(cubePickUpTrans);
        float defaultGravityScale = cube.GetComponent<Rigidbody2D>().gravityScale;
        cube.GetComponent<Rigidbody2D>().gravityScale = 0;
        cube.GetComponent<BoxCollider2D>().isTrigger = true;
        
        cubeHitbox.enabled = true;

        pickedUpCube = cube.transform;
        cubePickedUp = true;
        
        foreach (floorButton2D button in FindObjectsOfType<floorButton2D>())
        {
            if (button.cubes.Contains(cube))
            {
                button.cubes.Remove(cube);
                button.numOfTrigger--;
            }
        }

        _glitch = FindObjectOfType<PlehGlitchManager>();
        if (_glitch != null)
        {
            _glitch.GlitchCube(pickedUpCube.GetComponent<Rigidbody2D>());
        }
    }

    public void DropCube()
    {
        try { FindObjectOfType<music>().source.PlayOneShot(cubeSfx); }
        catch
        {
            try
            {
                GetComponent<AudioSource>().PlayOneShot(cubeSfx);
            }
            catch { }
        }
        pickedUpCube.transform.SetParent(null);
        pickedUpCube.GetComponent<Rigidbody2D>().gravityScale = 1;
        pickedUpCube.GetComponent<BoxCollider2D>().isTrigger = false;

        cubeHitbox.enabled = false;

        pickedUpCube.GetComponent<BoxCollider2D>().enabled = true;

        pickedUpCube = null;
        cubePickedUp = false;
    }

    private IEnumerator restart()
    {
        Debug.Log("Made it to restart!");
        if (SceneManager.GetActiveScene().name != "Level 1")
        {
            Debug.Log("We're not in the first level");
            lockRestart = true;

            if(SceneManager.GetActiveScene().name == "Level 3") // change this wasd
            {
                _infoFromScene.messageFromScene = "oh yea baby time to go big glitch";
            }
            else
            {
                _infoFromScene.messageFromScene = "fade in";
            }

            if (fadeAnim == null) fadeAnim = GameObject.Find("Fade Anim").GetComponent<Animator>(); 

            fadeAnim.SetTrigger("fadeOut");

            yield return GeneralManager.waitForSeconds(0.8f);
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            DontDestroyOnLoad(cam);

            LerpSolution.StopCoroutines();

            if (cubePickedUp) DropCube();
            foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) Destroy(cube);

            SceneManager.LoadScene(currentSceneIndex);
            foreach(Pipe pipe in FindObjectsOfType<Pipe>())
            {
                // Are you fucking kidding me
                Destroy(pipe.gameObject);
            }

            //Destroy(gameObject);
            transform.position = GameObject.Find("Pluto Spawn Pos").transform.position;

            lockRestart = false;
            canControl = true;
            cam.transform.position = GameObject.Find("Camera Final Pos").transform.position;
            GetComponentInChildren<groundCheck>().Respawn();
        }

    }

    public void scaleAccordingly(float left = 0)
    {
        //player scale so they can do a backflip when we start walking the other way
        Vector3 playerScale = transform.localScale;
        if (movement.x < 0 || left == 1)
        {
            playerScale.x = -defaultScale.x;
        }
        else if (movement.x > 0 || left == 2)
        {
            playerScale.x = defaultScale.x;
        }

        transform.localScale = playerScale;
    }

    void SeeIfTrailer()
    {
        TrailerFirstCutscene[] yada = FindObjectsOfType<TrailerFirstCutscene>();
        if(yada.Length > 0)
        {
            yada[0].StartEvilCutscene();
        }
    }

    public void CameraScrollScreen(int camDirInt)
    {
        if(!canCamSwapScene) return;
        SeeIfTrailer();

        CameraDirection camDir = (CameraDirection)camDirInt;
        Transform cam = Camera.main.transform;
        Vector2 vectorToMove = new Vector2(30, 0);

        switch(camDir)
        {
            case CameraDirection.left:
                if (!canCamSwapLeft) return;
                vectorToMove = new Vector2(-30, 0);
                break;
            case CameraDirection.right:
                if (!canCamSwapRight) return;
                vectorToMove = new Vector2(30, 0);
                break;
            case CameraDirection.up:
                vectorToMove = new Vector2(0, 15);
                break;
            case CameraDirection.down:
                if (camKill)
                {
                    CamKill();
                    return;
                }
                vectorToMove = new Vector2(0, -15);
                break;
        }

        Vector2 camPos = cam.transform.position;

        // moves the camera

        // Disable movement and other moving objects
        canControl = false;
        PlutoAnimator.speed = 0;
        Vector2 vel = rb.velocity;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        foreach (ScrollUV starfieldL in FindObjectsOfType<ScrollUV>())
        {
            starfieldL.canMove = false;
        }

        StartCoroutine(waitForCamToMove(vectorToMove, camPos, camDir, vel));        
    }

    void CamKill()
    {
        cam.GetComponent<Animator>().SetTrigger("Death");
        transform.position = GameObject.Find("Respawn").transform.position;
    }

    public void ChangeCamKill(bool canKill)
    {
        camKill = canKill;
    }

    IEnumerator waitForCamToMove(Vector3 vectorToMove, Vector2 camPos, CameraDirection camDir, Vector2 vel)
    {
        LerpSolution.lerpPosition(cam.transform, cam.transform.position + vectorToMove, 1.6f);
        
        yield return GeneralManager.waitForSeconds(0.6f);

        canControl = true;
        isGrounded = true;
        PlutoAnimator.speed = 1;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = vel;

        foreach (ScrollUV starfieldL in FindObjectsOfType<ScrollUV>())
        {
            starfieldL.canMove = true;
            StartCoroutine(starfieldL.scroll());
        }
    }

    public void StartSpinnn()
    {
        PlutoAnimator.SetBool("Spinnn", true);
        StartCoroutine(Spinnn());
    }

    IEnumerator Spinnn()
    {
        while (PlutoAnimator.GetBool("Spinnn") == true)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0, 0, spinnnSpeed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
    }

    public void StopSpinnn()
    {
        PlutoAnimator.SetBool("Spinnn", false);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}