using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemySkill : EntitySkill
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float speed;

    public short direction; // goes to -1 if moving left, 1 if moving right.

    [SerializeField] private float fireRate;
    private float lastFired;

    void Start()
    {
        lastFired = 0;
    }

    void Update()
    {
        
    }

    public override bool Dash()
    {
        throw new System.NotImplementedException();
    }

    public override bool Jump()
    {
        throw new System.NotImplementedException();
    }

    public override bool MoveLeft()
    {
        transform.Translate(Time.deltaTime * -speed, 0, 0, Space.World);
        direction = -1;

        return true;
    }

    public override bool MoveRight()
    {
        transform.Translate(Time.deltaTime * speed, 0, 0, Space.World);
        direction = 1;

        return true;
    }

    public override bool Shoot()
    {
        if (Time.time > fireRate + lastFired)
        {
            GameObject firedBullet = Instantiate(bullet, transform.position + Vector3.right * direction, Quaternion.identity);

            BasicBasicBullet bulletParameters = firedBullet.GetComponent<BasicBasicBullet>();

            firedBullet.transform.transform.Rotate(new Vector3(0, 0, 1), Random.Range(-bulletParameters.imprecision, bulletParameters.imprecision));
            firedBullet.GetComponent<Rigidbody>().AddForce(firedBullet.transform.right * bulletParameters.shootingPower * direction);

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
}