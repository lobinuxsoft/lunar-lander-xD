#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace CryingOnionTools.GravitySystem
{
    public class GravitySphere : GravitySource
    {
        [SerializeField] private float gravity = 9.81f;

        [SerializeField, Min(0f)] private float innerFalloffRadius = 15f, innerRadius = 18f;
        [SerializeField, Min(0f)] private float outerRadius = 22f, outerFalloffRadius = 22f;

        public float Gravity => gravity;

        public float InnerFalloffRadius => innerFalloffRadius;

        public float InnerRadius => innerRadius;

        public float OuterRadius => outerRadius;

        public float OuterFalloffRadius => outerFalloffRadius;

        private float innerFalloffFactor, outerFalloffFactor;

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
    }
}
