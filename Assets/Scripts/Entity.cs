using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySkill : MonoBehaviour
{
    public abstract bool Jump();
    public abstract bool MoveLeft();
    public abstract bool MoveRight();
    public abstract bool Shoot();
    public abstract bool Dash();
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
    [SerializeField] private EntitySkill entitySkill;
    [SerializeField] public bool controllable;

    public bool isHidden;

    private GameObject collidingObj;

    void Start()
    {
        isHidden = false; 
        collidingObj = null;
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

    public bool MoveLeft()
    {
        return entitySkill.MoveLeft();
    }

    public bool MoveRight()
    {
        return entitySkill.MoveRight();
    }

    public bool Dash()
    {
        return entitySkill.Dash();
    }

    public bool Shoot()
    {
        return entitySkill.Shoot();
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
}
