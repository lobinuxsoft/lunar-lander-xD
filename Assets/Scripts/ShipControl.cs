#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipControl : MonoBehaviour
{
    [SerializeField] private ShipControlSettings shipSettings;

    private Vector3 direction = Vector3.zero;
    private Rigidbody body;
    private Transform camTransform;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        camTransform = Camera.main!.transform;
        shipSettings.Refuel();
    }

    private void FixedUpdate()
    {
        if (shipSettings.ThrustersPotency > 0 && shipSettings.ShipFuel > 0)
        {
            body.AddForce(body.transform.up * (shipSettings.ThrustersPower * shipSettings.ThrustersPotency), ForceMode.Force);
            shipSettings.ShipFuel -= shipSettings.RatioFuelConsumition * Time.fixedDeltaTime;
        }

        if (direction.magnitude > 0)
        {
            Vector3 angularVelocity = direction * (shipSettings.RotationSpeed * Time.fixedDeltaTime);
            body.angularVelocity = angularVelocity;
        }

        if (body.velocity.magnitude > 0.005f)
        {
            body.velocity = Vector3.ClampMagnitude(body.velocity, shipSettings.MaxVelocity);
        }
    }

#if UNITY_EDITOR

    [SerializeField] private Color gizmoColor;
    
    private void OnDrawGizmos()
    {
        Handles.color = gizmoColor;
        Handles.DrawWireDisc(transform.position, Vector3.up, 1);
        Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(direction, Vector3.up),
            Mathf.Clamp01(direction.magnitude), EventType.Repaint);
        
        Handles.color = Color.cyan;
        Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(transform.up, Vector3.up),
            shipSettings.ThrustersPotency, EventType.Repaint);
    }
#endif

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        
        Vector3 forward = camTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = camTransform.right;
        right.y = 0;
        right.Normalize();

        direction = -input.x * forward + input.y * right;
    }

    public void OnThrusters(InputAction.CallbackContext context)
    {
        shipSettings.ThrustersPotency = context.ReadValue<float>();
    }
}