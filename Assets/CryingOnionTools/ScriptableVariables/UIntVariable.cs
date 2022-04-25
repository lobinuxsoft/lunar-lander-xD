using System;
using UnityEngine;

namespace CryingOnionTools.ScriptableVariables
{
    [CreateAssetMenu(fileName = "New UInt Variable", menuName = "Crying Onion Tools/ Scriptable Variables/ UInt Variable")]
    public class UIntVariable : BaseScriptableVariable
    {
        
        [SerializeField] private uint value = 0;

        public event Action<uint> onValueChange;

        public uint Value
        {
            get => value;
            set
            {
                this.value = value;
                onValueChange?.Invoke(this.value);
            }
        }

        public override void SaveData() => SaveData(new UIntStruct { value = value });

        public override void LoadData() => value = LoadData<UIntStruct>().value;

        public override void EraseSaveFile()
        {
            base.EraseSaveFile();

            value = 0;
        }
    }

    struct UIntStruct
    {
        public uint value;
    }
}