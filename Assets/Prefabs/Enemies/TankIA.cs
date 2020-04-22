using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    RIGHT = 1,
    LEFT = -1,
    NONE = 0,
};

public class TankIA : EntityAI
{
    [SerializeField] private float fireCount = 0f;
    private float nbFired = 0f;
    private bool shooting = false;

    public Direction direction = Direction.RIGHT;
    
    float currentLostTimer = 0.0f;
    [SerializeField] private float lostTimer = 0f;
    
    private bool hasPlayerGoneInBack = false;
    private bool HasTurnedOnce = false;
    
    [SerializeField] private float Backtimer = 0.0f;
    private float currentBackTimer = 0.0f;
    
    private TankSkill ts = null;
    [SerializeField] private Road Path = null;
    [SerializeField] private float AIResetTimer = 0.0f;
    private float currentAIResetTimer = 0.0f;
    
    void Start()
    {
        if (direction == Direction.LEFT)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        entity = GetComponent<Entity>();
        ts = entity.entitySkill as TankSkill;
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
                ts.changeRotation();
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (entity.lastPlayerPosKnown.x < transform.position.x && (direction == Direction.RIGHT))
            {
                direction = Direction.LEFT;
                ts.changeRotation();
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            //Debug.Log(ts.rangePoint);
            if ((entity.lastPlayerPosKnown.x) < (transform.position.x + ((int)direction * ts.rangePoint) - 0.15))
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
            else if ((entity.lastPlayerPosKnown.x) > (transform.position.x + ((int)direction * ts.rangePoint) + 0.15))
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
            if (entity.lastPlayerPosKnown.x > transform.position.x && (direction == Direction.LEFT) && !HasTurnedOnce)
            {
                direction = Direction.RIGHT;
                ts.changeRotation();
                hasPlayerGoneInBack = true;
                HasTurnedOnce = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (entity.lastPlayerPosKnown.x < transform.position.x && (direction == Direction.RIGHT) && !HasTurnedOnce)
            {
                direction = Direction.LEFT;
                ts.changeRotation();
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
                    entity.MoveLeft(1);
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
                    entity.MoveRight(-1);
                }
            }
            else if (hasPlayerGoneInBack && currentBackTimer < Backtimer)
            {
                currentBackTimer += Time.smoothDeltaTime;
                if (direction == Direction.RIGHT)
                {
                    entity.MoveRight(1);
                }
                else
                {
                    entity.MoveLeft(1);
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
                nbFired++;
                if (nbFired >= fireCount)
                {
                    shooting = false;
                    nbFired = 0;
                }
            }
        }
        else if (Path)
        {
            currentAIResetTimer = 0.0f;
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
            if (currentCheckpointPosX > transform.position.x && (direction == Direction.LEFT))
            {
                direction = Direction.RIGHT;
                ts.changeRotation();
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (currentCheckpointPosX < transform.position.x && (direction == Direction.RIGHT))
            {
                direction = Direction.LEFT;
                ts.changeRotation();
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if ((currentCheckpointPosX) < (transform.position.x - 0.15))
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveRight(-1);
                }
                else
                {
                    entity.MoveLeft(1);
                }

            }
            else if ((currentCheckpointPosX) > (transform.position.x + 0.15))
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveLeft(1);
                }
                else
                {
                    entity.MoveRight(-1);
                }
            }
            else
            {
                Path.CurrentIndex++;
            }
        }
    }
}
