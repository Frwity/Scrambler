using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletSharedClass : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;

    [HideInInspector] public Vector3 direction;

    [HideInInspector] public GameObject shooter;

    abstract public void doBehavior(GameObject hitObject);

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }
}
