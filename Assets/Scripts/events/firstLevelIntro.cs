using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using TMPro;

public class firstLevelIntro : MonoBehaviour
{
    [SerializeField] bool glitchForArg;
    [SerializeField] TextMeshPro weee;
    [SerializeField] PostProcessVolume volume;
    [SerializeField] AudioClip metal;
    public Animator cameraAnim, conveyerAnim;
    [SerializeField] GameObject cubes, onFloorCubes;
    [SerializeField] Rigidbody2D vent;
    [SerializeField] Transform beginPos;
    AudioLowPassFilter lowPass;
    public RuntimeAnimatorController glitchAnim, cameraFirstLevel;
    bool isGlitchLevel = false;
    PlehGlitchManager glitchManager;
    
    [Header("For Editor Cutscene")]
    [SerializeField] SpriteRenderer plutoSpriteRenderer;
    [SerializeField] Transform plutoSpawnPos, plutoNormalSpawnPos;
    Vector3 plutoDefaultScale;
    playerMove _move;
    Rigidbody2D _playerRb;
    bool canFocusOnPluto;

    IEnumerator Start()
    {
        if (SceneManager.GetActiveScene().name == "Level 1_G")
        {
            FindObjectOfType<DRP>().RP("exPloOo0Tring anN eMPty SpaC#3ShIIp");
        }
        else FindObjectOfType<DRP>().RP("Exploring an empty spaceship");

        glitchManager = FindObjectOfType<PlehGlitchManager>();
        isGlitchLevel = glitchManager != null;
        

        LerpSolution.LerpPPweight(volume, 1, 0.08f, 0);
        try
        {
            _move = FindObjectOfType<playerMove>();
            _playerRb = _move.GetComponent<Rigidbody2D>();
            plutoDefaultScale = _move.transform.lossyScale;
            if(plutoSpriteRenderer.transform.position == plutoSpawnPos.position) StartCoroutine(firstAnim());
        }
        catch
        {

        }

        yield return GeneralManager.waitForSeconds(0.6f);
        lowPass = FindObjectOfType<AudioLowPassFilter>();
        LerpSolution.lerpLowPass(lowPass, 1275, 1);
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.N))
        //{
        //    StartCoroutine(loadNextLevel());
        //} IMAGINE IF I LEFT THIS IN

        if (cameraAnim.transform.position == beginPos.position)
        {
            FindObjectOfType<playerMove>().canCamSwapLeft = false;
        }
        else if(isGlitchLevel && !glitchManager.overrideCamSwap)
        {
            FindObjectOfType<playerMove>().canCamSwapLeft = true;
        }
    }

    IEnumerator loadNextLevel()
    {
        _move.fadeAnim.SetTrigger("fadeOut");
        _move._infoFromScene.messageFromScene = "fade in";
        _move.lockRestart = true;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        LerpSolution.lerpLowPass(FindObjectOfType<AudioLowPassFilter>(), 6107, 1);
        yield return GeneralManager.waitForSeconds(2.4f);
        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator firstAnim()
    {
        _move.lockRestart = true;
        _move.canCamSwapScene = false;
        yield return GeneralManager.waitForSeconds(3);
        cameraAnim.enabled = true;
        cameraAnim.SetTrigger("Camera Zoom Trigger");
        yield return GeneralManager.waitForSeconds(0.4f);
        GetComponent<AudioSource>().clip = metal;
        GetComponent<AudioSource>().Play();
        conveyerAnim.SetTrigger("Shake Trigger");
    }

    public void OpenVent()
    {
        StartCoroutine(OpenVentEnum());
    }

    IEnumerator OpenVentEnum()
    {
        vent.transform.SetParent(null);
        GameObject InstantiatedCubes = Instantiate(cubes);
        vent.constraints = RigidbodyConstraints2D.None;
        yield return GeneralManager.waitForSeconds(1.5f);
        vent.GetComponent<HingeJoint2D>().enabled = false;

        if (glitchForArg)
        {
            weee.gameObject.SetActive(false);

            yield return GeneralManager.waitForSeconds(0.5f);
            vent.GetComponent<HingeJoint2D>().enabled = true;
            foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
            {
                rb.gravityScale = 0;
                rb.AddForce(Vector3.up * 4);
            }
        }

        yield return GeneralManager.waitForSeconds(2);

        Destroy(InstantiatedCubes);
        Destroy(vent.gameObject);
        Instantiate(onFloorCubes);
        _move.transform.position = plutoSpawnPos.position;
        _move.GetComponent<SpriteRenderer>().enabled = true;
        _move.transform.SetParent(null);
        //_move.transform.localScale = plutoDefaultScale;
        _playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return GeneralManager.waitForSeconds(0.4f);


        //cameraAnim.runtimeAnimatorController = null;
        cameraAnim.enabled = false;
        canFocusOnPluto = true;
        yield return GeneralManager.waitForSeconds(3);
        canFocusOnPluto = false;
        LerpSolution.lerpCamSize(Camera.main, 10, 5);
        LerpSolution.lerpPosition(cameraAnim.transform, _move.camEndPos.position, 7);
        if(!isGlitchLevel) _move.lockRestart = false;
        _move.canControl = true;
        _move.canCamSwapScene = true;

        yield return GeneralManager.waitForSeconds(2);

        cameraAnim.runtimeAnimatorController = glitchAnim;
        cameraAnim.enabled = true;
    }

    void FixedUpdate()
    {
        if(canFocusOnPluto)
        {
            Transform cam = Camera.main.transform;
            Vector3 targetPos = new Vector3(cam.position.x, _move.transform.position.y, cam.position.z);
            LerpSolution.lerpPosition(cam, targetPos, 7);
        }
    }

    public void MakeMusicLoud()
    {
        LerpSolution.lerpLowPass(lowPass, 6107, 0.2f);
    }

    public void StopAnimationForTranscript(Animator animator)
    {
        animator.enabled = false;
    }

    public void BeginAnimationForTranscript(Animator animator)
    {
        animator.enabled = true;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(firstLevelIntro))]
    public class firstLevelIntroEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            firstLevelIntro _firstLevelIntro = (firstLevelIntro)target;

            GUILayout.Space(10);
            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16};
            EditorGUILayout.LabelField("Warning: Will Save (actually nvm it doesn't work lol)", style);
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Activate Cutscene"))
            {
                _firstLevelIntro.plutoSpriteRenderer.enabled = false;
                _firstLevelIntro.plutoSpriteRenderer.transform.position = _firstLevelIntro.plutoSpawnPos.position;
                _firstLevelIntro.plutoSpriteRenderer.transform.SetParent(_firstLevelIntro.transform);
                _firstLevelIntro.plutoSpriteRenderer.GetComponent<playerMove>().canControl = false;
                _firstLevelIntro.plutoSpriteRenderer.GetComponent<playerMove>().canCamSwapScene = false;
                FindObjectOfType<infoFromScene>().messageFromScene = "scroll down";
                _firstLevelIntro.cameraAnim.runtimeAnimatorController = _firstLevelIntro.cameraFirstLevel;
                _firstLevelIntro.cameraAnim.enabled = false;
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
            if(GUILayout.Button("Deactivate Cutscene"))
            {
                _firstLevelIntro.plutoSpriteRenderer.enabled = true;
                _firstLevelIntro.plutoSpriteRenderer.transform.position = _firstLevelIntro.plutoNormalSpawnPos.position;
                _firstLevelIntro.plutoSpriteRenderer.transform.SetParent(null);
                _firstLevelIntro.plutoSpriteRenderer.GetComponent<playerMove>().canControl = true;
                _firstLevelIntro.plutoSpriteRenderer.GetComponent<playerMove>().canCamSwapScene = true;
                FindObjectOfType<infoFromScene>().messageFromScene = "fade in";
                _firstLevelIntro.cameraAnim.runtimeAnimatorController = _firstLevelIntro.glitchAnim;
                _firstLevelIntro.cameraAnim.enabled = true;
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }

            GUILayout.EndHorizontal();
        }
    }
    #endif

}
