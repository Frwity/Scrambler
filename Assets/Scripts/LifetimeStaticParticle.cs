using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleLauncher : MonoBehaviour
{
    public static void ActivateParticleWithName(GameObject obj, Transform newParent, string name)
    {
        LifetimeStaticParticle[] particles = obj.GetComponentsInChildren<LifetimeStaticParticle>();

        foreach(LifetimeStaticParticle particle in particles)
        {
            if (particle.gameObject.name == name)
            {
                Instantiate(particle, particle.transform.position, particle.transform.rotation, newParent).ActivateParticle();
            }
        }
    }

    public static void ActivateParticleWithNewParent(LifetimeStaticParticle particle, Transform newParent)
    {
        if (particle)
        {
            LifetimeStaticParticle newParticle = Instantiate(particle, particle.transform.position, particle.transform.rotation, newParent);
            newParticle.ActivateParticle();
            newParticle.transform.parent = newParent;
        }
    }
}

public class LifetimeStaticParticle : MonoBehaviour
{
    [HideInInspector] public float lifetime = 1.0f;
    private bool activate = false;
    Quaternion rotation;
    Quaternion lrot;


    void Awake()
    {
        rotation = transform.rotation;
        lrot = transform.localRotation;
    }

    void Update()
    {
        if (!activate)
            return;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Destroy(gameObject);
    }

    void LateUpdate()
    {
        //transform.localRotation = lrot;
    }

    public void ActivateParticle()
    {
        activate = true;
        lifetime = GetComponent<ParticleSystem>().main.duration;
        GetComponent<ParticleSystem>().Play();
        transform.parent = null;
        if (transform.GetChild(0).GetComponent<AudioSource>())
            transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    private void OnDestroy()
    {
        if (transform.GetChild(0).GetComponent<AudioSource>())
        {
            transform.GetChild(0).GetComponent<AudioSource>().Stop();
            transform.GetChild(0).GetComponent<AudioSource>().mute = true;
        }
    }

    private void OnApplicationQuit()
    {
        DestroyImmediate(gameObject);
    }
}
