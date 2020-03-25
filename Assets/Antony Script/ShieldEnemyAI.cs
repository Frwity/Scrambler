using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyAI : MonoBehaviour
{
    public bool isActive;
    private Entity entity;

    private bool dashing;
    private bool haveTarget;
    Vector3 targetPos;

    void Start()
    {
        dashing = false;
        haveTarget = false;

        entity = GetComponent<Entity>();
    }

    void Update()
    {
        if (!isActive || !haveTarget)
            return;

        if (targetPos.x - transform.position.x < -0.25)
            entity.MoveLeft(-1);
        else if (targetPos.x - transform.position.x > 0.25)
            entity.MoveRight(1);
       
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
