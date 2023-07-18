using UnityEngine;

public class MaterialGenerator : MonoBehaviour
{
    [SerializeField] Texture2D image;
    [SerializeField] Renderer render;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.dataPath + "/Resources/menuImage.png");
        //image = Resources.Load("menuImage") as Texture2D;

        render = GetComponent<Renderer>();
        render.material.mainTexture = (image);
    }

}
