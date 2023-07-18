using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UTintro : MonoBehaviour
{
    [SerializeField] Image blackScreen;
    [SerializeField] AudioClip stairMusic;
    [SerializeField] AudioClip stairBreaking;
    [SerializeField] Transform[] teleportingWalls; // clockwise, front, right, back, left
    PlayerController _controller;
    music oldMusic;
    music newMusic;
    UTGameManager Manager;
    bool hasStarted = false;
    bool canTeleport = true;
    void StateStart(UTGameManager _manager)
    {
        _controller = FindObjectOfType<PlayerController>();

        music[] musicsFound = FindObjectsOfType<music>();
        foreach(music Music in musicsFound)
        {
            if (Music.source.isPlaying)
            { 
                oldMusic = Music;
            }
            else
            {
                newMusic = Music;
            }
        }
        if(oldMusic == null)
        {
            newMusic.source.Play();
        }
        Manager = _manager;
        StartCoroutine(CheckForGravity());
    }

    public void teleportPlayer(string targetNewDirection)
    {
        Debug.Log("yo?");
        if(!canTeleport) return;
        // StartCoroutine(teleportPlayerEnum(targetNewDirection));
    }

    //IEnumerator teleportPlayerEnum(string targetNewDirection)
    //{
    //    Transform playerTrans;
    //    Transform targetPos;
    //    switch (targetNewDirection)
    //    {
    //        case "front":
    //            playerTrans = _controller.transform;
    //            _controller.controller.enabled = false;
    //            targetPos = teleportingWalls[0].GetComponentInChildren<Transform>();
    //            playerTrans.position = targetPos.position;
    //            yield return new WaitForEndOfFrame();
    //            _controller.controller.enabled = true;
    //            break;
    //        case "right":
    //            playerTrans = _controller.transform;
    //            _controller.controller.enabled = false;
    //            targetPos = teleportingWalls[1].GetComponentInChildren<Transform>();
    //            playerTrans.position = targetPos.position;
    //            yield return new WaitForEndOfFrame();
    //            _controller.controller.enabled = true;
    //            break;
    //        case "back":
    //            playerTrans = _controller.transform;
    //            _controller.controller.enabled = false;
    //            targetPos = teleportingWalls[2].GetComponentInChildren<Transform>();
    //            playerTrans.position = targetPos.position;
    //            yield return new WaitForEndOfFrame();
    //            _controller.controller.enabled = true;
    //            break;
    //        case "left":
    //            Debug.Log("????");
    //            playerTrans = _controller.transform;
    //            _controller.controller.enabled = false;
    //            targetPos = teleportingWalls[3].GetComponentInChildren<Transform>();
    //            playerTrans.position = targetPos.position;
    //            yield return new WaitForEndOfFrame();
    //            _controller.controller.enabled = true;
    //            break;
    //        default:
    //            Debug.LogError("I don't think you wrote front, right, back or left correctly :thinking:");
    //            break;
    //    }
    //}

    void StateUpdate(UTGameManager _manager)
    {

    }

    IEnumerator CheckForGravity()
    {
        LerpSolution.lerpVolume(newMusic.source, 1, 0.2f, 0);
        while (!_controller.controller.isGrounded) yield return new WaitForEndOfFrame();
        _controller.gravity = -50;
        yield return GeneralManager.waitForSeconds(oldMusic.source.clip.length - oldMusic.source.time);
        Destroy(oldMusic.gameObject);
        yield return GeneralManager.waitForSeconds(2);
        newMusic.source.Play();
    }


    public void PlayerFallStairs(GameObject stair)
    {
        StartCoroutine(PlayerFallStairsEnum(stair));
    }

    private IEnumerator PlayerFallStairsEnum(GameObject stair)
    {
        _controller.gravity = -15;
        stair.GetComponentInChildren<MeshCollider>().enabled = false;
        foreach (Rigidbody rb in stair.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(Vector3.down * Random.Range(400, 1000));
        }
        FindObjectOfType<music>().source.PlayOneShot(stairBreaking);

        yield return GeneralManager.waitForSeconds(0.4f);

        GameObject.Find("SFX").GetComponent<AudioSource>().Play();
        LerpSolution.lerpVolume(GameObject.Find("SFX").GetComponent<AudioSource>(), 1, 0.3f, 0);
        DontDestroyOnLoad(GameObject.Find("SFX"));
    }

    public void PlayerFallFade()
    {
        Debug.Log("Player should fade");
        StartCoroutine(PlayerFallFadeEnum());
    }

    IEnumerator PlayerFallFadeEnum()
    {
        LerpSolution.lerpImageColour(blackScreen, Color.black, 0.5f);
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        //DontDestroyOnLoad(_controller.gameObject);
        while(blackScreen.color != Color.black) yield return new WaitForEndOfFrame();
        yield return GeneralManager.waitForSeconds(2);
        AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(nextScene);
        sceneOperation.allowSceneActivation = false;
        sceneOperation.allowSceneActivation = true;
        while(sceneOperation.progress != 1) yield return new WaitForEndOfFrame();

        Manager.blackScreen = blackScreen;
        Manager.currentUTProjectState = UTGameManager.UTProjectStates.fall;
    }

    public void CheckStateStarted(UTGameManager _manager)
    {
        if(!hasStarted)
        {
            StateStart(_manager);
            StateUpdate(_manager);
            hasStarted = true;
        }
        else
        {
            StateUpdate(_manager);
        }
    }
}
