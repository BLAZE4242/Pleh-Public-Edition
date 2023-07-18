using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger2D : MonoBehaviour
{
    [SerializeField] float doorOpenSpeed;
    public Vector2 closedPos, openPos;

    public void Trigger()
    {
        doorInteract(false);
    }

    public void UnTrigger()
    {
        doorInteract(true);
    }

    public void ButtonToggle(ButtonHandle button)
    {
        if (button.isTriggered) Trigger();
        else UnTrigger();
    }

    void doorInteract(bool isClosing)
    {
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = new Vector3(openPos.x, openPos.y, transform.localPosition.z);
        if (isClosing)
        {
            //maybe the weird teleporting thing is because we need to += not just =
            targetPos = new Vector3(closedPos.x, closedPos.y, transform.localPosition.z);
        }

        LerpSolution.lerpPosition(transform, targetPos, doorOpenSpeed, true);
    }
}