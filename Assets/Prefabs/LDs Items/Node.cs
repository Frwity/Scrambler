using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject conjointNode;
    [SerializeField] private float teleportationSpeed;
    private GameObject toTeleport;
    private Vector3 teleportPos;
    private Vector3 direction;
    bool teleporting;

    void Start()
    {
        teleporting = false;
    }

    // Update is called once per frame
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
                toTeleport.GetComponent<MeshRenderer>().enabled = true;
            }
            else if ((toTeleport.transform.position - transform.position).magnitude > (teleportPos - transform.position).magnitude)
            {
                toTeleport.transform.position = teleportPos;
                teleporting = false;
                toTeleport.GetComponent<Rigidbody>().useGravity = true;
                toTeleport.GetComponent<Rigidbody>().isKinematic = false;
                toTeleport.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void Teleport(GameObject toTeleport)
    {
        teleporting = true;
        this.toTeleport = toTeleport;
        teleportPos = new Vector3(conjointNode.transform.position.x, conjointNode.transform.position.y, 0);
        direction = (teleportPos - toTeleport.transform.position).normalized;
        toTeleport.GetComponent<MeshRenderer>().enabled = false;
        toTeleport.GetComponent<Rigidbody>().useGravity = false;
        toTeleport.GetComponent<Rigidbody>().isKinematic = true;
    }
}
