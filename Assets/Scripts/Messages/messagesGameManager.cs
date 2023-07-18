using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messagesGameManager : MonoBehaviour
{
    [SerializeField] GameObject normalGround;
    [SerializeField] GameObject previousGround;
    playerControllerMessages _playerController;
    List<int> spawnHistory = new List<int>(); //If this equals 1 then there's a gap, otherwise it's solid
    
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<playerControllerMessages>();
        _playerController.playerSpeed = 0;
        //StartCoroutine(spawnCycle()); //only do this to start the game
    }

    IEnumerator spawnCycle()
    {
        for(int i = 0; i < 4; i++)
        {
            spawnNewGround();
        }

        while(true)
        {
            yield return GeneralManager.waitForSeconds(1);
            spawnNewGround();
        }
    }

    void Update()
    {
        foreach(GameObject ground in GameObject.FindGameObjectsWithTag("Ground"))
        {
            if(_playerController.transform.position.x > ground.transform.position.x + 15)
            {
                Destroy(ground);
            }
        }
    }

    void spawnNewGround()
    {
        float distanceFromGround = 6.5f;
        if(Random.Range(0, 2) == 1)
        {
            spawnHistory = new List<int>();
        }//the error is in normalGround, it's being destroyed
        else if(spawnHistory.Count != 4)
        {
            distanceFromGround = 4f;
            spawnHistory.Add(1);
        }
        previousGround = Instantiate(normalGround, new Vector3(previousGround.transform.position.x + distanceFromGround, previousGround.transform.position.y, previousGround.transform.position.z), Quaternion.identity);
    }

    public void die(bool diedBecauseMessage)
    {
        Debug.Log("You dead");
        _playerController.playerSpeed = 0;
    }
}
