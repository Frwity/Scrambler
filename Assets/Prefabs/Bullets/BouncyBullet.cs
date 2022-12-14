using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBullet : CurvedBulletSharedClass
{
    [SerializeField] short bounces = 3; // at 0, it explodes.

    [SerializeField] float explosionRange = 1f;

    // Start is called before the first frame update
    protected override void Start()
    {
        GetComponent<Rigidbody>().AddForce(direction.normalized * speed, ForceMode.Impulse);

        shooterTag = shooter.gameObject.tag;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        // unused
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
        {
            bounces--;

            if (bounces <= 0)
            {
                Explode();
            }
        }
    }

    public override void DoBehavior(GameObject hitObject)
    {
        if (hitObject.gameObject != shooter)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (Collider inRange in colliders)
        {
            if (inRange.CompareTag("Player") || inRange.CompareTag("Enemy"))
            {
                if (!Physics.Raycast(transform.position, inRange.transform.position - transform.position, 
                                                         Mathf.Abs(transform.position.magnitude - inRange.transform.position.magnitude),
                                                         1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Shield")) )
                {
                    if (inRange != shooter)
                        inRange.GetComponent<Entity>().InflictDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
}
