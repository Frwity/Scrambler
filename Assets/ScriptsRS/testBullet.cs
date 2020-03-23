using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBullet : MonoBehaviour
{
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
        velocity += Physics.gravity * Time.fixedDeltaTime ;
        transform.localRotation = Quaternion.LookRotation(velocity);
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"collided at {other.GetContact(0).point}");
        Vector3 v = other.GetContact(0).point - new Vector3(5.33f, 1.78158f, 0);
        Debug.Log($" length from tank to point = {v.magnitude}");
        Destroy(this.gameObject);
    }
}
