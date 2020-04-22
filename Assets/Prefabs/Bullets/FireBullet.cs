using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : CurvedBulletSharedClass
{
    [SerializeField] Vector3 fireRange = new Vector3(1f, 0.5f, 0.5f);
    
    [SerializeField] float fireLifeTime = 2f;

    [SerializeField] GameObject fireZone = null;

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DoBehavior(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            SpreadFire(collision.transform.rotation.eulerAngles);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 roc = collision.transform.rotation.eulerAngles; // roc = Rotation Of Collision
            SpreadFire(new Vector3(roc.x, roc.y, roc.z + (collision.transform.rotation.eulerAngles.z == 0 ? 90 : 0)));
        }
    }

    private void SpreadFire(Vector3 rotation)
    {
        GameObject fire = Instantiate(fireZone, transform.position, Quaternion.identity);
        
        fire.transform.localScale = fireRange;

        fire.transform.Rotate(rotation, Space.World);

        fire.GetComponent<DeadZone>().lifeTime = fireLifeTime;
        fire.GetComponent<Activable>().isActive = true;

        Destroy(gameObject);
    }
}
