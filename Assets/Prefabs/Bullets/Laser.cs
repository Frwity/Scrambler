using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : BulletSharedClass
{
    [SerializeField] float range = 5f;
    [SerializeField] float lifeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if ( Physics.Raycast(transform.position, direction, range) )
        {
            transform.localScale += new Vector3(0, range, 0);
            transform.Translate(direction * (range + 1));

            transform.Rotate(Quaternion.LookRotation(direction).eulerAngles);
            transform.Rotate(90, 0, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
            Destroy(gameObject);
    }

    public override void doBehavior(GameObject hitObject)
    {
        hitObject.GetComponent<Entity>().InflictDamage(damage);
    }
}
