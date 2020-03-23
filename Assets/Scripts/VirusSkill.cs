using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSkill : EntitySkill
{
    [SerializeField] private float speed;
    [SerializeField] private int nbJump;
    [SerializeField] private float jumpForce;

    private int jumped;
    private bool falling;
    private Rigidbody rb;

    void Start()
    {
        falling = true;
        jumped = 0;
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
        return false;
    }

    public override bool Dash()
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            jumped = 0;
        }
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
}
