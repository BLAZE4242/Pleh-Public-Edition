using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PipesCreator : MonoBehaviour
{
    [SerializeField] bool BoxToTickToSave = false;

    public enum PipeDirection {Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, LeftUp, LeftDown, RightUp, RightDown}

    public void CreatePipe(PipeDirection _direction)
    {
        Pipe lastPipe = gameObject.GetComponentsInChildren<Pipe>().Last();
        
        if(_direction != lastPipe.CurrentDirection)
        {
            CreateTurnedPipe(lastPipe.CurrentDirection, _direction, lastPipe.TargetNextPipe().position);
        }
        else
        {
            GameObject NewPipe = (GameObject)Resources.Load($"Pipe{_direction}");

            Instantiate(NewPipe, lastPipe.TargetNextPipe().position, NewPipe.transform.rotation).transform.SetParent(ChildrenPipes());
        }
    }

    void CreateTurnedPipe(PipeDirection oldDirection, PipeDirection newDirection, Vector3 spawnPos)
    {
        GameObject NewPipe = (GameObject)Resources.Load($"Pipe{oldDirection}{newDirection}");
        Debug.Log($"Pipe{oldDirection}{newDirection}");
        Instantiate(NewPipe, spawnPos, NewPipe.transform.rotation).transform.SetParent(ChildrenPipes());
    }

    public void CreateConveyerBelt()
    {
        
    }

    public void UndoCreatePipe()
    {
        DestroyImmediate(ChildrenPipes().GetComponentsInChildren<Pipe>().Last().gameObject);
    }

    Transform ChildrenPipes()
    {
        string name = "Children Pipes";

        foreach(Transform trans in GetComponentsInChildren<Transform>())
        {
            if(trans.name == name)
            {
                return trans;
            }
        }

        Transform newObject = new GameObject(name).transform;
        newObject.SetParent(transform);
        return newObject;
    }


    #if UNITY_EDITOR
    [CustomEditor(typeof(PipesCreator))]
    public class PipesCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PipesCreator _creator = (PipesCreator)target;

            bool CreateUpPipe = GUILayout.Button("↑");
            GUILayout.BeginHorizontal();
                bool CreateLeftPipe = GUILayout.Button("←");
                bool CreateRightPipe = GUILayout.Button("→");
            GUILayout.EndHorizontal();
            bool CreateDownPipe = GUILayout.Button("↓");
            GUILayout.Space(5);

            bool FinishPipe = GUILayout.Button("Spawn Conveyer Belt");
            GUILayout.Space(10);

            bool Undo = GUILayout.Button("Undo");

            if(CreateUpPipe)
            {
                _creator.CreatePipe(PipesCreator.PipeDirection.Up);
            }
            else if(CreateLeftPipe)
            {
                _creator.CreatePipe(PipesCreator.PipeDirection.Left);
            }
            else if(CreateRightPipe)
            {
                _creator.CreatePipe(PipesCreator.PipeDirection.Right);
            }
            else if(CreateDownPipe)
            {
                _creator.CreatePipe(PipesCreator.PipeDirection.Down);
            }

            else if(FinishPipe)
            {
                _creator.CreateConveyerBelt();
            }

            else if(Undo)
            {
                _creator.UndoCreatePipe();
            }
        }
    }
    #endif
}
