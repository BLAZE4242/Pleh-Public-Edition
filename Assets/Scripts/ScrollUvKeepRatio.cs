using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteAlways]
public class ScrollUvKeepRatio : MonoBehaviour
{
    [SerializeField] float yMulti = 12f;
    public MeshRenderer mr;
    void Update()
    {
        if(mr == null)
        {
            mr = GetComponent<MeshRenderer>();
        }
        Material mat = mr.sharedMaterial;
        Vector2 tiling = mat.mainTextureScale;

        tiling.x = transform.localScale.x / 12;
        if (Application.isPlaying)
        {
            ExecuteOnPlayUpdate();
        }
        else
        {
            mat.mainTextureScale = tiling;
        }
    }

    void ExecuteOnPlayUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Level 4")
        {
            mr.sharedMaterial.mainTextureScale = new Vector2(0.2045496f, yMulti);
            mr.material.SetFloat("_Blend", 1);
        }
        else
        {
            mr.sharedMaterial.mainTextureScale = new Vector2(transform.localScale.x / 12, 1);
        }
    }

    private void OnDisable()
    {
        if (Application.isPlaying)
        {
            mr.sharedMaterial.mainTextureScale = new Vector2(mr.sharedMaterial.mainTextureScale.x, 1);
            mr.material.SetFloat("_Blend", 0);
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(ScrollUvKeepRatio))]
    public class ScrollUvKeepRatioEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ScrollUvKeepRatio _script = (ScrollUvKeepRatio)target;
            if (GUILayout.Button("Assign Material"))
            {
                _script.mr = _script.GetComponent<MeshRenderer>();
            }
        }
    }
    #endif
}
