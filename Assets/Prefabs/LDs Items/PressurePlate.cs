using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject toTrigger;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            toTrigger.GetComponent<LDBlock>().isActive = !toTrigger.GetComponent<LDBlock>().isActive;
    }
}
