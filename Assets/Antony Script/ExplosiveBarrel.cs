using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private float explosionRange;
    [SerializeField] private int damage;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void Explode()
    {
        int visionMask = 1 << 13;
        int hoverMask = 1 << 8;
        int bulletMask = 1 << 10;
        int PossessMask = 1 << 15;
        bulletMask = ~bulletMask;
        hoverMask = ~hoverMask;
        visionMask = ~visionMask;
        PossessMask = ~PossessMask;

        visionMask += hoverMask;
        visionMask += bulletMask;
        visionMask += PossessMask;

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (Collider inRange in colliders)
        {
            if (inRange.CompareTag("Player") || inRange.CompareTag("Enemy"))
            {
                Physics.Raycast(transform.position, inRange.transform.position, out RaycastHit hitInfo, explosionRange, visionMask);
                //Debug.Log(hitInfo.collider.gameObject.tag);
                if (hitInfo.collider.gameObject.CompareTag("Player") || hitInfo.collider.gameObject.CompareTag("Enemy"))
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
