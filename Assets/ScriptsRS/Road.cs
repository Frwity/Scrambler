using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[ExecuteInEditMode] public class Road : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private float Radius = 1.0f;
    public CheckpointData[] Checkpoints = new CheckpointData[0];
    [HideInInspector] private int currentIndex;

    public int CurrentIndex
    {
        get => currentIndex;
        set
        {
            currentIndex = value;
            if (currentIndex > size-1)
            {
                currentIndex = 0;
            }
        }

    }

    [HideInInspector]public int size { get; private set; }

    private void Awake()
    {
        foreach (var variable in Checkpoints)
        {
            variable.ParentPos = transform;
        }
    }

    void Start()
    {
        foreach (var variable in Checkpoints)
        {
            variable.ParentPos = transform;
        }
        size = Checkpoints.Length;
        currentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Checkpoints.Length != size)
        {
            size = Checkpoints.Length;
        }
        
        
    }
    
    private void OnDrawGizmos()
    {
        
       
        foreach (var vari in Checkpoints)
        {
            Gizmos.color = vari.color;
            vari.ParentPos = transform;
            if (!vari.enabled)
            {
                Color c = Gizmos.color;
                c.a = 0.5f;
                Gizmos.color = c;
            }
            Gizmos.DrawSphere(vari.checkPointPos , Radius);
        }
    }
    
}

#if UNITY_EDITOR

[CustomEditor(typeof(Road)), CanEditMultipleObjects]
public class PathEditor : Editor
{
    
    public virtual void OnSceneGUI()
    {
        Road r = (Road) target;
        
        foreach (var vari in r.Checkpoints)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition =Handles.PositionHandle(vari.checkPointPos, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                //Undo.RecordObject(var, "Change Look At Target Position");
                vari.checkPointPos = newTargetPosition;
            }
        }
    }
}

#endif