using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBasicBullet : BulletSharedClass
{
    void Start()
    {
       
    }

    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Entity>().InflictDamage(damage);
        }
        if (collision.gameObject.CompareTag("Shield"))
        {
            Debug.Log(direction.ToString());
            direction = -direction;
            Debug.Log(direction.ToString());
            return;
        }
        
        Destroy(gameObject);
    }
}
