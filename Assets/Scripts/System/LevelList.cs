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
    public string SceneName;
}

[CreateAssetMenu(fileName = "Level Progression", menuName = "System/Level Progression")]
public class LevelList : ScriptableObject
{
    public List<LevelEntry> Levels = new List<LevelEntry>();

    private void OnValidate()
    {
        foreach(var l in Levels)
        {
            if (l.Asset)
                l.SceneName = l.Asset.name;
        }
    }
}
