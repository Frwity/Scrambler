using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float fireCount;
    [SerializeField]
    private GameObject bullet;
    private GameObject body;

    private float nbFired;
    private float lastFired;
    private int direction;
    private bool shooting;

    GameObject player;

    void Start()
    {
        nbFired = 0;
        direction = 0;
        lastFired = 0;
        shooting = false;
        player = GameObject.FindGameObjectWithTag("Player");
        body = transform.GetChild(0).gameObject;
    }


    void Update()
    {
        if (!shooting)
        { 
            if (player.transform.position.x - transform.position.x < -0.25)
                direction = -1;
            else if (player.transform.position.x - transform.position.x > 0.25)
                direction = 1;
            else
            {
                shooting = true;
                direction = 0;
            }
            transform.Translate(Time.deltaTime * speed * direction, 0, 0);
        }
        else
        {
            if (Time.time > lastFired + fireRate)
            {
                lastFired = Time.time;
                Instantiate(bullet, body.transform.position + Vector3.down, Quaternion.identity);
                nbFired++;
                if (nbFired >= fireCount)
                { 
                    shooting = false;
                    nbFired = 0;
                }
            }
        }
    }
}
