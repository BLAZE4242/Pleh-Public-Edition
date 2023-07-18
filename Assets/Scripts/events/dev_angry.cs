using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using Aura2API; // AURA HERE

public class dev_angry : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] GameObject[] lights;
    [Header("Sounds")]
    [SerializeField] AudioClip lightsOn;
    AudioSource source;


    void Start()
    {
        FindObjectOfType<DRP>().RP("What have I done?");
        source = GetComponent<AudioSource>();
        StartCoroutine(lightsGoUp());
    }

    IEnumerator lightsGoUp()
    {
        foreach (GameObject light in lights)
        {
            yield return GeneralManager.waitForSeconds(3);
            foreach (Transform child in light.transform)
            {
                // AuraLight lightRenderer = child.GetComponent<AuraLight>(); // AURA HERE
                //if (lightRenderer != null) // AURA HERE
                //{ // AURA HERE
                //    lightRenderer.strength = 30; // AURA HERE
                //} // AURA HERE

                source.PlayOneShot(lightsOn, 0.1f);
            }
        }
    }

    public void DevRageQuitLol()
    {
        StartCoroutine(DevRageQuitLolEnum());
    }

    IEnumerator DevRageQuitLolEnum()
    {
        PlayerPrefs.SetInt("glotchiness", 4);
        GeneralManager.SetSceneSave("RedText Intro");
        yield return GeneralManager.waitForSeconds(3f);
        Application.Quit();
        Debug.Log("Should quit");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Application.isEditor)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
