#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace CryingOnionTools.GravitySystem
{
    public class GravitySphere : GravitySource
    {
        [SerializeField] float gravity = 9.81f;

        [SerializeField, Min(0f)] private float innerFalloffRadius = 15f, innerRadius = 18f;
        [SerializeField, Min(0f)] float outerRadius = 22f, outerFalloffRadius = 22f;
        
        float innerFalloffFactor, outerFalloffFactor;

        void Awake() => OnValidate();

        void OnValidate ()
        {
            innerFalloffRadius = Mathf.Max(innerFalloffRadius, 0f);
            innerRadius = Mathf.Max(innerRadius, innerFalloffRadius);
            outerRadius = Mathf.Max(outerRadius, innerRadius);
            outerFalloffRadius = Mathf.Max(outerFalloffRadius, outerRadius);
            
            innerFalloffFactor = 1f / (innerRadius - innerFalloffRadius);
            outerFalloffFactor = 1f / (outerFalloffRadius - outerRadius);
        }

        public override Vector3 GetGravity (Vector3 position)
        {
            Vector3 vector = transform.position - position;
            float distance = vector.magnitude;
            
            if (distance > outerFalloffRadius || distance < innerFalloffRadius) return Vector3.zero;

            float g = gravity / distance;

            if (distance > outerRadius)
            {
                g *= 1f - (distance - outerRadius) * outerFalloffFactor;
            }
            else if (distance < innerRadius)
            {
                g *= 1f - (innerRadius - distance) * innerFalloffFactor;
            }
            
            return g * vector;
        }
        
#if UNITY_EDITOR
        void OnDrawGizmos () 
        {
            Vector3 p = transform.position;
            
            if (innerFalloffRadius > 0f && innerFalloffRadius < innerRadius) 
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(p, innerFalloffRadius);
            }
            
            Gizmos.color = Color.yellow;

            if (innerRadius > 0 && innerRadius < outerRadius)
            {
                Gizmos.DrawWireSphere(p, innerRadius);
            }
            
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
