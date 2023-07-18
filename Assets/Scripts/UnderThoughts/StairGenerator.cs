using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StairGenerator : MonoBehaviour
{
    [SerializeField] GameObject Sets;
    [SerializeField] Material oldMat, newMat;

    public void AddStair()
    {
        if(Sets == null)
        {
            Sets = gameObject;
        }

        int numOfStairs = Sets.GetComponentsInChildren<Transform>().Length;
        Transform lastStair = Sets.GetComponentsInChildren<Transform>()[numOfStairs - 2];
        Vector3 basePos = lastStair.position;
        Vector3 newPos = new Vector3(basePos.x, basePos.y + 2.674f, basePos.z + 5.896f);
        GameObject newStair = Instantiate(lastStair.gameObject, newPos, lastStair.rotation);
        newStair.transform.parent = Sets.transform;
        newStair.name = lastStair.name;
        Debug.Log("Added stair!");
    }

    public void ChangeMat()
    {
        foreach(MeshRenderer renderer in FindObjectsOfType<MeshRenderer>())
        {
            if(renderer.sharedMaterial == oldMat)
            {
                renderer.sharedMaterial = newMat;
            }
        }
    }


    #if UNITY_EDITOR
    [CustomEditor(typeof(StairGenerator))]
    public class StairGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            StairGenerator generator = (StairGenerator)target;
            if(GUILayout.Button("Add Stair"))
            {
                generator.AddStair();
            }
            else if(GUILayout.Button("Make Every Stair New Material"))
            {
                generator.ChangeMat();
            }
        }
    }
    #endif
}
