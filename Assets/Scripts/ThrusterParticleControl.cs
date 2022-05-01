using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ThrusterParticleControl : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float thrustersPower = 0;
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
            ThrusterPower(thrustersPower);
        }
        
    }
#endif
    

    public void ThrusterPower(float value)
    {
        thrustersPower = Mathf.Clamp01(value);

        if (particle)
        {
            emissionModule.enabled = thrustersPower > 0;
            
            mainModule.startSpeed = thrustersPower * speedMultiplier;
            mainModule.startSize = thrustersPower * sizeMultiplier;
        }
    }
}
