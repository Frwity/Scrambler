using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveBullet : BulletSharedClass
{
    public Vector3 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        
        transform.localRotation = Quaternion.LookRotation(velocity);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        velocity += Physics.gravity * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.LookRotation(velocity);
    }

    public override void doBehavior(GameObject hitObject)
    {
        hitObject.GetComponent<Entity>().InflictDamage(damage);
    }
}
