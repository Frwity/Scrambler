using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSkill : EntitySkill
{
    // Start is called before the first frame update
    [SerializeField] private int angleInDeg = 0;
    [SerializeField] private int precision = 0;
    [SerializeField] private float shootingStrength;
    
    [SerializeField] private GameObject bulletPrefab;
   
    [SerializeField] private float cooldown = 0;
    [SerializeField] private int numberOfProjectile = 1;
    [SerializeField] private float speed;
    
    private float currentCooldown ;
    private Vector2 shootingDir;
    private int lastAngle;
    private int lastRotationAngle;
    private int angletoPass;
    private int angleRotated;
    [HideInInspector] public float rangePoint;
    private Direction dir;

    [SerializeField] private GameObject cannon;
    Vector2 Rotate(Vector2 aPoint, float aDegree)
    {
        return Quaternion.Euler(0,0,aDegree) * aPoint;
    }

    public void changeRotation()
    {
        //angleRotated = angleInDeg * (int)dir;
        shootingDir.x = -shootingDir.x;
    }
    void Start()
    {
        angleRotated = angleInDeg;
        int a = angleRotated;
        rangePoint = (shootingStrength / Physics.gravity.magnitude) 
                  * Mathf.Cos(a * Mathf.Deg2Rad) 
                  * (shootingStrength * Mathf.Sin(a * Mathf.Deg2Rad) 
                     + Mathf.Sqrt(Mathf.Pow(shootingStrength * Mathf.Sin(a * Mathf.Deg2Rad), 2) 
                                  + 2* Physics.gravity.magnitude * transform.position.y ));
        //Debug.LogWarning(transform.position.x + rangePoint);
        shootingDir = Rotate(Vector3.right, angleInDeg);
        lastAngle = angleInDeg;
        currentCooldown = 0;
        dir = Direction.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastAngle != angleInDeg || lastRotationAngle != (int)transform.rotation.y)
        {
            //Debug.Log(transform.right);
            //angleRotated = angleInDeg * (int) dir;
            //shootingDir = Rotate(transform.right,  angleRotated);
            lastAngle = angleInDeg;
            lastRotationAngle = (int)transform.rotation.y;
        }
        if (currentCooldown > 0.0f)
        {
            currentCooldown -= Time.smoothDeltaTime;
        }
    }

    public override bool Jump()
    {
        return false;
    }

    public override bool MoveLeft(float moveSpeed)
    {
        if (!GetComponent<TankIA>().isActive)
        {
            if (Input.GetAxis("RHorizontal") < -0.05f && dir != Direction.LEFT)
            {
                transform.rotation = Quaternion.Euler(0,180,0);
                dir = Direction.LEFT;
                //changeRotation();
                
            }
            else if (Input.GetAxis("RHorizontal") > 0.05f && dir != Direction.RIGHT)
            {
                 transform.rotation = Quaternion.Euler(0,0,0);
                 dir = Direction.RIGHT;
                 //changeRotation();

            }
            transform.Translate(Time.deltaTime * speed * moveSpeed * (float) dir, 0, 0);
            //Debug.Log(Time.deltaTime * speed * moveSpeed * (float) dir);
            return true;
        }
        transform.Translate(Time.deltaTime * speed * moveSpeed, 0, 0);
        return true;
    }

    public override bool MoveRight(float moveSpeed)
    {
        if (!GetComponent<TankIA>().isActive)
        {
            
            transform.Translate(Time.deltaTime * speed * moveSpeed * (float) dir, 0, 0);
            //Debug.Log(Time.deltaTime * speed * moveSpeed * (float) dir);
            return true;
        }
        transform.Translate(Time.deltaTime * speed * moveSpeed, 0, 0);
        return true;
    }

    public override bool Shoot(Vector3 direction)
    {
        
        if (currentCooldown > 0)
            return false;
        
        for (int i = 0; i < numberOfProjectile; i++)
        {
            int angleToAdd = Random.Range(-precision, precision);
            Vector2 newShootDir = Rotate(shootingDir, angleToAdd);
            GameObject bulletInst = Instantiate(bulletPrefab, cannon.transform.position, transform.rotation);
            Physics.IgnoreCollision(bulletInst.GetComponent<Collider>(), 
                this.GetComponent<Collider>(), true);
            CurveBullet bullett = bulletInst.GetComponent<CurveBullet>();
            
            bullett.velocity = new Vector3(newShootDir.x, newShootDir.y, 0) * shootingStrength;

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
            collision.gameObject.GetComponent<BulletSharedClass>().doBehavior(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
