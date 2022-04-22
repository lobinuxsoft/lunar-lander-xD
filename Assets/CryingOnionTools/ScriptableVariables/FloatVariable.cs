using System;
using UnityEngine;

namespace CryingOnionTools.ScriptableVariables
{
    [CreateAssetMenu(fileName = "New Float Variable", menuName = "Crying Onion Tools/ Scriptable Variables/ Float Variable")]
    public class FloatVariable : BaseScriptableVariable
    {
        [SerializeField] private float value;

        public Action<float> onValueChange;

        public float Value => value;
        
        public void AddValue(float newValue)
        {
            value = Math.Clamp(value + newValue, float.MinValue, float.MaxValue);
            
            onValueChange?.Invoke(value);
        }

        public void SetValue(float newValue)
        {
            value = Math.Clamp(newValue, float.MinValue, float.MaxValue);
            onValueChange?.Invoke(value);
        }

        public override void SaveData()
        {
            FloatVariableStruct temp = new FloatVariableStruct { value = value };
            SaveData(temp);
        }

        public override void LoadData()
        {
            value = Math.Clamp(LoadData<FloatVariableStruct>().value, float.MinValue, float.MaxValue);
            value = LoadData<FloatVariableStruct>().value;
        }

        struct FloatVariableStruct
        {
            public float value;
        }
    }
}