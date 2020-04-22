using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSkill : EntitySkill
{
    [SerializeField] private float maxSpeed = 0f;
    [SerializeField] private int nbJump = 0;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private float wallFallSpeed = 0f;
    [SerializeField] [Range(0, 1)] private float airControlFactor = 0f;
    [SerializeField] [Range(0, 1)] private float accelerationFactor = 0f;
    [SerializeField] private bool canWallJump = false;

    [HideInInspector] public int jumped = 0;
    private int wallDir = 0;
    private bool falling = false;
    private bool grounded = false;
    private bool touchingWall = false;
    private Rigidbody rb = null;

    // Start is called before the first frame update
    [SerializeField] private int angleInDeg = 0;
    [SerializeField] private int precision = 0;
    [SerializeField] private float shootingStrength = 0f;
    [SerializeField] private float cooldown = 0f;
    [SerializeField] private int numberOfProjectile = 1;    
    [SerializeField] private GameObject bulletPrefab = null;

    public float rangePoint = 0f;
    private Direction dir = Direction.NONE;
    private Vector2 shootingDir = Vector2.zero;
    private float currentCooldown = 0f;
    private int lastAngle = 0;
    private int lastRotationAngle = 0;
    private int angleRotated = 0;

    [SerializeField] private GameObject cannon = null;

    Vector2 Rotate(Vector2 aPoint, float aDegree)
    {
        return Quaternion.Euler(0, 0, aDegree) * aPoint;
    }

    public void changeRotation()
    {
        shootingDir.x = -shootingDir.x;
    }

    void Start()
    {

        grounded = false;
        touchingWall = false;
        falling = false;

        wallDir = 0;
        rb = GetComponent<Rigidbody>();

        angleRotated = angleInDeg;
        int a = angleRotated + precision/2;
        rangePoint = (shootingStrength / Physics.gravity.magnitude)
                     * Mathf.Cos(a * Mathf.Deg2Rad)
                     * (shootingStrength * Mathf.Sin(a * Mathf.Deg2Rad)
                        + Mathf.Sqrt(Mathf.Pow(shootingStrength * Mathf.Sin(a * Mathf.Deg2Rad), 2)
                                     + 5* Physics.gravity.magnitude * cannon.transform.localPosition.y))
                     ;

        shootingDir = Rotate(Vector3.right, angleInDeg);
        lastAngle = angleInDeg;
        currentCooldown = 0;
        dir = Direction.NONE;
    }

    void Update()
    {
        if (lastAngle != angleInDeg || lastRotationAngle != (int)transform.rotation.y)
        {
            lastAngle = angleInDeg;
            lastRotationAngle = (int)transform.rotation.y;
        }
        if (currentCooldown > 0.0f)
        {
            currentCooldown -= Time.smoothDeltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.x > maxSpeed)
            rb.velocity = new Vector3(maxSpeed, rb.velocity.y, rb.velocity.z);

        if (rb.velocity.x < -maxSpeed)
            rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, rb.velocity.z);

        if (rb.velocity.y < 0)
            falling = true;
    }

    public override bool Jump()
    {
        if (canWallJump && touchingWall && !grounded)
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
        if (!GetComponent<TankIA>().isActive)
        {
            //if (Input.GetAxis("RHorizontal") < -0.05f && dir != Direction.LEFT)
            //{
            //    transform.rotation = Quaternion.Euler(0, 180, 0);
            //    dir = Direction.LEFT;
            //}
            //else if (Input.GetAxis("RHorizontal") > 0.05f && dir != Direction.RIGHT)
            //{
            //    transform.rotation = Quaternion.Euler(0, 0, 0);
            //    dir = Direction.RIGHT;

            //}
            if (!grounded)
                rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
            else
                rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);
        }
        else
        { 
            rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);
            return true;
        }
        return false;
    }

    public override bool MoveRight(float moveSpeed)
    {
        if (!GetComponent<TankIA>().isActive)
        {
            if (!grounded)
                rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * airControlFactor, rb.velocity.y, rb.velocity.z);
            else
                rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x + maxSpeed * moveSpeed * accelerationFactor, rb.velocity.y, rb.velocity.z);
            return true;
        }
        return false;
    }

    public override bool Shoot(Vector3 direction)
    {
        if (currentCooldown > 0)
            return false;

        for (int i = 0; i < numberOfProjectile; i++)
        {
            int angleToAdd = Random.Range(-precision, precision);
            Vector2 newShootDir = Rotate(shootingDir, angleToAdd);

            GameObject bulletInst = Instantiate(bulletPrefab, cannon.transform.position + transform.right * shootOriginPos.x + transform.up * shootOriginPos.y, transform.rotation);
            
            Physics.IgnoreCollision(bulletInst.GetComponent<Collider>(),
                                    GetComponent<Collider>(), true);

            CurvedBulletSharedClass bullett = bulletInst.GetComponent<CurvedBulletSharedClass>();
            bullett.direction = new Vector3(newShootDir.x, newShootDir.y, 0) * shootingStrength;
            bullett.shooter = gameObject;

        }

        currentCooldown = cooldown;
        return true;
    }

    public override bool ActivateAI()
    {
        GetComponent<TankIA>().direction = dir;
        GetComponent<TankIA>().isActive = true;
        return true;
    }

    public override bool DesactivateAI()
    {
        dir = GetComponent<TankIA>().direction;
        GetComponent<TankIA>().isActive = false;
        return true;
    }

    public override void AimDirection(Vector3 direction)
    {
        if (direction.x < -0.05f && dir != Direction.LEFT)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
            dir = Direction.LEFT;
            changeRotation();
            
            
        }
        else if (direction.x > 0.05f && dir != Direction.RIGHT)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            dir = Direction.RIGHT;
            changeRotation();
            
        }

        if (Mathf.Abs(direction.x) > 0.05 && direction.y > -0.05)
        {
            shootingDir = direction.normalized;
            Vector3 forw = new Vector3(-shootingDir.y, shootingDir.x, transform.position.z);
            cannon.transform.rotation = Quaternion.LookRotation(forw, shootingDir);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<BulletSharedClass>().DoBehavior(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            grounded = true;
            jumped = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
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
            if (!grounded)
                transform.Translate(0, -wallFallSpeed * Time.deltaTime, 0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            grounded = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallDir = 0;
            touchingWall = false;
        }
    }
}
