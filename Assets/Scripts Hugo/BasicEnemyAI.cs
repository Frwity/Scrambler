﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour
{
    public bool isActive;
    private Entity entity;

    [SerializeField] private float upperFireRange;
    [SerializeField] private float lowerFireRange;

    [SerializeField] private float detectionRange; // must be > upperFireRange
    [SerializeField] private float distanceToKeepAwayFromPlayer;

    [SerializeField] private float maxReactionTime;

    [SerializeField] private bool flipped; // shall be set to false if at it's spawn, the enemy is looking left.

    private GameObject player;

    private float reactionTime;

    private bool lookingRight; // Obviously, if FALSE, the enemy is looking left.

    private BasicEnemySkill associatedBES;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        reactionTime = maxReactionTime;

        lookingRight = flipped;

        entity = GetComponent<Entity>();

        associatedBES = GetComponent<BasicEnemySkill>();

        associatedBES.direction = (short)(lookingRight ? 1 : -1);
    }

    void Update()
    {
        if (isActive && player != null)
            IAcontrol();
        else
            PLdirectionLookCoheranceCheck();
    }

    private void AIdirectionLookCoheranceCheck()
    {
        if ((lookingRight == true && associatedBES.direction == -1) || (lookingRight == false && associatedBES.direction == 1))
        {
            associatedBES.direction = (short)(lookingRight ? 1 : -1);
        }
    }

    private void PLdirectionLookCoheranceCheck()
    {
        if ((lookingRight == true && associatedBES.direction == -1) || (lookingRight == false && associatedBES.direction == 1))
        {
            FlipAround();
        }
    }

    public void FlipAround()
    {
        transform.GetChild(0).Translate(new Vector3(0f, flipped ? 1f : -1f, 0));
        flipped ^= true;

        lookingRight ^= true;
    }

    private bool LookingAtPlayer(float playerXrelativetoEnemy)
    {
        if ((playerXrelativetoEnemy > 0 && !lookingRight) || (playerXrelativetoEnemy < 0 && lookingRight))
            return false;
        else
            return true;
    }

    private bool AIflipControl(float playerXrelativetoEnemy)
    {
        if (!LookingAtPlayer(playerXrelativetoEnemy))
        {
            if (reactionTime > 0)
                reactionTime -= Time.smoothDeltaTime;
            else
            {
                FlipAround();

                reactionTime = maxReactionTime;
            }

            return false;
        }
        else
        {
            if (reactionTime < maxReactionTime)
                reactionTime += Time.smoothDeltaTime / 2f;

            return true;
        }
    }

    private void IAcontrol()
    {
        Vector3 vecToPlayer = (player.transform.position - transform.position);
        float distToPlayer = vecToPlayer.magnitude;

        AIdirectionLookCoheranceCheck();

        if (AIflipControl(vecToPlayer.x) && distToPlayer < detectionRange)
        {
            if (distanceToKeepAwayFromPlayer < distToPlayer)
            {
                if (vecToPlayer.x > 0)
                    entity.MoveRight(1);
                else
                    entity.MoveLeft(-1);
            }

            if (lowerFireRange < distToPlayer && distToPlayer < upperFireRange)
                entity.Shoot(vecToPlayer.normalized);
        }
    }
}