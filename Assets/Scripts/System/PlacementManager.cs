using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PrePlacementInstance
{
    public ItemAsset Item;
    public PlaceableItem Placeholder;
    public Vector3 Position;
    public float Angle;
}

public class MouseDragData
{
    public bool MouseDragging { get; private set; }
    public bool MouseUp { get; private set; }
    public Vector2 Delta { get { return MousePos - PrevPos; } }
    
    public Vector2 PrevPos;
    public Vector2 MousePos;

    public void Reset()
    {
        MouseDragging = false;
        MouseUp = false;
        PrevPos = MousePos = Vector2.zero;
    }

    public void Update()
    {
        bool mouseDownThisFrame = false;
        if (Input.GetMouseButtonDown(0))
        {
            MouseDragging = true;
            MousePos = Input.mousePosition;
            PrevPos = MousePos;
            mouseDownThisFrame = true;
        }


        if (Input.GetMouseButtonUp(0))
        {
            MouseDragging = false;
            MouseUp = true;
        }
        else
        {
            MouseUp = false;
        }

        if (!mouseDownThisFrame && MouseDragging)
        {
            PrevPos = MousePos;
            MousePos = Input.mousePosition;
        }
    }
}

// --------------------------------------------------------------------
public class PlacementManager : MonoBehaviour
{
    public SpriteRenderer PreviewSprite;
    public float GridSize = 0.32f;
    
    private ItemAsset _itemToPlace;
    private List<PrePlacementInstance> _preplacements;
    private Dictionary<int, PrePlacementInstance> _preplacementTable;
    private Camera _camera;
    private Action _currentState;
    private PrePlacementInstance _selection;
    private MouseDragData _mouseDrag;

    // ----------------------------------------------------------------------------
    private void Awake()
    {
        _mouseDrag = new MouseDragData();
        _preplacementTable = new Dictionary<int, PrePlacementInstance>();
        _preplacements = new List<PrePlacementInstance>();

        GameEvents.ItemButtonClicked += OnItemButtonClicked;
    }

    // ----------------------------------------------------------------------------
    private void Start()
    {
        if (PreviewSprite)
            PreviewSprite.enabled = false;

        _camera = Camera.main;
        _currentState = NothingSelectedState;
    }

    // ----------------------------------------------------------------------------
    private void OnDestroy()
    {
        _preplacements?.Clear();

        GameEvents.ItemButtonClicked -= OnItemButtonClicked;
    }

    // ----------------------------------------------------------------------------
    private void Update()
    {
        _currentState?.Invoke();
    }

    // ----------------------------------------------------------------------------
    void ObjectPlacementState()
    {
        if (!_itemToPlace)
        {
            _currentState = NothingSelectedState;
            return;
        }

        var wPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
        PreviewSprite.transform.SnapToGrid(wPoint, GridSize);

        if (CanBePlaced(PreviewSprite))
        {
            PreviewSprite.color = Color.white;

            // Place item when clicked
            if (Input.GetMouseButtonDown(0))
            {
                PlaceItemAt(_itemToPlace, wPoint);

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

    // ----------------------------------------------------------------------------
    void NothingSelectedState()
    {
        // On click, check placed items for selection.
        if(Input.GetMouseButtonDown(0))
        {
            // Found valid placement? switch to selection state
            var entry = FindPlacementAtPoint(_camera.ScreenToWorldPoint(Input.mousePosition));
            if (entry != null)
            {
                GoToSelectedItemState(entry);
            }
        }
    }
    private void GoToSelectedItemState( PrePlacementInstance selection )
    {
        _selection = selection;
        _mouseDrag.Reset();
        _currentState = SelectedItemState;
    }

    // ----------------------------------------------------------------------------
    private void SelectedItemState()
    {
        if(_selection == null)
        {
            _currentState = NothingSelectedState;
            return;
        }

        // Allow user to drag position, delete, or rotate
        _mouseDrag?.Update();
        if( _mouseDrag.MouseDragging )
        {
            var wPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
            PreviewSprite.enabled = true;
            PreviewSprite.sprite = _selection.Item.Item.PreviewSprite;
            PreviewSprite.transform.SnapToGrid(wPoint, GridSize);

        }

        if( PreviewSprite.enabled )
        {
            bool canBePlaced = CanBePlaced(PreviewSprite);
            PreviewSprite.color = canBePlaced ? Color.white : Color.red;

            // If we release, reposition the element
            if (_mouseDrag.MouseUp) 
            {
                if(!canBePlaced)
                {
                    PreviewSprite.enabled = false;
                }
                else
                {
                    _selection.Position = PreviewSprite.transform.position.SnapToGrid(GridSize);
                }
            }
        }

    }

    // ----------------------------------------------------------------------------
    private void PlaceItemAt(ItemAsset item, Vector3 worldPosition)
    {
        var prefab = item.Item.Prefab;

        var placeholder = GameObject.Instantiate<PlaceableItem>(item.Item.PlaceholderPrefab);
        placeholder.transform.SnapToGrid(worldPosition, GridSize);

        var ppi = new PrePlacementInstance()
        {
            Item = item,
            Position = worldPosition.SnapToGrid(GridSize),
            Angle = prefab.AngleInDeg,
            Placeholder = placeholder,
        };
        _preplacements.Add(ppi);
        _preplacementTable.Add(ppi.Placeholder.GetInstanceID(), ppi);
    }

    // ----------------------------------------------------------------------------
    private PrePlacementInstance FindPlacementAtPoint(Vector3 worldPosition)
    {
        var testPoint = new Vector2(worldPosition.x, worldPosition.y);
        var collider2D = Physics2D.OverlapPoint(testPoint, 1 << LayerMask.NameToLayer("Placement"));

        PlaceableItem item = null;
        if (collider2D && collider2D.attachedRigidbody)
            item = collider2D.attachedRigidbody.transform.FindItemUpHierarchy();

        return GetFromPlaceholder(item);
    }

    // ----------------------------------------------------------------------------
    private PrePlacementInstance GetFromPlaceholder(PlaceableItem placeholder)
    {
        if (placeholder == null)
            return null;

        PrePlacementInstance entry = null;
        if (_preplacementTable.TryGetValue(placeholder.GetInstanceID(), out entry))
            return entry;

        return null;
    }

    // ----------------------------------------------------------------------------
    private bool CanBePlaced( SpriteRenderer s )
    {
        return !Physics2D.OverlapBox(s.transform.position, 0.75f* s.size, 0);
    }


    // ----------------------------------------------------------------------------
    private void OnItemButtonClicked(UiItemButton obj)
    {
        _itemToPlace = obj.Item;

        if (PreviewSprite)
        {
            PreviewSprite.enabled = true;
            PreviewSprite.sprite = obj.Item.Item.PreviewSprite;
        }

        if (_itemToPlace)
            _currentState = ObjectPlacementState;
    }

}
