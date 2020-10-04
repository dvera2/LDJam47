using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ItemInfo
{
    public PlaceableItem Prefab;
    public PlaceableItem PlaceholderPrefab;
    public Sprite PreviewSprite;
}

[CreateAssetMenu(fileName = "ItemAsset", menuName = "Game/Create Item Asset")]
public class ItemAsset : ScriptableObject
{
    [SerializeField]
    private ItemInfo _itemInfo;
    public ItemInfo Item => _itemInfo;

}
