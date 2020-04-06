﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : BulletSharedClass
{
    [SerializeField] Vector3 fireRange = new Vector3(1f, 0.5f, 0.5f);

    [SerializeField] GameObject fireZone;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            SpreadFire(collision.transform.rotation.eulerAngles);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 roc = collision.transform.rotation.eulerAngles; // roc = Rotation Of Collision
            SpreadFire(new Vector3(roc.x, roc.y, roc.z + (collision.transform.rotation.eulerAngles.z == 0 ? 90 : 0)));

            Debug.Log(collision.transform.rotation.eulerAngles.z);
        }
    }

    public override void doBehavior(GameObject hitObject)
    {
        //SpreadFire(hitObject.transform.rotation);
    }

    private void SpreadFire(Vector3 rotation)
    {
        GameObject fire = Instantiate(fireZone, transform.position, Quaternion.identity);
        
        fire.transform.localScale = fireRange;

        fire.transform.Rotate(rotation, Space.World);

        Debug.Log(rotation);

        Destroy(gameObject);
    }
}
