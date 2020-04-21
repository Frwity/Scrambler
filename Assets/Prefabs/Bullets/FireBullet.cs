using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : CurvedBulletSharedClass
{
    [SerializeField] Vector3 fireRange = new Vector3(1f, 0.5f, 0.5f);
    
    [SerializeField] float fireLifeTime = 2f;

    [SerializeField] GameObject fireZone;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().AddForce(velocity.normalized * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            SpreadFire(collision.transform.rotation.eulerAngles);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 roc = collision.transform.rotation.eulerAngles; // roc = Rotation Of Collision
            SpreadFire(new Vector3(roc.x, roc.y, roc.z + (collision.transform.rotation.eulerAngles.z == 0 ? 90 : 0)));

            //Debug.Log(collision.transform.rotation.eulerAngles.z);
        }
    }

    private void SpreadFire(Vector3 rotation)
    {
        GameObject fire = Instantiate(fireZone, transform.position, Quaternion.identity);
        
        fire.transform.localScale = fireRange;

        fire.transform.Rotate(rotation, Space.World);

        fire.GetComponent<DeadZone>().lifeTime = fireLifeTime;
        fire.GetComponent<Activable>().isActive = true;

        //Debug.Log(rotation);

        Destroy(gameObject);
    }
}
