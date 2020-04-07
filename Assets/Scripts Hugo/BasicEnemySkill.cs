using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemySkill : EntitySkill
{
    [SerializeField] private int nbJump;
    [SerializeField] private float jumpForce;

    private int jumped;
    private bool falling;
    private Rigidbody rb;

    [SerializeField] private GameObject bullet;
    
    [SerializeField] private float moveSpeed;

    [SerializeField] private float fireRate;
    [SerializeField] private float imprecision; // is a measure in degrees, forming a cone of fire. 
    private float lastFired;

    [HideInInspector] public float xAim = 0f;

    void Start()
    {
        falling = true;
        jumped = 0;
        rb = GetComponent<Rigidbody>();
        lastFired = 0;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y >= -0.1 && rb.velocity.y <= 0.1)
            falling = true;
    }

    public override bool Jump()
    {
        if (jumped < nbJump && falling)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumped++;
            falling = false;
            return true;
        }
        return false;
    }

    public override bool MoveLeft(float _moveSpeed)
    {
        rb.velocity = new Vector3(moveSpeed * _moveSpeed, rb.velocity.y, rb.velocity.z);

        return true;
    }

    public override bool MoveRight(float _moveSpeed)
    {
        rb.velocity = new Vector3(moveSpeed * _moveSpeed, rb.velocity.y, rb.velocity.z);

        return true;
    }

    public override bool Shoot(Vector3 directionVector)
    {
        if (Time.time > fireRate + lastFired && directionVector != new Vector3(0, 0, 0))
        {
            GameObject firedBullet = Instantiate(bullet, transform.position - transform.right, Quaternion.identity);

            directionVector = Quaternion.Euler(0, 0, Random.Range(-imprecision, imprecision)) * directionVector;

            BulletSharedClass firedBulletInfo = firedBullet.GetComponent<BulletSharedClass>();
            firedBulletInfo.direction = directionVector.normalized;
            firedBulletInfo.shooter = gameObject;

            lastFired = Time.time;
            return true;
        }
        else
            return false;
    }

    public override bool ActivateAI()
    {
        GetComponent<BasicEnemyAI>().isActive = true;
        return true;
    }

    public override bool DesactivateAI()
    {
        GetComponent<BasicEnemyAI>().isActive = false;
        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<BulletSharedClass>().doBehavior(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            jumped = 0;
        }
    }

    public override void AimDirection(Vector3 direction)
    {
        xAim = direction.x;
    }
}