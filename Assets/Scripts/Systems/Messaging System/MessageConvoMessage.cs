using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageConvoMessage : MonoBehaviour
{
    public TextMeshPro GetText()
    {
        return GetComponentInChildren<TextMeshPro>();
    }

    public TestFitBubble GetBubble()
    {
        return GetComponentInChildren<TestFitBubble>();
    }

    public Transform GetTop()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.name.ToLower().Contains("top"))
            {
                return child;
            }
        }

        return null;
    }

    public Transform GetBottom()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.name.ToLower().Contains("bottom"))
            {
                return child;
            }
        }

        return null;
    }
}
