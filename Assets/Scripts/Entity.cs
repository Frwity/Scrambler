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
        Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hitInfo, 3);
            
        if (hitInfo.collider && hitInfo.collider.CompareTag("HidingZone"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 2);
        }
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
    public bool isPlayerInSight = false;
    [HideInInspector]public bool LostPlayer = false;
    [HideInInspector]public Vector3 lastPlayerPosKnown;

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
        isHidden = false;
        entitySkill.ExitHiding();
    }
    public void InflictDamage(int damage)
    {
        life -= damage;
    }
    Vector2 Rotate(Vector2 aPoint, float aDegree)
    {
        return Quaternion.Euler(0,0,aDegree) * aPoint;
    }

    private void OnTriggerStay(Collider other)
    {
        collidingObj = other.gameObject;
        if (CompareTag("Player") || CompareTag("PossessZone"))
        {
            //Debug.Log($"collided with {gameObject.name}");
            return;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HidingZone"))
        {
            ExitHiding();
        }
        collidingObj = null;
    }
}

