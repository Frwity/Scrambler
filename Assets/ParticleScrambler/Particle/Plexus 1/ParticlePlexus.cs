using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePlexus : MonoBehaviour                     //Script GD Martin
{
    public float maxDistance = 1f;                          //Initialisation

    public int maxConnections = 5;
    int maxLineRenderers = 1000;

    new ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;

    ParticleSystem.MainModule particleSystemMain;

    public LineRenderer lineRendererTemplate;
    List<LineRenderer> lineRenderers = new List<LineRenderer>();

    Transform _transform;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystemMain = particleSystem.main;

        _transform = transform;
    }

    
    void LateUpdate()
    {
        int maxParticles = particleSystemMain.maxParticles;

        if (particles == null || particles.Length < maxParticles)               //Attribution of Particles Array Lenght
        {
            particles = new ParticleSystem.Particle[maxParticles];
        }
        int lrIndex = 0;

        int lineRendererCount = lineRenderers.Count;

        if(lineRendererCount > maxLineRenderers)
        {
            for(int i = maxLineRenderers; i < lineRendererCount; i++)
            {
                Destroy(lineRenderers[i].gameObject);
            }

            int removedCount = lineRendererCount - maxLineRenderers;
            lineRenderers.RemoveRange(maxLineRenderers, removedCount);
            lineRendererCount = removedCount;
        }

        if (maxConnections > 0 && maxLineRenderers > 0)
        {
            particleSystem.GetParticles(particles);

            int particleCount = particleSystem.particleCount;

            float maxDistanceSqr = maxDistance * maxDistance;

            ParticleSystemSimulationSpace simulationSpace = particleSystemMain.simulationSpace;

            switch (simulationSpace)
            {
                case ParticleSystemSimulationSpace.Local:
                    {
                        _transform = transform;
                        lineRendererTemplate.useWorldSpace = false;
                        break;
                    }
                case ParticleSystemSimulationSpace.Custom:
                    {
                        _transform = particleSystemMain.customSimulationSpace;
                        lineRendererTemplate.useWorldSpace = false;
                        break;
                    }
                case ParticleSystemSimulationSpace.World:
                    {
                        _transform = transform;
                        lineRendererTemplate.useWorldSpace = true;
                        break;
                    }
                default:
                    throw new System.NotSupportedException(string.Format("Unsupported simulation space", System.Enum.GetName(typeof(ParticleSystemSimulationSpace), particleSystemMain.simulationSpace)));

            }

            for (int i = 0; i < particleCount; i++)
            {
                if (lrIndex == maxLineRenderers)
                {

                } 

                Vector3 p1_Position = particles[i].position;                        // Position of the first Point

                int connections = 0;

                for (int j = i + 1; j < particleCount; j++)
                {
                    Vector3 p2_Position = particles[j].position;                    // Position of the second Point
                    float distanceSqr = Vector3.SqrMagnitude(p1_Position - p2_Position);    // Retrieve the distance to connect the two points

                    if (distanceSqr <= maxDistanceSqr)
                    {
                        LineRenderer lr;
                        if (lrIndex == lineRendererCount)
                        {
                            lr = Instantiate(lineRendererTemplate, _transform, false);
                            lineRenderers.Add(lr);

                            lineRendererCount++;
                        }
                        lr = lineRenderers[lrIndex];

                        lr.enabled = true;
                        lr.useWorldSpace = simulationSpace == ParticleSystemSimulationSpace.World ? true : false;

                        lr.SetPosition(0, p1_Position);
                        lr.SetPosition(1, p2_Position);

                        lrIndex++;
                        connections++;

                        if (connections == maxConnections || lrIndex == maxLineRenderers)
                        {
                            break;
                        }
                    }
                }
            }
        }
        for (int i = lrIndex; i < lineRendererCount; i++)
        {
            lineRenderers[i].enabled = false;
        }
    }
}
