using UnityEngine;

namespace CryingOnionTools.GravitySystem
{
    public class GravityBox : GravitySource
    {
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private Vector3 boundaryDistance = Vector3.one;
        [SerializeField] private float innerDistance = 0f, innerFalloffDistance = 0f;

        private float innerFalloffFactor;

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            boundaryDistance = Vector3.Max(boundaryDistance, Vector3.zero);
            float maxInner = Mathf.Min(Mathf.Min(boundaryDistance.x, boundaryDistance.y), boundaryDistance.z);
            innerDistance = Mathf.Min(innerDistance, maxInner);
            innerFalloffDistance = Mathf.Max(Mathf.Min(innerFalloffDistance, maxInner), innerDistance);

            innerFalloffFactor = 1f / (innerFalloffDistance - innerDistance);
        }

        float GetGravityComponent(float coordinate, float distance)
        {
            if (distance > innerFalloffDistance)
            {
                return 0f;
            }
            float g = gravity;
            if (distance > innerDistance)
            {
                g *= 1f - (distance - innerDistance) * innerFalloffFactor;
            }
            return coordinate > 0f ? -g : g;
        }

        public override Vector3 GetGravity(Vector3 position)
        {
            position = transform.InverseTransformDirection(position - transform.position);
            Vector3 vector = Vector3.zero;
            Vector3 distances;

            distances.x = boundaryDistance.x - Mathf.Abs(position.x);
            distances.y = boundaryDistance.y - Mathf.Abs(position.y);
            distances.z = boundaryDistance.z - Mathf.Abs(position.z);

            if (distances.x < distances.y)
            {
                if (distances.x < distances.z)
                {
                    vector.x = GetGravityComponent(position.x, distances.x);
                }
                else
                {
                    vector.z = GetGravityComponent(position.z, distances.z);
                }
            }
            else if (distances.y < distances.z)
            {
                vector.y = GetGravityComponent(position.y, distances.y);
            }
            else
            {
                vector.z = GetGravityComponent(position.z, distances.z);
            }

            return transform.TransformDirection(vector);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            Vector3 size;

            if (innerFalloffDistance > innerDistance)
            {
                size.x = 2f * (boundaryDistance.x - innerFalloffDistance);
                size.y = 2f * (boundaryDistance.y - innerFalloffDistance);
                size.z = 2f * (boundaryDistance.z - innerFalloffDistance);

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(Vector3.zero, size);

                Gizmos.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, .25f);
                Gizmos.DrawCube(Vector3.zero, size);
            }

            if (innerDistance > 0f)
            {
                size.x = 2f * (boundaryDistance.x - innerDistance);
                size.y = 2f * (boundaryDistance.y - innerDistance);
                size.z = 2f * (boundaryDistance.z - innerDistance);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(Vector3.zero, size);
                Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, .25f);
                Gizmos.DrawCube(Vector3.zero, size);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, 2f * boundaryDistance);

            Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, .25f);
            Gizmos.DrawCube(Vector3.zero, 2f * boundaryDistance);
        }
#endif
    }
}