using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CollisionManager : MonoBehaviour
{
    public enum CollisionTypes {trigger, collision, mouseDown}
    public CollisionTypes collisionType;
    public UnityEvent collisionAction;
    public bool CanTriggerOnce = false;
    bool hasBeenTriggered;
    public bool OverrideTag;
    [HideInInspector] public string OverridenTag;


    void OnTriggerEnter(Collider col)
    {
        if(collisionType == CollisionTypes.trigger)
        {
            CollidedWith(col.transform);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(collisionType == CollisionTypes.collision)
        {
            CollidedWith(col.transform);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (collisionType == CollisionTypes.trigger)
        {
            Debug.Log("ur mum");
            CollidedWith(col.transform);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(collisionType == CollisionTypes.collision)
        {
            CollidedWith(col.transform);
        }
    }

    private void OnMouseDown()
    {
        if(collisionType == CollisionTypes.mouseDown)
        {
            CollidedWith(null);
        }
    }

    void CollidedWith(Transform colTransform)
    {
        if(OverrideTag)
        {
            if(!colTransform.CompareTag(OverridenTag))
            {
                return;
            }
        }

        if(CanTriggerOnce)
        {
            if(hasBeenTriggered)
            {
                return;
            }
            
            hasBeenTriggered = true;
        }

        collisionAction.Invoke();
    }

    public void ResetIfTriggered()
    {
        hasBeenTriggered = false;
    }


    #if UNITY_EDITOR
    [CustomEditor(typeof(CollisionManager)), CanEditMultipleObjects]
    public class CollisionManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CollisionManager _CollisionManager = (CollisionManager)target;

            if(_CollisionManager.OverrideTag)
            {
                _CollisionManager.OverridenTag = EditorGUILayout.TextField("Overriden Tag", _CollisionManager.OverridenTag);
            }
        }
    }
    #endif
}
