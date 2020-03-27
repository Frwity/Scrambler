using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySkill : MonoBehaviour
{
    public abstract bool Jump();
    public abstract bool MoveLeft(float moveSpeed);
    public abstract bool MoveRight(float moveSpeed);
    public abstract bool Shoot(Vector3 direction);
    public abstract void AimDirection(Vector3 direction);
    public abstract bool ActivateAI();
    public abstract bool DesactivateAI();
    public void HideInBG()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 2);
    }
    public void ExitHiding()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}


public class Entity : MonoBehaviour
{
    [SerializeField] private int life;
    [SerializeField] public EntitySkill entitySkill;
    [SerializeField] public bool controllable;
    public bool isHidden;

    private GameObject collidingObj;

    void Start()
    {
        isHidden = false; 
        collidingObj = null;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Vision"), LayerMask.NameToLayer("Ground"), true);
        
    }

    void Update()
    {
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool Jump()
    {
        return entitySkill.Jump();
    }

    public bool MoveLeft(float moveSpeed)
    {
        return entitySkill.MoveLeft(moveSpeed);
    }

    public bool MoveRight(float moveSpeed)
    {
        return entitySkill.MoveRight(moveSpeed);
    }

    public bool Shoot(Vector3 direction)
    {
        return entitySkill.Shoot(direction);
    }

    public void AimDirection(Vector3 direction)
    {
        entitySkill.AimDirection(direction);
    }

    public bool ActivateAI()
    {
        return entitySkill.ActivateAI();
    }

    public bool DesactivateAI()
    {
        return entitySkill.DesactivateAI();
    }

    public GameObject Collinding()
    {
        return collidingObj;
    }

    public bool isControllable()
    {
        return controllable;
    }

    public void HideInGB()
    {
        isHidden = true;
        entitySkill.HideInBG();
    }
    public void ExitHiding()
    {
        entitySkill.ExitHiding();
        isHidden = false;
    }
    public void InflictDamage(int damage)
    {
        life -= damage;
    }

    private void OnCollisionStay(Collision collision)
    {
        collidingObj = collision.gameObject;
    }
    private void OnCollisionExit(Collision collision)
    {
        collidingObj = null;
    }
    Vector2 Rotate(Vector2 aPoint, float aDegree)
    {
        return Quaternion.Euler(0,0,aDegree) * aPoint;
    }

    private void OnTriggerStay(Collider other)
    {
        if (CompareTag("Player"))
        {
            return;
        }
        float mag = (other.transform.position - transform.position).magnitude;
        //int visionMask = LayerMask.NameToLayer("Vision");
        int visionMask = 1 << 13 ;
        int hoverMask = 1 << 8;
        int bulletMask = 1 << 10;
        int shieldMask = 1 << 14;
        int PossessMask = 1 << 15;
        bulletMask = ~bulletMask;
        hoverMask = ~hoverMask;
        visionMask = ~visionMask;
        shieldMask = ~shieldMask;
        PossessMask = ~PossessMask;
        
        visionMask += hoverMask;
        visionMask += bulletMask;
        visionMask += shieldMask;
        visionMask += PossessMask;
        Vector3 playerPos;
        
        if (other.CompareTag("Player"))
        {
            playerPos = other.transform.position;
            //Debug.Log("player found");
        }
        else
        {
            //Debug.Log("player not found");
            return;
        }

        playerPos = (playerPos - transform.position);
        playerPos.y -= other.transform.localScale.y/2.0f;
        
        for (int i = 0; i <= 2; i++)
        {
            Vector2 toShoot = playerPos;
            toShoot.y += (other.transform.localScale.y / 2.0f)* i;
            toShoot.Normalize();
            Ray ray = new Ray(transform.position, toShoot);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, mag + 10.0f, visionMask))
            {
                Debug.Log("ouch");
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(transform.position, toShoot * hit.distance, Color.magenta);
                }
                else
                {
                    Debug.Log(hit.collider.name);
                    Debug.DrawRay(transform.position, toShoot * hit.distance, Color.green);
                }
            }
        }

    }

}

