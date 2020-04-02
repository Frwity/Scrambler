using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGizmos : MonoBehaviour
{
    [SerializeField] private Color color;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
