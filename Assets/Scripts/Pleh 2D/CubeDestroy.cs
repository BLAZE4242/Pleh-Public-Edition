using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroy : MonoBehaviour
{
    [SerializeField] Vector2 speed = new Vector2(3, 5);
    [SerializeField] Material blendMat;
    [SerializeField] conveyerBeltController controller;
    [SerializeField] Transform spawnPos;
    public bool isActive = true;
    SpriteRenderer mr;
    Material mat;

    private void Start()
    {
        mr = GetComponent<SpriteRenderer>();
        mat = mr.material;

        mat.mainTextureOffset = new Vector2(Random.Range(0, 10), 0);
        ToggleActiveState(isActive);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = mat.mainTextureOffset;

        offset.x += Time.deltaTime / 20f * Random.Range(speed.x, speed.y);

        mat.mainTextureOffset = offset;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cube") && isActive)
        {
            if (controller != null)
            {
                Destroy(collision.gameObject);

                controller.numOfAliveCubes--;
                controller.cubeTextDisplay();
            }
            else
            {
                collision.transform.position = spawnPos.position;
            }

            Camera.main.GetComponent<Animator>().SetTrigger("Death");
        }
    }

    public void ToggleActiveState(bool _isActive)
    {
        isActive = _isActive;

        foreach (SpriteRenderer _renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            if(_renderer != mr) _renderer.enabled = !_isActive;
        }
        
    }
}
