using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveBullet : MonoBehaviour
{
    [SerializeField] private int damage;
    public Vector3 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        
        transform.localRotation = Quaternion.LookRotation(velocity);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        velocity += Physics.gravity * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.LookRotation(velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Entity>().InflictDamage(damage);
        }
        //Debug.Log($"collided at {collision.collider.name}");
        Vector3 v = collision.GetContact(0).point - new Vector3(5.33f, 1.78158f, 0);
        
        Destroy(this.gameObject);
    }
}
