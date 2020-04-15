using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBasicBullet : BulletSharedClass
{
    void Start()
    {

    }

    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }
}
