using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// using Aura2API; // AURA HERE

public class LBW_intro : MonoBehaviour
{
    [SerializeField] Transform door, endPos;
    //public AuraLight spotLight;  // AURA HERE
    public Light spotLight; // AURA NOT HERE
    public TextMeshPro directionText;
    [HideInInspector] public float defaultLightStrength;
    [HideInInspector] public Color defaultDirectionColour;

    bool hasRun = false;

    public void stateStart()
    {
        // defaultLightStrength = spotLight.strength; // AURA HERE
        defaultDirectionColour = directionText.color;

        // spotLight.strength = 0; // AURA HERE
        directionText.color = Color.clear;
    }

    public void stateUpdate()
    {

    }

    public void pushDoorBack()
    {
        LerpSolution.lerpPosition(door, endPos.position, 1.2f);
    }

    public void nextState()
    {
        GetComponent<LBW_manager>().currentState++;
    }

    public void runState()
    {
        if (!hasRun)
        {
            hasRun = true;
            stateStart();
        }
        else stateUpdate();
    }
}
