using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class devroom_manager : MonoBehaviour
{
    [SerializeField] AudioClip inception;
    [SerializeField] TextMeshPro chairText;
    [Header("Pleh 2")]
    [SerializeField] SpriteRenderer black;
    [SerializeField] TextMeshPro text;
    [SerializeField] Transform pleh2spawnPos;
    [Header("Button")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] TextMeshPro buttonText;
    [Header("Chair god")]
    [SerializeField] Transform chairSpawnPoint;

    PlayerController controller;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();

        if (PlayerPrefs.GetString("thats it") == "pleh2")
        {
            controller.TeleportPlayer(pleh2spawnPos.position);
        }
        else if (PlayerPrefs.GetString("thats it") == "chair")
        {
            controller.TeleportPlayer(chairSpawnPoint.position);
            LerpSolution.lerpSpriteColour(black, Color.clear, 0.3f, Color.white);
            LerpSolution.lerpCamFov(controller.GetComponentInChildren<Camera>(), 60, 0.3f, 0);
        }

        if (PlayerPrefs.GetString("inception") == "yeppers")
        {
            GetComponent<AudioSource>().PlayOneShot(inception);
            PlayerPrefs.DeleteKey("inception");
        }

        UpdateChairText();
    }

    public void UpdateChairText()
    {
        chairText.text = "btw uve hit\nthe chair " + PlayerPrefs.GetInt("chair") + " times it's true";
    }

    public void pleh2()
    {
        StartCoroutine(pleh2Enum());
    }

    private IEnumerator pleh2Enum()
    {
        black.color = Color.black;
        text.text = "loading pleh 2...";
        controller.lockMovement = true;

        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("jesterGames");
        sceneOp.allowSceneActivation = false;
        yield return GeneralManager.waitForSeconds(3f);

        PlayerPrefs.SetString("thats it", "pleh2");
        sceneOp.allowSceneActivation = true;
    }

    public void OnButtonClick()
    {
        if (Random.Range(0, 100) != 0)
        {
            controller.TeleportPlayer(spawnPoint.position);
        }
        else
        {
            // do nothing... NOT

            buttonText.text = "HAHA JK IT DID DO SOMETHING (making the text say this(that's all it did(i hope it was worth it)))";
        }
    }

    public void bingus()
    {
        Gas.EarnAchievement("ACH_10.17");
    }
}
