using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaLaser : Activable
{
    [SerializeField] float delay = 0f;
    [SerializeField] float cooldowTime = 2f;
    [SerializeField] float warningTime = 2f; // Also serves for the charge-up squence
    [SerializeField] float fireTime = 1f;

    float totalTime = 0f;
    float currentTime = 0f;
    
    [SerializeField] GameObject laserObject = null;
    [SerializeField] Vector3 laserDimension = Vector3.zero;

    GameObject actualLazer = null;

    GameObject waningSphere = null;
    [SerializeField] float maxWarningSize = 2f;

    // Start is called before the first frame update
    void Start()
    {
        totalTime = cooldowTime + warningTime + fireTime;

        currentTime -= delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            if (actualLazer)
                Destroy(actualLazer);
            return;
        }

        currentTime += Time.deltaTime;

        if (currentTime < cooldowTime)
        {
            
        }
        else if (currentTime < cooldowTime + warningTime)
        {
            if (!waningSphere)
            {
                waningSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                waningSphere.transform.position = transform.position - transform.forward;

                waningSphere.GetComponent<Renderer>().material.color = laserObject.GetComponentInChildren<Renderer>(false).sharedMaterial.color;
                
                waningSphere.transform.localScale = new Vector3(0, 0, 0);
            }

            if (waningSphere.transform.localScale.x < maxWarningSize)
            {
                float expansionRate = Time.deltaTime * maxWarningSize / warningTime;

                waningSphere.transform.Translate(0, 0, -expansionRate / 2);
                waningSphere.transform.localScale += new Vector3(expansionRate, expansionRate, expansionRate);
            }
        }
        else if (currentTime < cooldowTime + warningTime + fireTime)
        { 
            if (!actualLazer)
            {
                actualLazer = Instantiate(laserObject, transform.position - transform.forward, Quaternion.identity);
                actualLazer.transform.localScale = laserDimension;

                actualLazer.transform.Translate(0, 0, -laserDimension.z / 2);


                waningSphere.transform.localScale = Vector3.zero;
                waningSphere.transform.position = transform.position - transform.forward;
            }
        }


        if (currentTime > totalTime)
        { 
            currentTime = 0f;

            if (actualLazer) 
                Destroy(actualLazer);
        }
    }
}
