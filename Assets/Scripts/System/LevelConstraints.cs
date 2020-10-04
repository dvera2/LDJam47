using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemConstraint
{
    public string Name;
    public ItemAsset Item;
    [Min(1)]
    public int Count = 1;
}

public class LevelConstraints : MonoBehaviour
{
    [Header("Add Available Items Here")]
    public ItemConstraint[] Items;

    private void OnValidate()
    {
        foreach (var i in Items)
            if (i.Item)
                i.Name = i.Item.name;
    }
}
