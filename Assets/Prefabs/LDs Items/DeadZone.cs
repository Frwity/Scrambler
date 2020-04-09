using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : LDBlock
{
    [SerializeField] private int damage;
    [SerializeField] private float timeToTrigger;
    [SerializeField] private float timeToReTrigger;
    private float actualTriggeringTime;

    private Entity entity;

    private bool isTriggering;
    private float timer;

    void Start()
    {
        actualTriggeringTime = timeToTrigger;
        isTriggering = false;
        timer = 0;
    }


    void Update()
    {
        if (!isActive || !isTriggering || !entity)
            return;

        timer += Time.deltaTime;
        if (timer > actualTriggeringTime)
        {
            entity.InflictDamage(damage);
            actualTriggeringTime = timeToReTrigger;
            timer = 0;
            isTriggering = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggering = true;
            entity = other.gameObject.GetComponent<Entity>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0;
            isTriggering = false;
            actualTriggeringTime = timeToTrigger;
            entity = null;
        }
    }
}
