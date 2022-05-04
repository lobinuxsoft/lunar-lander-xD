using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleControl : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float particlePower = 0;
    [SerializeField] private float sizeMultiplier = 1, speedMultiplier = 1;
    
    private ParticleSystem particle;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        mainModule = particle.main;
        emissionModule = particle.emission;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!particle)
        {
            particle = GetComponent<ParticleSystem>();
            mainModule = particle.main;
            emissionModule = particle.emission;
        }
        else
        {
            ParticlePower(particlePower);
        }
        
    }
#endif
    

    public void ParticlePower(float value)
    {
        particlePower = Mathf.Clamp01(value);

        if (particle)
        {
            emissionModule.enabled = particlePower > 0;
            
            mainModule.startSpeed = particlePower * speedMultiplier;
            mainModule.startSize = particlePower * sizeMultiplier;
        }
    }
}
