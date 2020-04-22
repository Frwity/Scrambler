using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelZone : MonoBehaviour
{
    [SerializeField] Color gizmoColor = Color.blue;
    private bool hasEnded = false;
    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireCube(box.center, box.size);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!hasEnded)
            {
                GameObject.FindGameObjectWithTag("Manager").GetComponent<MenuManager>().levelEnded = true;
                hasEnded = true;
            }

            other.GetComponentInParent<PlayerControl>().enabled = false;
        }
    }
}
