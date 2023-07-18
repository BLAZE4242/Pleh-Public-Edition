using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PipeMovement : MonoBehaviour
{
    [SerializeField] Pipe StartPipe;
    [SerializeField] float PullForce = 3;
    [SerializeField] float pipeSpeed = 1;
    [Header("Pipe Trigger")]
    [SerializeField] bool enableRestartAfter = true;
    [SerializeField] UnityEvent PipeEvent;
    [SerializeField] Pipe TriggerPipe;
    [SerializeField] doorTrigger2D wall;
    playerMove _pluto;
    Rigidbody2D rb;
    Camera cam;

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q))
        //    FindObjectOfType<playerMove>().GetComponent<Rigidbody2D>().fr
        //    FindObjectOfType<playerMove>().StartSpinnn();
    }

    void Start()
    {
        _pluto = FindObjectOfType<playerMove>();
        rb = _pluto.GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    public void PlayerEnterPipesButton(ButtonHandle button = null)
    {
        if (button != null && !button.isTriggered) return;
        PlayerEnterPipes();
    }

    [SerializeField] Collider2D specificCube;
    public void PlayerEnterPipes()
    {
        if (wall != null) StartCoroutine(openWall());
        _pluto.canCamSwapScene = false;
        _pluto.canControl = false;
        _pluto.lockRestart = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Vector3 difference = _pluto.transform.position - StartPipe.PullOrbit().position;

        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube"))
        {
            if (Vector2.Distance(cube.transform.position, _pluto.transform.position) < 7)
            {
                cube.GetComponent<Rigidbody2D>().gravityScale = 0;
                cube.transform.SetParent(_pluto.transform);
            }
        }

        // rb.AddForce(difference.normalized * PullForce, ForceMode2D.Force);
        LerpSolution.lerpPosition(_pluto.transform, vectorWithoutZ(StartPipe.PullOrbit().position, _pluto.transform.position.z), PullForce);
        
        // StartCoroutine(throwPlayer0());
        
        StartCoroutine(FollowPipes(0.3f));
    }

    IEnumerator openWall()
    {
        while (true)
        {
            wall.Trigger();
            yield return new WaitForEndOfFrame();
        }
    }
     
    float multi = 0f;
    IEnumerator throwPlayer0()
    {
        float speed = 0.4f;
        float dist = 0f;
        while (dist < 1)
        {
            dist += Time.deltaTime * speed;
            multi = Mathf.Lerp(0, 1, dist);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FollowPipes(float waitTime)
    {
        yield return GeneralManager.waitForSeconds(waitTime);

        _pluto.StartSpinnn();

        LerpSolution.lerpCamSize(cam, 13, 1);

        Vector3 camTarget = vectorWithoutZ(StartPipe.TargetNextPipe().position, cam.transform.position.z);
        LerpSolution.lerpPosition(cam.transform, camTarget, pipeSpeed);

        while (cam.transform.position != camTarget)
        {
            yield return new WaitForEndOfFrame();
        }

        foreach(Pipe pipe in StartPipe.PipeChildren())
        {
            if(pipe == TriggerPipe)
            {
                PipeEvent.Invoke();
            }

            camTarget = vectorWithoutZ(pipe.TargetNextPipe().position, cam.transform.position.z);
            Vector3 plutoTarget = vectorWithoutZ(pipe.TargetNextPipe().position, _pluto.transform.position.z);

            LerpSolution.lerpPosition(cam.transform, camTarget, pipeSpeed);
            LerpSolution.lerpPosition(_pluto.transform, plutoTarget, pipeSpeed);

            while(cam.transform.position != camTarget)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        List<Transform> cubes = new List<Transform>();

        for(int i = 0; i < _pluto.transform.childCount; i++)
        {
            if (_pluto.transform.GetChild(i).CompareTag("Cube"))
            {
                _pluto.transform.GetChild(i).GetComponent<Rigidbody2D>().gravityScale = 1;
                cubes.Add(_pluto.transform.GetChild(i ).transform);
            }
        }

        foreach (Transform cube in cubes)
        {
            cube.SetParent(null);
            if(_pluto.pickedUpCube != cube) cube.position = _pluto.transform.position;
        }
        _pluto.canCamSwapScene = true;
        _pluto.canControl = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = new Vector2(rb.velocity.x, 1 * -pipeSpeed);

        GameObject spawnPos = GameObject.Find("Camera Final Pos");
        if(spawnPos != null)
        {
            LerpSolution.lerpPosition(cam.transform, vectorWithoutZ(spawnPos.transform.position, cam.transform.position.z), pipeSpeed);
        }

        _pluto.StopSpinnn();
        try { _pluto.DropCube(); } catch { }

        LerpSolution.lerpCamSize(cam, 10, 1);

        if(enableRestartAfter) _pluto.lockRestart = false;
    }

    Vector3 vectorWithoutZ(Vector3 vector, float zValue)
    {
        return new Vector3(vector.x, vector.y, zValue);
    }

    public void LoadNextLevel()
    {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        int NextScene = CurrentScene + 1;
        DontDestroyOnLoad(cam);
        SceneManager.LoadSceneAsync(NextScene);
    }

    void OnLevelWasLoaded()
    {
        // Undo DontDestroyOnLoad for the camera
        SceneManager.MoveGameObjectToScene(cam.gameObject, SceneManager.GetActiveScene());
    }

    public void movePipesZ(PipeMovement pipe)
    {
        pipe.transform.position = new Vector3(pipe.transform.position.x, pipe.transform.position.y, 5);
    }
}
