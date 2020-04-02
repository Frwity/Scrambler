﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceShield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<BulletSharedClass>().direction = -collision.gameObject.GetComponent<BulletSharedClass>().direction;
        }
    } 
}