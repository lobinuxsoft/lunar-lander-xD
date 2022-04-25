using System;
using UnityEngine;

namespace CryingOnionTools.ScriptableVariables
{
    [CreateAssetMenu(fileName = "New Float Variable", menuName = "Crying Onion Tools/ Scriptable Variables/ Float Variable")]
    public class FloatVariable : BaseScriptableVariable
    {
        [SerializeField] private float value;

        public event Action<float> onValueChange;

        public float Value
        {
            get => value;
            set
            {
                this.value = value;
                onValueChange?.Invoke(this.value);
            }
        }

        public override void SaveData() => SaveData(new FloatStruct{ value = value});

        public override void LoadData() => value = LoadData<FloatStruct>().value;

        public override void EraseSaveFile()
        {
            base.EraseSaveFile();
            value = 0;
        }
    }

    struct FloatStruct
    {
        public float value;
    }
}