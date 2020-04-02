using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suicideIA : MonoBehaviour
{
     enum Direction
    {
        RIGHT = 1,
        LEFT = -1,
    };
    // Start is called before the first frame update
    public bool isActive;
    private Entity entity;
    GameObject player;
    private bool shooting;
    [SerializeField]
    private Direction direction = Direction.RIGHT;

    private short waitFrame = 0;
    private SuicideSkill scs;
    private float currentLostTimer = 0.0f;
    [SerializeField]private float lostTimer = 0.0f;
    [SerializeField]private bool hasPlayerGoneInBack = false; 
    [SerializeField]private bool HasTurnedOnce = false;
    [SerializeField] private float Backtimer = 0.0f;
    [SerializeField] private float currentBackTimer = 0.0f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (direction == Direction.LEFT)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        entity = GetComponent<Entity>(); 
        scs = entity.entitySkill as SuicideSkill;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive || player == null)
            return;
        if (!shooting && entity.isPlayerInSight)
        {
            
            currentLostTimer = 0.0f;
            currentBackTimer = 0.0f;
            hasPlayerGoneInBack = false;
            HasTurnedOnce = false;
            
            if (entity.lastPlayerPosKnown.x > transform.position.x && (direction == Direction.LEFT))
            {
                direction = Direction.RIGHT;
                
                transform.rotation =  Quaternion.Euler(0,0,0);
            }
            else if (entity.lastPlayerPosKnown.x < transform.position.x &&(direction == Direction.RIGHT)) 
            {
                direction = Direction.LEFT;
                
                transform.rotation = Quaternion.Euler(0,180,0);
            }

            if ((entity.lastPlayerPosKnown.x ) < (transform.position.x  -0.45))
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveRight(-1);
                }
                else
                {
                    entity.MoveLeft(-1);
                }
                    
            }
            else if ((entity.lastPlayerPosKnown.x ) > (transform.position.x +0.45))
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveLeft(1);
                }
                else
                {
                    entity.MoveRight(1);
                }
            }
            else
                shooting = true;
        }
        else if (entity.LostPlayer)
        {
            if (currentLostTimer < lostTimer)
            {
                currentLostTimer += Time.smoothDeltaTime;
                return;
            }

            if (entity.lastPlayerPosKnown.x > transform.position.x && (direction == Direction.LEFT) && !HasTurnedOnce)
            {
                direction = Direction.RIGHT;
                
                hasPlayerGoneInBack = true;
                HasTurnedOnce = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (entity.lastPlayerPosKnown.x < transform.position.x && (direction == Direction.RIGHT) &&
                     !HasTurnedOnce)
            {
                direction = Direction.LEFT;
                
                hasPlayerGoneInBack = true;
                HasTurnedOnce = true;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            if ((entity.lastPlayerPosKnown.x) < (transform.position.x - 0.15) && !hasPlayerGoneInBack)
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveRight(-1);
                }
                else
                {
                    
                    entity.MoveLeft(-1);
                }

            }
            else if ((entity.lastPlayerPosKnown.x) > (transform.position.x + 0.15) && !hasPlayerGoneInBack)
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveLeft(1);
                }
                else
                {
                    entity.MoveRight(1);
                }
            }
            else if (hasPlayerGoneInBack && currentBackTimer < Backtimer)
            {
                currentBackTimer += Time.smoothDeltaTime;
                Debug.Log("HERE");
                if (direction == Direction.RIGHT)
                {
                    entity.MoveLeft(1);
                }
                else
                {
                    entity.MoveRight(-1);
                }
            }
            else if (shooting)
            {
                if (entity.Shoot(new Vector3(1, 1, 1)))
                {
                    Destroy(gameObject);
                }
                else
                {
                    if (Mathf.Abs(player.transform.position.x - transform.position.x) > 0.5f && waitFrame == 1)
                    {
                        scs.ResetTimer();
                        shooting = false;
                    }

                    if (waitFrame == 0)
                        waitFrame = 1;
                }
            }
        }
    }
}
