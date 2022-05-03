#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace CryingOnionTools.GravitySystem
{
    public class GravitySphere : GravitySource
    {
        [SerializeField] float gravity = 9.81f;

        [SerializeField, Min(0f)] float outerRadius = 10f, outerFalloffRadius = 15f;
        
        float outerFalloffFactor;

        void Awake() => OnValidate();

        void OnValidate ()
        {
            outerFalloffRadius = Mathf.Max(outerFalloffRadius, outerRadius);
            outerFalloffFactor = 1f / (outerFalloffRadius - outerRadius);
        }

        public override Vector3 GetGravity (Vector3 position)
        {
            Vector3 vector = transform.position - position;
            float distance = vector.magnitude;
            
            if (distance > outerFalloffRadius) return Vector3.zero;

            float g = gravity / distance;

            if (distance > outerRadius) g *= 1f - (distance - outerRadius) * outerFalloffFactor;
            
            return g * vector;
        }
        
#if UNITY_EDITOR
        void OnDrawGizmos () 
        {
            Vector3 p = transform.position;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(p, outerRadius);
            Handles.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, .15f); 
            Handles.SphereHandleCap(0, p, Quaternion.identity, outerRadius * 2, EventType.Repaint);
            
            if (outerFalloffRadius > outerRadius)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(p, outerFalloffRadius);
                Handles.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, .15f);
                Handles.SphereHandleCap(0, p, Quaternion.identity, outerFalloffRadius * 2, EventType.Repaint);
            }
        }
#endif
    }
}
