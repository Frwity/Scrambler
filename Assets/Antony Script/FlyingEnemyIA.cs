using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyIA : MonoBehaviour
{
    public bool isActive;
    
    private Entity entity;
    
    [SerializeField]
    private float fireCount;
    private float nbFired;

    private bool shooting;

    GameObject player;

    void Start()
    {
        nbFired = 0;
        isActive = true;
        shooting = false;
        player = GameObject.FindGameObjectWithTag("Player");
        entity = GetComponent<Entity>();
    }

    void Update()
    {
        if (!isActive)
            return;
        if (!shooting)
        {
            if (player.transform.position.x - transform.position.x < -0.25)
                entity.MoveLeft();
            else if (player.transform.position.x - transform.position.x > 0.25)
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
