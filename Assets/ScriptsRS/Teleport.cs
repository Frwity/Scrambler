using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [HideInInspector]public Transform toTP;
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
        if (other.CompareTag("Bullet"))
        {
            other.transform.position = new Vector3(toTP.position.x, transform.position.y, toTP.position.z);
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
