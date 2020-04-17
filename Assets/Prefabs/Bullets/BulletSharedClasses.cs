using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletSharedClass : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;

    [HideInInspector] public Vector3 direction;

    [HideInInspector] public GameObject shooter;

    public virtual void DoBehavior(GameObject hitObject)
    {
        if ((hitObject != shooter) && !(shooter.CompareTag("Enemy") && hitObject.CompareTag("Enemy")) )
        {
            hitObject.GetComponent<Entity>().InflictDamage(damage);

            hitObject.GetComponent<Entity>().HitFlash();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }
}

public abstract class CurvedBulletSharedClass : BulletSharedClass
{
    protected virtual void FixedUpdate()
    {
        transform.position += direction * Time.fixedDeltaTime;
        direction += Physics.gravity * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.LookRotation(direction);
    }
}
