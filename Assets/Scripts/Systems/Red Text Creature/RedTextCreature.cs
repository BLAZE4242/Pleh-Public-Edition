using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedTextCreature : MonoBehaviour
{
    [SerializeField] string[] BadWords;
    [SerializeField] GameObject Text;
    [SerializeField] float increasedSize = 10f;
    public AudioClip In, Out;
    public AudioSource source;
    [Header("Spawn Settings")]
    public float unitSphereSize = 15f;
    public int wordCount = 100;
    public float wordSizeMulti = 1f;
    bool canBeat = true;

    [HideInInspector] public List<TextMeshPro> redTexts = new List<TextMeshPro>();
    Light redLight;

    // Start is called before the first frame update
    void Start()
    {
        redLight = GetComponent<Light>();

        for(int i = 0; i < wordCount; i++)
        {
            SpawnText(false);
        }

        StartCoroutine(heartBeat());
    }

    void SpawnText(bool fadeSpawn)
    {
        TextMeshPro text = Instantiate(Text, transform, true).GetComponent<TextMeshPro>();
        
        if (fadeSpawn)
        {
            LerpSolution.lerpTextColour(text, text.color, 0.6f, Color.clear);
        }

        text.transform.position = RandPosition();
        text.transform.localScale = text.transform.localScale * wordSizeMulti;
        text.text = BadWords[Random.Range(0, BadWords.Length - 1)];
        text.GetComponent<RedTextIndividual>().host = this;
        redTexts.Add(text);
    }

    IEnumerator heartBeat()
    {
        while(true)
        {
            foreach(TextMeshPro textToBeat in redTexts)
            {
                StartCoroutine(textToBeat.GetComponent<RedTextIndividual>().beatText());
            }

            LerpSolution.LerpLightRange(redLight, redLight.range + increasedSize, 4);
            if (source != null && canBeat) source.PlayOneShot(In);
            yield return GeneralManager.waitForSeconds(0.3f);

            StartCoroutine(redTexts[Random.Range(0, redTexts.Count-1)].GetComponent<RedTextIndividual>().fallText());

            if (source != null && canBeat) source.PlayOneShot(Out);
            LerpSolution.LerpLightRange(redLight, redLight.range - increasedSize, 4);
            yield return GeneralManager.waitForSeconds(1f);

            SpawnText(true);
        }
    }

    public void ChangeBeatValue(bool isActive) { canBeat = isActive; }

    public Vector3 RandPosition()
    {
        return transform.position + Random.insideUnitSphere * unitSphereSize;
    }
}
