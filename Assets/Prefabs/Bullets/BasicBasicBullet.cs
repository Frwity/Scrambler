using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBasicBullet : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
