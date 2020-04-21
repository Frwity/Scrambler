using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<RoomManager>().respawnPoint = transform.position;
            transform.parent.GetComponent<RoomManager>().ActivateSpawner();
            GameObject.FindGameObjectWithTag("PlayerMove").GetComponent<PlayerControl>().actualRoom = transform.parent.GetComponent<RoomManager>();
        }
    }
}
