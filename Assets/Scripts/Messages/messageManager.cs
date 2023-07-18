using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class messageManager : MonoBehaviour
{
    [SerializeField] TextMeshPro senderText, contentText, replyText;
    [SerializeField] float moveSpeed;
    public UnityEvent onClick; 
    gameMessagesState _manager;

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            Destroy(gameObject);
        }
    }

    public void updateInfo(Message message)
    {
        foreach (TextMeshPro childText in GetComponentsInChildren<TextMeshPro>())
        {
            switch (childText.transform.tag)
            {
                case "Sender Text":
                    senderText = childText;
                    break;
                case "Message Text":
                    contentText = childText;
                    break;
                case "Reply Text":
                    replyText = childText;
                    break;
            }
        }

        senderText.text = "Message: " +  message.sender;
        contentText.text = message.content;
        if(message.italics) contentText.fontStyle = FontStyles.Italic;
        if(message.bold) contentText.fontStyle = FontStyles.Bold;
        if(message.replyContent != "") replyText.text = message.replyContent;
        LerpSolution.lerpPosition(transform, GameObject.Find("Messages end point").transform.position, moveSpeed);
        if(message.timeToDie != 0) Destroy(gameObject, message.timeToDie);
    }

    public void moveDown()
    {
        LerpSolution.lerpPosition(transform, new Vector3(GameObject.Find("Messages end point").transform.position.x, transform.position.y - 3.78f, transform.position.z), moveSpeed);
    }

    void OnMouseDown()
    {
        try
        {
            onClick.Invoke();
            return;
        }
        catch
        {
            _manager = FindObjectOfType<gameMessagesState>();
            _manager.die(true);
            // Instead of dying, make the text show on screen like the no escape from WTDC
        }
    }

    IEnumerator OnLevelWasLoaded()
    {
        yield return GeneralManager.waitForSeconds(3);
        Destroy(gameObject);
    }
}
