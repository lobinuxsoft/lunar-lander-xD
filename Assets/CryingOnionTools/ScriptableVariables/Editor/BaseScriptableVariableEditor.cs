using System;
using UnityEditor;
using UnityEngine;

namespace CryingOnionTools.ScriptableVariables.Editor
{
    [CustomEditor(typeof(BaseScriptableVariable))]
    public class BaseScriptableVariableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BaseScriptableVariable scriptableVariable = (BaseScriptableVariable)target;

            DrawInspector(scriptableVariable.SaveData, scriptableVariable.LoadData, scriptableVariable.EraseSaveFile, scriptableVariable.finderPath);
        }

        protected virtual void DrawInspector(Action saveAction, Action loadAction, Action eraseAction, string folderPath)
        {
            GUILayout.Space(10);

            EditorGUILayout.HelpBox($"Salva un archivo en: {folderPath}", MessageType.Info);
            if (GUILayout.Button("Force save data"))
            {
                saveAction();
            }

            GUILayout.Space(10);
            EditorGUILayout.HelpBox($"Carga los datos del archivo: {folderPath}", MessageType.Info);
            if (GUILayout.Button("Force load data"))
            {
                loadAction();
            }

            GUILayout.Space(10);
            EditorGUILayout.HelpBox($"Borra el archivo: {folderPath}", MessageType.Info);
            if (GUILayout.Button("Erase Save file and reset"))
            {
                eraseAction();
            }

            GUILayout.Space(10);
            EditorGUILayout.HelpBox($"Abre el explorador en: {folderPath}", MessageType.Info);
            if (GUILayout.Button("Show file in explorer"))
            {
                EditorUtility.RevealInFinder(folderPath);
            }
        }
    }
}