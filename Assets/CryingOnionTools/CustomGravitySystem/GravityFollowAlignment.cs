using CryingOnionTools.GravitySystem;
using UnityEngine;

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
            //gravityAlignment = Quaternion.SlerpUnclamped(gravityAlignment, newAlignment, maxAngle / angle);
            gravityAlignment = Quaternion.RotateTowards(gravityAlignment, newAlignment, maxAngle);
        }

        transform.rotation = gravityAlignment;
    }
}
