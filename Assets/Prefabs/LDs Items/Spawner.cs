using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Activable
{
    [SerializeField] private GameObject Entity = null;
    [SerializeField] private float spawnRate = 0f;
    
    private float lastTime = 0f;
    
    [SerializeField] private float remainingSpawn = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;
        if (remainingSpawn >= 1 && Time.time - lastTime > spawnRate)
        { 
            Instantiate(Entity, transform.position, Quaternion.identity);
            lastTime = Time.time;
            remainingSpawn--;
        }
        if (remainingSpawn <= 0)
            Destroy(gameObject);
    }
}
