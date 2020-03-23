using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    private Direction direction = Direction.RIGHT;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        entity = GetComponent<Entity>();
        TankSkill ts = entity.entitySkill as TankSkill;
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
         
                transform.rotation =  Quaternion.Euler(0,0,0);
            }
            else if (player.transform.position.x < transform.position.x &&(direction == Direction.RIGHT)) 
            {
                direction = Direction.LEFT;
                transform.rotation = Quaternion.Euler(0,180,0);
            }
            if (player.transform.position.x - (transform.position.x + (rangePoint * (int)direction)) < -0.15)
                entity.MoveLeft();
            else if (player.transform.position.x - (transform.position.x + (rangePoint * (int)direction)) > 0.15)
                entity.MoveRight();
            else
                shooting = true;
        }
        else
        {
            if (entity.Shoot())
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
