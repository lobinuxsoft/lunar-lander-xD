using System;
using UnityEngine;

namespace CryingOnionTools.ScriptableVariables
{
    [CreateAssetMenu(fileName = "New Int Variable", menuName = "Crying Onion Tools/ Scriptable Variables/ Int Variable")]
    public class IntVariable : BaseScriptableVariable
    {
        [SerializeField] private int value;

        public event Action<int> onValueChange;

        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                onValueChange?.Invoke(this.value);
            }
        }

        public override void SaveData() => SaveData(new IntStruct{value = value});

        public override void LoadData() => value = LoadData<IntStruct>().value;

        public override void EraseSaveFile()
        {
            base.EraseSaveFile();

            value = 0;
        }
    }

    struct IntStruct
    {
        public int value;
    }
}
