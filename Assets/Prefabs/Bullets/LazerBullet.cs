using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBullet : BulletSharedClass
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Debug.Log("LuL");
    }

    public override void doBehavior(GameObject hitObject)
    {
        hitObject.GetComponent<Entity>().InflictDamage(damage);
    }
}
