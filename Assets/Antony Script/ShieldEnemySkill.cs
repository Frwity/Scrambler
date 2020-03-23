using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemySkill : EntitySkill
{
    [SerializeField] private float speed;

    [SerializeField] private int nbJump;
    [SerializeField] private float jumpForce;
    private int jumped;
    private bool falling;
    private Rigidbody rb;

    [SerializeField] private float dashTime;
    private float dashed;
    private bool dashing;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCD;
    private float lastDash;

    void Start()
    {
        falling = true;
        dashing = false;
        dashed = 0;
        jumped = 0;
        lastDash = 0;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y >= -0.1 && rb.velocity.y <= 0.1)
            falling = true;
    }

    void Update()
    {
        if (dashing)
            transform.Translate(Time.deltaTime * dashSpeed, 0, 0);
        if (Time.time > dashTime + dashed)
        {
            dashing = false;
        }
    }

    public override bool Dash()
    {
        throw new System.NotImplementedException();
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

    public override bool MoveLeft()
    {
        transform.Translate(Time.deltaTime * -speed, 0, 0);
        return true;
    }

    public override bool MoveRight()
    {
        transform.Translate(Time.deltaTime * speed, 0, 0);
        return true;
    }

    public override bool Shoot()
    {
        if (Time.time > lastDash + dashCD)
        {
            dashing = true;
            dashed = Time.time;
            lastDash = Time.time;

            return true;
        }
        else
            return false;
    }

    public override bool ActivateAI()
    {
        GetComponent<ShieldEnemyAI>().isActive = true;
        return true;
    }

    public override bool DesactivateAI()
    {
        GetComponent<ShieldEnemyAI>().isActive = false;
        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            jumped = 0;
        }
    }
}
