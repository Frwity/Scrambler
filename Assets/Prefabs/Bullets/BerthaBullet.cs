using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerthaBullet : BulletSharedClass
{
    [SerializeField] private float explosionRange = 0f;

    protected override void Start()
    {
        transform.Rotate(-90, 0, 0);
        transform.Rotate(Quaternion.LookRotation(direction).eulerAngles);

        shooterTag = shooter.gameObject.tag;
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
            if (inRange.CompareTag("Player") || inRange.CompareTag("Enemy") || inRange.GetComponent<ExplosiveBarrel>())
            {
                if (!Physics.Raycast(transform.position, inRange.transform.position - transform.position,
                                                         Mathf.Abs(transform.position.magnitude - inRange.transform.position.magnitude),
                                                         1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Shield")))
                {
                    if (inRange.GetComponent<ExplosiveBarrel>())
                        inRange.GetComponent<ExplosiveBarrel>().Invoke("Explode", 0.3f);

                    else
                        inRange.GetComponent<Entity>().InflictDamage(damage);
                }
            }
        }

        ParticleLauncher.ActivateParticleWithName(gameObject, null, "Explosion");

        Destroy(gameObject);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Explode();
        }
    }
}
