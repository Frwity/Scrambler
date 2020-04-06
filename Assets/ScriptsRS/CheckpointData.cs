using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]public class CheckpointData
{
        
    [HideInInspector]public Transform ParentPos = new RectTransform() ;

    [SerializeField]private Vector3 pos;



    [HideInInspector]
    public Vector3 checkPointPos
    {
        get { return pos + ParentPos.position; }
        set { pos = value - ParentPos.position; }
    }

    public Color color = Color.black;
    public bool enabled;

}
