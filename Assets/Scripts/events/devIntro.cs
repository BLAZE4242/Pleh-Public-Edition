using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class devIntro : MonoBehaviour
{
    [SerializeField] Material glitchMat;
    [SerializeField] float glitchScrollSpeed;
    [SerializeField] PostProcessVolume _post;
    [SerializeField] Image blackImage;
    [SerializeField] GameObject jesterImage;
    DepthOfField _dof;
    LensDistortion _lensDistort;
    bool canScrollGlitch = true;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<DRP>().RP("Error: Cannot locate player");
        GeneralManager.SetSceneSave();

        _post.profile.TryGetSettings(out _dof);
        _post.profile.TryGetSettings(out _lensDistort);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        source = GetComponent<AudioSource>();
        StartCoroutine(devIntroSequence());
    }

    void Update()
    {
        if(canScrollGlitch)
        {
            glitchMat.mainTextureOffset = new Vector2(0,  glitchMat.mainTextureOffset.y + Time.deltaTime * glitchScrollSpeed);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            onTriggerGlitchClear();
        }
    }

    IEnumerator devIntroSequence()
    {
        LerpSolution.lerpCamSize(Camera.main, 5, 1, 1);
        yield return GeneralManager.waitForSeconds(4);
        LerpSolution.lerpDOFdistance(_dof, 0, 1);
        while(_dof.focusDistance != 0) yield return new WaitForEndOfFrame();
        yield return GeneralManager.waitForSeconds(1f);
        LerpSolution.lerpDOFdistance(_dof, 10, 1);
    }

    public void onTriggerGlitchClear()
    {
        LerpSolution.lerpDOFdistance(_dof, 0, 2f);
        LerpSolution.lerpLensDistortion(_lensDistort, -100, 2f);
        LerpSolution.lerpImageColour(blackImage, Color.black, 3f);
    }

    public void showJesterSorry()
    {
        jesterImage.SetActive(true);
    }

    public void restartGame()
    {
        screenConsole _console = FindObjectOfType<screenConsole>();
        UnityEvent endEvent = new UnityEvent();
        endEvent.AddListener(startToRestartGame);
        _console.writeConsole("Application.Restart();", "Loading SceneIndex 0: jesterGames", endEvent);
    }

    void startToRestartGame()
    {
        StartCoroutine(startToRestartGameEnum());
    }

    private IEnumerator startToRestartGameEnum()
    {
        yield return GeneralManager.waitForSeconds(1);
        PlayerPrefs.SetInt("glotchiness", 2);
        SceneManager.LoadScene("jesterGames");
    }
}
