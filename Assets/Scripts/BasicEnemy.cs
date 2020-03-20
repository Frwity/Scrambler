using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private float upperFireRange;
    [SerializeField] private float lowerFireRange;

    [SerializeField] private float detectionRange; // must be > upperFireRange
    [SerializeField] private float distanceToKeepAwayFromPlayer;
    [SerializeField] private float speed;

    [SerializeField] private float maxReactionTime;

    [SerializeField] private bool flipped; // shall be set to false if at it's spawn, the enemy is looking left.

    [SerializeField] private float firePower;

    [SerializeField] private float FireCooldown;

    [SerializeField] private GameObject bullet;

    private GameObject playerTarget;

    private float cooldownCounter;
    private float reactionTime;

    private bool lookingRight; // Obviously, if FALSE, the enemy is looking left.

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("PlayerMove");

        cooldownCounter = FireCooldown;
        reactionTime = maxReactionTime;

        lookingRight = flipped;
    }

    // Update is called once per frame
    void Update()
    {
        IAcontrol();
    }

    public bool Shoot()
    {
        if (cooldownCounter <= 0)
        {
            GameObject firedBullet = Instantiate(bullet, transform.position, Quaternion.identity);

            if (lookingRight)
            {
                firedBullet.GetComponent<Rigidbody>().AddForce(firePower, 0, 0);
            }
            else
            {
                firedBullet.GetComponent<Rigidbody>().AddForce(-firePower, 0, 0);
            }

            return true;
        }
        else return false;
    }

    public bool WalkRight()
    { 
        transform.Translate((flipped ? -speed : speed), 0, 0); 
        
        return true; 
    }

    public bool WalkLeft()
    { 
        transform.Translate((flipped ? speed : -speed), 0, 0); 
        
        return true; 
    }

    public void FlipAround()
    {
        transform.Rotate(0, (flipped ? -180 : 180), 0);
        flipped ^= true;
    }

    private bool LookingAtPlayer(float playerXrelativetoEnemy)
    {
        if ((playerXrelativetoEnemy > 0 && !lookingRight) || (playerXrelativetoEnemy < 0 && lookingRight))
            return false;
        else
            return true;
    }

    private bool AIflipControl(float distToPlayer, float playerXrelativetoEnemy)
    {
        if (!LookingAtPlayer(playerXrelativetoEnemy))
        {
            if (reactionTime > 0)
            {
                reactionTime -= Time.smoothDeltaTime;
            }
            else
            {
                FlipAround();

                lookingRight ^= true;

                reactionTime = maxReactionTime;
            }

            return false;
        }
        else
        {
            if (reactionTime < maxReactionTime)
                reactionTime += Time.smoothDeltaTime / 2f;

            return true;
        }
    }

    private void IAcontrol()
    {
        Vector3 vecToPlayer = (playerTarget.transform.position - transform.position);
        float distToPlayer  = vecToPlayer.magnitude;

        if (AIflipControl(distToPlayer, vecToPlayer.x) && distToPlayer < detectionRange)
        {
            if (distanceToKeepAwayFromPlayer < distToPlayer)
            {
                if (lookingRight)
                {
                    WalkRight();
                }
                else
                {
                    WalkLeft();
                }

                Debug.Log(lookingRight);
            }

            if (lowerFireRange < distToPlayer &&
                                 distToPlayer < upperFireRange)
            {
                if (!Shoot())
                {
                    cooldownCounter -= Time.smoothDeltaTime;
                }
                else
                {
                    cooldownCounter = FireCooldown;
                }
            }
            else if (cooldownCounter < FireCooldown)
            {
                cooldownCounter += Time.smoothDeltaTime / 2f;
            }
        }
    }
}
