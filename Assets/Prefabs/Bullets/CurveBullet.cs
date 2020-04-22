using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveBullet : CurvedBulletSharedClass
{   
    // Start is called before the first frame update
    protected override void Start()
    {
        transform.localRotation = Quaternion.LookRotation(direction);

        shooterTag = shooter.gameObject.tag;
    }
}
