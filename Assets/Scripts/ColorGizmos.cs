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
        Vector3 c = GetComponent<BoxCollider>().center;
        Gizmos.matrix = Matrix4x4.TRS(transform.position  , transform.rotation, transform.lossyScale);
        Gizmos.color = color;
        Vector3 s = GetComponent<BoxCollider>().size;
        
        Gizmos.DrawWireCube(c, s);
    }
}
