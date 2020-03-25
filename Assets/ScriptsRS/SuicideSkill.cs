using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideSkill : EntitySkill
{
    [SerializeField] private float speed;
    [SerializeField] private float chargeTime;
    [SerializeField] private float exploRay;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
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
        throw new System.NotImplementedException();
    }

    public override bool MoveLeft(float moveSpeed)
    {
        transform.Translate(Time.deltaTime * -speed, 0, 0);
        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        transform.Translate(Time.deltaTime * speed, 0, 0);
        return true;
    }

    public override bool Shoot(Vector3 direction)
    {
        timer += Time.smoothDeltaTime;
        Debug.Log($"timer : {timer} / {chargeTime}");
        if (timer >= chargeTime)
        {
            GameObject player =GameObject.FindWithTag("Player");
            Vector3 toPlayer = player.transform.position - transform.position;
            if (toPlayer.magnitude < exploRay)
            {
                player.GetComponent<Entity>().InflictDamage(6);
            }
            return true;
        }
        return false;
    }

    public override bool ActivateAI()
    {
        throw new System.NotImplementedException();
    }

    public override bool DesactivateAI()
    {
        throw new System.NotImplementedException();
    }
}
