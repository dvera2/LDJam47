using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UiItemButton : MonoBehaviour, IPointerDownHandler
{
    public ItemAsset Item;
    public Image Image;
    public TMPro.TMP_Text CountLabel;
    public int Count = 5;

    private void Start()
    {
        SetItem(Item);
        SetCount(Count);
    }

    public void SetItem(ItemAsset item)
    {
        Item = item;

        if (item && Image)
            Image.sprite = item.Item.PreviewSprite;
    }

    public void SetCount(int itemCount)
    {
        Count = Mathf.Max(0, itemCount);
        GetComponent<Button>().interactable = Count > 0;

        if (CountLabel)
        {
            CountLabel.text = Count.ToString();
            CountLabel.color = Count > 0 ? Color.white : Color.red;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(Count > 0)
            GameEvents.TriggerItemButtonPointer(this, eventData);
    }
}
