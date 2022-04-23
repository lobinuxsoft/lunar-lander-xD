using System;
using UnityEngine;

namespace CryingOnionTools.ScriptableVariables
{
    [CreateAssetMenu(fileName = "New String Variable", menuName = "Crying Onion Tools/ Scriptable Variables/ String Variable")]
    public class StringVariable : BaseScriptableVariable
    {
        [SerializeField] private string value;

        public Action<string> onValueChange;

        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                onValueChange?.Invoke(this.value);
            }
        }

        public override void SaveData() => SaveData(new StringStruct { value = value });

        public override void LoadData() => value = LoadData<StringStruct>().value;

        public override void EraseSaveFile()
        {
            base.EraseSaveFile();
            value = string.Empty;
        }
    }

    struct StringStruct
    {
        public string value;
    }
}
