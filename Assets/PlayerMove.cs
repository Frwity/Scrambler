using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private int nbJump;

    [SerializeField]
    private float jumpForce;

    private int jumped;

    private bool falling;


    private Rigidbody rb;

    void Start()
    {
        falling = true;
        jumped = 0;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);
    }

    private void FixedUpdate()
    {
        if (jumped < nbJump && falling && Input.GetAxisRaw("Fire2") != 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumped++;
            falling = false;
        }
        if (rb.velocity.y >= -0.1 && rb.velocity.y <= 0.1)
            falling = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumped = 0;
        }
    }
}
