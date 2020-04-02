using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBasicBullet : BulletSharedClass
{
    void Start()
    {
        direction = new Vector3(0.0f, -1.0f, 0.0f);
    }

    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        
        
    }

    public override void doBehavior(GameObject hitObject)
    {
        hitObject.gameObject.GetComponent<Entity>().InflictDamage(damage);
    }
}
