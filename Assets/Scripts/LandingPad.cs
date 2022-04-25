using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider)), RequireComponent(typeof(ParticleSystem)), RequireComponent(typeof(LineRenderer))]
public class LandingPad : MonoBehaviour
{
    [SerializeField] private ShipControlSettings shipSettings;
    
    private ShipControl shipToLand;
    private bool isLandingCompleted = false;
    private ParticleSystem particle;
    private LineRenderer lineRenderer;
    private Transform target;
    
    private void Awake()
    {
        if (TryGetComponent(out BoxCollider box))
        {
            box.isTrigger = true;
        }

        target = Camera.main.transform;
        particle = GetComponent<ParticleSystem>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.endColor = Color.clear;
        particle.Stop();
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
                particle.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shipToLand = null;
            isLandingCompleted = false;
            particle.Stop();
        }
    }
}
