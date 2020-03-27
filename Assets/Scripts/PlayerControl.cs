using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float controlCD;
    [HideInInspector] public float lastControl;
    public GameObject virus;

    [HideInInspector] public Entity entity;

    [HideInInspector] public bool isInVirus;

    public short lastDirection;

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

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            entity.MoveRight(0);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            entity.MoveRight(Input.GetAxis("Horizontal"));
            lastDirection = 1;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            entity.MoveLeft(Input.GetAxis("Horizontal"));
            lastDirection = -1;
        }

        aimDireciton.x = Input.GetAxis("RHorizontal");
        aimDireciton.y = -Input.GetAxis("RVertical");

        entity.AimDirection(aimDireciton);

        if (Input.GetAxisRaw("RT") == 1)
        {
            entity.Shoot(aimDireciton);
        }

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

        GameObject.FindGameObjectWithTag("Camera").GetComponent<SmoothCamera>().ActualizeTarget();
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Fire2") == 1)
            entity.Jump();
    }
}
