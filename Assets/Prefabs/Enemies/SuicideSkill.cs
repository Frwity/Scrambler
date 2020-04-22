using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideSkill : EntitySkill
{
    [SerializeField] public float speed = 0f;
    [SerializeField] private float chargeTime = 0f;
    [SerializeField] public float exploRay = 0f;
    //[SerializeField] private int damage = 0;
    [SerializeField] private int nbJump = 0;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] [Range(0, 1)] private float airControlFactor = 0f;
    [SerializeField] [Range(0, 1)] private float accelerationFactor = 0f;

    enum Direction
    {
        RIGHT = 1,
        LEFT = -1,
    };

    
    private int jumped = 0;
    private int wallDir = 0;
    private bool falling = false;
    public bool grounded = false;
    public bool touchingWall = false;
    public bool Roofed = false;
    
    private Rigidbody rb = null;
    private float timer = 0;
    private Direction direction = Direction.RIGHT;
    private suicideIA ia = null;
    
    //Start is called before the first frame update
    void Start()
    {
        grounded = false;
        touchingWall = false;
        falling = true;
        Roofed = false;
        jumped = 0;
        wallDir = 0;
        rb = GetComponent<Rigidbody>();
        ia = GetComponent<suicideIA>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ia.isActive)
        {
            direction = (Direction)ia.direction;
        }
       
    }
    private void FixedUpdate()
    {
        if (rb.velocity.x > speed)
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);

        if (rb.velocity.x < -speed)
            rb.velocity = new Vector3(-speed, rb.velocity.y, rb.velocity.z);
        if (rb.velocity.y > speed && touchingWall)
        {
            rb.velocity = new Vector3(rb.velocity.x, speed, rb.velocity.z);
        }

        if (rb.velocity.y < 0)
            falling = true;
    }
    public void ResetTimer()
    {
        timer = 0;
    }

    public override bool Jump()
    {
        if (touchingWall && !grounded && !Roofed)
        {
            
            rb.velocity = new Vector3(jumpForce * wallDir, jumpForce, rb.velocity.z);
            jumped = 1;
            falling = false;
            touchingWall = false;
            
            return true;
        }
        else if (Roofed)
        {
            rb.velocity = new Vector3(rb.velocity.x, -jumpForce, rb.velocity.z);
            Roofed = false;
            return true;
        }
        else if (jumped < nbJump && falling)
        {
            
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumped++;
            falling = false;
            

            return true;
        }
        return false;
    }

    public override bool MoveLeft(float moveSpeed)
    {
        int i = 1;
        if (!ia.isActive)
        {
            if (direction != Direction.LEFT)
            {
                direction = Direction.LEFT;
                
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            i *= -1;
        }
        if (!grounded && !touchingWall && !Roofed)
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
        else if (Roofed && touchingWall)
        {
                    rb.velocity = Vector3.zero;
                    float xy = -(2*speed) * wallDir * moveSpeed * accelerationFactor * Time.smoothDeltaTime;
                    transform.Translate(new Vector3(xy,xy, 0));
                    
        }
        else if (touchingWall)
        {
            
            rb.velocity = Vector3.zero;
            Vector3 f = new Vector3(0, (speed) * wallDir * moveSpeed, 0);
            Vector3 v = (f / rb.mass) * Time.smoothDeltaTime;
            transform.Translate(v);
            
            
        }
        else if (Roofed)
        {
            rb.velocity = Vector3.zero;
            Vector3 f = new Vector3(-(speed) * moveSpeed ,0, 0);
            Vector3 v = (f / rb.mass) * Time.smoothDeltaTime;
            transform.Translate(v);
            

        }

        if (grounded && touchingWall)
        {
            moveSpeed = Mathf.Abs(moveSpeed);
            Vector3 f = new Vector3((speed)* moveSpeed* 0.1f* -wallDir *i ,-speed*wallDir*0.1f, 0);
            Vector3 v = (f / rb.mass) * Time.fixedDeltaTime;
            transform.Translate(v);
         
        }
        else if (grounded)
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);
        
       
        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        if (!ia.isActive)
        {
            if (direction != Direction.RIGHT)
            {
                direction = Direction.RIGHT;
                
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        if (!grounded && !touchingWall && !Roofed)
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
        else if (Roofed && touchingWall)
        {
            rb.velocity = Vector3.zero;
            float xy = -(2*speed) * moveSpeed * wallDir * accelerationFactor * Time.smoothDeltaTime;
            
            transform.Translate(new Vector3(xy,xy, 0));
            
        }
        else if (touchingWall)
        {
            
            rb.velocity = Vector3.zero;
            Vector3 f = new Vector3(0, (speed) * wallDir * moveSpeed, 0);
            Vector3 v = (f / rb.mass) * Time.smoothDeltaTime;
            transform.Translate(v);
            
        }
        else if (Roofed)
        {
            rb.velocity = Vector3.zero;
            
            Vector3 f = new Vector3((-speed) * moveSpeed,0, 0);
            Vector3 v = (f / rb.mass) * Time.smoothDeltaTime;
            transform.Translate(v);
        }
        if (grounded && touchingWall)
        {
            moveSpeed = Mathf.Abs(moveSpeed);
         
            Vector3 f = new Vector3((speed) * moveSpeed * 0.1f * -wallDir, speed*wallDir*0.1f, 0);
            Vector3 v = (f / rb.mass) * Time.smoothDeltaTime;
            transform.Translate(v);

        }
        else if (grounded)
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * accelerationFactor , rb.velocity.y, rb.velocity.z);
        
     
        return true;
    }

    public override bool Shoot(Vector3 direction)
    {
        timer += Time.smoothDeltaTime;
        
        if (timer >= chargeTime)
        {
            Entity[] entitylist = FindObjectsOfType<Entity>();
            foreach (var entity in entitylist)
            {
                if (!GetComponent<suicideIA>().isActive )
                {
                    if (entity.CompareTag("Player"))
                    {
                        continue;
                    }
                }

                Vector3 toEnt = entity.transform.position - (transform.position + transform.right * shootOriginPos.x + transform.up * shootOriginPos.y);
                if (toEnt.magnitude < exploRay)
                {
                    entity.InflictDamage(damage);
                  
                }
            }

            ParticleLauncher.ActivateParticleWithName(gameObject, null, "BigExplosion");

            PlayerControl pl = gameObject.GetComponentInParent<PlayerControl>();
            if (!pl)
                return true;

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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<BulletSharedClass>().DoBehavior(gameObject);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            grounded = true;
            jumped = 0;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.useGravity = false;
        }
        if (collision.gameObject.CompareTag("Roof"))
        {
            Roofed = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
        if (collision.gameObject.CompareTag("Wall"))
        { 
            touchingWall = true;
            if ( collision.gameObject.transform.position.x >transform.position.x + 0.015*speed)
                wallDir = 1;
            else
                wallDir = -1;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
        if (collision.gameObject.CompareTag("Wall") )
        {
            wallDir = 0;
            touchingWall = false;
            if (!Roofed)
                rb.useGravity = true;
        }
        if (collision.gameObject.CompareTag("Roof"))
        {
            Roofed = false;
            if(!touchingWall)
                rb.useGravity = true;
            
            
        }
    }

    private void OnDrawGizmos()
    {
        Color c = Color.red;
        c.a = 0.3f;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, exploRay);
    }
}
