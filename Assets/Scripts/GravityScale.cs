using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScale : MonoBehaviour
{
    [SerializeField] float gravity;

    private Rigidbody rb;

    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isOnGround = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isOnGround)
            rb.AddForce(new Vector3(0, gravity, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isOnGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
            isOnGround = false;
    }
}
