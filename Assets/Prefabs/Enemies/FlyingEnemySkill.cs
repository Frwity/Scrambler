using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemySkill : EntitySkill
{
    [SerializeField] private float speed;

    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate;

    private Vector3 lastDirection;
    private Vector3 toShoot;
    private float currentCD = 0.0f;

    void Start()
    {
        lastDirection = Vector3.down;
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
        if (direction.magnitude > 0.1)
            lastDirection = direction;
        if (currentCD > fireRate)
        {
            currentCD = 0.0f;

            GameObject firedBullet = Instantiate(bullet, transform.position + Vector3.down, Quaternion.identity);

            BulletSharedClass firedBulletInfo = firedBullet.GetComponent<BulletSharedClass>();
            firedBulletInfo.direction = lastDirection.normalized;
            firedBulletInfo.shooter = gameObject;

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
        }
    }
}
