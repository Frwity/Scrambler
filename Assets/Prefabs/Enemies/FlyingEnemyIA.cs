using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyIA : MonoBehaviour
{
    public bool isActive;
    private Entity entity;
    
    [SerializeField] private float fireCount;
    private float nbFired;
    private bool shooting;
    private float currentLostTimer = 0.0f;
    [SerializeField]private float lostTimer = 0.0f;
    
    [SerializeField] private Road Path;
    [SerializeField] private float AIResetTimer = 0.0f;
    private float currentAIResetTimer = 0.0f;
    void Start()
    {
        nbFired = 0;
        shooting = false;

        entity = GetComponent<Entity>();
    }

    void Update()
    {
        if (!isActive)
            return;
        if (!shooting && entity.isPlayerInSight)
        {
            //Debug.Log("EEEE");
            currentLostTimer = 0.0f;
            if (entity.lastPlayerPosKnown.x - transform.position.x < -0.25)
                entity.MoveLeft(-1);
            else if (entity.lastPlayerPosKnown.x - transform.position.x > 0.25)
                entity.MoveRight(1);
            else
            {
                shooting = true;
            }
        }
        else if (entity.LostPlayer)
        {
            if (currentLostTimer < lostTimer)
            {
                currentLostTimer += Time.smoothDeltaTime;
                return;
            }
            currentAIResetTimer += Time.smoothDeltaTime;
            if (entity.lastPlayerPosKnown.x - transform.position.x < -0.25)
                entity.MoveLeft(-1);
            else if (entity.lastPlayerPosKnown.x - transform.position.x > 0.25)
                entity.MoveRight(1);
            else if (currentAIResetTimer > AIResetTimer)
            {
                entity.LostPlayer = false;
            }
           
        }
        else if (shooting)
        {
            if (entity.Shoot((entity.lastPlayerPosKnown - transform.position).normalized))
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
           
            if ((currentCheckpointPosX ) < (transform.position.x  -0.15))
            {
                entity.MoveLeft(-1);
            }
            else if ((currentCheckpointPosX ) > (transform.position.x +0.15))
            {
                entity.MoveRight(1);
                
            }
            else
            {
                Path.CurrentIndex++;
            }
        }
    }
}
