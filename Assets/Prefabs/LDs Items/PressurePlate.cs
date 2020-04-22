using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Activable
{
    [SerializeField] private GameObject toTrigger = null;

    private Activable activable;
    void Start()
    {
        if (!toTrigger)
        {
            isActive = false;
            return;
        }
        if (toTrigger.GetComponent<Activable>())
        {
            activable = toTrigger.GetComponent<Activable>();
            isActive = true;
        }
        else
            isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
            return;
        if (other.CompareTag("Player"))
        {
            activable.isActive = !activable.isActive;
        }
    }
}
