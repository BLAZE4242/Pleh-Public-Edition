using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class menuMessagesState : messagesState
{
    public gameMessagesState gameState;
    bool hasBeenRun;
    bool isDone;
    [SerializeField] PostProcessVolume _volume;
    [SerializeField] GameObject loadingUI, menuUI;
    [SerializeField] TextMeshProUGUI deadText;
    [SerializeField] SpriteRenderer handSprite;
    [SerializeField] musicManagerMessages _musicManager;
    infoFromScene _info;
    playerControllerMessages _playerController;

    void startCurrentState()
    {
        if (hasBeenRun) return;
        _playerController = FindObjectOfType<playerControllerMessages>();
        _info = FindObjectOfType<infoFromScene>();
        _musicManager.gameObject.SetActive(true);

        loadingUI.SetActive(false);
        menuUI.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            gameState.spawnNewGround();
        }
        
        
        if(_info.messageFromScene == "dead")
        {
            deadText.text = @"Game over!
Try again?";
        }
        else if(_info.messageFromScene == "message dead")
        {
            deadText.text = "Game over!\nTry again?";
        }
        else
        {
            _musicManager.staticGlitch();
            Bloom _bloom;
            SpriteRenderer whiteScreen = GameObject.Find("BIG WHITE THING").GetComponent<SpriteRenderer>(); // I'm sorry lmao
            _volume.profile.TryGetSettings(out _bloom);
            whiteScreen.color = Color.white;
            _bloom.intensity.value = 30;

            // yield return GeneralManager.waitForSeconds(1f);
            LerpSolution.lerpSpriteColour(whiteScreen, Color.clear, 0.1f);
            LerpSolution.lerpBloomIntensity(_bloom, 7.9f, 0.1f);
        }
        _playerController.canJump = true;
        hasBeenRun = true;
    }
    public override messagesState runCurrentState()
    {
        startCurrentState();
        if (_playerController.hasJumped) StartCoroutine(startGame());
        if(isDone) return gameState;
        else return this;
    }

    IEnumerator startGame()
    {
        deadText.text = "";
        handSprite.sprite = null;
        yield return GeneralManager.waitForSeconds(1f);
        _playerController.playerSpeed = 4;
        isDone = true;
    }

}
