using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBasicBullet : BulletSharedClass
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
