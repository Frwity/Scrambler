using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suicideIA : EntityAI
{
    public enum Direction
    {
        RIGHT = 1,
        LEFT = -1,
    };

    GameObject player;
    private bool shooting;
    
    public Direction direction = Direction.RIGHT;

    private short waitFrame = 0;
    private SuicideSkill scs;
    
    private float currentLostTimer = 0.0f;
    [SerializeField] private float lostTimer = 0.0f;
    private bool hasPlayerGoneInBack = false; 
    private bool HasTurnedOnce = false;
    
    [SerializeField] private float Backtimer = 0.0f;
     private float currentBackTimer = 0.0f;
    
    [SerializeField] private float AIResetTimer = 0.0f;
    private float currentAIResetTimer = 0.0f;

    [SerializeField] private Road Path;

    [SerializeField] Color possessedColor;
    [SerializeField] Color disabledColor;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (direction == Direction.LEFT)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        entity = GetComponent<Entity>(); 
        scs = entity.entitySkill as SuicideSkill;

        entity.onPossess.AddListener(LightChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
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



                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (entity.lastPlayerPosKnown.x < transform.position.x && (direction == Direction.RIGHT))

            {
                direction = Direction.LEFT;



                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            if ((entity.lastPlayerPosKnown.x) < (transform.position.x - 0.45))
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
            else if ((entity.lastPlayerPosKnown.x) > (transform.position.x + 0.45))
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
            currentAIResetTimer += Time.smoothDeltaTime;
            if (entity.lastPlayerPosKnown.x > transform.position.x + 0.15 * scs.speed && (direction == Direction.LEFT) && !HasTurnedOnce)
            {
                direction = Direction.RIGHT;



                hasPlayerGoneInBack = true;
                HasTurnedOnce = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (entity.lastPlayerPosKnown.x < transform.position.x - 0.15 * scs.speed && (direction == Direction.RIGHT) &&
                     !HasTurnedOnce)
            {
                direction = Direction.LEFT;



                hasPlayerGoneInBack = true;
                HasTurnedOnce = true;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            if ((entity.lastPlayerPosKnown.x) < (transform.position.x - 0.15 * scs.speed) && !hasPlayerGoneInBack)
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
            else if ((entity.lastPlayerPosKnown.x) > (transform.position.x + 0.15 * scs.speed) && !hasPlayerGoneInBack)
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
            else if (currentAIResetTimer > AIResetTimer)
            {
                entity.LostPlayer = false;
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
                if (Mathf.Abs(entity.lastPlayerPosKnown.x - transform.position.x) > scs.exploRay && waitFrame == 1)
                {
                    scs.ResetTimer();
                    shooting = false;
                }

                if (waitFrame == 0)
                    waitFrame = 1;
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



            float currentCheckpointPosX = (Path.Checkpoints[Path.CurrentIndex].checkPointPos.x);
            float currentCheckpointPosY = (Path.Checkpoints[Path.CurrentIndex].checkPointPos.y);
            if (currentCheckpointPosX > transform.position.x && (direction == Direction.LEFT))
            {
                direction = Direction.RIGHT;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (currentCheckpointPosX < transform.position.x && (direction == Direction.RIGHT))

            {
                direction = Direction.LEFT;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if ((currentCheckpointPosX) < (transform.position.x - (0.15 * scs.speed)))
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
            else if ((currentCheckpointPosX) > (transform.position.x + (0.15 * scs.speed)))
            {
                int i = 1;
                if (scs.touchingWall && scs.grounded)
                {
                    
                }
                else if (scs.touchingWall && scs.Roofed)
                {
                    i *= -1;
                }
                if (direction == Direction.RIGHT)
                {
                    entity.MoveLeft(1*i);
                }
                else
                {
                    entity.MoveRight(1*i);
                }
            }
            else if ((currentCheckpointPosY) < (transform.position.y - (0.15 * scs.speed)))
            {
                int i = 1;
                if (scs.touchingWall && scs.grounded)
                {
                   
                }
                else if (scs.touchingWall && scs.Roofed)
                {
                    i *= -1;
                }
                if (direction == Direction.RIGHT)
                {
                    entity.MoveRight(-1*i);
                }
                else
                {
                    entity.MoveLeft(-1*i);
                }
            }
            else if ((currentCheckpointPosY) > (transform.position.y + (0.15 * scs.speed)))
            {
                int i = 1;
                if (scs.touchingWall && scs.grounded)
                {
                    
                }
                else if (scs.touchingWall && scs.Roofed)
                {
                    i *= -1;
                }
                if (direction == Direction.RIGHT)
                {
                    entity.MoveLeft(1*i);
                }
                else
                {
                    entity.MoveRight(1*i);
                }
            }
            else
            {
                Path.CurrentIndex++;
            }
        }
    }

    public void LightChange()

    {

        if (gameObject.CompareTag("Player"))

        {

            GetComponentInChildren<Light>().color = possessedColor;

        }

        else if (gameObject.CompareTag("Enemy"))

        {

            GetComponentInChildren<Light>().color = disabledColor;

        }

    }
}
