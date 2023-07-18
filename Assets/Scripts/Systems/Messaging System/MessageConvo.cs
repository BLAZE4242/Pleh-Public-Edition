using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pleh System/Message Conversation")]
public class MessageConvo : ScriptableObject
{
    public List<string> messages = new List<string>(new string[1]);
    public List<string> responses = new List<string>(new string[4]);
    public List<MessageConvo> messageConvos = new List<MessageConvo>(new MessageConvo[4]);
    public bool IsForced = false;
    public bool IsEndIntro = false;
    public List<VoiceLine> lineToPlay = new List<VoiceLine>();

}
