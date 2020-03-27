using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideSkill : EntitySkill
{
    [SerializeField] private float speed;
    [SerializeField] private float chargeTime;
    [SerializeField] private float exploRay;
    [SerializeField] private int damage;
    private float timer = 0;

    private GameObject player;
    //Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetTimer()
    {
        timer = 0;
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
        timer += Time.smoothDeltaTime;
        //Debug.Log($"timer : {timer} / {chargeTime}");
        if (timer >= chargeTime)
        {
            Entity[] entitylist = GameObject.FindObjectsOfType<Entity>();
            foreach (var entity in entitylist)
            {
                if (!GetComponent<suicideIA>().isActive )
                {
                    if (entity.CompareTag("Player"))
                    {
                        continue;
                    }
                }
                Vector3 toEnt = entity.transform.position - transform.position;
                if (toEnt.magnitude < exploRay)
                {
                    entity.InflictDamage(6);
                    //Debug.LogWarning($"inflicted damage to {entity.name}");
                }
            }

            PlayerControl pl = gameObject.GetComponentInParent<PlayerControl>();
            
            pl.lastControl = Time.time;
            pl.entity.transform.parent = transform.parent;
            pl.entity.ActivateAI();
            pl.entity.tag = "Enemy";
            pl.entity = Instantiate(pl.virus, pl.entity.transform.position + (Vector3.up * 3), Quaternion.identity, pl.transform).GetComponent<Entity>();

            pl.isInVirus = true;
            
            Destroy(pl.transform.GetChild(0).gameObject);
            return true;
        }
        return false;
    }

    public override bool ActivateAI()
    {
        GetComponent<suicideIA>().isActive = true;
        ResetTimer();
        return true;
    }

    public override bool DesactivateAI()
    {
        GetComponent<suicideIA>().isActive = false;
        ResetTimer();
        return true;
    }

    public override void AimDirection(Vector3 direction)
    {

    }
}
