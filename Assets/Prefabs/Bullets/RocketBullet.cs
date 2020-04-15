using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBullet : BulletSharedClass
{
    [SerializeField] float rateOfAcceleration = 1f;
    [SerializeField] float explosionRange = 1f;
    [SerializeField] float speedLimit = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.LookRotation(direction);
        transform.Translate(direction, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);

        if (speed < speedLimit)
            speed += rateOfAcceleration;
        else if (speed > speedLimit)
            speed = speedLimit;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    public override void DoBehavior(GameObject hitObject)
    {

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
