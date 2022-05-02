using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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

    private CancellationTokenSource cancelToken = new CancellationTokenSource();

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
    /// Damage the ship until it is destroyed (this can be canceled with the method <see cref="CancelAutoDestroy"/>>)
    /// </summary>
    /// <param name="damageRatio"></param>
    public void AutoDestroy(float damageRatio = 100)
    {
        cancelToken.Dispose();
        cancelToken = new CancellationTokenSource();
        
        AutoDestroyTask(damageRatio, cancelToken.Token);
    }

    private async void AutoDestroyTask(float damageRatio, CancellationToken ct)
    {
        while ( !ct.IsCancellationRequested)
        {
            shipSettings.CurDurability -= Mathf.CeilToInt(damageRatio * Time.unscaledDeltaTime);

            if (shipSettings.CurDurability <= 0)
            {
                DestroyShip();
                cancelToken.Cancel();
            }
                
            await Task.Yield();
        }
    }

    /// <summary>
    /// Cancel the self destruct of the ship.
    /// </summary>
    public void CancelAutoDestroy()
    {
        cancelToken.Cancel();
        
    }
}