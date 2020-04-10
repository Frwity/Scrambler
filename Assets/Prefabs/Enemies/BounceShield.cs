using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceShield : MonoBehaviour
{
    private ShieldEnemySkill entitySkill;

    void Start()
    {
        entitySkill = GetComponentInParent<ShieldEnemySkill>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Vector3 direction = collision.gameObject.GetComponent<BulletSharedClass>().direction;
            collision.gameObject.transform.Rotate(transform.forward, Vector3.SignedAngle(direction, transform.right, Vector3.forward) * 2 + 180);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Entity>())
        {
            if (entitySkill.shieldSlamCollider)
            {
                if ((transform.position - other.transform.position).magnitude < (transform.position - entitySkill.shieldSlamCollider.transform.position).magnitude)
                    entitySkill.shieldSlamCollider = other.gameObject;
            }
            else
            {
                entitySkill.shieldSlamCollider = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        entitySkill.shieldSlamCollider = null;
    }
}
