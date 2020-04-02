using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travolator : LDBlock
{
    [SerializeField] private Direction direction;
    [SerializeField] private float speed;

    private int directionInt;

    void Start()
    {
        if (direction == Direction.LEFT)
            directionInt = -1;
        else if (direction == Direction.RIGHT)
            directionInt = 1;
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive)
            return;
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            other.transform.Translate(speed * directionInt * Time.deltaTime, 0, 0);
    }
}
