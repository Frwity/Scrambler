using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckshotPellet : BulletSharedClass
{
    [SerializeField] float speedImprecision = 1f;

    // Start is called before the first frame update
    void Start()
    {
        speed += Random.Range(-speedImprecision, speedImprecision);

        GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
