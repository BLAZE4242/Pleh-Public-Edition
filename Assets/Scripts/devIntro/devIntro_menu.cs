using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
// using Aura2API; // AURA HERE

public class devIntro_menu : MonoBehaviour
{
    [SerializeField] VoiceLine dev_menu_01;
    [SerializeField] TMP_FontAsset evilFont;
    [SerializeField] Camera cam;
    [SerializeField] Color redColour;
    VoiceLineManager _vlManager;
    glitchText _gText;
    menu _menu;
    Animator camAnim;
    music _music;
    bool hasAlreadyPressedH = false;
    bool canLoadIntoPhone = false;
    AsyncOperation sceneLoadOp;

    private void Awake()
    {

    }

    private void Start()
    {
        _vlManager = FindObjectOfType<VoiceLineManager>();
        _gText = GetComponent<glitchText>();
        _menu = GetComponent<menu>();
    }

    public void OnKnock() { StartCoroutine(OnKnockEnum()); }

    IEnumerator OnKnockEnum()
    {
        _music.source.Stop();
        cam.fieldOfView /= 1.4f;
        CameraShake.Shake(0.8f, 0.1f);
        yield return GeneralManager.waitForSeconds(0.8f);
        cam.fieldOfView *= 1.4f;
    }

    public IEnumerator playerTryToPlay(AudioSource _source, music tempMusic, Animator tempCamAnim)
    {
        if (!hasAlreadyPressedH)
        {
            hasAlreadyPressedH = true;

            _music = tempMusic;
            camAnim = tempCamAnim;

            yield return GeneralManager.waitForSeconds(2f);
            LerpSolution.lerpVolume(_source, _source.volume - 0.6f, 0.5f);
            _vlManager.PlayLine(dev_menu_01);
        }
        else if(canLoadIntoPhone)
        {
            _music.staticGlitch();
            //_music.source.Stop();
            //GameObject.Find("Black Screen").GetComponent<RawImage>().color = Color.black;
            Destroy(_music.gameObject);
            Destroy(_menu._scrollUV.gameObject);
            sceneLoadOp.allowSceneActivation = true;

            //StartCoroutine(_gText.createGlitchedText("Okay so actually breaking the fourth wall here (this is out of character) this part of the game is broken so we're gonna skip it :p", _menu.startText));
            //yield return GeneralManager.waitForSeconds(6);
            //FindObjectOfType<infoFromScene>().messageFromScene = "messages";
            //FindObjectOfType<music>().Destroy();
            //PlayerPrefs.SetInt("glotchiness", 3);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void onDevFinishTalking()
    {
        StartCoroutine(onDevFinishTalkingEnum());
    }

    IEnumerator onDevFinishTalkingEnum()
    {
        LerpSolution.lerpVolume(_music.source, _music.source.volume + 0.6f, 1);
        yield return GeneralManager.waitForSeconds(3f);
        //camAnim.SetInteger("glitchMenuNum", 3);
        //_music.staticGlitch();
        //StartCoroutine(_music.musicGetGlitch(7));
        //yield return GeneralManager.waitForSeconds(0.4f);
        //_menu.threedcam.GetComponent<PostProcessVolume>().enabled = true;
        //camAnim.SetInteger("glitchMenuNum", 1);
        //camAnim.SetInteger("glitchMenuNum", 2); // doesn't work rn
        //_music.switchTrack(_music.lowPitch);
        //_menu.startText.font = evilFont;
        //_music.loopSfx(_music.longStatic, 0.4f);
        //LerpSolution.LerpLightIntensity(GameObject.Find("lmao").GetComponent<Light>(), 1.84f, 0.6f);
        //for (int i = 0; i < 100; i++)
        //{
        //    if (i == 30)
        //    {
        //        StartCoroutine(_gText.createGlitchedText("Can you hear me?", _menu.startText));
        //    }
        //    else if(i == 50)
        //    {
        //        LerpSolution.lerpTextColour(_menu.startText, FindObjectOfType<lookAwayJumpScare>().GetComponent<TextMeshPro>().color, 4f);
        //    }

        //    _menu.startText.transform.position += new Vector3(4, 3);
        //    yield return GeneralManager.waitForSeconds(0.01f);
        //    _menu.startText.transform.position -= new Vector3(4, 3);
        //    yield return GeneralManager.waitForSeconds(0.01f);
        //}

        sceneLoadOp = SceneManager.LoadSceneAsync("dox");
        sceneLoadOp.allowSceneActivation = false;
        StartCoroutine(_gText.createGlitchedText("Is he gone yet?", _menu.startText));
        yield return GeneralManager.waitForSeconds(0.5f);
        _menu.startText.font = evilFont;
        _menu.startText.color = redColour;
        yield return GeneralManager.waitForSeconds(2.5f);
        StartCoroutine(_gText.createGlitchedText("I thought he would never shut up.", _menu.startText));
        yield return GeneralManager.waitForSeconds(4f);
        StartCoroutine(_gText.createGlitchedText("Follow me.", _menu.startText));
        yield return GeneralManager.waitForSeconds(3f);

        StartCoroutine(_gText.createGlitchedText("Press H to begin", _menu.startText));
        canLoadIntoPhone = true;
    }
}
