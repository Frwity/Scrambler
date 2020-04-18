using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Entity;
    [SerializeField]
    private float spawnRate;
    private float lastTime;
    [SerializeField]
    private float remainingSpawn;
    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingSpawn >= 1 && Time.time - lastTime > spawnRate)
        { 
            Instantiate(Entity, transform.position, Quaternion.identity);
            lastTime = Time.time;
            remainingSpawn--;
        }
    }
}
