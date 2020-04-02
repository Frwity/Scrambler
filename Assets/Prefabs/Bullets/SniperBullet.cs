using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : BulletSharedClass
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.LookRotation(direction);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
    }

    public override void doBehavior(GameObject hitObject)
    {
        hitObject.gameObject.GetComponent<Entity>().InflictDamage(damage);
    }
}
