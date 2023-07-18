using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class glitchInChamber2 : MonoBehaviour
{
    [SerializeField] GameObject allGlitches, switchGlitches;
    [SerializeField] Color defaultColor;
    [SerializeField] Color bgGlitchColor;
    [SerializeField] AudioClip staticGlitch;
    [SerializeField] Animator glitchAnim;
    [SerializeField] GameObject restartText;
    Camera cam;
    playerMove _playerMove;
    bool isOnBigGlitch;
    music _music;
    bool i;

    private void Start()
    {
        _playerMove = FindObjectOfType<playerMove>();
        _music = _playerMove._music;
        cam = Camera.main;
        defaultColor = cam.backgroundColor;
        if (_playerMove._infoFromScene.messageFromScene == "oh yea baby time to go big glitch")
        {
            isOnBigGlitch = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOnBigGlitch)
        {
            StartCoroutine(firstGlitch());
        }
        else
        {
            StartCoroutine(bigGlitch());
            return;
        }
    }

    private IEnumerator firstGlitch()
    {
        //refrences yada yada
        GameObject glitch1 = allGlitches.transform.Find("glitch1").gameObject;
        

        //play glitching sfx
        allGlitches.SetActive(true);
        cam.backgroundColor = bgGlitchColor;
        StartCoroutine(_music.musicGetGlitch());
        yield return GeneralManager.waitForSeconds(0.1f);
        allGlitches.transform.position += new Vector3(0.03f, 0.3f, 0);
        yield return GeneralManager.waitForSeconds(0.1f);
        for(int i = 0; i < 5; i++)
        {
            switchGlitches.transform.position += new Vector3(2, -1.1f, 0);
            yield return GeneralManager.waitForSeconds(0.1f);
            if(i == 2)
            {
                cam.backgroundColor = defaultColor;
            }
            else if (i == 3)
            {
                glitch1.transform.position = new Vector2(0, 4);
                _music.source.PlayOneShot(staticGlitch);
            }
            else if(i == 4)
            {
                glitch1.transform.position = new Vector2(0, 6);
            }
            switchGlitches.transform.position -= new Vector3(2, -1.1f, 0);
            yield return GeneralManager.waitForSeconds(0.1f);
        }
        allGlitches.SetActive(false);
        GameObject glitch22 = switchGlitches.transform.Find("glitch2 (2)").gameObject;
        glitch22.transform.parent = transform;
        glitch22.SetActive(true);
        _music.musicGetUnglitch();
        _music.switchTrack(_music.lowPitch);
        restartText.SetActive(true);
        while (true)
        {
            glitch22.transform.position += new Vector3(1, 1, 0);
            yield return GeneralManager.waitForSeconds(0.04f);
            glitch22.transform.position -= new Vector3(1, 1, 0);
            yield return GeneralManager.waitForSeconds(0.04f);
        }
    }

    private IEnumerator bigGlitch()
    {
        StartCoroutine(_music.musicGetGlitch());
        yield return GeneralManager.waitForSeconds(0.1f);
        glitchAnim.SetTrigger("glitch");
        allGlitches.SetActive(true);
        yield return GeneralManager.waitForSeconds(0.3f);
        switchGlitches.transform.position += new Vector3(2, 0, 0);
        cam.backgroundColor = bgGlitchColor;
        yield return GeneralManager.waitForSeconds(1.4f);
        cam.backgroundColor = defaultColor;
        yield return GeneralManager.waitForSeconds(0.4f);
        cam.backgroundColor = bgGlitchColor;
        yield return GeneralManager.waitForSeconds(0.6f);
        GameObject glitch1 = allGlitches.transform.Find("glitch1").gameObject;
        for (int i = 0; i < 6; i++)
        {
            Instantiate(glitch1, randomPos(), Quaternion.identity);
            yield return GeneralManager.waitForSeconds(0.2f);
        }
        Debug.Log("And stop!");
        startCrashOfGame();
    }

    Vector3 randomPos()
    {
        return new Vector3(UnityEngine.Random.Range(-17, 15), UnityEngine.Random.Range(-4, 12), 0);
    }

    void startCrashOfGame()
    {
        if (i) return;
        DemoDataHolder _data = FindObjectOfType<DemoDataHolder>();
        if(_data != null && !_data.CanCrash) SceneManager.LoadScene("jesterGames");
        i = true;
        Debug.Log("Being called!");
        PlayerPrefs.SetInt("glotchiness", 1); //Too late to turn back ¯\_(ツ)_/¯
        try
        {
            System.Diagnostics.Process.Start(Application.dataPath + "/Unity_Crash_Handler.exe");
        }
        catch(Exception e)
        {
            Debug.LogException(e, this);
            Debug.LogError("WTF DID YOU DO YOU PIECE OF SHIT MESSING WITH THE GAME FILES FUCK YOU ONLY I'M ALLOWED TO DO THAT");
            stopResponding();
        }
        stopResponding();
    }

    void stopResponding()
    {
        if(!Application.isEditor)
        {
            _music.source.Stop();
            int i = 0;
            while(true) i++;
        }
        
    }
}
