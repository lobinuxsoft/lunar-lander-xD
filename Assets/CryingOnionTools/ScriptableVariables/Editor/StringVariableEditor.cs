using UnityEditor;
using CryingOnionTools.ScriptableVariables.Editor;

[CustomEditor(typeof(StringVariable))]
public class StringVariableEditor : ScriptableVariableEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawInspector((StringVariable)target);
    }
}