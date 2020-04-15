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

    private DroneSkill ds;
    [SerializeField] private GameObject bullet;
    private float currCooldown = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Teleport>().toTP = Drone.transform;
        
        if (Drone != null)
        {
            ds = Drone.GetComponent<DroneSkill>();
            ds.OnShoot.AddListener(shootBullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Drone)
            return;

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
        if (!Drone)
            return;

        if (currCooldown < Cooldown)
        {
            return;
        }

        GameObject bu = Instantiate(bullet, transform.position + transform.up * 10, Quaternion.identity);
        bu.transform.localScale += Vector3.one;
        
        Physics.IgnoreCollision(bu.GetComponent<Collider>(), Drone.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(bu.GetComponent<Collider>(), GetComponent<Collider>(), true);
        
        Rigidbody rb = bu.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * shootForce, ForceMode.Impulse);

        BulletSharedClass buSCI = bu.GetComponent<BulletSharedClass>();
        buSCI.shooter = gameObject;
        buSCI.direction = Vector3.up;

        
        currCooldown = 0;
    }
}
