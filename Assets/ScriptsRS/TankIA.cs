using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankIA : MonoBehaviour
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
    [SerializeField] private float fireCount;
    [SerializeField] private float rangePoint;
    private float nbFired;
    private bool shooting;
    [SerializeField]
    private Direction direction = Direction.RIGHT;

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
        if (!shooting)
        {
            
            if (player.transform.position.x > transform.position.x && (direction == Direction.LEFT))
            {
                direction = Direction.RIGHT;
                ts.changeRotation();
                transform.rotation =  Quaternion.Euler(0,0,0);
            }
            else if (player.transform.position.x < transform.position.x &&(direction == Direction.RIGHT)) 
            {
                direction = Direction.LEFT;
                ts.changeRotation();
                transform.rotation = Quaternion.Euler(0,180,0);
            }
            Debug.Log( (transform.position.x + ((int) direction * rangePoint)));
            if ((player.transform.position.x ) < (transform.position.x + ((int) direction * rangePoint) -0.15))
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveRight(1);
                }
                else
                {
                    entity.MoveLeft(-1);
                }
                    
            }
            else if ((player.transform.position.x ) > (transform.position.x + ((int) direction * rangePoint)+0.15))
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveLeft(-1);
                }
                else
                {
                    entity.MoveRight(1);
                }
            }
            else
                shooting = true;
        }
        else
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
    }
}
