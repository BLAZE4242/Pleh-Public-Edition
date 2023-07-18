using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Dev_Confess_Manager : MonoBehaviour
{
    [SerializeField] AudioClip click2;
    [SerializeField] GameObject text;
    [SerializeField] GameObject roof;
    [SerializeField] GameObject RedText;
    [SerializeField] AudioClip impact;
    [SerializeField] PostProcessVolume volume;
    [SerializeField] MeshRenderer[] rooms;
    ColorGrading grade;
    AsyncOperation sceneOp;

    private void Start()
    {
        FindObjectOfType<DRP>().RP("IT MADE ME DO THIS.");
        GeneralManager.SetSceneSave();
        volume.profile.TryGetSettings(out grade);
    }

    public void ShowOption()
    {
        GetComponent<AudioSource>().PlayOneShot(click2);
        text.SetActive(true);
    }

    public void end1()
    {
        sceneOp = SceneManager.LoadSceneAsync("Amelia_Confess");
        sceneOp.allowSceneActivation = false;
        text.SetActive(false);
    }

    public void end2()
    {
        foreach (MeshRenderer room in rooms)
        {
            Destroy(room);
        }
    }

    public void end()
    {
        sceneOp.allowSceneActivation = true;
    }



    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && text.activeInHierarchy && roof.activeInHierarchy)
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + " arg", "1");

            roof.SetActive(false);
            RedText.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(impact);
            CameraShake.Shake(0.2f, 0.1f);
            grade.postExposure.value = -2.96f;
        }
    }
}
