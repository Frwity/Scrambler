using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float controlCD;
    private float lastControl;
    [SerializeField] private GameObject virus;

    private Entity entity;

    private bool isInVirus;

    private Vector3 aimDireciton;

    void Start()
    {
        lastControl = 0;
        entity = GetComponentInChildren<Entity>();
        isInVirus = true;
    }

    void Update()
    {

        if (Input.GetAxisRaw("Vertical") == 1)
            entity.HideInGB();
        if (entity.isHidden && Input.GetAxis("Vertical") < 1)
            entity.ExitHiding();

        entity.MoveRight(Input.GetAxis("Horizontal"));
        entity.MoveLeft(Input.GetAxis("Horizontal"));

        aimDireciton.x = Input.GetAxis("RHorizontal");
        aimDireciton.y = Input.GetAxis("RVertical");

        if (Input.GetAxisRaw("RT") == 1)
            entity.Shoot(aimDireciton);

        if (entity.Collinding() != null && entity.Collinding().gameObject.GetComponent<Entity>() != null && entity.Collinding().gameObject.GetComponent<Entity>().isControllable() 
        &&  Input.GetAxisRaw("Fire1") == 1 && Time.time > controlCD + lastControl && isInVirus) // TODO opti
        {
            lastControl = Time.time;
            Destroy(transform.GetChild(0).gameObject);
            entity.Collinding().gameObject.transform.parent = transform;
            entity = entity.Collinding().gameObject.GetComponent<Entity>();
            isInVirus = false;
            entity.tag = "Player";
            entity.DesactivateAI();
        }
        if (Input.GetAxisRaw("Fire1") == 1 && Time.time > controlCD + lastControl && !isInVirus)
        {
            lastControl = Time.time;
            entity.transform.parent = transform.parent;
            entity.ActivateAI();
            entity.tag = "Enemy";
            entity = Instantiate(virus, entity.transform.position + (Vector3.up * 3), Quaternion.identity, transform).GetComponent<Entity>();
            isInVirus = true;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Fire2") == 1)
            entity.Jump();
    }
}
