using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [SerializeField]
    private float hoverSpeed;
    [SerializeField]
    private float maxHeight;
    [SerializeField]
    private float minHeight;

    private float hoverHeight;
    private float hoverRange;

    void Start()
    {
        hoverHeight = (maxHeight + minHeight) / 2.0f;
        hoverRange = maxHeight - minHeight;
    }

    void Update()
    {
        transform.Translate(0, (hoverHeight + Mathf.Cos(Time.time * hoverSpeed) * hoverRange) * Time.deltaTime, 0);
    }
}
