using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerthaBullet : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]public Transform toTP;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BerthaBulletZone"))
        {
            transform.position = new Vector3(toTP.position.x, transform.position.y, toTP.position.z);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
       Destroy(gameObject);
    }
}
