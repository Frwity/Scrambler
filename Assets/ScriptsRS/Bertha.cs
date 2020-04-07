using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bertha : MonoBehaviour
{
    [SerializeField] private float Cooldown;
    [SerializeField] private float Imprecision;
    [SerializeField] private float shootForce = 0.0f;
    [SerializeField] private float damage;
    [SerializeField] private Entity Drone;
    [SerializeField] private GameObject bullet;
    private float currCooldown = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currCooldown < Cooldown)
        {
            currCooldown += Time.smoothDeltaTime;
            return;
        }
        if (Drone.isPlayerInSight)
        { 
            shootBullet();
            currCooldown = 0.0f;
        }
    }

    void shootBullet()
    {
        GameObject bu = Instantiate(bullet, transform.GetChild(0).position, transform.rotation);
        BerthaBullet bbu = bu.GetComponent<BerthaBullet>();
        bbu.toTP = Drone.transform;
        Physics.IgnoreCollision(bu.GetComponent<Collider>(), Drone.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(bu.GetComponent<Collider>(), GetComponent<Collider>(), true);
        Rigidbody rb = bu.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * shootForce);
            
    }
}
