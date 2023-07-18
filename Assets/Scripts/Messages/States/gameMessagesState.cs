using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameMessagesState : messagesState
{
    bool hasBeenRun;
    [HideInInspector] public bool isDone;
    [SerializeField] messagesState endingState;
    [SerializeField] GameObject normalGround;
    [SerializeField] GameObject previousGround;
    public bool canDie = true;
    playerControllerMessages _playerController;
    List<int> spawnHistory = new List<int>(); //If this equals 1 then there's a gap, otherwise it's solid
    infoFromScene _info;
    messagesSpawnerManager _spawner;

    void startCurrentState()
    {
        if(hasBeenRun) return;
        canDie = true;
        _playerController = FindObjectOfType<playerControllerMessages>();
        _info = FindObjectOfType<infoFromScene>();
        _spawner = GetComponent<messagesSpawnerManager>();
        StartCoroutine(spawnCycle());
        StartCoroutine(_spawner.spawningCycle());
        //StartCoroutine(_spawner.startSpam());
        hasBeenRun = true;
    }

    public override messagesState runCurrentState()
    {
        startCurrentState();
        foreach (GameObject ground in GameObject.FindGameObjectsWithTag("Ground"))
        {
            if (_playerController.transform.position.x > ground.transform.position.x + 15)
            {
                Destroy(ground);
            }
        }
        if(!isDone) return this;
        else return endingState;
    }

    IEnumerator spawnCycle()
    {
        for (int i = 0; i < 4; i++)
        {
            spawnNewGround();
        }

        while (true)
        {
            yield return GeneralManager.waitForSeconds(1);
            spawnNewGround();
        }
    }

    public void spawnNewGround()
    {
        float distanceFromGround = 6.5f;
        if (Random.Range(0, 2) == 1)
        {
            spawnHistory = new List<int>();
        }//the error is in normalGround, it's being destroyed
        else if (spawnHistory.Count != 3)
        {
            distanceFromGround = 4f;
            spawnHistory.Add(1);
        }
        previousGround = Instantiate(normalGround, new Vector3(previousGround.transform.position.x + distanceFromGround, previousGround.transform.position.y, previousGround.transform.position.z), Quaternion.identity);
    }

    public void die(bool diedBecauseMessage)
    {
        Debug.Log("Should be dead");
        if(!canDie) return;
        foreach(messageManager _message in GameObject.FindObjectsOfType<messageManager>())
        {
            SpriteRenderer renderer = _message.GetComponent<SpriteRenderer>();
            LerpSolution.lerpSpriteColour(renderer, Color.clear, 2.7f);
        }
        _playerController.playerSpeed = 0;
        if(diedBecauseMessage) _info.messageFromScene = "message dead";
        else _info.messageFromScene = "dead";
        _info.messageArgument = _spawner.recentMessage;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
