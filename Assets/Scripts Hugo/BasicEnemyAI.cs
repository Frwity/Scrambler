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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        reactionTime = maxReactionTime;

        lookingRight = flipped;

        entity = GetComponent<Entity>();
    }

    void Update()
    {
        if (!isActive || player == null)
            return;
        IAcontrol();
    }

    public void FlipAround()
    {
        transform.Rotate(0, (flipped ? -180 : 180), 0);
        flipped ^= true;
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

                lookingRight ^= true;

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

        if (AIflipControl(vecToPlayer.x) && distToPlayer < detectionRange)
        {
            if (distanceToKeepAwayFromPlayer < distToPlayer)
            {
                if (vecToPlayer.x > 0)
                    entity.MoveRight();
                else
                    entity.MoveLeft();
            }

            if (lowerFireRange < distToPlayer && distToPlayer < upperFireRange)
                entity.Shoot();
        }
    }
}