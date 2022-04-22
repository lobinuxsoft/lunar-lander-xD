using System;
using UnityEngine;

namespace CryingOnionTools.ScriptableVariables
{
    [CreateAssetMenu(fileName = "New UInt Variable", menuName = "Crying Onion Tools/ Scriptable Variables/ UInt Variable")]
    public class UIntVariable : BaseScriptableVariable
    {
        
        [SerializeField] private uint value = 0;

        public Action<uint> onValueChange;

        public uint Value => value;
            
        public void AddValue(uint newValue)
        {
            value = Math.Clamp(value + newValue, uint.MinValue, uint.MaxValue);
            onValueChange?.Invoke(value);
        }

        public void SetValue(uint newValue)
        {
            value = Math.Clamp(newValue, uint.MinValue, uint.MaxValue);
            onValueChange?.Invoke(value);
        }

        public override void SaveData()
        {
            UIntVariableStruct temp = new UIntVariableStruct { value = value };
            SaveData(temp);
        }

        public override void LoadData()
        {
            value = Math.Clamp(LoadData<UIntVariableStruct>().value, uint.MinValue, uint.MaxValue);
        }

        public override void EraseSaveFile()
        {
            base.EraseSaveFile();

            value = 0;
        }
    }

    struct UIntVariableStruct
    {
        public uint value;
    }
}