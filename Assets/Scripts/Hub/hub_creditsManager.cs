using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class hub_creditsManager : MonoBehaviour
{
    [SerializeField] Transform camPos;
    [SerializeField] Image blackScreen;
    [SerializeField] Camera secondCam;
    [SerializeField] MeshRenderer[] nooseMat;
    [Header("Credits")]
    //[Header("Credits")]
    [SerializeField] GameObject Creator, Jester, Cast, Playtesters, Playtesters1, Support, Support1, Support2, SpecialThanks, Unity, Help, Final;
    [SerializeField] TextMeshPro nameText;
    [SerializeField] TextMeshPro argNameText;
    [SerializeField] GameObject argThanks;
    [SerializeField] playerMove pluto;
    [SerializeField] Transform plutoPos1, plutoPos2;
    [SerializeField] TextMeshPro debugText;
    PlayerController _controller;
    AudioSource source;
    [HideInInspector] public bool canQuit = false;
    [SerializeField] string creditsDir = "";

    private void Start()
    {
        FindObjectOfType<DRP>().RP("Goodbye.");
        MovePDF();
        pluto.gameObject.SetActive(false);

        _controller = FindObjectOfType<PlayerController>();
        source = GetComponent<AudioSource>();
        secondCam.gameObject.SetActive(true);

        _controller.controller.enabled = false;
        _controller.lockLook = true;
        _controller.transform.position = camPos.position;
        _controller.transform.rotation = camPos.rotation;

        LerpSolution.lerpImageColour(blackScreen, Color.clear, 0.1f, Color.black);
        StartCoroutine(Credits());
        StartCoroutine(fadeNoose());
    }

    IEnumerator fadeNoose()
    {
        Color currentTrans = nooseMat[0].material.GetColor("_Color");
        float dist = 0;
        float time = 80;
        while (dist < time)
        {
            dist += Time.deltaTime;
            nooseMat[0].material.SetColor("_Color", Color.Lerp(currentTrans, Color.clear, dist / time));
            nooseMat[1].material.SetColor("_Color", Color.Lerp(currentTrans, Color.clear, dist / time));
            yield return GeneralManager.waitForEndOfFrame;
        }
    }

    void MovePDF()
    {
        if (Application.isEditor) return;
        try
        {
            string FirstDir = Application.dataPath;
            FirstDir = FirstDir.Replace('/', '\\');
            FirstDir += "\\Resources\\ligma.data";

            string SecondDir = Application.dataPath;
            List<string> SecondDirOperation = new List<string>(SecondDir.Split('/'));
            SecondDirOperation.RemoveAt(SecondDirOperation.Count - 1);
            SecondDir = string.Join("\\", SecondDirOperation);
            SecondDir += "\\Full Credits Document.pdf";
            File.Move(FirstDir, SecondDir);
            creditsDir = SecondDir.Replace('\\','/');
        }
        catch(Exception e)
        {
            //debugText.text = e.ToString();
        }
    }

    IEnumerator Credits()
    {
        yield return GeneralManager.waitForSeconds(4);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _controller.lockCursor = false;
        _controller.showCursor = true;
        source.Play();

        Creator.SetActive(true);

        yield return GeneralManager.waitForSeconds(6);

        Creator.SetActive(false);
        Jester.SetActive(true);

        yield return GeneralManager.waitForSeconds(6);

        Jester.SetActive(false);
        Cast.SetActive(true);

        yield return GeneralManager.waitForSeconds(6);

        Cast.SetActive(false);
        Playtesters.SetActive(true);

        yield return GeneralManager.waitForSeconds(8.5f);


        Playtesters.SetActive(false);
        Playtesters1.SetActive(true);

        yield return GeneralManager.waitForSeconds(8.5f);

        Playtesters1.SetActive(false);
        Support.SetActive(true);

        yield return GeneralManager.waitForSeconds(8.5f);

        Support.SetActive(false);
        Support1.SetActive(true);

        yield return GeneralManager.waitForSeconds(8.5f);

        Support1.SetActive(false);
        Support2.SetActive(true);

        yield return GeneralManager.waitForSeconds(8.5f);

        Support2.SetActive(false);
        if (FindObjectOfType<hub_glitchCredits>() == null)
        {
            SpecialThanks.SetActive(true);
            nameText.text = PlayerPrefs.GetString("playerName");
        }
        else
        {
            argThanks.SetActive(true);
            argNameText.text = PlayerPrefs.GetString("playerName");
        }
        StartCoroutine(movePluto());

        yield return GeneralManager.waitForSeconds(12);

        if (FindObjectOfType<hub_glitchCredits>() == null)
        {
            SpecialThanks.SetActive(false);
        }
        else
        {
            argThanks.SetActive(false);
        }
        Unity.SetActive(true);

        yield return GeneralManager.waitForSeconds(6);

        Unity.SetActive(false);
        Help.SetActive(true);

        yield return GeneralManager.waitForSeconds(10);

        Help.SetActive(false);
        Gas.EarnAchievement("ACH_CREDITS");
        GeneralManager.SetSceneSave("Reboot");
        Final.SetActive(true);

        canQuit = true;

    }


    IEnumerator movePluto()
    {
        yield return GeneralManager.waitForSeconds(2);
        pluto.gameObject.SetActive(true);

        pluto.canControl = false;

        while (Vector2.Distance(pluto.transform.position, plutoPos1.position) > 0.1f)
        {
            pluto.GetComponent<SpriteRenderer>().flipX = true;
            pluto.transform.localPosition += new Vector3(-1, 0) * pluto.moveSpeed * Time.deltaTime;
            pluto.PlutoAnimator.SetFloat("Speed", 1);
            //Debug.Log(Vector2.Distance(pluto.transform.position, plutoPos1.position));
            yield return new WaitForEndOfFrame();

        }

        pluto.PlutoAnimator.SetFloat("Speed", 0);

        yield return GeneralManager.waitForSeconds(3);

        while (true)
        {
            pluto.GetComponent<SpriteRenderer>().flipX = false;
            pluto.transform.localPosition += new Vector3(1, 0) * pluto.moveSpeed * Time.deltaTime;
            pluto.PlutoAnimator.SetFloat("Speed", 1);
            //Debug.Log(Vector2.Distance(pluto.transform.position, plutoPos1.position));
            yield return new WaitForEndOfFrame();

        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && canQuit)
        {
            Application.Quit();
        }
    }

    public void OpenLink(bool discord)
    {
        if (discord) Application.OpenURL("https://discord.gg/7JW9R4waEg");
        else Application.OpenURL("https://buymeacoffee.com/blaze42");
    }

    public void OpenCredits()
    {
        if (!Application.isEditor)
        {
            Application.OpenURL(creditsDir);
        }
    }
}
