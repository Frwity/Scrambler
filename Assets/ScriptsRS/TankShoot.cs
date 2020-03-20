using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TankShoot : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private int angleInDeg = 0;
    [SerializeField] private int precision = 0;
    [SerializeField] private float shootingStrength;
    [Header("Prefab")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldown = 0;
    [SerializeField] private int numberOfProjectile = 1;
    
    private float currentCooldown ;
    private Vector2 shootingDir;
    private int lastAngle;
    private int lastRotationAngle;
    Vector2 Rotate(Vector2 aPoint, float aDegree)
    {
        return Quaternion.Euler(0,0,aDegree) * aPoint;
    }
    void Start()
    {
        shootingDir = Rotate(transform.right, angleInDeg);
        lastAngle = angleInDeg;
        currentCooldown = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastAngle != angleInDeg || lastRotationAngle != (int)transform.rotation.y)
        {
            Debug.Log(transform.right);
            shootingDir = Rotate(transform.right,  angleInDeg);
            lastAngle = angleInDeg;
            lastRotationAngle = (int)transform.rotation.y;
        }
        
        if (Input.GetKey(KeyCode.F) && currentCooldown <= 0.01f)
        {
            for (int i = 1; i <= numberOfProjectile; i++)
            {
                int angleToAdd = Random.Range(-precision, precision);
                Vector2 newShootDir = Rotate(shootingDir, angleToAdd);
                GameObject bulletInst = Instantiate(bulletPrefab, transform.position, transform.rotation);
                Physics.IgnoreCollision(bulletInst.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
                Rigidbody rbInst = bulletInst.GetComponent<Rigidbody>();
                rbInst.AddForce(new Vector3(newShootDir.x, newShootDir.y, 0) * shootingStrength);
            }
            
            currentCooldown = cooldown;
        }

        if (currentCooldown > 0.0f)
        {
            currentCooldown -= Time.smoothDeltaTime;
        }
    }
}
