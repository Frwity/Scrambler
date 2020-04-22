using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : BulletSharedClass
{
    // Start is called before the first frame update
    protected override void Start()
    {
        transform.localRotation = Quaternion.LookRotation(direction);

        shooterTag = shooter.gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
    }
}
