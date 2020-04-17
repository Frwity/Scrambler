using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]public class SpawnPointData
{
        
    [HideInInspector]public Transform ParentPos = new RectTransform() ;

    [SerializeField]private Vector3 pos;
    public float radius;


    [HideInInspector]
    public Vector3 SpawnPointPos
    {
        get { return pos + ParentPos.position; }
        set { pos = value - ParentPos.position; }
    }

    public Color color = Color.black;
    public bool enabled;

}
