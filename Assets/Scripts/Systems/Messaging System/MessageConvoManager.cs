using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MessageConvoManager : MonoBehaviour
{
    public enum MessageConvoState { Idle, AmeliaTexting, PlayerChoosing }
    public MessageConvoState currentMessagingState;
    public enum AmeliaTypeState { Idle, Typing}
    public AmeliaTypeState currentAmeliaTypeState;
    public enum AmeliaStoryState { Flirt, Confess}
    public static AmeliaStoryState currentStoryState;
    public MessageConvo currentConvo;
    [SerializeField] MessageConvo confessConvo;
    [SerializeField] MessageConvo reflectionConvo;
    [SerializeField] MessageConvo se;

    [SerializeField] TextMeshPro typingText;
    Coroutine lerpSolutionTextTine;
    [Header("Instantiating")]
    [SerializeField] AudioSource SFX;
    [SerializeField] AudioClip msgSfx;
    [SerializeField] GameObject messageParent;
    [SerializeField] GameObject ameliaMessagePrefab;
    [SerializeField] GameObject ameliaMessageSpawnPos;
    [SerializeField] GameObject devMessagePrefab;
    [SerializeField] GameObject devMessageSpawnPos;
    [SerializeField] float TextDistance;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("thats it") == "glotchiness")
        {
            currentConvo = se;
        }
        else if (PlayerPrefs.GetInt("glotchiness") == 6)
        {
            currentStoryState = AmeliaStoryState.Confess;
            currentConvo = confessConvo;
        }
        else if(PlayerPrefs.GetInt("glotchiness") == 7)
        {
            currentConvo = reflectionConvo;
        }
        else
        {
            currentStoryState = AmeliaStoryState.Flirt;
        }

        StartCoroutine(messageSequence(currentConvo));
    }

    private void Update()
    {
        if(Application.isEditor && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator typing()
    {
        float timeInbetween = 0.3f;

        while(true)
        {
            lerpSolutionTextTine = StartCoroutine(LerpSolution.lerpTextColourEnum(typingText, Color.black, 3f));
            while(typingText.color != Color.black) yield return new WaitForEndOfFrame();
            yield return GeneralManager.waitForSeconds(timeInbetween);
            lerpSolutionTextTine = StartCoroutine(LerpSolution.lerpTextColourEnum(typingText, Color.clear, 3f));
            while (typingText.color != Color.clear) yield return new WaitForEndOfFrame();
            yield return GeneralManager.waitForSeconds(timeInbetween);
        }
    }

    public IEnumerator messageSequence(MessageConvo convosation)
    {
        while (currentAmeliaTypeState == AmeliaTypeState.Idle)
        {
            yield return new WaitForEndOfFrame();
        }

        for(int i = 0; i < convosation.messages.Count; i++)
        {
            string message = convosation.messages[i];

            if(message.StartsWith("{s}") && !message.StartsWith("{nw}"))
            {
                message = message.Remove(0, 3);
                yield return GeneralManager.waitForSeconds(3f);
            }
            else if(!message.StartsWith("{nw}"))
            {
                yield return GeneralManager.waitForSeconds(0.5f);
            }

            if(message.StartsWith("{ts}") && !message.StartsWith("{nw}"))
            {
                message = message.Remove(0, 4);
                Coroutine typingTine = StartCoroutine(typing());
                yield return GeneralManager.waitForSeconds(timeToType(message) / 2);
                StopCoroutine(typingTine);
                StopCoroutine(lerpSolutionTextTine);
                typingText.color = Color.clear;
                yield return GeneralManager.waitForSeconds(1.5f);
                typingTine = StartCoroutine(typing());
                yield return GeneralManager.waitForSeconds(timeToType(message) / 2);
                StopCoroutine(typingTine);
                StopCoroutine(lerpSolutionTextTine);
                typingText.color = Color.clear;
            }
            else if(!message.StartsWith("{nw}"))
            {
                Coroutine typingTine = StartCoroutine(typing());
                yield return GeneralManager.waitForSeconds(timeToType(message) / 2);
                StopCoroutine(typingTine);
                StopCoroutine(lerpSolutionTextTine);
                typingText.color = Color.clear;
            }

            if (message.StartsWith("{nw}")) message = message.Remove(0, 4);
            message = message.Replace("{name}", PlayerPrefs.GetString("playerName"));

            if (convosation.lineToPlay.Count == convosation.messages.Count && convosation.lineToPlay[i] != null)
            {
                //FindObjectOfType<VoiceLineManager>().PlayLine(convosation.lineToPlay[i]);
            }

            SpawnMessage(message);
            if (message == "fuck you " + PlayerPrefs.GetString("playerName"))
            {
                StartCoroutine(EndGlitch());
            }
            else if(message == "goodbye")
            {
                StartCoroutine(FindObjectOfType<AmeliaSeState>().credits());
            }
            yield return GeneralManager.waitForSeconds(1);
        }

        if (convosation.responses.Count == 0)
        {
            if(convosation.messageConvos[0].name.ToLower().EndsWith("end"))
            {
                GetComponent<MessageConvoChoice>().Finish();
            }
            StartCoroutine(messageSequence(convosation.messageConvos[0]));
        }
        else
        {
            currentMessagingState = MessageConvoState.PlayerChoosing;
        }
    }

    private IEnumerator EndGlitch()
    {
        AsyncOperation sceneOp = SceneManager.LoadSceneAsync("City_End");
        sceneOp.allowSceneActivation = false;
        Debug.Log("Should get called");
        yield return GeneralManager.waitForSeconds(TimeToWait(GetComponent<AudioSource>().time));
        gameObject.AddComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.Drugged;
        yield return GeneralManager.waitForSeconds(0.1f);
        GetComponent<AudioSource>().Pause();
        messageManager[] dieeeee = FindObjectsOfType<messageManager>();
        foreach (messageManager lmao in dieeeee)
        {
            Destroy(lmao.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        sceneOp.allowSceneActivation = true;
        yield return GeneralManager.waitForSeconds(10);
        Destroy(gameObject);
    }

    float TimeToWait(float currentTimestamp) //13.5
    {
        if (currentTimestamp <= 13.4f)
        {
            return 13.4f - currentTimestamp;
        }
        else if (currentTimestamp <= 26.9)
        {
            return 26.9f - currentTimestamp;
        }
        else if (currentTimestamp <= 40.4)
        {
            return 40.4f - currentTimestamp;
        }
        else if (currentTimestamp <= 53.9)
        {
            return 53.9f - currentTimestamp;
        }
        else
        {
            return 0;
        }
    }

    float timeToType(string message)
    {
        //return 4f;
        float wordCount = message.Split(' ').Length;
        return wordCount * 0.3f;
    }

    List<MessageConvoMessage> sentMessages = new List<MessageConvoMessage>();
    public void SpawnMessage(string messageContent, bool isDevSent = false)
    {
        SFX.PlayOneShot(msgSfx);

        Transform spawnPoint;
        GameObject messagePrefab;

        if(!isDevSent)
        {
            spawnPoint = ameliaMessageSpawnPos.transform;
            messagePrefab = ameliaMessagePrefab;
        }    
        else
        {
            spawnPoint = devMessageSpawnPos.transform;
            messagePrefab = devMessagePrefab;
        }

        GameObject SpawnedMessage = Instantiate(messagePrefab, spawnPoint.transform.position, Quaternion.identity, messageParent.transform);

        MessageConvoMessage convoMessage = SpawnedMessage.GetComponent<MessageConvoMessage>();
        convoMessage.GetText().text = messageContent;
        convoMessage.GetBubble().ResizeBubble();
        convoMessage.GetBubble().SetTop();
        convoMessage.GetBubble().SetBottom();

        float spawnDist = SpawnedMessage.transform.position.y - convoMessage.GetBottom().position.y; // maybe localPosition
        GameObject bottomOfCurrent = convoMessage.GetBottom().gameObject;
        bottomOfCurrent.transform.position = new Vector3(bottomOfCurrent.transform.position.x, ameliaMessageSpawnPos.transform.position.y, bottomOfCurrent.transform.position.z);
        bottomOfCurrent.transform.SetParent(null);
        SpawnedMessage.transform.position += new Vector3(0, spawnDist, 0);
        Destroy(bottomOfCurrent);
        convoMessage.GetBubble().SetBottom();

        MessageConvoMessage[] allSentMessages = FindObjectsOfType<MessageConvoMessage>();
        for(int i = 0; i < allSentMessages.Length; i++)
        {
            if (i != 0)
            {
                float currentLineCount = allSentMessages[i].GetText().textInfo.lineCount;

                float lineCountOnPreviousMessage = allSentMessages[i - 1].GetText().textInfo.lineCount;

                float dist = allSentMessages[i].transform.position.y - allSentMessages[i].GetBottom().position.y + 0.2f;
                allSentMessages[i].transform.position = new Vector3(allSentMessages[i].transform.position.x, allSentMessages[i - 1].GetTop().position.y + dist, allSentMessages[i].transform.position.z);

                //LerpSolution.lerpPosition(allSentMessages[i].transform, new Vector3(allSentMessages[i].transform.position.x, allSentMessages[i - 1].GetTop().position.y + dist, allSentMessages[i].transform.position.z), 10); // Doesn't rly work rn

                sentMessages.Add(allSentMessages[i]);
            }

        }
    }
}
