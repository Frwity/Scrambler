using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyAI : MonoBehaviour
{
    public bool isActive;
    private Entity entity;
    private float currentLostTimer = 0.0f;
    [SerializeField]private float lostTimer = 0.0f;
    private bool shooting = false;
    //private bool dashing;
    private bool haveTarget;
    Vector3 targetPos;

    void Start()
    {
        //dashing = false;
        haveTarget = false;

        entity = GetComponent<Entity>();
    }

    void Update()
    {
        if (!isActive)
            return;
        if(entity.isPlayerInSight)
        {
            currentLostTimer = 0.0f;
            if (entity.lastPlayerPosKnown.x - transform.position.x < -0.25)
                entity.MoveLeft(-1);
            else if (entity.lastPlayerPosKnown.x - transform.position.x > 0.25)
                entity.MoveRight(1);
        }
        else if (entity.LostPlayer)
        {
            if (currentLostTimer < lostTimer)
            {
                currentLostTimer += Time.smoothDeltaTime;
                return;
            }
            if (entity.lastPlayerPosKnown.x - transform.position.x < -0.25)
                entity.MoveLeft(-1);
            else if (entity.lastPlayerPosKnown.x - transform.position.x > 0.25)
                entity.MoveRight(1);
        }
        else if (shooting)
        {
            
        }
    }
    
}
