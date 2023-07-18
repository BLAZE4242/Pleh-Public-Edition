using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class fileExplorer_se : MonoBehaviour // se for secret :shush:
{
    [SerializeField] GameObject texts;
    [SerializeField] VideoClip clip;
    [SerializeField] GameObject pause;
    VideoPlayer player;
    AnalogGlitch analog;
    DigitalGlitch digital;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Instantiate(pause);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = FindObjectOfType<VideoPlayer>();
        analog = FindObjectOfType<AnalogGlitch>();
        digital = FindObjectOfType<DigitalGlitch>();

        GetComponent<fileExplorer_inputManager>().canType = false;
        texts.SetActive(false);

        analog.scanLineJitter = 0;
        analog.verticalJump = 0;
        analog.horizontalShake = 0;
        analog.colorDrift = 0;
        player.renderMode = VideoRenderMode.RenderTexture;
        player.clip = null;

        yield return GeneralManager.waitForSeconds(3);

        player.clip = clip;
        player.time = 10f;
        player.GetComponent<AudioSource>().volume = 0.9f;
        player.Play();

        yield return new WaitUntil(() => player.isPrepared);
        yield return GeneralManager.waitForSeconds(0.1f);
        player.renderMode = VideoRenderMode.CameraFarPlane;
        yield return GeneralManager.waitForSeconds(12.19f);

        analog.scanLineJitter = 0.05f;
        analog.verticalJump = 0;
        analog.horizontalShake = 0;
        analog.colorDrift = 0.03f;

        yield return GeneralManager.waitForSeconds(6.75f);

        digital.intensity = 0.06f;

        yield return GeneralManager.waitForSeconds(2.06f);

        digital.intensity = 0f;

        yield return GeneralManager.waitForSeconds(1);

        StartCoroutine(turnDownAnalog());

        yield return new WaitUntil(() => !player.isPlaying && Time.timeScale != 0);
        player.enabled = false;
        yield return GeneralManager.waitForSeconds(3);

        PlayerPrefs.SetString("thats it", "glotchiness");
        SceneManager.LoadScene("New Messages");
    }

    IEnumerator turnDownAnalog()
    {
        while (analog.scanLineJitter != 0 && analog.colorDrift != 0)
        {
            analog.scanLineJitter -= 1 * 0.02f * Time.deltaTime;
            analog.colorDrift -= 1 * 0.02f * Time.deltaTime;

            analog.scanLineJitter = Mathf.Clamp(analog.scanLineJitter, 0, 1);
            analog.colorDrift = Mathf.Clamp(analog.scanLineJitter, 0, 1);
            yield return GeneralManager.waitForEndOfFrame;
        }

        analog.enabled = false;
    }
}
