using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class redTextIntro : MonoBehaviour
{
    [SerializeField] TextMeshPro[] script;
    [SerializeField] MeshRenderer wallLook;
    [Header("After first teleport")]
    [SerializeField] MeshRenderer firstTeleportLook;
    [Header("After second teleport")]
    [SerializeField] MeshRenderer secondTeleportLook;
    PlayerController player;
    glitchText _gText;

    enum RedTextIntroState { intro, afterFirstTeleport, afterSecondTeleport }
    RedTextIntroState currentRedTextIntroState;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        FindObjectOfType<DRP>().RP("I didn't mean for any of this...");
        GeneralManager.SetSceneSave();
        _gText = GetComponent<glitchText>();
        if(player.transform.position == new Vector3(0, 1, 0)) StartCoroutine(start());
    }

    IEnumerator start()
    {
        player.lockMovement = true;
        player.lockLook = true;

        foreach (TextMeshPro text in script) text.gameObject.SetActive(false);

        foreach(TextMeshPro text in script)
        {
            yield return GeneralManager.waitForSeconds(4f);
            text.gameObject.SetActive(true);
            StartCoroutine(_gText.createGlitchedText(text.text, text));
        }
        player.lockMovement = false;
        player.lockLook = false;
    }

    public void StartMusic(AudioSource music)
    {
        StartCoroutine(StartMusicEnum(music));
    }

    IEnumerator StartMusicEnum(AudioSource music)
    {
        yield return GeneralManager.waitForSeconds(3f);
        music.Play();
    }

    

    public void triggerMovePlayer(PlayerController player)
    {
        StartCoroutine(triggerMovePlayerEnum(player));
    }

    IEnumerator triggerMovePlayerEnum(PlayerController player)
    {
        Camera playerCam = player.playerCamera.GetComponent<Camera>();
        while (wallLook.IsVisibleFrom(playerCam))
        {
            yield return new WaitForEndOfFrame();
        }

        player.TeleportPlayer(player.transform.position + new Vector3(0, 0, 10));        

        currentRedTextIntroState = RedTextIntroState.afterFirstTeleport;
        StartCoroutine(afterFirstTeleportUpdate());
    }

    private void Update()
    {
        
    }

    IEnumerator afterFirstTeleportUpdate()
    {
        Camera playerCam = player.playerCamera.GetComponent<Camera>();
        while (!firstTeleportLook.IsVisibleFrom(playerCam))
        {
            yield return new WaitForEndOfFrame();
        }

        player.TeleportPlayer(player.transform.position + new Vector3(0, 0, 10));
        yield return GeneralManager.waitForSeconds(0.2f);

        TextMeshPro text = secondTeleportLook.GetComponent<TextMeshPro>();
        StartCoroutine(_gText.createGlitchedText(text.text, text));
        GetComponent<redTextIntro_end>().canIncreaseMusic = true;
    }
}
