using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LevelList))]
public class LevelListEditor : Editor
{
    
    /// <summary>
    /// Draw the Inspector Window
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if(GUILayout.Button("Add to Build Settings"))
        {
            var levelList = target as LevelList;
            var existingBuildScenes = EditorBuildSettings.scenes;
            var newScenes = new List<EditorBuildSettingsScene>();

            AddLevel(newScenes, levelList.MainMenu.Asset);
            foreach (var l in levelList.Levels)
                AddLevel(newScenes, l.Asset);

            AddLevel(newScenes, levelList.Ending.Asset);
            
            EditorBuildSettings.scenes = newScenes.ToArray();
            AssetDatabase.SaveAssets();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddLevel(List<EditorBuildSettingsScene> scenes, SceneAsset asset)
    {
        if (asset)
        {
            scenes.Add(new EditorBuildSettingsScene()
            {
                enabled = true,
                path = AssetDatabase.GetAssetPath(asset),
            });
        }
    }
}