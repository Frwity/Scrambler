using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerControl : MonoBehaviour
{
    public Vector2 RoundVector2(Vector2 vector, int multiplier = 1)
    {
        return new Vector2(Mathf.Round(vector.x * multiplier) / multiplier,
                           Mathf.Round(vector.y * multiplier) / multiplier);
    }
    [SerializeField] public GameObject virus;
    [HideInInspector] public RoomManager actualRoom;

    private Cinemachine.CinemachineVirtualCamera virtualCamera;
    private GameObject tTransform;

    [HideInInspector] public Entity entity;
    [HideInInspector] public bool isInVirus;
    private GameObject collidingObj;

    [SerializeField] bool autoSetDefaultSpawnPt = false;
    [SerializeField] private Vector3 defaultSpawnPoint;

    [SerializeField] private float controlCD;
    [HideInInspector] public float lastControl;
    [HideInInspector] public short lastDirection;
    private Vector3 aimDireciton;
    private bool jumped;

    void Start()
    {
        virtualCamera = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        lastControl = 0;
        entity = GetComponentInChildren<Entity>();
        jumped = false;
        isInVirus = true;
        tTransform = new GameObject();
        tTransform.name = "CameraFocus";
        virtualCamera.Follow = tTransform.transform;
        virtualCamera.LookAt = tTransform.transform;

        if (autoSetDefaultSpawnPt) defaultSpawnPoint = transform.GetChild(0).position;
    }

    void Update()
    {
        if (!entity) // on entity die
        {
            if (!isInVirus)
            {
                entity = Instantiate(virus, tTransform.transform.position, Quaternion.identity, transform).GetComponent<Entity>();
            }
            else if (actualRoom)
            {
                entity = Instantiate(virus, actualRoom.respawnPoint, Quaternion.identity, transform).GetComponent<Entity>();
                actualRoom.ResetRoom();
            }
            else
            {
                entity = Instantiate(virus, defaultSpawnPoint, Quaternion.identity, transform).GetComponent<Entity>();               
            }
            isInVirus = true;
            return;
        }

        aimDireciton.x = Input.GetAxis("RHorizontal");
        aimDireciton.y = Input.GetAxis("RVertical");

        entity.AimDirection(RoundVector2(aimDireciton));

        if (Input.GetAxisRaw("RT") == 1)
        {
            entity.Shoot(RoundVector2(aimDireciton));
        }

        collidingObj = entity.Collinding();

        if (collidingObj != null 
        && collidingObj.CompareTag("PossessZone") 
        && collidingObj.transform.parent.GetComponent<Entity>() != null 
        && collidingObj.transform.parent.GetComponent<Entity>().isControllable() 
        && Input.GetAxisRaw("Fire3") == 1 
        && Time.time > controlCD + lastControl && isInVirus) // TODO plsu bo
        {
            lastControl = Time.time;
            Destroy(transform.GetChild(0).gameObject);
            collidingObj.transform.parent.parent = transform;
            entity = collidingObj.transform.parent.GetComponent<Entity>();
            isInVirus = false;
            entity.tag = "Player";
            entity.DesactivateAI();
            entity.possessFlash();
            tTransform.transform.position = entity.transform.position;
            return;
        }
        if (Input.GetAxisRaw("Fire3") == 1 && Time.time > controlCD + lastControl && !isInVirus)
        {
            lastControl = Time.time;
            entity.transform.parent = transform.parent;
            entity.tag = "Enemy";
            entity = Instantiate(virus, entity.transform.position + (Vector3.up * 3), Quaternion.identity, transform).GetComponent<Entity>();
            entity.GetComponent<VirusSkill>().jumped = 1;
            isInVirus = true;
            tTransform.transform.position = entity.transform.position;
            return;
        }
        if (Input.GetAxisRaw("Fire3") == 1 && Time.time > controlCD + lastControl && isInVirus)
        {
            if (!entity.InteractWithBG())
            {
                if (entity.InteractWithFG())
                    lastControl = Time.time;
            }
            else
                lastControl = Time.time;

        }
        else if (entity.isHidden && Input.GetAxis("Fire3") == 0)
            entity.Uninteract();
        tTransform.transform.position = entity.transform.position;
    }

    private void FixedUpdate()
    {
        if (!entity)
            return;
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            entity.MoveRight(0);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0.01)
        {
            entity.MoveRight(Input.GetAxis("Horizontal"));
            lastDirection = 1;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0.01)
        {
            entity.MoveLeft(Input.GetAxis("Horizontal"));
            lastDirection = -1;
        }
        if (Input.GetAxis("Fire1") == 0)
        {
            jumped = false;
        }
        if (!jumped && Input.GetAxisRaw("Fire1") == 1)
        {   
            jumped = true;
            entity.Jump();
        }
    }
}
