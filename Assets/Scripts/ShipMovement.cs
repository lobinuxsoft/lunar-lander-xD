#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float maxVelocity = 150;
    [SerializeField, Range(1, 100)] private float thrustersPower = 10;
    [SerializeField, Range(0, 360)] private float rotationSpeed = 30;
    
    private Vector3 direction = Vector3.zero;
    private Rigidbody body;
    private Transform camTransform;
    private float thrustersPotency;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        camTransform = Camera.main!.transform;
    }

    private void Update()
    {
        if (direction.magnitude > 0)
        {
            body.angularVelocity = Vector3.zero;
            transform.Rotate(direction * (rotationSpeed * Time.deltaTime), Space.World);
        }
    }

    private void FixedUpdate()
    {
        if(thrustersPotency > 0) body.AddForce(body.transform.up * (thrustersPower * thrustersPotency), ForceMode.Force);

        body.velocity = Vector3.ClampMagnitude(body.velocity, maxVelocity);
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
            thrustersPotency, EventType.Repaint);
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
        thrustersPotency = context.ReadValue<float>();
    }
}