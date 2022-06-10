using UnityEditor;
using UnityEngine;

namespace CryingOnionTools.GravitySystem
{
    [CustomEditor(typeof(GravitySphere))]
    public class GravitySphereEditor : Editor
    {
        private static GUIStyle style;

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void RenderCustomGizmo(GravitySphere gravitySphere, GizmoType gizmoType)
        {
            Vector3 p = gravitySphere.transform.position;

            if (style == null) 
            { 
                style = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).label);
                style.richText = true;
                style.alignment = TextAnchor.MiddleCenter;
            }

            if (gravitySphere.InnerFalloffRadius > 0f && gravitySphere.InnerFalloffRadius < gravitySphere.InnerRadius)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(p, gravitySphere.InnerFalloffRadius);
            }

            Gizmos.color = Color.yellow;

            if (gravitySphere.InnerRadius > 0 && gravitySphere.InnerRadius < gravitySphere.OuterRadius)
            {
                Gizmos.DrawWireSphere(p, gravitySphere.InnerRadius);
            }

            Gizmos.DrawWireSphere(p, gravitySphere.OuterRadius);
            Handles.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, .05f);
            Handles.SphereHandleCap(0, p, Quaternion.identity, gravitySphere.OuterRadius * 2, EventType.Repaint);

            if (gravitySphere.OuterFalloffRadius > gravitySphere.OuterRadius)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(p, gravitySphere.OuterFalloffRadius);
                Handles.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, .05f);
                Handles.SphereHandleCap(0, p, Quaternion.identity, gravitySphere.OuterFalloffRadius * 2, EventType.Repaint);
            }

            style.normal.textColor = Color.cyan;
            Handles.Label(p + Vector3.up * gravitySphere.OuterFalloffRadius * 1.1f, $"<b>G: {gravitySphere.Gravity:0.00 m/s}</b>", style);
        }
    }
}