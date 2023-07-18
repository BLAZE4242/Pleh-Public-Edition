using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestFitBubble : MonoBehaviour
{
    public void ResizeBubble()
    {
        GetComponentInParent<TextMeshPro>().ForceMeshUpdate();
        SpriteRenderer bgSprite = GetComponent<SpriteRenderer>();
        TextMeshPro text = GetComponentInParent<TextMeshPro>();

        Vector2 padding = new Vector2(4, 0.4f);

        bgSprite.size = text.GetRenderedValues(false) + padding;

        transform.localPosition = text.textBounds.center;
    }

    public void SetBottom()
    {
        MessageConvoMessage convoMessage = GetComponentInParent<MessageConvoMessage>();
        TextMeshPro text = convoMessage.GetText();

        GameObject bottom = new GameObject("Bottom of message");
        bottom.transform.SetParent(transform.parent);
        bottom.transform.localPosition = bottomPos(text.textInfo.lineCount);
    }

    public void SetTop()
    {
        MessageConvoMessage convoMessage = GetComponentInParent<MessageConvoMessage>();
        TextMeshPro text = convoMessage.GetText();

        GameObject bottom = new GameObject("Top of message");
        bottom.transform.SetParent(transform.parent);
        bottom.transform.localPosition = topPos(text.textInfo.lineCount);
    }

    public Vector3 bottomPos(float lineCount)
    {
        float yDist = 0f;

        yDist = lineCount * 3.219727f;

        return new Vector3(transform.localPosition.x, transform.localPosition.y - (yDist / 1.3f), transform.localPosition.z);
    }

    public Vector3 topPos(float lineCount)
    {
        float yDist = 0f;

        yDist = lineCount * 3.219727f;

        return new Vector3(transform.localPosition.x, transform.localPosition.y + (yDist / 1.3f), transform.localPosition.z);
    }



    #if UNITY_EDITOR
    [CustomEditor(typeof(TestFitBubble))]
    public class TestFitBubbleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TestFitBubble _test = (TestFitBubble)target;

            base.OnInspectorGUI();

            bool resize = GUILayout.Button("Resize Text Bubble");
            bool bottom = GUILayout.Button("Attempt to set bottom");
            bool top = GUILayout.Button("Attempt to set top");

            if (resize)
            {
                _test.ResizeBubble();
            }
            else if(bottom)
            {
                _test.SetBottom();
            }
            else if(top)
            {
                _test.SetTop();
            }
        }
    }
    #endif
}
