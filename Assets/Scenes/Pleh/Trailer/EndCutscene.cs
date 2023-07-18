using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class EndCutscene : MonoBehaviour
{
    [SerializeField] GameObject[] pieces;
    [SerializeField] GameObject Text;
    [SerializeField] GameObject Door;
    [SerializeField] float downwardsForce = 10f;
    Vignette vig;

    List<string> scriptList = new List<string>();

    // Start is called before the first frame update
    IEnumerator Start()
    {
        vig = FindObjectOfType<PostProcessVolume>().profile.GetSetting<Vignette>();
        script();
        while(true)
        {
            for(int i = 0; i < 7; i++)
            {
                Rigidbody rb = Instantiate(pieces[Random.Range(0, pieces.Length - 1)], spawnPos(), randRot()).AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.AddForce(Vector3.down * Random.Range(300, 1000));
            }
            
            yield return GeneralManager.waitForSeconds(0.3f);
        }

    }

    Vector3 spawnPos(bool isText = false, bool isCenter = false)
    {
        if(isCenter) return new Vector3(0.1f, transform.position.y + 30, transform.position.z);

        else if(!isText) return new Vector3(Random.Range(-13, 13), transform.position.y + 30, transform.position.z - Random.Range(-9, 9));
        else return new Vector3(Random.Range(-6, 6), transform.position.y + 30, transform.position.z - Random.Range(-5, 5));
    }

    Quaternion randRot()
    {
        return Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

    void script()
    {
        say("Come, let me show you what he's hiding.");
        say("Isn't that only fair?");
        say("Isn't that what you want?");
        say("All you need to do is come with me.");
        say("I can show you EVERYTHING.");
        say("EVERYTHING");
        StartCoroutine(startScript());
    }


    void say(string message)
    {
        scriptList.Add(message);
    }

    IEnumerator startScript()
    {
        yield return GeneralManager.waitForSeconds(4);
        foreach(string line in scriptList)
        {
            TextMeshPro spawnedText = null;
            if(line == "EVERYTHING")
            {
                spawnedText = Instantiate(Text, spawnPos(true, true), Quaternion.identity).GetComponent<TextMeshPro>();
                spawnedText.fontSize += 7;
            }
            else
            {
                spawnedText = Instantiate(Text, spawnPos(true), Quaternion.identity).GetComponent<TextMeshPro>();
            }
            LerpSolution.lerpTextColour(spawnedText, spawnedText.color, 0.8f, Color.clear);
            Rigidbody rb = spawnedText.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.AddForce(Vector3.down * 600);
            spawnedText.text = line;
            StartCoroutine(rotateText(spawnedText.transform));
            if(line != "EVERYTHING") yield return GeneralManager.waitForSeconds(4);
            else
            {
                yield return GeneralManager.waitForSeconds(1);
                GameObject spawnedDoor = Instantiate(Door, transform.position + new Vector3(0, 30, 2), Quaternion.Euler(-90.947f, -102.351f, 99.351f));
                spawnedDoor.AddComponent<Rigidbody>().useGravity = false;
                spawnedDoor.GetComponent<Rigidbody>().AddForce(Vector3.down * 460);
                LerpSolution.lerpVignetteIntensity(vig, 0.457f, 0.4f);
                CameraShake.Shake(3, 0.1f);
                Camera.main.GetComponent<Animator>().SetTrigger("glitch");
            }
        }
    }

    IEnumerator rotateText(Transform textTrans)
    {
        while(true)
        {
            textTrans.rotation = Quaternion.LookRotation(Camera.main.transform.position) * Quaternion.Euler(-90, 180, 0);
            yield return new WaitForEndOfFrame();
        }
    }
}
