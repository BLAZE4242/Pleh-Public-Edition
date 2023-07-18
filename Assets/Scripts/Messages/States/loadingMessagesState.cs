using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class loadingMessagesState : messagesState
{
    bool hasBeenRun;
    bool isDone;
    public menuMessagesState menuState;
    [SerializeField] string[] loadingMessages;
    [SerializeField] float[] timeIndex;
    [SerializeField] TextMeshProUGUI loadingText, percentageText;
    [SerializeField] Slider loadingBar;
    [SerializeField] AudioClip introDialouge;
    float loadPercent = 0f;
    AudioSource source;

    void startCurrentState()
    {
        if(hasBeenRun) return;
        source = GetComponent<AudioSource>();
        StartCoroutine(loadingSequence());
        hasBeenRun = true;
    }

    public override messagesState runCurrentState()
    {
        startCurrentState();
        loadingBar.value = loadPercent / 100;
        if(isDone)
            return menuState;
        else
            return this;
    }

    public IEnumerator loadingSequence()
    {
        //I don't want to talk about this. WHY CANT YOU SERIALIZE DICTIONARY'S UNITY???
        int iteration = -1;
        foreach (string loadingMessage in loadingMessages)
        {
            iteration++;
            loadingText.text = loadingMessage;
            StartCoroutine(percentLoading(loadPercent + (100 / loadingMessages.Length)));
            if(timeIndex[iteration] == 0)
                yield return GeneralManager.waitForSeconds(introDialouge.length / loadingMessages.Length);
            else
                yield return GeneralManager.waitForSeconds(timeIndex[iteration]);
        }
        StartCoroutine(percentLoading(100));
    }

    IEnumerator percentLoading(float targetPercentage)
    {
        float percentSpeed = Random.Range(0.4f, 0.9f);
        float travelPercent = 0f;
        float currentPercent = loadPercent;
        while (travelPercent < 1)
        {
            travelPercent += Time.deltaTime * percentSpeed;
            loadPercent = Mathf.Lerp(currentPercent, targetPercentage, travelPercent);
            percentageText.text = loadPercent.ToString("0") + "%";
            yield return new WaitForEndOfFrame();
        }
        if (targetPercentage == 100)
        {
            yield return GeneralManager.waitForSeconds(1);
            isDone = true;
        }
    }
}
