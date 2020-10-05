using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class LevelEntry
{
#if UNITY_EDITOR
    public SceneAsset Asset;
#endif
    public string Name;
}

[CreateAssetMenu(fileName = "Level Progression", menuName = "System/Level Progression")]
public class LevelList : ScriptableObject
{
    public LevelEntry MainMenu;

    public List<LevelEntry> Levels;

    public LevelEntry Ending;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (MainMenu.Asset)
            MainMenu.Name = MainMenu.Asset.name;

        foreach(var l in Levels)
        {
            if (l.Asset)
                l.Name = l.Asset.name;
        }

        if (Ending.Asset)
            Ending.Name = Ending.Asset.name;
    }
#endif
}
