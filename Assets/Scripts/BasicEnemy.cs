using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private float upperFireRange;
    [SerializeField] private float lowerFireRange;

    [SerializeField] private float detectionRange;
    [SerializeField] private float distanceToKeepAwayFromPlayer;
    [SerializeField] private float speed;

    [SerializeField] private float firePower;

    [SerializeField] private float FireCooldown;

    [SerializeField] private GameObject bullet;

    private GameObject playerTarget;

    private float cooldownCounter;

    private bool lookingRight; // Obviously, if FALSE, the enemy is looking left.

    private bool hasFirstFired = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("PlayerMove");

        cooldownCounter = FireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vecToPlayer     = (playerTarget.transform.position - transform.position);
        float   distToPlayer    = vecToPlayer.magnitude;

        IAlogicUpdate(vecToPlayer);

        Debug.Log(distToPlayer);

        if ( distanceToKeepAwayFromPlayer < distToPlayer &&
                                            distToPlayer < detectionRange )
            Walk();

        if ( lowerFireRange < distToPlayer &&
                              distToPlayer < upperFireRange)
        {
            if (cooldownCounter > 0)
            {
                cooldownCounter -= Time.smoothDeltaTime;
            }
            else
            {
                cooldownCounter = FireCooldown;

                Shoot();
            }
        }
        else if (cooldownCounter < FireCooldown)
        {
            cooldownCounter += Time.smoothDeltaTime / 2f;
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

    public void Walk()
    {
        if (lookingRight)
        {
            transform.Translate( speed, 0, 0);
        }
        else
        {
            transform.Translate(-speed, 0, 0);
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
