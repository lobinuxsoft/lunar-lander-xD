using UnityEngine;

namespace CryingOnionTools.GravitySystem
{
    public class GravityPlane : GravitySource
    {
        [SerializeField] private float gravity = 9.81f;
        [SerializeField, Min(0)] private float range = 1;
        
        public override Vector3 GetGravity(Vector3 position)
        {
            Vector3 up = transform.up;
            
            float distance = Vector3.Dot(up, position - transform.position);

            if (distance > range) return Vector3.zero;
            
            float g = -gravity;

            if (distance > 0) g *= 1f - distance / range;
            
            return g * up;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 scale = transform.localScale;
            scale.y = range;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);
            
            Vector3 size = new Vector3(1, 0, 1);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero, size);
            Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, .25f);
            Gizmos.DrawCube(Vector3.zero, size);

            if (range > 0)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(Vector3.up, size);
                Gizmos.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, .25f);
                Gizmos.DrawCube(Vector3.up, size);
            }
        }
#endif
    }
}