using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class messagesEvent : MonoBehaviour
{
    [Header("Distort")]
    [SerializeField] GameObject missingDisplay;
    [SerializeField] Camera gameCam, mainCam;
    Animator _gameAnim;
    public Animator _mainAnim;
    [SerializeField] Transform renderTexture;
    [SerializeField] AudioClip weirdLoop, loop, creepyLoop;
    [SerializeField] MeshRenderer bg;
    [SerializeField] Material missingTexture;
    [SerializeField] PostProcessProfile profile;
    [SerializeField] Image deserveImage;
    [SerializeField] float cameraZoomOutSpeed;
    Material defaultMaterial;
    messagesSpawnerManager _messageManager;
    Rigidbody2D playerRb;
    [Header("Bot Play Game")]
    [SerializeField] Transform ghostPlayer;
    musicManagerMessages _manager;
    groundCheck _groundCheck;
    playerControllerMessages _controller;
    AudioSource _source;

    void Start()
    {
        _gameAnim = gameCam.GetComponent<Animator>();
        _manager = FindObjectOfType<musicManagerMessages>();
        _groundCheck = FindObjectOfType<groundCheck>();
        _controller = FindObjectOfType<playerControllerMessages>();
        playerRb = _controller.GetComponent<Rigidbody2D>();
        _messageManager = FindObjectOfType<messagesSpawnerManager>();
    }

    void Update()
    {
        if(_manager == null) _manager = FindObjectOfType<musicManagerMessages>();
    }

    #region distort0

    IEnumerator distort0()
    {
        while(!_groundCheck.isGrounded) yield return new WaitForEndOfFrame();
        StartCoroutine(_manager.pitchAudio(0.7f, 1, 0.3f, 1, 1.2f));
        playerRb.constraints = RigidbodyConstraints2D.FreezePositionY;
        float currentSpeed = _controller.playerSpeed;
        float currentCamFov = gameCam.orthographicSize;
        StartCoroutine(moveBackwards(-currentSpeed, 1, 0.3f, currentSpeed, 1.2f));
        yield return GeneralManager.waitForSeconds(0.4f);
        _gameAnim.Play("Light1");
        yield return GeneralManager.waitForSeconds(0.8f);
        LerpSolution.lerpCamSize(gameCam, 3.2f, 1, currentCamFov);
        missingDisplay.SetActive(true);
        yield return GeneralManager.waitForSeconds(0.2f);
        missingDisplay.SetActive(false);
        yield return GeneralManager.waitForSeconds(0.7f);
        yield return GeneralManager.waitForSeconds(0.4f);
        _gameAnim.Play("Light0");
        gameCam.orthographicSize = currentCamFov;
    }

    IEnumerator moveBackwards(float speed1, float transSpeed1, float breakTime, float speed2, float transSpeed2)
    {
        float changePercentage = 0f;
        float currentSpeed = _controller.playerSpeed;

        while (changePercentage < 1f)
        {
            changePercentage += Time.deltaTime * transSpeed1;
            _controller.playerSpeed = Mathf.Lerp(currentSpeed, speed1, changePercentage);
            yield return new WaitForEndOfFrame();
        }

        yield return GeneralManager.waitForSeconds(breakTime);

        changePercentage = 0f;
        currentSpeed = _controller.playerSpeed;

        while (changePercentage < 1f)
        {
            changePercentage += Time.deltaTime * transSpeed2;
            _controller.playerSpeed = Mathf.Lerp(currentSpeed, speed2, changePercentage);
            yield return new WaitForEndOfFrame();
        }

        playerRb.constraints = RigidbodyConstraints2D.None;
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    #endregion

    void distort1()
    {
        _manager.changeTrack(weirdLoop);
        _manager.staticGlitch();
        //WHY DOESN'T THIS FUCKING WORK I'VE TRIED EVERYTHING BUT NOOOOOOOOOOO UNITY DOESN'T LIKE MY CODE WHEN I TRY TO DO SOMETHING PRODUCTIVE AHHHHHHHHH
        //oh nvm i didn't have a ( lol
        renderTexture.rotation = Quaternion.Euler(-90, 0, 0);
        _gameAnim.Play("upside1");
        defaultMaterial = bg.material;
        bg.material = missingTexture;
        missingTexture.mainTextureOffset = new Vector2(0, 0.2f);
    }

    IEnumerator distort2()
    {
        _manager.changeTrack(loop);
        _manager.staticGlitch();
        renderTexture.rotation = Quaternion.Euler(90, 0, -180);
        bg.material = defaultMaterial;
        yield return GeneralManager.waitForSeconds(3);
        StartCoroutine(distort3());
    }

    IEnumerator distort3()
    {
        float currentCamFov = mainCam.orthographicSize;
        float currentVolume = _manager._source.volume;
        LerpSolution.lerpCamSize(mainCam, 7, cameraZoomOutSpeed);
        LerpSolution.lerpVolume(_manager._source, 0.2f, cameraZoomOutSpeed);
        while(_manager._source.volume != 0.2f) yield return new WaitForEndOfFrame();

        yield return GeneralManager.waitForSeconds(5);
        deserveImage.enabled = true;
        CameraShake.Shake(1, 1);
        PostProcessVolume volume = mainCam.GetComponent<PostProcessVolume>();
        volume.profile = profile;
        _manager._source.clip = creepyLoop;
        _manager._source.Play();
        _manager.omoriGlitch(1);
        _manager._source.time = 3;
        _manager._source.volume = currentVolume;
        deserveImage.enabled = true;
        mainCam.GetComponent<VideoPlayer>().enabled = true;
        _mainAnim.Play("Light1");
        yield return GeneralManager.waitForSeconds(1.4f);
        _mainAnim.Play("Digi0");
        StartCoroutine(_messageManager.startSpam());
        yield return GeneralManager.waitForSeconds(6.6f);
        deserveImage.enabled = false;
        _manager.staticGlitch(2);
        _manager.pitchAudio(1.75f, 0.5f, 0, 1.75f, 0.5f);
        mainCam.orthographicSize = currentCamFov;
        mainCam.GetComponent<VideoPlayer>().enabled = false;
        LensDistortion lens;
        volume.profile.TryGetSettings(out lens);
        lens.active = false;
        
        yield return GeneralManager.waitForSeconds(2);
        StartCoroutine(bgRandomGlitch());
    }

    IEnumerator bgRandomGlitch()
    {
        while(true)
        {
            yield return GeneralManager.waitForSeconds(Random.Range(0.0f, 0.6f));
            if (Random.Range(0, 2) == 1)
            {
                defaultMaterial = bg.material;
                bg.material = missingTexture;
                //missingTexture.mainTextureOffset = new Vector2(0, 0.2f);
                yield return GeneralManager.waitForSeconds(Random.Range(0.0f, 0.7f));
                bg.material = defaultMaterial;
            }
        }
    }

    bool canCheckJump = true;

    void botPlayGame()
    {
        _controller.canJump = false;
        //check if there is a gap coming up and if there is jump. Then check if we can jump.
        //if we can't jump wait until we can, then jump. If we can jump, well jump. Easy as pie
        canCheckJump = true;
        StartCoroutine(updateBotPlay());
    }

    IEnumerator updateBotPlay()
    {
        bool canJump = true;
        while(true)
        {
            while (canCheckJump)
            {
                RaycastHit2D hit = Physics2D.Raycast(ghostPlayer.position, -Vector2.up);
                if (hit.collider == null && canJump)
                {
                    canJump = false;
                    while (!_groundCheck.isGrounded)
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    _controller.jump();
                }
                yield return GeneralManager.waitForSeconds(0.5f);
                canJump = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
