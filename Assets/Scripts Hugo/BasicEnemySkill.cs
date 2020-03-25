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
    [SerializeField] private float speed;

    public short direction; // goes to -1 if moving left, 1 if moving right.

    [SerializeField] private float fireRate;
    private float lastFired;

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

    public override bool MoveLeft(float moveSpeed)
    {
        transform.Translate(Time.deltaTime * speed * moveSpeed, 0, 0, Space.World);
        direction = -1;

        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        transform.Translate(Time.deltaTime * speed * moveSpeed, 0, 0, Space.World);
        direction = 1;

        return true;
    }

    public override bool Shoot(Vector3 directionVector)
    {
        if (Time.time > fireRate + lastFired)
        {
            GameObject firedBullet = Instantiate(bullet, transform.position + Vector3.right * direction, Quaternion.identity);

            BasicBasicBullet bulletParameters = firedBullet.GetComponent<BasicBasicBullet>();

            firedBullet.transform.transform.Rotate(new Vector3(0, 0, 1), Random.Range(-bulletParameters.imprecision, bulletParameters.imprecision));
            firedBullet.GetComponent<Rigidbody>().AddForce(directionVector * bulletParameters.shootingPower);

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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            jumped = 0;
        }
    }
}