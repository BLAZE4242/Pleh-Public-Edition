using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollUV : MonoBehaviour
{
	[SerializeField] bool ThereCanOnlyBeOne;
	public bool isScrollingBackwards, canMove = true;
	[SerializeField] bool isDemoStarField = false;
	[SerializeField] playerControllerMessages _player;
	[SerializeField] TextMeshProUGUI startText;
	[SerializeField] Transform logoTransform, endPos;
	[SerializeField] AudioClip plehThemeEnd;
	[SerializeField] RawImage blackScreen;

	MeshRenderer mr;
    float multi = 1f;
	float downSpeed;
	Material mat;

    void Awake()
	{
        if (ThereCanOnlyBeOne)
        {
            ScrollUV[] scrollUVs = FindObjectsOfType<ScrollUV>();
            if(scrollUVs.Length > 1)
			{
				Destroy(gameObject);
			}
        }
	}

	void OnLevelWasLoaded()
    {
		if (SceneManager.GetActiveScene().name == "devIntro" || !FindObjectOfType<StarfieldDontDestroy>()) Destroy(gameObject);
		else Debug.Log(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
		Debug.Log(PlayerPrefs.GetInt("glotchiness"));
		if(PlayerPrefs.GetInt("glotchiness") != 1 && SceneManager.GetActiveScene().buildIndex != 0)
		{
			DontDestroyOnLoad(gameObject);
		}
        mr = GetComponent<MeshRenderer>();
		mat = mr.material;
		StartCoroutine(scroll());
    }

	void OnLevelWasLoaded(int level)
	{
		if(level == 1 && !isDemoStarField)
		{
			Destroy(gameObject);
		}
		
		if(FindObjectOfType<infoFromScene>().messageFromScene != "scroll down")
		{
			canMove = true;
			StopAllCoroutines();
			StartCoroutine(scroll());
		}
	}

    public IEnumerator scroll()
    {
		while(canMove){

			while (!isScrollingBackwards && canMove)
			{
				//if(mat.mainTextureOffset.x >= 1.65f) canMove = false; why is this here???
                if (_player != null) multi = _player.playerSpeed / 2;

                mr = GetComponent<MeshRenderer>();

				Vector2 offset = mat.mainTextureOffset;

				offset.x += Time.deltaTime / 20f * multi;

				mat.mainTextureOffset = offset;
				yield return new WaitForEndOfFrame();
			}

			while (isScrollingBackwards && canMove)
			{
                if (_player != null) multi = _player.playerSpeed / 2;

				MeshRenderer mr = GetComponent<MeshRenderer>();

				Vector2 offset = mat.mainTextureOffset;

				offset.x -= Time.deltaTime / 20f * multi;

				mat.mainTextureOffset = offset;
				yield return new WaitForEndOfFrame();
			}
        
		}
	}

	public IEnumerator movedown(bool loadNull = false)
	{
        LerpSolution.lerpTextColour(startText, Color.clear, 1);
		while(startText.color.a > 0.4f) yield return new WaitForEndOfFrame();
		StartCoroutine(lerpValue(0));
		StartCoroutine(lerpDownSpeed(loadNull));
		StartCoroutine(actuallyMoveDown());
	}

	IEnumerator actuallyMoveDown()
	{
        while (true)
        {
            mr = GetComponent<MeshRenderer>();

            //mr.material = blured;
            //mat = blured;
            Vector2 offset = mat.mainTextureOffset;

            offset.y += Time.deltaTime * downSpeed;

            mat.mainTextureOffset = offset;
            yield return new WaitForEndOfFrame();
        }
	}

	IEnumerator lerpValue(float targetMulti)
	{
        float dist = 0;
        float currentMulti = multi;
        while (dist < 1)
        {
            dist += Time.deltaTime * 0.6f;
            multi = Mathf.Lerp(currentMulti, targetMulti, dist);
            yield return new WaitForEndOfFrame();
        }
	}

	IEnumerator lerpDownSpeed(bool loadNull = false)
	{
        float dist = 0;
        float currentDownSpeed = 2;
		Vector3 currentLogoPos = logoTransform.position;
		logoTransform.GetComponent<Animator>().enabled = false;
        LerpSolution.lerpPosition(logoTransform, endPos.position, 0.9f);
        while (dist < 1)
        {
            dist += Time.deltaTime * 0.4f;
            downSpeed = Mathf.Lerp(0, currentDownSpeed, dist);
			mr.material.SetFloat("_Blend", Mathf.Lerp(0, 1, dist));
			logoTransform.position = Vector3.Lerp(currentLogoPos, endPos.position, downSpeed);
            yield return new WaitForEndOfFrame();
        }
		yield return GeneralManager.waitForSeconds(2);
		infoFromScene _infoFromScene = FindObjectOfType<infoFromScene>();
		if (PlayerPrefs.GetInt("glotchiness") == 5)
        {
			_infoFromScene.messageFromScene = "scroll down";
			SceneManager.LoadSceneAsync("Level 1_G");
		}
		else if(PlayerPrefs.GetInt("glotchiness") == 7)
        {
			GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().loop = false;
			yield return new WaitUntil(() => !GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().isPlaying);
			GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().clip = plehThemeEnd;
			GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();
			blackScreen.color = Color.black;
			yield return GeneralManager.waitForSeconds(1);

			Gas.EarnAchievement("ACH_COMPLETE"); // that's it. my god. im done.
			yield return GeneralManager.waitForSeconds(4);
			PlayerPrefs.SetInt("glotchiness", 8);
			SceneManager.LoadSceneAsync("hub_main");
		}
		else if (!loadNull)
        {
			_infoFromScene.messageFromScene = "scroll down";
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
		else
        {
			foreach (AudioSource source in FindObjectsOfType<AudioSource>())
            {
				Destroy(source.gameObject);
            }

			_infoFromScene.messageFromScene = "";
			SceneManager.LoadSceneAsync("null");
        }
	}

	public IEnumerator reachTarget(float targetMulti = 2, bool canInvertCamStatus = false)
	{
		StartCoroutine(actuallyMoveDown());
        float dist = 0;
		float currentDownSpeed = 1;
		bool recursion = false;
        while (dist < 1)
        {
            dist += Time.deltaTime * 0.4f;
            downSpeed = Mathf.Lerp(currentDownSpeed, 0, dist);
            mr.material.SetFloat("_Blend", Mathf.Lerp(1, 0, dist));
			if(!recursion && dist > 0.4f)
			{
                StartCoroutine(lerpValue(targetMulti));
				recursion = true;
			}
            yield return new WaitForEndOfFrame();
        }

		transform.SetParent(null);
		if(canInvertCamStatus) FindObjectOfType<playerMove>().canCamSwapScene = !FindObjectOfType<playerMove>().canCamSwapScene;
	}
}
