using UnityEngine;
using CryingOnionTools.ScriptableVariables;

[CreateAssetMenu(fileName = "New String Variable", menuName = "Crying Onion Tools/ Scriptable Variables/ String Variable")]
public class StringVariable : ScriptableVariable<string>
{
    public override void EraseData()
    {
        base.EraseData();
        value = string.Empty;
    }
}