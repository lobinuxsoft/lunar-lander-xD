using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class ShipDamageControl : MonoBehaviour
{
    [SerializeField] private AnimationCurve damageBehaviour;
    [SerializeField, Range(0, 1000)] private float explosionForce = 10;
    [SerializeField, Range(0, 1000)] private float explosionRadius = 20;
    [SerializeField] private ShipControlSettings shipSettings;
    [SerializeField] private ParticleSystem[] destroyParticles;
    
    private Rigidbody body;
    private Collider[] childColliders;

    public UnityEvent onShipDestroyed;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        childColliders = GetComponentsInChildren<Collider>();

        foreach (Collider childCol in childColliders)
        {
            if (childCol.name != name) childCol.enabled = false;
        }
        
        shipSettings.Repair();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float evaluation = damageBehaviour.Evaluate(shipSettings.CurVelocity / shipSettings.MaxVelocity);
        int damageCalculation = Mathf.RoundToInt(evaluation * shipSettings.MaxDurability);
        shipSettings.CurDurability -= damageCalculation;
        
        if(shipSettings.CurDurability <= 0) DestroyShip();
    }

    private void DestroyShip()
    {
        shipSettings.ThrustersPotency = 0;
        body.constraints = RigidbodyConstraints.FreezeAll;

        foreach (ParticleSystem dp in destroyParticles)
        {
            Instantiate(dp, transform.position, Quaternion.identity);
        }

        foreach (var childCol in childColliders)
        {
            if (childCol.name != name)
            {
                childCol.enabled = true;
                childCol.gameObject.AddComponent<Rigidbody>();
                childCol.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                
                if (childCol.TryGetComponent<ParticleSystem>(out ParticleSystem ps))
                {
                    ps.Play();
                }
            }
        }

        onShipDestroyed?.Invoke();
    }

    /// <summary>
    /// Delay in seconds
    /// </summary>
    /// <param name="damageRatio"></param>
    public async void AutoDestroy(float damageRatio = 100)
    {
        while (shipSettings.CurDurability > 0)
        {
            shipSettings.CurDurability -= Mathf.CeilToInt(damageRatio * Time.unscaledDeltaTime);
            await Task.Yield();
        }

        DestroyShip();
    }
}