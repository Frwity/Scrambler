using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : LDBlock
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
            if (inRange.CompareTag("Player") || inRange.CompareTag("Enemy"))
            {
                if (!Physics.Raycast(transform.position, inRange.transform.position - transform.position, 
                                                         Mathf.Abs(transform.position.magnitude - inRange.transform.position.magnitude),
                                                         1 << LayerMask.NameToLayer("Ground") | (1 << LayerMask.NameToLayer("Shield"))))
                    inRange.GetComponent<Entity>().InflictDamage(damage);
            }
        }
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
