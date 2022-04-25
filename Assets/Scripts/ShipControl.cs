#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipControl : MonoBehaviour
{
    [Header("Ship Settings, can be saved.")]
    [SerializeField] private ShipControlSettings shipSettings;
    
    [Header("Internal use.")]
    [SerializeField] private float smoothInputSpeed = .2f;

    [Header("Thruster vfx")]
    [SerializeField] private ThrusterParticleControl thrusterParticleControl;

    private Vector3 targetDir;
    private Vector3 direction;
    private Vector3 smoothDirVelocity;

    private float targetThruster;
    private float thruster;
    private float smoothTargetThruster;
    
    private Rigidbody body;
    private Transform camTransform;
    private float baseAngularDrag;

    private Vector2 dirInput;

    public Rigidbody Body => body;
    
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        baseAngularDrag = body.angularDrag;
        camTransform = Camera.main!.transform;
        shipSettings.Refuel();
    }

    private void Update()
    {
        CalculateMovement();
        CalculateThruster();
    }

    private void FixedUpdate()
    {
        if (shipSettings.ThrustersPotency > 0.001f && shipSettings.ShipFuel > 0)
        {
            body.AddForce(body.transform.up * (shipSettings.ThrustersPower * shipSettings.ThrustersPotency), ForceMode.Force);
            shipSettings.ShipFuel -= shipSettings.RatioFuelConsumition * shipSettings.ThrustersPotency * Time.fixedDeltaTime;
        }

        if (direction.magnitude > 0.01f)
        {
            body.angularDrag = baseAngularDrag;
            Vector3 angularVelocity = direction * (shipSettings.RotationSpeed * Time.fixedDeltaTime);
            body.angularVelocity = angularVelocity;
        }
        else
        {
            body.angularDrag = 5;
        }

        if (body.velocity.magnitude > 0.01f)
        {
            body.velocity = Vector3.ClampMagnitude(body.velocity, shipSettings.MaxVelocity);
            shipSettings.CurVelocity = body.velocity.magnitude;
        }
        else
        {
            shipSettings.CurVelocity = 0;
        }
    }

    public void OnMove(InputAction.CallbackContext context) => dirInput = context.ReadValue<Vector2>();
    public void OnThrusters(InputAction.CallbackContext context) => targetThruster = context.ReadValue<float>();
    
    
    private void CalculateMovement()
    {
        Vector3 forward = camTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = camTransform.right;
        right.y = 0;
        right.Normalize();
        
        targetDir = -dirInput.x * forward + dirInput.y * right;
        
        direction = Vector3.SmoothDamp(direction, targetDir, ref smoothDirVelocity, smoothInputSpeed);
    }
    
    private void CalculateThruster()
    {
        thruster = Mathf.SmoothDamp(thruster, targetThruster, ref smoothTargetThruster, smoothInputSpeed);
        shipSettings.ThrustersPotency = thruster > 0.001f ? thruster : 0;
        
        thrusterParticleControl.ThrusterPower(shipSettings.ShipFuel > 0 ? shipSettings.ThrustersPotency : 0);
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
}