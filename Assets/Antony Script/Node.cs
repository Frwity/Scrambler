using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject conjointNode;
    [SerializeField] private float teleportationTime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport(GameObject toTeleport)
    {
        toTeleport.transform.position = conjointNode.transform.position + Vector3.back * 2;
        
    }
}
