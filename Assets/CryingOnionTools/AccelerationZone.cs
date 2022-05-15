#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AccelerationZone : MonoBehaviour
{
    [SerializeField, Min(0f)] private float acceleration = 20f, speed = 5f, thresholdRigidbodyVelocityDetection = 0.0001f;

    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody body = other.attachedRigidbody;

        if (body) Accelerate(body);
    }

    void OnTriggerStay(Collider other)
    {
        Rigidbody body = other.attachedRigidbody;

        if (body) Accelerate(body);
    }

    void Accelerate(Rigidbody body)
    {
        Vector3 velocity = transform.InverseTransformDirection(body.velocity);

        if (velocity.sqrMagnitude < thresholdRigidbodyVelocityDetection) return;

        if (velocity.y >= speed) return;

        if (acceleration > 0f)
        {
            velocity.y = Mathf.MoveTowards(velocity.y, speed, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.y = speed;
        }

        body.velocity = transform.TransformDirection(velocity);
    }


#if UNITY_EDITOR

    [SerializeField] Color gizmoColor = Color.green;

    private void OnDrawGizmos()
    {
        if (!boxCollider) boxCollider = GetComponent<BoxCollider>();

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);

        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, .25f);
        Gizmos.DrawCube(boxCollider.center, boxCollider.size);

        Handles.matrix = transform.localToWorldMatrix;
        Handles.color = gizmoColor;
        Handles.ArrowHandleCap(0, boxCollider.center, Quaternion.LookRotation(Vector3.up), boxCollider.size.y * .4f, EventType.Repaint);
        Handles.SphereHandleCap(0, boxCollider.center, Quaternion.identity, .25f, EventType.Repaint);
    }
#endif
}
