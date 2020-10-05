using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InterfaceActions))]
public class InterfaceActionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            var t = target as InterfaceActions;
            if (GUILayout.Button("MainMenu"))
                t.MainMenu();

            if (GUILayout.Button("Next Level"))
                t.NextLevel();

            if (GUILayout.Button("Restart"))
                t.RestartLevel();

            if (GUILayout.Button("Start New Game"))
                t.StartNewGame();
        }
    }
}
