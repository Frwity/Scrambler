using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveBullet : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel = new Vector3(-rb.velocity.y, rb.velocity.x, 0);
        
        
        transform.localRotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity, vel);
        //Debug.Log(transform.position.z);
    }

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log($"collided with {other.gameObject.name}");
        Destroy(this.gameObject);
    }
}
