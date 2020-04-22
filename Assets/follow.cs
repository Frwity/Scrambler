using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform pointer = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pointer.position;
    }
}
