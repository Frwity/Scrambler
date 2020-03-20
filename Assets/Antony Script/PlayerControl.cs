using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float controlCD;
    private float lastControl;
    [SerializeField]
    private GameObject virus;

    private Entity entity;

    private bool isInVirus;

    void Start()
    {
        lastControl = 0;
        entity = GetComponentInChildren<Entity>();
        isInVirus = true;
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
            entity.MoveRight();

        if (Input.GetAxis("Horizontal") < 0)
            entity.MoveLeft();

        if (Input.GetAxisRaw("Fire1") == 1)
            entity.Shoot();

        if (entity.Collinding() != null && Input.GetAxisRaw("Fire3") == 1 && Time.time > controlCD + lastControl && isInVirus) // TODO opti
        {
            lastControl = Time.time;
            Destroy(transform.GetChild(0).gameObject);
            entity.Collinding().gameObject.transform.parent = transform;
            entity = entity.Collinding().gameObject.GetComponent<Entity>();
            isInVirus = false;
            entity.DesactivateAI();
        }
        if (Input.GetAxisRaw("Fire3") == 1 && Time.time > controlCD + lastControl && !isInVirus)
        {
            lastControl = Time.time;
            entity.transform.parent = transform.parent;
            entity = Instantiate(virus, entity.transform.position + (Vector3.up * 3), Quaternion.identity, transform).GetComponent<Entity>();
            isInVirus = true;
            entity.ActivateAI();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Fire2") == 1)
            entity.Jump();
    }
}
