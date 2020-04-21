using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : Activable
{
    [SerializeField] private float explosionRange;
    [SerializeField] private int damage;

    void Start()
    {
        isActive = false;
    }


    void Update()
    {
        if (isActive)
            Explode();
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

        ParticleLauncher.ActivateParticleWithName(gameObject, null, "BigExplosion");

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Explode();
        }
    }
}
