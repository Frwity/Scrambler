using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSkill : EntitySkill
{
    [SerializeField] private float speed;
    [SerializeField] private int nbJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallFallSpeed;
    [SerializeField] [Range(0, 1)] private float airControlFactor;
    [SerializeField] [Range(0, 1)] private float accelerationFactor;

    [HideInInspector] public int jumped;
    private int wallDir;
    private bool falling;
    private bool grounded;
    private bool touchingWall;
    private Rigidbody rb;

    void Start()
    {
        grounded = false;
        touchingWall = false;
        falling = false;
        //jumped = 0;
        wallDir = 0;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (rb.velocity.x > speed)
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);

        if (rb.velocity.x < -speed)
            rb.velocity = new Vector3(-speed, rb.velocity.y, rb.velocity.z);

        if (rb.velocity.y < 0)
            falling = true;
    }

    public override bool Jump()
    {
        if (touchingWall && !grounded)
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
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
        else
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);

        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        if (!grounded)
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
        else
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);

        return true;
    }

    public override bool Shoot(Vector3 direction)
    {
        return false;
    }

    public override bool ActivateAI()
    {
        return true;
    }

    public override bool DesactivateAI()
    {
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

    }
}
