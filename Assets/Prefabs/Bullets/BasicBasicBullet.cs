using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBasicBullet : MonoBehaviour
{
    [SerializeField] public float shootingPower;
    [SerializeField] private int damage;

    [SerializeField] public float imprecision; // is a measure in degrees, forming a cone of fire. 

    void Start()
    {

    }

    void Update()
    {
        
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
