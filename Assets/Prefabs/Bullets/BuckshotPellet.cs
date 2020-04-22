using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckshotPellet : BulletSharedClass
{
    [SerializeField] float speedImprecision = 1f;

    // Start is called before the first frame update
    protected override void Start()
    {
        speed += Random.Range(-speedImprecision, speedImprecision);

        GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);

        shooterTag = shooter.gameObject.tag;
    }
}
