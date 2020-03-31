using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    RIGHT = 1,
    LEFT = -1,
    NONE = 0,
};
public class TankIA : MonoBehaviour
{
    
    // Start is called before the first frame update
    public bool isActive;
    private Entity entity;
    GameObject player;
    [SerializeField] private float fireCount;
    [SerializeField] private float rangePoint;
    private float nbFired;
    private bool shooting;
    public Direction direction = Direction.RIGHT;
    [SerializeField] float currentLostTimer = 0.0f;
    [SerializeField] private float lostTimer;
    [SerializeField]private bool hasPlayerGoneInBack = false; 
    [SerializeField]private bool HasTurnedOnce = false;
    [SerializeField] private float Backtimer = 0.0f;
    [SerializeField] private float currentBackTimer = 0.0f;
    private TankSkill ts;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (direction == Direction.LEFT)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        entity = GetComponent<Entity>(); 
        ts = entity.entitySkill as TankSkill;
        rangePoint = ts.rangePoint;
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
                ts.changeRotation();
                transform.rotation =  Quaternion.Euler(0,0,0);
            }
            else if (entity.lastPlayerPosKnown.x < transform.position.x &&(direction == Direction.RIGHT)) 
            {
                direction = Direction.LEFT;
                ts.changeRotation();
                transform.rotation = Quaternion.Euler(0,180,0);
            }
            //Debug.Log( (transform.position.x + ((int) direction * rangePoint)));
            if ((entity.lastPlayerPosKnown.x ) < (transform.position.x + ((int) direction * rangePoint) -0.15))
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
            else if ((entity.lastPlayerPosKnown.x ) > (transform.position.x + ((int) direction * rangePoint)+0.15))
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
                ts.changeRotation();
                hasPlayerGoneInBack = true;
                HasTurnedOnce = true;
                transform.rotation =  Quaternion.Euler(0,0,0);
            }
            else if (entity.lastPlayerPosKnown.x < transform.position.x &&(direction == Direction.RIGHT) && !HasTurnedOnce) 
            {
                direction = Direction.LEFT;
                ts.changeRotation();
                hasPlayerGoneInBack = true;
                HasTurnedOnce = true;
                transform.rotation = Quaternion.Euler(0,180,0);
            }
            if ((entity.lastPlayerPosKnown.x ) < (transform.position.x  -0.15) && !hasPlayerGoneInBack)
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
            else if ((entity.lastPlayerPosKnown.x ) > (transform.position.x+0.15)&& !hasPlayerGoneInBack)
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
                Debug.Log("HERE");
                if (direction == Direction.RIGHT)
                {
                    entity.MoveLeft(1);
                }
                else
                {
                    entity.MoveRight(1);
                }
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
        else
        {
            //move
        }
    }
}
