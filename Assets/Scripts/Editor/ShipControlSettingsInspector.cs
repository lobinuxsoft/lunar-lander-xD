using UnityEditor;
using CryingOnionTools.ScriptableVariables.Editor;

[CustomEditor(typeof(ShipControlSettings))]
public class ShipControlSettingsInspector : ScriptableVariableEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawInspector((ShipControlSettings)target);
    }
}
