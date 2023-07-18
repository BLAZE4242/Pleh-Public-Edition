using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class conveyerBeltController : MonoBehaviour
{
    public bool isInControl;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int cubeStorageMax = 2;
    [SerializeField] TextMeshPro textForCubes;
    [SerializeField] TextMeshProUGUI tutTextRight;
    [SerializeField] GameObject cubePrefab;
    [SerializeField] GameObject rarePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float cubeSpawnForce = 10f;
    [Header("Weird shit bc I'm bad at math")]
    [SerializeField] float min = 18.03f;
    [SerializeField] float max = 38.8f;

    [HideInInspector] public int numOfAliveCubes;
    Vector3 move;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cubeTextDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInControl)
        {
            move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(trySpawnNewCube());
            }
        }
    }

    void FixedUpdate()
    {
        if(isInControl)
        {
            transform.position = new Vector3(transform.position.x + move.x * moveSpeed * Time.deltaTime, transform.position.y + move.y * moveSpeed * Time.deltaTime, transform.position.z);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, min, max), transform.position.y, transform.position.z);
        }
    }

    private IEnumerator trySpawnNewCube()
    {
        if(numOfAliveCubes < cubeStorageMax - 1)
        {
            spawnNewCube();
        }
        else if(numOfAliveCubes < cubeStorageMax)
        {
            spawnNewCube();
            yield return GeneralManager.waitForSeconds(4f);
            tutTextRight.text = "Press R to restart";
        }
    }

    private void spawnNewCube()
    {
        numOfAliveCubes++;
        Instantiate(Random.Range(0, 69) != 42 ? cubePrefab : rarePrefab, spawnPoint.position + new Vector3(0, 0, 1), Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector3.down * cubeSpawnForce;
        cubeTextDisplay();
    }

    public void cubeTextDisplay()
    {
        textForCubes.text = $"{numOfAliveCubes}/{cubeStorageMax}";
    }

    public void DisableRestart()
    {
        FindObjectOfType<playerMove>().lockRestart = true;
    }
}
