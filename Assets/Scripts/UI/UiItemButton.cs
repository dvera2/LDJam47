using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiItemButton : MonoBehaviour
{
    public ItemAsset Item;
    public Image Image;

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

    public void DoSelection()
    {
        GameEvents.TriggerItemButtonClicked(this);
    }
}
