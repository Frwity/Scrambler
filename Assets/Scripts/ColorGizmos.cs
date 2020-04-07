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
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.color = color;
        Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
    }
}
