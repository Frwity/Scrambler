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

    [SerializeField] private float slamCD;
    [SerializeField] private int slamDamage;
    private float lastSlam;

    [HideInInspector] public GameObject shieldSlamCollider;

    private GameObject shield;

    void Start()
    {
        falling = true;
        jumped = 0;
        lastSlam = 0;
        rb = GetComponent<Rigidbody>();
        shield = transform.GetChild(2).gameObject;
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
        if (Time.time > lastSlam + slamCD && direction.magnitude > 0.1)
        {
            if (shieldSlamCollider)
                shieldSlamCollider.GetComponent<Entity>().InflictDamage(slamDamage);
            lastSlam = Time.time;
            return true;
        }
        else
            return false;
    }

    public override void AimDirection(Vector3 direction)
    {
        float tempY;
        if (direction.y < 0.15f)
            tempY = 0.15f;
        else
            tempY = direction.y;
        if (direction.magnitude > 0.3)
        {
            shield.transform.rotation = Quaternion.LookRotation(new Vector3(-direction.x, -tempY, 90), Vector3.forward);
            transform.GetChild(1).localRotation = Quaternion.LookRotation(new Vector3(-direction.x, -tempY, 90), Vector3.forward);
            transform.GetChild(1).position = transform.position + (transform.position - shield.gameObject.transform.GetChild(0).transform.GetChild(0).position) * 1.5f;
        }
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
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<BulletSharedClass>().doBehavior(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            jumped = 0;
        }

    }
}
