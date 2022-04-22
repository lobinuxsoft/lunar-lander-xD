using System;
using UnityEngine;

namespace CryingOnionTools.ScriptableVariables
{
    [CreateAssetMenu(fileName = "New Int Variable", menuName = "Crying Onion Tools/ Scriptable Variables/ Int Variable")]
    public class IntVariable : BaseScriptableVariable
    {
        [SerializeField] private int value = 0;

        public Action<int> onValueChange;

        public int Value => value;
        
        public void AddValue(int newValue)
        {
            value = Math.Clamp(value + newValue, int.MinValue, int.MaxValue);
            onValueChange?.Invoke(value);
        }

        public void SetValue(int newValue)
        {
            value = Math.Clamp(newValue, int.MinValue, int.MaxValue);
            onValueChange?.Invoke(value);
        }

        public override void SaveData()
        {
            IntVariableStruct temp = new IntVariableStruct { value = value };
            SaveData(temp);
        }

        public override void LoadData()
        {
            value = Math.Clamp(LoadData<IntVariableStruct>().value, int.MinValue, int.MaxValue);
        }

        public override void EraseSaveFile()
        {
            base.EraseSaveFile();

            value = 0;
        }
    }

    struct IntVariableStruct
    {
        public int value;
    }
}
