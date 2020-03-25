using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 defaultDistance = new Vector3(0, 2f, -10f);
    [SerializeField] float dampDistance = 10f;
    [SerializeField] float dampRotation = 10f;

    Transform myTrsf;

    private void Awake()
    {
        myTrsf = transform;
    }

    public void ActualizeTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 ToPos = target.position + (target.rotation * defaultDistance);
        myTrsf.position = Vector3.Lerp(myTrsf.position, ToPos, dampDistance * Time.deltaTime);

        Quaternion toRot = Quaternion.LookRotation(target.position - myTrsf.position, target.up);
        myTrsf.rotation = Quaternion.Slerp(myTrsf.rotation, toRot, dampRotation * Time.deltaTime);
    }
}
