using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    public bool isActive;
    private Entity entity;

    [SerializeField] private float upperFireRange;
    [SerializeField] private float lowerFireRange;

    [SerializeField] private float detectionRange; // must be > upperFireRange
    [SerializeField] private float distanceToKeepAwayFromPlayer;

    [SerializeField] private float maxReactionTime;

    [SerializeField] private bool flipped; // If checked, the enemy will spawn looking RIGHT, do *NOT* rotate the model manually.

    private GameObject player;

    private float reactionTime;

    private bool lookingRight; // Obviously, if FALSE, the enemy is looking left.

    private BasicEnemySkill associatedBES;

    void Start()
    {
        player          = GameObject.FindGameObjectWithTag("Player");
        entity          = GetComponent<Entity>();
        associatedBES   = GetComponent<BasicEnemySkill>();


        reactionTime = maxReactionTime;

        if (flipped)
        {
            LookAround();
        }
    }

    void Update()
    {
        if (isActive && player != null)
            IAcontrol();
        else
            PLdirectionLookbyStick();
    }

    public void PLdirectionLookbyStick()
    {
        float stickX = associatedBES.xAim;

        if (stickX > 0.05f && !lookingRight)
        {
            LookRight();
        }
            
        if (stickX < -0.05f && lookingRight)
        {
            LookLeft();
        }
    }

    public void LookRight()
    {
        transform.GetChild(2).rotation = Quaternion.Euler(0, 180, 0);
        
        flipped = true;
        lookingRight = true;
    }

    public void LookLeft()
    {
        transform.GetChild(2).rotation = Quaternion.Euler(0, 0, 0);

        flipped = false;
        lookingRight = false;
    }

    public void LookAround()
    {
        transform.GetChild(2).Rotate(0, flipped ? -180 : 180, 0);

        flipped ^= true;
        lookingRight ^= true;
    }

    private bool LookingAtPlayer(float playerXrelativetoEnemy)
    {
        if ((playerXrelativetoEnemy > 0 && !lookingRight) || (playerXrelativetoEnemy < 0 && lookingRight))
            return false;
        else
            return true;
    }

    private bool AIflipControl(float playerXrelativetoEnemy)
    {
        if (!LookingAtPlayer(playerXrelativetoEnemy))
        {
            if (reactionTime > 0)
                reactionTime -= Time.smoothDeltaTime;
            else
            {
                reactionTime = maxReactionTime;

                LookAround();

                return true;
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
        Vector3 vecToPlayer = (player.transform.position - transform.position);
        float distToPlayer = vecToPlayer.magnitude;

        if (AIflipControl(vecToPlayer.x) && distToPlayer < detectionRange)
        {
            if (distanceToKeepAwayFromPlayer < distToPlayer)
            {
                if (vecToPlayer.x > 0)
                    entity.MoveRight(1);
                else
                    entity.MoveLeft(-1);
            }

            if (lowerFireRange < distToPlayer && distToPlayer < upperFireRange)
                entity.Shoot(vecToPlayer.normalized);
        }
    }
}