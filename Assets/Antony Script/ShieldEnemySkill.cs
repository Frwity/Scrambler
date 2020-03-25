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
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCD;
    [SerializeField] private int dashDamage;
    private float dashed;
    private bool dashing;
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
        if (Time.time > dashTime + dashed && dashing)
        {
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
            dashing = false;
        }
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

    public override bool MoveLeft(float moveSpeed)
    {
        if (!dashing)
            rb.velocity = new Vector3(speed * moveSpeed, rb.velocity.y, rb.velocity.z);
        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        if (!dashing)
            rb.velocity = new Vector3(speed * moveSpeed, rb.velocity.y, rb.velocity.z);
        return true;
    }

    public override bool Shoot(Vector3 direction)
    {
        if (Time.time > lastDash + dashCD && direction.magnitude > 0.1)
        {
            rb.velocity = new Vector3(dashForce * direction.normalized.x, dashForce * -direction.normalized.y, rb.velocity.z);
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

        if (dashing && gameObject.CompareTag("Player") && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Entity>().InflictDamage(dashDamage);
        }
        if (dashing && gameObject.CompareTag("Enemy") && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Entity>().InflictDamage(dashDamage);
        }
    }
}
