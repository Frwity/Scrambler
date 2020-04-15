using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buckshot : BulletSharedClass
{
    [SerializeField] float coneOfFire = 40; // in degrees, positive half for upper, negative half for lower.

    float lowerAngle;
    float upperAngle;

    [SerializeField] short numberOfBullets = 5;

    float anglesInBetween;

    [SerializeField] GameObject buckshotPellet;


    // Start is called before the first frame update
    void Start()
    {
        lowerAngle = -coneOfFire / 2;
        upperAngle = coneOfFire / 2;
        
        anglesInBetween = (upperAngle - lowerAngle) /numberOfBullets;


        for (float angle = lowerAngle; angle < upperAngle; angle += anglesInBetween)
        {
            GameObject firedBullet = Instantiate(buckshotPellet, transform.position, Quaternion.identity);

            BulletSharedClass firedBulletInfo = firedBullet.GetComponent<BulletSharedClass>();

            firedBulletInfo.direction = Quaternion.Euler(0, 0, angle) * direction;
            firedBulletInfo.shooter = shooter;
        }

        Destroy(gameObject);
    }
}
