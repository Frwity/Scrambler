using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityAI : Activable
{
    protected Entity entity;
}

public abstract class EntitySkill : MonoBehaviour
{
    [SerializeField] private float interactionRange;
    public Vector2 shootOriginPos;

    public abstract bool Jump();
    public abstract bool MoveLeft(float moveSpeed);
    public abstract bool MoveRight(float moveSpeed);
    public abstract bool Shoot(Vector3 direction);
    public abstract void AimDirection(Vector3 direction);
    public abstract bool ActivateAI();
    public abstract bool DesactivateAI();

    public bool InteractWithBG()
    {
        Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hitInfo, interactionRange);

        if (hitInfo.collider && hitInfo.collider.CompareTag("HidingZone"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, hitInfo.transform.position.z);
            return true;
        }
        else if (hitInfo.collider && hitInfo.collider.CompareTag("Node"))
        {
            hitInfo.collider.gameObject.GetComponent<Node>().Teleport(gameObject);
            return true;
        }
        return false;
    }
    public bool InteractWithFG()
    {
        Physics.Raycast(transform.position, Vector3.back, out RaycastHit hitInfo, interactionRange);

        if (hitInfo.collider && hitInfo.collider.CompareTag("HidingZone"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, hitInfo.transform.position.z);
            return true;

        }
        else if (hitInfo.collider && hitInfo.collider.CompareTag("Node"))
        {
            hitInfo.collider.gameObject.GetComponent<Node>().Teleport(gameObject);
            return true;
        }
        return false;
    }
    public void Uninteract()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position * shootOriginPos, 0.4f);
    }
}


public class Entity : MonoBehaviour
{
    [SerializeField] private int life;
    private int maxLife;
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

    [HideInInspector] public UnityEvent onPossess;

    Renderer[] renderers;
    Color[] originalColors;

    [SerializeField] ParticleSystem triQuartLifeParticle;
    [SerializeField] ParticleSystem halfLifeParticle;
    [SerializeField] ParticleSystem quartLifeParticle;
    [SerializeField] ParticleSystem shootingParticle;


    void Start()
    {
        maxLife = life;
        renderers = GetComponentsInChildren<Renderer>();
        isHidden = false; 
        collidingObj = null;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Vision"), LayerMask.NameToLayer("Ground"), true);

        if (gameObject.CompareTag("Player"))
        {
            renderers = transform.GetChild(4).GetComponents<Renderer>();
        }
        else
        { 
            renderers = GetComponentsInChildren<Renderer>();
        }

        originalColors = new Color[renderers.Length];

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
        if (entitySkill.Shoot(direction))
        {
            if (shootingParticle)
            {
                ParticleLauncher.ActivateParticleWithNewParent(shootingParticle.GetComponent<LifetimeStaticParticle>(), transform);
            }
            return true;
        }
        else
            return false;
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

    public bool InteractWithBG()
    {
        isHidden = true;
        return entitySkill.InteractWithBG();
    }
    public bool InteractWithFG()
    {
        isHidden = true;
        return entitySkill.InteractWithFG();
    }
    public void Uninteract()
    {
        isHidden = false;
        entitySkill.Uninteract();
    }
    public void InflictDamage(int damage)
    {
        life -= damage;
        if (life <= maxLife / 4 * 3)
        {
            if (triQuartLifeParticle)
                triQuartLifeParticle.Play();
        }
        if (life <= maxLife / 2)
        {
            if (halfLifeParticle)
                halfLifeParticle.Play();
        }
        if (life <= maxLife / 4)
        {
            if (quartLifeParticle)
                quartLifeParticle.Play();
        }

        if (gameObject.GetComponent<VirusSkill>())
        {
            if (life <= 2)
            {
                ParticleSystem.MainModule test = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().main;
                test.startColor = new Color(243, 126, 0, 0.04f);

                if (life <= 1)
                {
                    test.startColor = new Color(255, 0, 0, 0.04f);
                }
            }
        }
    }

    public void HitFlash()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = hitColor;
        }

        Invoke("ResetFlash", flashTime);
    }

    public virtual void PossessFlash()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = possessColor;
        }

        Invoke("ResetFlash", flashTime);
        
        onPossess.Invoke();
    }

    private void ResetFlash()
    {
        int counter = 0;
        if (renderers == null)
            return;

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = originalColors[counter];
            counter++;
        }
    }

    public void ResetEntity()
    {
        ResetFlash();

        isPlayerInSight = false;
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

