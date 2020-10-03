using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public SpriteRenderer PreviewSprite;
    
    private ItemAsset _itemToPlace;

    private void Awake()
    {
        GameEvents.ItemButtonClicked += OnItemButtonClicked;
    }

    private void Start()
    {
        if (PreviewSprite)
            PreviewSprite.enabled = false;
    }

    private void OnDestroy()
    {
        GameEvents.ItemButtonClicked -= OnItemButtonClicked;
    }

    private void Update()
    {
        if(Input.mousePresent && _itemToPlace)
        {
            var wPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PreviewSprite.transform.SnapObjectToGrid(wPoint, _itemToPlace.Item.Prefab);

            if( CanBePlaced() )
            {
                PreviewSprite.color = Color.white;

                // Place item when clicked
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceItem(_itemToPlace.Item.Prefab, wPoint);

                    PreviewSprite.enabled = false;
                    _itemToPlace = null;
                }
            }
            else
            {
                // Signal unusable
                PreviewSprite.color = Color.red;
            }
        }
    }

    private bool CanBePlaced()
    {
        return !Physics2D.OverlapBox(PreviewSprite.transform.position, 0.75f* PreviewSprite.size, 0);
    }

    private void PlaceItem(PlaceableItem prefab, Vector3 position)
    {
        var item = GameObject.Instantiate<PlaceableItem>(prefab, position, Quaternion.identity);
        item.SnapToGrid(position, Quaternion.identity);
    }

    private void OnItemButtonClicked(UiItemButton obj)
    {
        _itemToPlace = obj.Item;
        PreviewSprite.enabled = true;
        PreviewSprite.sprite = obj.Item.Item.PreviewSprite;
    }

}
