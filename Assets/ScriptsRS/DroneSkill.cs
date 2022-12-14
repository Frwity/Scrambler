using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DroneSkill : EntitySkill
{
    [SerializeField] private float speed = 0f;
    [HideInInspector] public UnityEvent OnShoot = null;
    [HideInInspector] public Bertha myBertha = null;
    private float DestroyCD = 0.0f;
    
    private Entity entity = null;
    
    void Start()
    {
        entity = GetComponent<Entity>();
    }

    void Update()
    {
        DestroyCD += Time.smoothDeltaTime;
        if (myBertha == null && DestroyCD >1.0f)
        {
            Destroy(this.gameObject);    
        }
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
        OnShoot.Invoke();
        return true;
    }

    public override bool ActivateAI()
    {
        GetComponent<DroneAi>().isActive = true;
        
        return true;
    }

    public override bool DesactivateAI()
    {
        GetComponent<DroneAi>().isActive = false;
        entity.isPlayerInSight = false;
        return true;
    }

    public override void AimDirection(Vector3 direction)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<BulletSharedClass>().DoBehavior(gameObject);
        }
    }
}
