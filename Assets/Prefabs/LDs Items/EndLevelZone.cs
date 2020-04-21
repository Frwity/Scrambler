using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelZone : MonoBehaviour
{
    [SerializeField] Color gizmoColor = Color.blue;

    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireCube(box.center, box.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("EndScreen").GetComponent<Animator>().SetTrigger("Win");

            other.GetComponentInParent<PlayerControl>().enabled = false;
        }
    }
}
