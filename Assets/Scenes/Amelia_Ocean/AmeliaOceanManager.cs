using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.SceneManagement;
//using Aura2API; // AURA HERE
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AmeliaOceanManager : MonoBehaviour
{
    [HideInInspector] public bool IsQuizing = false;
    [HideInInspector] public bool CanShowAhead = true;
    [Header("Idle state")]
    [SerializeField] AudioSource waves;
    [SerializeField] Transform PlayerSpawnPos;
    [SerializeField] TextMeshPro[] redTexts;
    [SerializeField] SpriteRenderer blackScreen;
    [Header("Quizing state")]
    [SerializeField] GameObject LookAheadObject;
    [SerializeField] MeshRenderer LookAheadTrigger;
    [SerializeField] Transform PlayerQuizingPos;
    [SerializeField] PostProcessVolume darkVolume;
    [Header("Reflection state")]
    [SerializeField] TextMeshPro Amelia;
    //[SerializeField] AuraLight ameliaLight; // AURA HERE
    [SerializeField] Light ameliaLight; // AURA NOT HERE
    [SerializeField] AudioClip click1;

    Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        
        if (PlayerPrefs.GetInt("glotchiness") != 7)
        {
            GeneralManager.SetSceneSave();
            FindObjectOfType<DRP>().RP("IT did this.");
        }
        else
        {
            StartCoroutine(ReflectionState());
            return;
        }

        if (GameObject.FindGameObjectsWithTag("Music").Length > 1)
        {
            Destroy(waves.gameObject);
        }

        if (IsQuizing) GetComponent<AmeliaOceanQuizing>().SetScript();
        foreach (TextMeshPro redText in redTexts)
        {
            LerpSolution.lerpTextColour(redText, Color.clear, 0.2f);
        }
        LerpSolution.lerpSpriteColour(blackScreen, Color.clear, 0.09f);
    }

    IEnumerator ReflectionState()
    {
        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("Amelia_Restaurant");
        sceneOp.allowSceneActivation = false;

        foreach (TextMeshPro redText in redTexts)
        {
            redText.color = Color.clear;
        }
        blackScreen.color = Color.clear;
        player.GetComponent<PlayerController>().lockMovement = true;
        player.GetComponent<PlayerController>().lockLook = true;
        waves.Stop();
        LerpSolution.LerpLightIntensity(ameliaLight, 0, 0.17f);

        Amelia.text = "What happened has happened, there is no escape back to that time.";
        waves.PlayOneShot(click1);
        yield return GeneralManager.waitForSeconds(4);
        Amelia.text = "We need to move on from these events.";
        waves.PlayOneShot(click1);
        yield return GeneralManager.waitForSeconds(4);
        sceneOp.allowSceneActivation = true;
    }

    public void SetState(bool _IsQuizing)
    {
        FindObjectOfType<PlayerController>().lockGravity = _IsQuizing;
        FindObjectOfType<PlayerController>().checkForMovement = !_IsQuizing;

        if (!Application.isPlaying)
        {
            FindObjectOfType<PlayerController>().transform.position = _IsQuizing ? PlayerQuizingPos.position : PlayerSpawnPos.position;
        }
        else
        {
            FindObjectOfType<PlayerController>().TeleportPlayer(_IsQuizing ? PlayerQuizingPos.position : PlayerSpawnPos.position);
        }




        LookAheadObject.SetActive(_IsQuizing);

        if (_IsQuizing && Application.isPlaying)
        {
            GetComponent<AmeliaOceanQuizing>().SetScript();
            FindObjectOfType<Transcript>().gameObject.SetActive(false);
        }

        IsQuizing = _IsQuizing;
    }

    bool hasTextBeenGlitched = false;
    private void Update()
    {
        if (IsQuizing)
        {
            // -176
            darkVolume.weight = Mathf.Abs(player.rotation.y / 1.8f);

            if (!hasTextBeenGlitched && LookAheadTrigger.IsVisibleFrom(player.GetComponentInChildren<Camera>()))
            {
                hasTextBeenGlitched = true;
                StartCoroutine(GlitchAmelia());
            }
        }
    }

    private IEnumerator GlitchAmelia()
    {
        if (CanShowAhead)
        {
            TextMeshPro ameliaText = LookAheadObject.GetComponentInChildren<TextMeshPro>();
            yield return GeneralManager.waitForSeconds(0.4f);
            StartCoroutine(GetComponent<glitchText>().createGlitchedText("Look ahead", LookAheadObject.GetComponentInChildren<TextMeshPro>()));
        }
    }

    public void NotAgain(TextMeshPro ameliaText)
    {
        GameObject.Find("SFX").GetComponent<AudioSource>().PlayOneShot(GetComponent<quizDialogueManager>().click1, 4);
        ameliaText.text = "Not like this again...";
    }

    public void End()
    {
        SceneManager.MoveGameObjectToScene(FindObjectOfType<AudioLowPassFilter>().gameObject, SceneManager.GetActiveScene());
        GetComponent<quizEvents>().sceneOp.allowSceneActivation = true;
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(AmeliaOceanManager))]
    public class AmeliaOceanManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AmeliaOceanManager _manager = (AmeliaOceanManager)target;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set to idle state"))
            {
                _manager.SetState(false);
            }
            else if (GUILayout.Button("Set to quizing state"))
            {
                _manager.SetState(true);
            }
            GUILayout.EndHorizontal();
        }
    }
#endif
}
