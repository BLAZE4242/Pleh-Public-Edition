using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.SceneManagement;

public class hub_glitchCredits : MonoBehaviour
{
    [SerializeField] GameObject[] props;
    [SerializeField] GameObject door;
    [SerializeField] Transform deadPos;
    [SerializeField] GameObject Credits;
    [SerializeField] TextMeshPro hacker;
    [SerializeField] GameObject final;

    IEnumerator Start()
    {
        StartCoroutine(otherGlitches());
        StartCoroutine(RunMemory());
        Credits.transform.SetParent(FindObjectOfType<PlayerController>().transform);
        door.transform.SetParent(null);
        foreach (GameObject Object in props)
        {
            yield return GeneralManager.waitForSeconds(6f);

            Object.SetActive(false); // glitch;
        }

    }


    IEnumerator otherGlitches()
    {
        yield return GeneralManager.waitForSeconds(13);

        foreach (PostProcessLayer layer in FindObjectsOfType<PostProcessLayer>())
        {
            layer.enabled = false;
        }

        yield return GeneralManager.waitForSeconds(6);

        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            cam.backgroundColor = Color.black;
        }

        yield return GeneralManager.waitForSeconds(4);

        PlayerController _controller = FindObjectOfType<PlayerController>();

        _controller.controller.enabled = false;
        _controller.lockLook = true;
        _controller.transform.position = deadPos.position;
        _controller.transform.rotation = deadPos.rotation;

        yield return GeneralManager.waitForSeconds(15);

        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            cam.backgroundColor = new Color(0.37f, 0, 0.62f);
        }

        yield return GeneralManager.waitForSeconds(35);

        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            cam.backgroundColor = Color.black;
        }
    }
    IEnumerator RunMemory()
    {
        while (true)
        {
            hacker.text = $"Error: Missing/corrupt chunk {randomLongNumber()} from memory address {randomMemoryAddress()}";
            yield return GeneralManager.waitForSeconds(Random.Range(0.05f, 0.1f));
        }
    }

    string randomLongNumber()
    {
        string result = "";
        for (int i = 0; i < Random.Range(5, 10); i++)
        {
            result += Random.Range(0, 10);
        }
        return result;
    }

    string randomMemoryAddress()
    {
        return $"0{randomNumber()}{randomNumber()}{randomChar()}{randomNumber()}x{randomNumber()}{randomChar()}{randomChar()}{randomChar()}{randomNumber()}";
    }

    char randomChar()
    {
        string chars = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        return chars[Random.Range(0, chars.Length - 1)];
    }

    int randomNumber()
    {
        return Random.Range(0, 10);
    }

    private void Update()
    {
        if (final.activeInHierarchy)
        {
            PlayerPrefs.SetString("Backup", "yepp");
            SceneManager.LoadScene(0);
        }
    }
}
