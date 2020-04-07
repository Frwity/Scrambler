using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemySkill : EntitySkill
{
    [SerializeField] private float speed;

    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate;
    private float lastFired;
    [SerializeField] private int MaxAngle;
    [SerializeField] private Vector3 toShoot;
    private int precision = 0;
    private float currentCD = 0.0f;

    void Start()
    {
        lastFired = 0;
    }

    void Update()
    {
        if (currentCD < fireRate)
            currentCD += Time.smoothDeltaTime;
    }

    public override bool Jump()
    {
        return false;
    }

    public override bool MoveLeft(float moveSpeed)
    {
        transform.Translate(Time.deltaTime * speed * moveSpeed, 0, 0);
        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        transform.Translate(Time.deltaTime * speed * moveSpeed, 0, 0);
        return true;
    }

    public override bool Shoot(Vector3 direction)
    {
        //toShoot = direction;
        //if (Time.time > lastFired + fireRate)
        
        if (currentCD > fireRate)
        {
            currentCD = 0.0f;
            Instantiate(bullet, transform.position + Vector3.down, Quaternion.identity);
            BulletSharedClass b = bullet.GetComponent<BulletSharedClass>();
            
            //Debug.Log(toShoot);
            
           
            b.direction = toShoot.normalized;
            lastFired = Time.time;
            return true;
        }
        else
            return false;
    }

    public override bool ActivateAI()
    {
        GetComponent<FlyingEnemyIA>().isActive = true;
        return true;
    }

    public override bool DesactivateAI()
    {
        GetComponent<FlyingEnemyIA>().isActive = false;
        return true;
    }

    public override void AimDirection(Vector3 direction)
    {
        toShoot = direction;
        if (toShoot.y > -0.05)
        {
            toShoot.y = -0.05f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<BulletSharedClass>().doBehavior(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
