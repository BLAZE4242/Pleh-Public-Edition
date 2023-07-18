using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorButton2D : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToTrigger;
    [SerializeField] bool invertWtfAmIDoing = false;
     public List<GameObject> cubes = new List<GameObject>();
    [HideInInspector] public bool acceptCubes = true;

    List<doorTrigger2D> _doorTrigger2D = new List<doorTrigger2D>();
    List<CubeDestroy> _cubeDestroyer = new List<CubeDestroy>();

    public int numOfTrigger;

    public bool isWorking = true;

    private void Awake()
    {
        foreach (GameObject trigger in objectsToTrigger)
        {
            if(trigger.GetComponent<doorTrigger2D>() != null)
                _doorTrigger2D.Add(trigger.GetComponent<doorTrigger2D>());
            if(trigger.GetComponent<CubeDestroy>() != null)
                _cubeDestroyer.Add(trigger.GetComponent<CubeDestroy>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GetComponent<BoxCollider2D>().isTrigger) return;

        Debug.Log(collision.transform.name);
        if (collision.transform.CompareTag("Cube") || collision.transform.CompareTag("Player"))
        {

            if (collision.transform.CompareTag("Cube") && acceptCubes)
            {
                cubes.Add(collision.gameObject);
            }
            else if (!acceptCubes) return;

            foreach (GameObject trigger in objectsToTrigger)
            {
                switch (trigger.tag)
                {
                    case "Door":
                        CheckForTrigger(true);
                        break;
                    case "Cube Destroy":
                        CheckForTrigger(true);
                        break;
                }
            }
        }
    }

    private void CheckForTrigger(bool trigger)
    {
        if (trigger) numOfTrigger++;
        else numOfTrigger--;
    }

    private void Update()
    {
        if (!isWorking)
        {
            _doorTrigger2D[0].UnTrigger();
            return;
        }

        if (_doorTrigger2D.Count > 0)
        {
            foreach (doorTrigger2D trigger in _doorTrigger2D)
            {
                if (numOfTrigger > 0) trigger.Trigger();
                else trigger.UnTrigger();
            }
        }

        if (_cubeDestroyer.Count > 0)
        {

            foreach (CubeDestroy trigger in _cubeDestroyer)
            {
                if(invertWtfAmIDoing && trigger == _cubeDestroyer[0]) trigger.ToggleActiveState(numOfTrigger > 0 ? true : false);
                else trigger.ToggleActiveState(numOfTrigger > 0 ? false : true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!GetComponent<BoxCollider2D>().isTrigger) return;

        Debug.Log(collision.transform.name);
        if (collision.transform.CompareTag("Cube") || collision.transform.CompareTag("Player"))
        {
            if (collision.transform.CompareTag("Cube"))
            {
                if (cubes.Contains(collision.gameObject)) cubes.Remove(collision.gameObject);
                else return;
            }

            foreach (GameObject trigger in objectsToTrigger)
            {
                switch (trigger.tag)
                {
                    case "Door":
                        CheckForTrigger(false);
                        break;
                    case "Cube Destroy":
                        CheckForTrigger(false);
                        break;
                }
            }
        }
    }

    public void HitSide(bool left)
    {
        Debug.Log("sdfsdf");
        if (left) FindObjectOfType<playerMove>().transform.position += new Vector3(0.01f, 0.512694f, 0);
        else FindObjectOfType<playerMove>().transform.position += new Vector3(-0.01f, 0.512694f, 0);
    }

    public void stopWorking()
    {
        isWorking = false;
    }
}
