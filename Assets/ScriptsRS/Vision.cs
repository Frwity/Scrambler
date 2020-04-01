using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    private Entity entity;
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (CompareTag("Player") || CompareTag("PossessZone"))
        {
            Debug.Log($"collided with {gameObject.name}");
            return;
        }

        
        Debug.Log($"{name}");
        float mag = (other.transform.position - transform.position).magnitude;
        //int visionMask = LayerMask.NameToLayer("Vision");
        int visionMask = 1 << 13 ;
        int hoverMask = 1 << 8;
        int bulletMask = 1 << 10;
        int shieldMask = 1 << 14;
        int PossessMask = 1 << 15;
        bulletMask = ~bulletMask;
        hoverMask = ~hoverMask;
        visionMask = ~visionMask;
        shieldMask = ~shieldMask;
        PossessMask = ~PossessMask;
        
        visionMask += hoverMask;
        visionMask += bulletMask;
        visionMask += shieldMask;
        visionMask += PossessMask;
        Vector3 playerPos;
        
        if (other.CompareTag("Player"))
        {
            playerPos = other.transform.position;
            //Debug.Log("player found");
        }
        else
        {
            //Debug.Log("player not found");
            return;
        }

        playerPos = (playerPos - transform.position);
        playerPos.y -= other.transform.localScale.y/2.0f;
        
        for (int i = 0; i <= 2; i++)
        {
            Vector2 toShoot = playerPos;
            toShoot.y += (other.transform.localScale.y / 2.0f)* i;
            toShoot.Normalize();
            Ray ray = new Ray(transform.position, toShoot);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, mag + 10.0f, visionMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    entity.isPlayerInSight = true;
                    entity.LostPlayer = false;
                    entity.lastPlayerPosKnown = other.transform.position;
                }
                else
                {
                    continue;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            entity.isPlayerInSight = false;
            entity.LostPlayer = true;
        }
    }
}
