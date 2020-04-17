using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySkill : MonoBehaviour
{
    [SerializeField] private float interactionRange;
    [SerializeField] protected Vector2 shootOriginPos;

    public abstract bool Jump();
    public abstract bool MoveLeft(float moveSpeed);
    public abstract bool MoveRight(float moveSpeed);
    public abstract bool Shoot(Vector3 direction);
    public abstract void AimDirection(Vector3 direction);
    public abstract bool ActivateAI();
    public abstract bool DesactivateAI();

    public void InteractWithBG()
    {
        Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hitInfo, interactionRange);

        if (hitInfo.collider && hitInfo.collider.CompareTag("HidingZone"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, hitInfo.transform.position.z);
        }
        else if (hitInfo.collider && hitInfo.collider.CompareTag("Node"))
        {
            hitInfo.collider.gameObject.GetComponent<Node>().Teleport(gameObject);
        }
    }
    public void InteractWithFG()
    {
        Physics.Raycast(transform.position, Vector3.back, out RaycastHit hitInfo, interactionRange);

        if (hitInfo.collider && hitInfo.collider.CompareTag("HidingZone"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, hitInfo.transform.position.z);
        }
        else if (hitInfo.collider && hitInfo.collider.CompareTag("Node"))
        {
            hitInfo.collider.gameObject.GetComponent<Node>().Teleport(gameObject);
        }
    }
    public void Uninteract()
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
    /*[HideInInspector]*/public bool isPlayerInSight = false;
    [HideInInspector]public bool LostPlayer = false;
    /*[HideInInspector]*/public Vector3 lastPlayerPosKnown;
    public bool isInBackGround = false;

    [SerializeField] Color hitColor = new Color(1, 0, 0);
    [SerializeField] Color possessColor = new Color(0, 0, 1);
    [SerializeField] float flashTime = 0.5f;

    Renderer[] renderers;

    Color[] originalColors;

    void Start()
    {
        isHidden = false; 
        collidingObj = null;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Vision"), LayerMask.NameToLayer("Ground"), true);

        renderers = GetComponentsInChildren<Renderer>();

        int counter = 0;
        foreach (Renderer renderer in renderers)
        {
            originalColors[counter] = renderer.material.color;
            counter++;
        }
    }

    void Update()
    {
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        collidingObj = null;
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

    public void InteractWithBG()
    {
        isHidden = true;
        entitySkill.InteractWithBG();
    }
    public void InteractWithFG()
    {
        isHidden = true;
        entitySkill.InteractWithFG();
    }
    public void Uninteract()
    {
        isHidden = false;
        entitySkill.Uninteract();
    }
    public void InflictDamage(int damage)
    {
        life -= damage;
    }

    public void HitFlash()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = hitColor;
        }

        Invoke("ResetFlash", flashTime);
    }

    private void ResetFlash()
    {
        int counter = 0;
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = originalColors[counter];
            counter++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Vision"))
        {
            return;
        }
        collidingObj = other.gameObject;
        

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HidingZone"))
        {
            Uninteract();
        }
        collidingObj = null;
    }
}

