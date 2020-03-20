using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemySkill : EntitySkill
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float fireRate;
    private float lastFired;

    [SerializeField]
    private GameObject bullet;
    private GameObject body;

    void Start()
    {
        lastFired = 0;
        body = transform.GetChild(0).gameObject;
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
        if (Time.time > lastFired + fireRate)
        {
            Instantiate(bullet, body.transform.position + Vector3.down, Quaternion.identity);
            lastFired = Time.time;
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
}
