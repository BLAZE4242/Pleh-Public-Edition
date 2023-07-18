using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CamEditor2D : MonoBehaviour
{
    public bool MovePluto;

    public void MoveCamLeft()
    {
        transform.position -= new Vector3(30, 0);
        if (MovePluto)
        {
            FindObjectOfType<playerMove>().transform.position -= new Vector3(30, 0);
        }
    }

    public void MoveCamRight()
    {
        transform.position += new Vector3(30, 0);
        if (MovePluto)
        {
            FindObjectOfType<playerMove>().transform.position += new Vector3(30, 0);
        }
    }

    public void MoveCamDown()
    {
        transform.position -= new Vector3(0, 15);
        if (MovePluto)
        {
            FindObjectOfType<playerMove>().transform.position -= new Vector3(0, 15);
        }
    }

    public void MoveCamUp()
    {
        transform.position += new Vector3(0, 15);
        if (MovePluto)
        {
            FindObjectOfType<playerMove>().transform.position += new Vector3(0, 15);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(CamEditor2D))]
    public class CamEditor2DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CamEditor2D cam = (CamEditor2D)target;

            cam.MovePluto = GUILayout.Toggle(cam.MovePluto, "Move Pluto with cam?");

            if (GUILayout.Button("Up"))
            {
                cam.MoveCamUp();
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Left"))
            {
                cam.MoveCamLeft();
            }
            else if (GUILayout.Button("Right"))
            {
                cam.MoveCamRight();
            }

            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Down"))
            {
                cam.MoveCamDown();
            }
        }
    }
#endif
}
