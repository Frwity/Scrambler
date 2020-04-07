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
    
    [SerializeField]private float currentLostTimer = 0.0f;
    [SerializeField]private float lostTimer = 0.0f;
    [SerializeField]private bool hasPlayerGoneInBack = false; 
    [SerializeField]private bool HasTurnedOnce = false;
    [SerializeField] private float Backtimer = 0.0f;
    [SerializeField] private float currentBackTimer = 0.0f;
    [SerializeField] private float lostSpeed = 2.0f;
    private bool previousFlip = false;
    [SerializeField] private Road Path;
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
            if (reactionTime > 0 && !(!entity.isPlayerInSight && !entity.LostPlayer))
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
        Vector3 vecToPlayer = (entity.lastPlayerPosKnown - transform.position);
        float distToPlayer = vecToPlayer.magnitude;
        
        if (entity.isPlayerInSight)
        {
            AIflipControl(vecToPlayer.x);
            previousFlip = flipped;
            currentLostTimer = 0.0f;
            currentBackTimer = 0.0f;
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

             if (!HasTurnedOnce)
             {
                 AIflipControl(vecToPlayer.x);
             }
             
             if (previousFlip != flipped)
             {
                 previousFlip = flipped;
                 hasPlayerGoneInBack = true;
                 HasTurnedOnce = true;
             }
             
             if (vecToPlayer.x > 0.15*lostSpeed && !hasPlayerGoneInBack)
             {
                 
                 entity.MoveRight(lostSpeed/ associatedBES.moveSpeed);
             }
             else if (vecToPlayer.x < -0.15*lostSpeed && !hasPlayerGoneInBack)
             {
                 
                 entity.MoveLeft(-lostSpeed / associatedBES.moveSpeed);
             }

             if (hasPlayerGoneInBack)
             {
                 
                 if (currentBackTimer < Backtimer)
                 {
                     currentBackTimer += Time.smoothDeltaTime;
                     if (lookingRight)
                     {
                         entity.MoveRight(1/ associatedBES.moveSpeed);
                     }
                     else
                     {
                         entity.MoveRight(-1/ associatedBES.moveSpeed);
                     }
                 }
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
            if (toCheckpoint.x > 0.15* associatedBES.moveSpeed)
                entity.MoveRight(1);
            else if (toCheckpoint.x < -0.15* associatedBES.moveSpeed)
                entity.MoveLeft(-1);
            else
            {
                Path.CurrentIndex++;
            }
        }
    }
}