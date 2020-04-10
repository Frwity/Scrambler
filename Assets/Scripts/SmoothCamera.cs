using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] Vector3 defaultDistance = new Vector3(0, 2f, -10f);
    [SerializeField] Vector3 offsetToTarget = new Vector3(5f, 0, 0); // used primarly for X
    [SerializeField] float dampDistance = 2f;
    [SerializeField] float dampRotation = 1f;

    Transform myTrsf;

    Transform target;
    PlayerControl plrCtrl;

    private void Start()
    {
        plrCtrl = GameObject.FindGameObjectWithTag("PlayerMove").GetComponent<PlayerControl>();
        ActualizeTarget();
    }
    private void Awake()
    {
        myTrsf = transform;
    }

    public void ActualizeTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 ToPos = (target.position + (offsetToTarget * plrCtrl.lastDirection)) + (target.rotation * defaultDistance);
        myTrsf.position = Vector3.Lerp(myTrsf.position, ToPos, dampDistance * Time.deltaTime);

        Quaternion toRot = Quaternion.LookRotation((target.position + (offsetToTarget * plrCtrl.lastDirection)) - myTrsf.position, target.up);
        myTrsf.rotation = Quaternion.Slerp(myTrsf.rotation, toRot, dampRotation * Time.deltaTime);
    }
}
