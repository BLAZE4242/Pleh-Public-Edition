using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    playerMove pluto;
    [SerializeField] bool isCube;
    bool isCubeRightNow;
    [SerializeField] bool pushCube;
    [SerializeField] float platformSpeed;

    private void Awake()
    {
        lastPos = transform.position.x;
    }

    private void Start()
    {
        pluto = FindObjectOfType<playerMove>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCube)
        {
            pluto.transform.SetParent(GetComponentInParent<Animator>().transform, true);
            isCubeRightNow = true;
        }
        else
        {
            collision.transform.SetParent(transform, true);

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isCube)
        {
            pluto.transform.SetParent(null, true);
            isCubeRightNow = false;
        }
        else collision.transform.SetParent(null, true);
    }

    float lastPos;
    private void Update()
    {
        if (!isCube) return;
        if (transform.localPosition.x > 64.9f) transform.localPosition = new Vector3(16.9f, transform.localPosition.y, transform.localPosition.z);
        transform.position += new Vector3(1, 0, 0) * platformSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError("Trigger");
        if(pushCube) collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(3000, 100), ForceMode2D.Force);
    }
}
