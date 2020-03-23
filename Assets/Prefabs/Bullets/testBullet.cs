using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBullet : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float speed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * speed, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Entity>().InflictDamage(damage);
        }
        Destroy(gameObject);

    }
}
