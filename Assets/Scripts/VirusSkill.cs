using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSkill : EntitySkill
{
    [SerializeField] private float speed;
    [SerializeField] private int nbJump;
    [SerializeField] private float jumpForce;



    private int jumped;
    private int wallDir;
    private bool falling;
    private bool touchingWall;
    private Rigidbody rb;

    void Start()
    {
        touchingWall = false;
        falling = true;
        jumped = 0;
        wallDir = 0;
        rb = GetComponent<Rigidbody>();
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
        if (touchingWall)
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
        rb.velocity = new Vector3(speed * moveSpeed, rb.velocity.y, rb.velocity.z);
        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        rb.velocity = new Vector3(speed * moveSpeed, rb.velocity.y, rb.velocity.z);
        return true;
    }

    public override bool Shoot(Vector3 direction)
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            jumped = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        { 
            touchingWall = true;
            if (collision.gameObject.transform.position.x - transform.position.x > 0)
                wallDir = -1;
            else
                wallDir = 1;
        }
        else
        {
            wallDir = 0;
            touchingWall = false;
        }
    }
}
