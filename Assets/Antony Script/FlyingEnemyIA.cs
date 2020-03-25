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
    private bool haveTarget;
    Vector3 targetPos;

    void Start()
    {
        nbFired = 0;
        shooting = false;
        haveTarget = false;

        entity = GetComponent<Entity>();
    }

    void Update()
    {
        if (!isActive || !haveTarget && !shooting)
            return;
        if (!shooting)
        {
            if (targetPos.x - transform.position.x < -0.25)
                entity.MoveLeft(-1);
            else if (targetPos.x - transform.position.x > 0.25)
                entity.MoveRight(1);
            else
                shooting = true;
        }
        else
        {
            if (entity.Shoot(new Vector3(0, 0, 0)))
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
    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            haveTarget = true;
            targetPos = other.gameObject.transform.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            haveTarget = false;
        }
    }
}
