using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Message")]
public class Message : ScriptableObject
{
    public string sender = "Unknown";
    public bool italics;
    public bool bold;
    public string content = "Hello world!";
    public float timeAfterPreviousMessage = 4f;
    public string replyContent;
    public string methodToCall; //invoke this
    public string coroutineToCall;
    public float timeToWaitForCall;
    public float timeToDie;

    public Message(string _sender, string _content, bool _bold = false, float _timeToDie = 0, string _replyContent = "Tap to reply")
    {
        sender = _sender;
        content = _content;
        replyContent = _replyContent;
        bold = _bold;
        timeToDie = _timeToDie;
    }
}