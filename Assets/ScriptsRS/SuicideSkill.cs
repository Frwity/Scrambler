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
    [SerializeField] private int nbJump;
    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0, 1)] private float airControlFactor;
    [SerializeField] [Range(0, 1)] private float accelerationFactor;


    
    private int jumped;
    private int wallDir;
    private bool falling;
    private bool grounded;
    private bool touchingWall;
    private Rigidbody rb;
    private float timer = 0;

    private GameObject player;
    //Start is called before the first frame update
    void Start()
    {
        grounded = false;
        touchingWall = false;
        falling = true;
        jumped = 0;
        wallDir = 0;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (rb.velocity.x > speed)
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);

        if (rb.velocity.x < -speed)
            rb.velocity = new Vector3(-speed, rb.velocity.y, rb.velocity.z);
        if (rb.velocity.y > speed)
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
        if (touchingWall && !grounded)
        {
            rb.velocity = new Vector3(jumpForce * wallDir, jumpForce, rb.velocity.z);
            jumped = 1;
            falling = false;
            touchingWall = false;
            
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
        if (!grounded && !touchingWall)
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
        else if (touchingWall)
        {
            
            rb.velocity = Vector3.zero;
            transform.Translate(new Vector3(0,-speed* wallDir * moveSpeed * accelerationFactor * Time.smoothDeltaTime, 0));
        }

        if (grounded && touchingWall)
        {
            transform.Translate(new Vector3(speed* moveSpeed * accelerationFactor * Time.smoothDeltaTime,0, 0));
        }
        else if (grounded)
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);
        
        
        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        if (!grounded && !touchingWall)
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
        else if (touchingWall)
        {
            
            rb.velocity = Vector3.zero;
            transform.Translate(new Vector3(0,-speed* wallDir * moveSpeed* accelerationFactor * Time.smoothDeltaTime, 0));
        }
        if (grounded && touchingWall)
        {
            Debug.Log("test");
            transform.Translate(new Vector3(speed* moveSpeed * accelerationFactor * Time.smoothDeltaTime,0, 0));
        }
        else if (grounded)
            rb.velocity = new Vector3(rb.velocity.x + speed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);
       
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            grounded = true;
            jumped = 0;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.useGravity = false;
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
            if (collision.gameObject.transform.position.x - transform.position.x > 0)
                wallDir = -1;
            else
                wallDir = 1;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallDir = 0;
            touchingWall = false;
            rb.useGravity = true;
        }
    }
}
