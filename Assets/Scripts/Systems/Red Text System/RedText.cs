using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedText : MonoBehaviour
{
    public static RedText instance = null;

    public static RedText Instance
    {
        get
        {
            if (instance != null)
            {
                instance = FindObjectOfType<RedText>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "RedText";
                    instance = go.AddComponent<RedText>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Talk(Dictionary<string, Vector3> Script, float TimeInterval)
    {
        instance.StartCoroutine(instance.TalkEnum(Script, TimeInterval));
    }


    IEnumerator TalkEnum(Dictionary<string, Vector3> Script, float TimeInterval)
    {
        Transform parent = new GameObject("Red Texts").transform;
        foreach(KeyValuePair<string, Vector3> line in Script)
        {
            Transform TextTrans = new GameObject(line.Key.Split(' ')[0]).transform;
            TextTrans.SetParent(parent);
            TextTrans.position = line.Value;

            yield return GeneralManager.waitForSeconds(TimeInterval);
        }
    }
}
