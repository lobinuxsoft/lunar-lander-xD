using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace CryingOnionTools.ScriptableVariables
{
    public abstract class BaseScriptableVariable : ScriptableObject
    {
        [SerializeField] private bool redeableFile;
        [SerializeField] protected string fileExtention = ".save";

        public string folderPath => $"{Application.persistentDataPath}/";
        public string completePath => $"{folderPath}{name.GetHashCode()}{fileExtention}";
        public string finderPath => File.Exists(completePath) ? completePath : folderPath;

        /// <summary>
        /// Salva datos en un archivo
        /// </summary>
        public virtual void SaveData() { }

        /// <summary>
        /// Cargar datos de un archivo
        /// </summary>
        public virtual void LoadData() { }

        /// <summary>
        /// Funcion generica para guardar datos.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        protected void SaveData<T>(T value)
        {
            string jsonData = redeableFile ? JsonUtility.ToJson(value) : Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(value)));
            File.WriteAllText(completePath, jsonData);

    #if UNITY_EDITOR
            Debug.LogWarning($"<color=green>{completePath}</color> file saved.");
    #endif
        }

        /// <summary>
        /// Funcion generica para cargar datos.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T LoadData<T>() where T : new()
        {
            if (File.Exists(completePath))
            {
    #if UNITY_EDITOR
                Debug.LogWarning($"<color=green>{completePath}</color> file loaded.");
    #endif
                
                return JsonUtility.FromJson<T>(redeableFile ? File.ReadAllText(completePath) : Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(completePath))));
            }
            else
            {
    #if true
                Debug.LogWarning($"File not found: <color=red>{completePath}</color>");
    #endif
                
                return new T();
            }
        }

        /// <summary>
        /// Borra el archivo de salvado si existiera.
        /// </summary>
        public virtual void EraseSaveFile()
        {
            if (File.Exists(completePath))
            {
                File.Delete(completePath);

    #if UNITY_EDITOR
                Debug.Log($"<color=yellow>{completePath}</color> deleted");
    #endif
            }
            
    #if UNITY_EDITOR
            else
            {
                Debug.LogWarning($"<color=red>{completePath}</color> not found");
            }
    #endif
            
        }
    }
}