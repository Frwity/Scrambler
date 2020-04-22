using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedStickyBullet : CurvedBulletSharedClass
{
    [SerializeField] float explosionRange = 1f;

    [SerializeField] float timeLimit = 5f;

    bool touchedSurface = false;

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(velocity * Time.deltaTime * speed, Space.World);

        timeLimit -= Time.deltaTime;

        if (timeLimit <= 0)
        {
            Explode();
        }
    }

    protected override void FixedUpdate()
    {
        if (!touchedSurface)
        {
            transform.position += direction * Time.fixedDeltaTime;
            direction += Physics.gravity * Time.fixedDeltaTime;
            transform.localRotation = Quaternion.LookRotation(direction);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject != shooter) && !(shooter.CompareTag("Enemy") && collision.gameObject.CompareTag("Enemy")))
        {
            Explode();
        }
        else
        {
            direction = Vector3.zero;
            touchedSurface = true;
        }
    }

    public override void DoBehavior(GameObject hitObject)
    {
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
                                                         1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Shield")))
                {
                    inRange.GetComponent<Entity>().InflictDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
}
