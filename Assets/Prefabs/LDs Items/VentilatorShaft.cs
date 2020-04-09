using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilatorShaft : LDBlock
{
    [SerializeField] private float speed;


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive)
            return;
  
        if ((other.CompareTag("Player") || other.CompareTag("Enemy")) && other.GetComponent<Rigidbody>())
            other.GetComponent<Rigidbody>().AddForce(0, speed, 0);
    }
}
