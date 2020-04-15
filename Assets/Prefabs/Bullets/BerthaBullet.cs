using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerthaBullet : BulletSharedClass
{
    

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(-90, 0, 0);
        transform.Rotate(Quaternion.LookRotation(direction).eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
