#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using CryingOnionTools.GravitySystem;

public class GravityFollowAlignment : MonoBehaviour
{
    [SerializeField] private Transform targetToFollow;
    [SerializeField, Min(0f)] float upAlignmentSpeed = 360f;
    
    Quaternion gravityAlignment = Quaternion.identity;

    private void Update()
    {
        UpdateGravityAlignment();
    }

    private void UpdateGravityAlignment()
    {
        transform.position = targetToFollow.position;
        
        Vector3 fromUp = gravityAlignment * Vector3.up;
        Vector3 toUp = CustomGravity.GetUpAxis(transform.position);

        float dot = Mathf.Clamp(Vector3.Dot(fromUp, toUp), -1f, 1f);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float maxAngle = upAlignmentSpeed * Time.deltaTime;

        Quaternion newAlignment = Quaternion.FromToRotation(fromUp, toUp) * gravityAlignment;
        
        if (angle <= maxAngle) 
        {
            gravityAlignment = newAlignment;
        }
        else 
        {
            gravityAlignment = Quaternion.RotateTowards(gravityAlignment, newAlignment, maxAngle);
        }

        transform.rotation = gravityAlignment;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.matrix = transform.localToWorldMatrix;
        Handles.CircleHandleCap(0, Vector3.zero, Quaternion.LookRotation(Vector3.up), 2, EventType.Repaint);
        Handles.ArrowHandleCap(0, Vector3.zero, Quaternion.LookRotation(Vector3.down), 1.5f, EventType.Repaint);
    }

#endif
}
