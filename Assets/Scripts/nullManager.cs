using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nullManager : MonoBehaviour
{
    private void Start()
    {
        GeneralManager.SetSceneSave();
        FindObjectOfType<DRP>().RP("null", "null");
    }

    string input = "";
    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            input += c;
        }

        if (input.Contains("null"))
        {
            Gas.EarnAchievement("ACH_NULL");
        }
    }
}
