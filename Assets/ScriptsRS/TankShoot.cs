using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector2 shootingDir;
    [SerializeField] private float shootingStrength;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldown = 0;
    private float currentCooldown ;
    
    void Start()
    {
        
        shootingDir.Normalize();
        currentCooldown = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F) && currentCooldown <= 0.01f)
        {
            GameObject bulletInst = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Physics.IgnoreCollision(bulletInst.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
            Rigidbody rbInst = bulletInst.GetComponent<Rigidbody>();
            rbInst.AddForce(new Vector3(shootingDir.x, shootingDir.y, 0) * shootingStrength);
            currentCooldown = cooldown;
        }

        if (currentCooldown > 0.0f)
        {
            currentCooldown -= Time.smoothDeltaTime;
        }
    }
}
