using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private float upperRange;
    [SerializeField] private float lowerRange;

    [SerializeField] private float firePower;

    [SerializeField] private float FireCooldown;

    [SerializeField] private GameObject bullet;

    private GameObject playerTarget;

    private float cooldownCounter;

    private bool lookingRight; // Obviously, if FALSE, the enemy is looking left.

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("PlayerMove");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vecToPlayer     = (playerTarget.transform.position - transform.position);
        float   distToPlayer    = vecToPlayer.magnitude;

        IAlogicUpdate(vecToPlayer);

        if (cooldownCounter > 0)
            cooldownCounter -= Time.smoothDeltaTime;

        Debug.Log(distToPlayer);

        if (lowerRange < distToPlayer               &&
                         distToPlayer < upperRange  &&

            cooldownCounter <= 0)
        {
            cooldownCounter = FireCooldown;

            Shoot();
        }
    }

    public void Shoot()
    {
        GameObject firedBullet = Instantiate(bullet, transform.position, Quaternion.identity);

        if (lookingRight)
        {
            firedBullet.GetComponent<Rigidbody>().AddForce( firePower, 0, 0);
        }
        else
        {
            firedBullet.GetComponent<Rigidbody>().AddForce(-firePower, 0, 0);
        }
    }

    void IAlogicUpdate(Vector3 vecToPlayer)
    {
        if (vecToPlayer.x > 0)
            lookingRight = true;
        else
            lookingRight = false;
    }
}
