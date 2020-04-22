using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : Activable
{
    [SerializeField] private int damage = 0;
    [SerializeField] private float timeToTrigger = 0f;
    [SerializeField] private float timeToReTrigger = 0f;
    private float actualTriggeringTime = 0f;

    private Entity entity = null;

    private bool isTriggering = false;
    private float timer = 0f;

    public float lifeTime = -1f; // May be modified externally at creation / Set at 0 or bellow and the time drain will not be triggered (infinite life time)

    void Start()
    {
        actualTriggeringTime = timeToTrigger;
        isTriggering = false;
        timer = 0;
    }


    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0) Destroy(gameObject);
        }

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
