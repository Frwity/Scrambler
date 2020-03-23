﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBasicBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(0, Time.deltaTime * -speed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Entity>().InflictDamage(damage);
        }
        Destroy(gameObject);
    }
}
