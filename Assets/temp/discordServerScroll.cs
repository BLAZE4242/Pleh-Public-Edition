using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class discordServerScroll : MonoBehaviour
{
    [SerializeField] float multi = 1f;
    MeshRenderer mr;
	Material mat;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mat = mr.material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = mat.mainTextureOffset;

        if(offset.x >= 1)
        {
            offset.x = 1;
        }
        else
        {
            offset.x += Time.deltaTime / 20f * multi;


        }
        mat.mainTextureOffset = offset;

    }
}
