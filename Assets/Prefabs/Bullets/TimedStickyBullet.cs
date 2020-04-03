using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedStickyBullet : BulletSharedClass
{
    [SerializeField] float explosionRange = 1f;

    [SerializeField] float timeLimit = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);

        timeLimit -= Time.deltaTime;

        if (timeLimit <= 0)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
        else
        {
            direction = Vector3.zero;
        }
    }

    public override void doBehavior(GameObject hitObject)
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
                inRange.GetComponent<Entity>().InflictDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
