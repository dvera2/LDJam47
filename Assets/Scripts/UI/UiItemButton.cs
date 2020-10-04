using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiItemButton : MonoBehaviour
{
    public ItemAsset Item;
    public Image Image;
    public int Count = 5;

    private void Start()
    {
        SetItem(Item);
    }

    public void SetItem(ItemAsset item)
    {
        Item = item;

        if (item && Image)
            Image.sprite = item.Item.PreviewSprite;
    }

    public void SetCount(int itemCount)
    {
        itemCount = Mathf.Max(0, itemCount);
    }

    public void DoSelection()
    {
        GameEvents.TriggerItemButtonClicked(this);
    }
}
