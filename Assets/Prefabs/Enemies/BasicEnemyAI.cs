using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : EntityAI
{
    [SerializeField] private float upperFireRange = 0.0f;
    [SerializeField] private float lowerFireRange = 0.0f;

    //[SerializeField] private float detectionRange = 0.0f; // must be > upperFireRange
    [SerializeField] private float distanceToKeepAwayFromPlayer = 0.0f;

    [SerializeField] private float maxReactionTime = 0.0f;

    [SerializeField] public bool flipped = false; // If checked, the enemy will spawn looking RIGHT, do *NOT* rotate the model manually.

    private GameObject player;

    private float reactionTime;

    private bool lookingRight = false; // Obviously, if FALSE, the enemy is looking left.

    private BasicEnemySkill associatedBES;

    [SerializeField] private float lostTimer = 0.0f;
    [SerializeField] private bool HasTurnedOnce = false;
    [SerializeField] private float Backtimer = 0.0f;
    //[SerializeField] private float lostSpeed = 2.0f;
    [SerializeField] private float AIResetTimer = 0.0f;
    [SerializeField] private Road Path = null;
    private float currentLostTimer = 0.0f;
    private bool hasPlayerGoneInBack = false;
    private float currentBackTimer = 0.0f;
    private float currentAIResetTimer = 0.0f;
    private bool previousFlip = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        entity = GetComponent<Entity>();
        associatedBES = GetComponent<BasicEnemySkill>();
        if ((int)transform.rotation.eulerAngles.y == 0)
        {
            lookingRight = false;
        }
        else if ((int)transform.rotation.eulerAngles.y == 180)
        {
            lookingRight = true;
        }

        reactionTime = maxReactionTime;

        if (flipped)
        {
            LookRight();
        }
    }

    void Update()
    {
        if (isActive)
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
        transform.rotation = Quaternion.Euler(0, 180, 0);

        flipped = true;
        lookingRight = true;
    }

    public void LookLeft()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        flipped = false;
        lookingRight = false;
    }

    public void LookAround()
    {
        previousFlip = flipped;
        flipped ^= true;
        transform.Rotate(0, flipped ? -180 : 180, 0);



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
            if (!entity.isPlayerInSight && !entity.LostPlayer)
            {
                LookAround();
                return true;
            }
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
        Vector3 vecToPlayer = entity.lastPlayerPosKnown - (transform.position + (Vector3)associatedBES.shootOriginPos);
        float distToPlayer = vecToPlayer.magnitude;

        if (entity.isInBackGround)
        {
            if (entity.isPlayerInSight)
                entity.Shoot(vecToPlayer.normalized);
            return;

        }
        if (entity.isPlayerInSight)
        {
            AIflipControl(vecToPlayer.x);
            previousFlip = flipped;
            currentLostTimer = 0.0f;
            currentBackTimer = 0.0f;
            currentAIResetTimer = 0.0f;
            hasPlayerGoneInBack = false;
            HasTurnedOnce = false;

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
        else if (entity.LostPlayer)
        {
            
            if (currentLostTimer < lostTimer)
            {
                currentLostTimer += Time.smoothDeltaTime;
                return;
            }
            
            currentAIResetTimer += Time.smoothDeltaTime;
            if (!HasTurnedOnce)
            {
                bool b = LookingAtPlayer(vecToPlayer.x);
                
                if (!b)
                {
                    LookAround();
                }
                else
                {
                    HasTurnedOnce = true;
                }
            }

            if (previousFlip != flipped)
            {
                
                previousFlip = flipped;
                hasPlayerGoneInBack = true;
                HasTurnedOnce = true;
            }

            if (vecToPlayer.x > 0.15 * associatedBES.maxSpeed && !hasPlayerGoneInBack)
            {

                entity.MoveRight( associatedBES.maxSpeed);
            }
            else if (vecToPlayer.x < -0.15 * associatedBES.maxSpeed && !hasPlayerGoneInBack)
            {

                entity.MoveLeft(- associatedBES.maxSpeed);
            }
            else if (hasPlayerGoneInBack)
            {

                if (currentBackTimer < Backtimer)
                {
                    currentBackTimer += Time.smoothDeltaTime;
                    if (lookingRight)
                    {
                        entity.MoveRight( associatedBES.maxSpeed);
                    }
                    else
                    {
                        entity.MoveRight(- associatedBES.maxSpeed);
                    }
                }
            }
            else if (currentAIResetTimer > AIResetTimer)
            {
                entity.LostPlayer = false;
            }

        }
        else
        {
            if (!Path)
                return;
            for (int i = 0; i < Path.size; i++)
            {
                if (!Path.Checkpoints[Path.CurrentIndex].enabled)
                {
                    Path.CurrentIndex++;
                }
                else
                {
                    break;
                }
            }
            Vector3 toCheckpoint = Path.Checkpoints[Path.CurrentIndex].checkPointPos - transform.position;
            AIflipControl(toCheckpoint.x);
            previousFlip = flipped;
            if (toCheckpoint.x > 0.15 * associatedBES.maxSpeed)
                entity.MoveRight(1);
            else if (toCheckpoint.x < -0.15 * associatedBES.maxSpeed)
                entity.MoveLeft(-1);
            else
            {
                Path.CurrentIndex++;
            }
        }
    }
}