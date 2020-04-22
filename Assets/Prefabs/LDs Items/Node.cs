﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject conjointNode = null;
    [SerializeField] private float teleportationSpeed = 0f;
    private GameObject toTeleport = null;
    private Vector3 teleportPos = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    bool teleporting = false;

    void Start()
    {
        teleporting = false;
    }

    void Update()
    {
        if (teleporting)
        {
            toTeleport.transform.position += direction * teleportationSpeed;
            if ((teleportPos - toTeleport.transform.position).magnitude < 0.5)
            {
                teleporting = false;
                toTeleport.GetComponent<Rigidbody>().useGravity = true;
                toTeleport.GetComponent<Rigidbody>().isKinematic = false;
                toTeleport.transform.GetChild(0).gameObject.SetActive(true);
                toTeleport.GetComponent<Collider>().enabled = true;
            }
            else if ((toTeleport.transform.position - transform.position).magnitude > (teleportPos - transform.position).magnitude)
            {
                toTeleport.transform.position = teleportPos;
                teleporting = false;
                toTeleport.GetComponent<Rigidbody>().useGravity = true;
                toTeleport.GetComponent<Rigidbody>().isKinematic = false;
                toTeleport.transform.GetChild(0).gameObject.SetActive(true);
                toTeleport.GetComponent<Collider>().enabled = true;
            }
        }
    }

    public void Teleport(GameObject toTeleport)
    {
        teleporting = true;
        this.toTeleport = toTeleport;
        teleportPos = new Vector3(conjointNode.transform.position.x, conjointNode.transform.position.y, 0);
        direction = (teleportPos - toTeleport.transform.position).normalized;
        toTeleport.transform.GetChild(0).gameObject.SetActive(false);
        toTeleport.GetComponent<Rigidbody>().useGravity = false;
        toTeleport.GetComponent<Rigidbody>().isKinematic = true;
        toTeleport.GetComponent<Collider>().enabled = false;
    }
}
