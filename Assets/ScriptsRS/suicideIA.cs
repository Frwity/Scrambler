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
            Debug.Log( transform.position.x );
            if ((player.transform.position.x ) < (transform.position.x  -0.45))
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveLeft();
                }
                else
                {
                    entity.MoveRight();
                }
                    
            }
            else if ((player.transform.position.x ) > (transform.position.x +0.45))
            {
                if (direction == Direction.RIGHT)
                {
                    entity.MoveRight();
                }
                else
                {
                    entity.MoveLeft();
                }
            }
            else
                shooting = true;
        }
        else
        {
            if (entity.Shoot())
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
