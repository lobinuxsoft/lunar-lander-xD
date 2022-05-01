using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(ParticleSystem)), RequireComponent(typeof(LineRenderer))]
public class LandingPad : MonoBehaviour
{
    [SerializeField] private ShipControlSettings shipSettings;

    private ShipControl shipToLand;
    private bool isLandingCompleted;
    private ParticleSystem particle;
    private ParticleSystem.MainModule mainModule;
    private LineRenderer lineRenderer;
    private Transform target;
    private BoxCollider box;

    public event Action onLandingCompleted; 

    private void Awake()
    {
        target = Camera.main.transform;
        
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;

        particle = GetComponent<ParticleSystem>();
        mainModule = particle.main;
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.endColor = Color.clear;
        
        DisableLanding();
    }

    private void LateUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        lineRenderer.SetPosition(1, distance * .25f * Vector3.up + transform.position);
        lineRenderer.startWidth = distance * .01f;
        lineRenderer.endWidth = distance * .01f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!shipToLand)
        {
            if(other.CompareTag("Player")) shipToLand = other.GetComponent<ShipControl>();
        }
        else
        {
            if ( !isLandingCompleted && shipToLand.Body.IsSleeping())
            {
                isLandingCompleted = true;
                shipSettings.Refuel();
                shipSettings.Repair();

                shipToLand.enabled = true;
                
                if (shipToLand.TryGetComponent(out ShipDamageControl damageControl)) damageControl.CancelAutoDestroy();

                onLandingCompleted?.Invoke();
                
                DisableLanding();
            }
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         shipToLand = null;
    //         isLandingCompleted = false;
    //         particle.Stop();
    //     }
    // }

    public void EnableLanding(Color signalColor)
    {
        lineRenderer.startColor = signalColor;
        
        box.enabled = true;
        lineRenderer.enabled = true;
        mainModule.startColor = signalColor;
        particle.Play();
    }

    public void DisableLanding()
    {
        box.enabled = false;
        lineRenderer.enabled = false;
        particle.Stop();
    }
}
