using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletSharedClass : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;

    [SerializeField] bool hasLimitedTime = true;
    [SerializeField] float bulletLifeTime = 10f;

    [HideInInspector] public Vector3 direction;

    [HideInInspector] public GameObject shooter;

    protected string shooterTag = null;

    protected virtual void Start()
    {
        shooterTag = shooter.gameObject.tag;
    }    
    
    private void LateUpdate()
    {
        if (hasLimitedTime)
        {
            bulletLifeTime -= Time.deltaTime;

            if (bulletLifeTime <= 0)
                Destroy(gameObject);
        }
    }

    public virtual void DoBehavior(GameObject hitObject)
    {
        if (!hitObject || !shooter)
            return;

        if ((!hitObject.CompareTag(shooterTag)) && hitObject.GetComponent<Entity>())
        {
            hitObject.GetComponent<Entity>().InflictDamage(damage);

            hitObject.GetComponent<Entity>().HitFlash();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Shield") && !(collision.gameObject == shooter))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Roof"))
        {
            DoBehavior(collision.gameObject);
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
