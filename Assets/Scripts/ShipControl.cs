#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipControl : MonoBehaviour
{
    [Header("Ship Settings, can be saved.")]
    [SerializeField] private ShipControlSettings shipSettings;
    
    [Header("Internal use.")]
    [SerializeField] private float smoothInputSpeed = .2f;

    [Header("Thruster vfx")]
    [SerializeField] private ParticleControl thrusterParticle;
    
    [Header("Gravity Break vfx")]
    [SerializeField] private ParticleControl gravityBreakParticle;

    public UnityEvent onOutOfFuel;
    
    private Vector3 targetDir;
    private Vector3 direction;
    private Vector3 smoothDirVelocity;

    private float targetThruster;
    private float thruster;
    private float smoothTargetThruster;
    
    private float targetGravityBreak;
    private float gravityBreak;
    private float smoothTargetGravityBreak;
    
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
        camTransform = Camera.main.transform;
        shipSettings.Refuel();
    }

    private void Update()
    {
        CalculateMovement();
        CalculateThruster();
        CalculateGravityBreak();
    }

    private void FixedUpdate()
    {
        if (shipSettings.ShipFuel <= 0)
        {
            onOutOfFuel?.Invoke();
            this.enabled = false;
        }
        
        if (shipSettings.ThrustersPotency > 0.001f && shipSettings.ShipFuel > 0)
        {
            body.AddForce(body.transform.up * (shipSettings.ThrustersPower * shipSettings.ThrustersPotency), ForceMode.Force);
            shipSettings.ShipFuel -= shipSettings.RatioFuelConsumition * shipSettings.ThrustersPotency * Time.fixedDeltaTime;
        }

        if (shipSettings.GravityBreak > 0.001f)
        {
            body.AddForce(-body.velocity * shipSettings.GravityBreak, ForceMode.Force);
            gravityBreakParticle.transform.rotation = Quaternion.LookRotation(body.velocity.normalized, transform.up);
            shipSettings.ShipFuel -= shipSettings.RatioFuelConsumition * shipSettings.GravityBreak * Time.fixedDeltaTime;
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

    private void OnDisable() => thrusterParticle.ParticlePower(0);

    public void OnMove(InputAction.CallbackContext context) => dirInput = context.ReadValue<Vector2>();
    public void OnThrusters(InputAction.CallbackContext context) => targetThruster = context.ReadValue<float>();
    public void OnGravityBreak(InputAction.CallbackContext context) => targetGravityBreak = context.ReadValue<float>();
    
    
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
        
        thrusterParticle.ParticlePower(shipSettings.ShipFuel > 0 ? shipSettings.ThrustersPotency : 0);
    }

    private void CalculateGravityBreak()
    {
        gravityBreak = Mathf.SmoothDamp(gravityBreak, targetGravityBreak, ref smoothTargetGravityBreak, smoothInputSpeed);
        shipSettings.GravityBreak = gravityBreak > 0.001f ? gravityBreak : 0;
        
        gravityBreakParticle.ParticlePower(shipSettings.ShipFuel > 0 ? shipSettings.GravityBreak * body.velocity.normalized.magnitude : 0);
    }
    
#if UNITY_EDITOR

    [SerializeField] private Color dirColor;
    [SerializeField] private Color thrusterColor;
    [SerializeField] private Color breakColor;
    
    private void OnDrawGizmos()
    {
        
        Handles.color = breakColor;
        if (body && body.velocity.magnitude > 0)
        {
            float arrowSize = Mathf.Clamp01(body.velocity.magnitude);
            Quaternion arrowDir = Quaternion.LookRotation(body.velocity.normalized, Vector3.forward);
            Handles.ArrowHandleCap(0, transform.position, arrowDir, arrowSize, EventType.Repaint);
        }

        if (direction.magnitude > 0)
        {
            float arrowSize = Mathf.Clamp01(direction.magnitude);
            Quaternion arrowDir = Quaternion.LookRotation(direction, Vector3.up);
            Handles.ArrowHandleCap(0, transform.position, arrowDir, arrowSize, EventType.Repaint);
        }

        Handles.matrix = transform.localToWorldMatrix;
        Handles.color = dirColor;
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, 1);

        Handles.color = thrusterColor;
        Handles.ArrowHandleCap(0, Vector3.zero, Quaternion.LookRotation(Vector3.up),
            shipSettings.ThrustersPotency, EventType.Repaint);
    }
#endif
}