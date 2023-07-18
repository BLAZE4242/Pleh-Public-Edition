using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DemoDataHolder : MonoBehaviour
{
    public bool CanCrash, CanShowFPS;
    [SerializeField] GameObject FPSCanvas;
    [SerializeField] TextMeshProUGUI FPSText;

    void Awake()
    {
        DemoDataHolder[] objs = FindObjectsOfType<DemoDataHolder>();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().buildIndex == 1) Application.Quit();
            else
            {
                SceneManager.LoadScene(1);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if(CanShowFPS)
        {
            FPSCanvas.SetActive(true);
            FPSText.text = "FPS: " + Mathf.RoundToInt(1 / Time.deltaTime);

        }
        else
        {
            FPSCanvas.SetActive(false);
        }
    }
}
