using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalculPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 ContactPoint;
    public bool hasTouched = false;
    public PhysicsScene myScene;
    public PhysicsScene playSceneP;
    public Scene Calculs;
    public Scene playScene;
    void Start()
    {

     
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
           
            
            
        
    }

    private void OnCollisionEnter(Collision other)
    {
        hasTouched = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        
        ContactPoint = transform.position;
    }

    
}
