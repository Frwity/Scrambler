using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemySkill : EntitySkill
{
    [SerializeField] public float maxSpeed = 0f;
    [SerializeField] private int nbJump = 0;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private float wallFallSpeed = 0f;
    [SerializeField] [Range(0, 1)] private float airControlFactor = 0f;
    [SerializeField] [Range(0, 1)] private float accelerationFactor = 0f;
    [SerializeField] private bool canWallJump = false;

    [HideInInspector] public int jumped = 0;
    private int wallDir = 0;
    private bool falling = false;
    private bool grounded = false;
    private bool touchingWall = false;
    private Rigidbody rb = null;


    [SerializeField] private GameObject bullet = null;

    [SerializeField] private float fireRate = 0f;
    [SerializeField] private float imprecision = 0f; // is a measure in degrees, forming a cone of fire. 
    private float lastFired = 0f;

    [HideInInspector] public float xAim = 0f;

    Vector3 lastDirection = Vector3.zero;
    [SerializeField] [Range(0, 1)] float fireDirectionLerpingFactor = 0.05f;

    void Start()
    {
        grounded = false;
        touchingWall = false;
        falling = false;

        wallDir = 0;
        rb = GetComponent<Rigidbody>();
        lastFired = 0;

        lastDirection = -transform.right * (GetComponent<BasicEnemyAI>().flipped ? -1 : 1);
    }

    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        if (rb.velocity.x > maxSpeed)
            rb.velocity = new Vector3(maxSpeed, rb.velocity.y, rb.velocity.z);

        if (rb.velocity.x < -maxSpeed)
            rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, rb.velocity.z);

        if (rb.velocity.y < 0)
            falling = true;
    }

    public override bool Jump()
    {
        if (canWallJump && touchingWall && !grounded)
        {
            rb.velocity = new Vector3(jumpForce * wallDir, jumpForce, rb.velocity.z);
            jumped = 1;
            falling = false;
            touchingWall = false;
            return true;
        }
        else if (jumped < nbJump && falling)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumped++;
            falling = false;
            return true;
        }
        return false;
    }

    public override bool MoveLeft(float moveSpeed)
    {
        if (!grounded)
            rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
        else
            rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);

        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        if (!grounded)
            rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
        else
            rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);

        return true;
    }

    public override bool Shoot(Vector3 directionVector)
    {
        if ((GetComponent<BasicEnemyAI>().flipped && lastDirection.x < 0) || (!GetComponent<BasicEnemyAI>().flipped && lastDirection.x > 0))
        { 
            lastDirection = Vector3.zero;
        }

        Vector3 gunTransition = Vector3.zero;
        
        if (directionVector.magnitude < 0.1)
        {

            if (directionVector.x == 0)
            {
                gunTransition.x = GetComponent<BasicEnemyAI>().flipped ? 1 : -1;
            }
        }

        directionVector = Vector3.Lerp(lastDirection, gunTransition + directionVector, fireDirectionLerpingFactor);
        
        lastDirection = directionVector;
        transform.GetChild(2).transform.rotation = Quaternion.LookRotation(new Vector3(-directionVector.y, directionVector.x, 90), Vector3.forward);

        if (Time.time > fireRate + lastFired)
        {    
            GameObject firedBullet = Instantiate(bullet, transform.position + transform.right * shootOriginPos.x + transform.up * shootOriginPos.y, Quaternion.identity);

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
            collision.gameObject.GetComponent<BulletSharedClass>().DoBehavior(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            grounded = true;
            jumped = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            grounded = true;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            touchingWall = true;
            if (collision.gameObject.transform.position.x - transform.position.x > 0)
                wallDir = -1;
            else
                wallDir = 1;
            if (!grounded)
                transform.Translate(0, -wallFallSpeed * Time.deltaTime, 0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            grounded = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallDir = 0;
            touchingWall = false;
        }
    }

    public override void AimDirection(Vector3 direction)
    {
        xAim = direction.x;


        Vector3 gunTransition = Vector3.zero;

        if (direction.magnitude < 0.1)
        {

            if (direction.x == 0)
            {
                gunTransition.x = GetComponent<BasicEnemyAI>().flipped ? 1 : -1;
            }
        }

        direction = Vector3.Lerp(lastDirection, gunTransition + direction, fireDirectionLerpingFactor);

        lastDirection = direction;
        transform.GetChild(2).transform.rotation = Quaternion.LookRotation(new Vector3(-direction.y, direction.x, 90), Vector3.forward);

    }
}