using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PrePlacementInstance
{
    public ItemAsset Item;
    public PlaceableItem Placeholder;
    public Vector3 Position;
    public PlaceableItem LinkedItem;
    public LineRenderer Line;
    public float Angle;
}

public class PreviewUpdateArgs : System.EventArgs 
{
    public Vector3 WorldPosition;
    public float Angle;
    public Sprite Sprite;
    public bool ValidSpot;
    public bool Enabled;
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
    public SpriteRenderer DebugSprite;
    public SpriteRenderer SelectionSprite;
    public LineRenderer LinePrefab;

    public Color SpriteColorPreview = Color.white;
    public Color SpriteColorBad = Color.red;

    public int GridSizeX = 20;
    public int GridSizeY = 20;
    public float GridSize = 0.32f;
    
    private ItemAsset _itemToPlace;
    private List<PrePlacementInstance> _preplacements;
    private List<PreplacementObject> _startingObjects;
    private Dictionary<int, PrePlacementInstance> _preplacementTable;
    private Camera _camera;
    private Action _currentState;
    private PrePlacementInstance _selection;
    private MouseDragData _mouseDrag;
    private Collider2D[] _resultBuffer;
    private PreplacementObject _lastHoveredObject;
    private UiItemButton _currentButton;

    public List<PrePlacementInstance> PreplacementItems => _preplacements;

    private Transform _preplacementContainer;
    private PreviewUpdateArgs _cachedArgs;

    // ----------------------------------------------------------------------------
    private void Awake()
    {
        _mouseDrag = new MouseDragData();
        _preplacementTable = new Dictionary<int, PrePlacementInstance>();
        _preplacements = new List<PrePlacementInstance>();
        _startingObjects = new List<PreplacementObject>();
        _resultBuffer = new Collider2D[20];
        _cachedArgs = new PreviewUpdateArgs();

        // Find existing level objects in the scene.
        var startingObjects = GameObject.FindObjectsOfType<PreplacementObject>();
        _startingObjects.AddRange(startingObjects);


        GameObject container = new GameObject("Container");
        container.transform.localPosition = Vector3.zero;
        container.transform.localRotation = Quaternion.identity;
        _preplacementContainer = container.transform;

        GameEvents.ItemButtonClicked += OnItemButtonClicked;
    }

    // ----------------------------------------------------------------------------
    private void Start()
    {
        if (PreviewSprite)
            PreviewSprite.enabled = false;

        _camera = Camera.main;

        GoToNothingSelectedState();
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
        SelectionSprite.enabled = false;
        SetDebugColor(Color.yellow);

        if (!_itemToPlace)
        {
            GoToNothingSelectedState();
            return;
        }

        var wPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
        wPoint = wPoint.SnapToGrid(GridSize);
        PreviewSprite.transform.position = wPoint;
        
        bool canBePlaced = CanBePlaced(PreviewSprite, false);

        _cachedArgs.WorldPosition = wPoint;
        _cachedArgs.Sprite = PreviewSprite.sprite;
        _cachedArgs.ValidSpot = canBePlaced;
        _cachedArgs.Enabled = true;
        _cachedArgs.Angle = _itemToPlace.Item.Angle;
        GameEvents.TriggerPreviewUpdated(_cachedArgs);

        if (canBePlaced)
        {
            PreviewSprite.color = SpriteColorPreview;

            // Place item when clicked
            if (Input.GetMouseButtonUp(0))
            {
                var ppi = PlaceItemAt(_itemToPlace, wPoint);

                PreviewSprite.enabled = false;
                _itemToPlace = null;

                GoToSelectedItemState(ppi);
            }
        }
        else
        {
            // Signal unusable
            PreviewSprite.color = SpriteColorBad;

            if(Input.GetMouseButtonUp(0))
            {
                _itemToPlace = null;
                PreviewSprite.enabled = false;
                GoToNothingSelectedState();
                return;
            }
        }
    }

    void GoToNothingSelectedState()
    {
        _selection = null;
        _itemToPlace = null;
        _currentButton = null;
        _currentState = NothingSelectedState;

        _cachedArgs.Enabled = false;
        GameEvents.TriggerPreviewUpdated(_cachedArgs);
    }

    // ----------------------------------------------------------------------------
    void NothingSelectedState()
    {
        SelectionSprite.enabled = false;
        SetDebugColor(Color.black);

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
        else
        {
            // Found valid placement? switch to selection state
            var hoveredItem = FindPreplacementAtPoint(_camera.ScreenToWorldPoint(Input.mousePosition));
            SetHoveredItem(hoveredItem);
        }
    }

    private void SetHoveredItem(PreplacementObject hoveredItem)
    {
        if (hoveredItem == _lastHoveredObject)
            return;

        if (_lastHoveredObject != null)
            _lastHoveredObject.OnHoverEnded();

        _lastHoveredObject = hoveredItem;

        if (_lastHoveredObject != null)
        {
            GameFlow.GF.Cues.UiHover.PlayUiSource();
            _lastHoveredObject.OnHoverStarted();
        }
    }

    private void GoToSelectedItemState(PrePlacementInstance selection)
    {
        SetHoveredItem(null);

        _selection = selection;

        if (_selection == null)
        {
            GoToNothingSelectedState();
            return;
        }


        _cachedArgs.Enabled = false;
        GameEvents.TriggerPreviewUpdated(_cachedArgs);

        GameFlow.GF.Cues.UiSelectItem.PlayUiSource();
        _mouseDrag.Reset();
        _currentState = SelectedItemState;

        if (SelectionSprite)
            SelectionSprite.transform.SnapToGrid(selection.Position, GridSize);
    }

    // ----------------------------------------------------------------------------
    private void SelectedItemState()
    {
        SetDebugColor(Color.red);
        SelectionSprite.enabled = true;

        if (_selection == null)
        {
            GoToNothingSelectedState();
            return;
        }

        // Allow user to drag position, delete, or rotate
        _mouseDrag?.Update();
        if( Input.GetMouseButtonDown(0))
        {
            // Found valid placement? switch to selection state
            var entry = FindPlacementAtPoint(_camera.ScreenToWorldPoint(Input.mousePosition));
            if (entry == null)
            {
                _selection = null;
                return;
            }
            else if( _selection != entry )
            {
                GoToSelectedItemState(entry);
            }
        }

        if (_mouseDrag.MouseDragging)
        {
            var wPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
            PreviewSprite.enabled = true;
            PreviewSprite.transform.rotation = Quaternion.Euler(0, 0, _selection.Angle);
            PreviewSprite.sprite = _selection.Item.Item.PreviewSprite;

            var newPos = wPoint.SnapToGrid(GridSize);
            PreviewSprite.transform.position = newPos;
            SelectionSprite.transform.position = newPos; 
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _selection.Angle += 90f;
                if (_selection.Angle >= 360f)
                    _selection.Angle -= 360f;

                var rot = Quaternion.Euler(0, 0, _selection.Angle);
                _selection.Placeholder.transform.rotation = rot;
                PreviewSprite.transform.rotation = rot;
            }

            // Found valid placement? switch to selection state
            var hoveredItem = FindPreplacementAtPoint(_camera.ScreenToWorldPoint(Input.mousePosition));
            if( !hoveredItem || hoveredItem.gameObject != _selection.Placeholder.gameObject )
                SetHoveredItem(hoveredItem);
        }

        if( PreviewSprite.enabled )
        {
            bool canBePlaced = CanBePlaced(PreviewSprite, true);

            PreviewSprite.color = canBePlaced ? SpriteColorPreview : SpriteColorBad;

            // If we release, reposition the element
            if (_mouseDrag.MouseUp) 
            {
                if(canBePlaced)
                {
                    var newPos = PreviewSprite.transform.position.SnapToGrid(GridSize);
                    _selection.Placeholder.transform.position = newPos;
                    _selection.Position = newPos;
                    SelectionSprite.transform.position = newPos;

                    //GameManager.GM.Cues.UiPlaceItem.PlayUiSource();
                }
                else
                {
                    SelectionSprite.transform.position = _selection.Position;
                }
                PreviewSprite.enabled = false;
            }
        }

    }

    // ----------------------------------------------------------------------------
    private PrePlacementInstance PlaceItemAt(ItemAsset item, Vector3 worldPosition)
    {
        var prefab = item.Item.Prefab;

        var placeholder = GameObject.Instantiate<PlaceableItem>(item.Item.PlaceholderPrefab, _preplacementContainer);
        placeholder.transform.SnapToGrid(worldPosition, GridSize);
        placeholder.transform.rotation = Quaternion.Euler(0, 0, item.Item.Angle);


        var preObject = placeholder.GetComponent<PreplacementObject>();
        if (preObject)
            preObject.SetLocked(false);

        var ppi = new PrePlacementInstance()
        {
            Item = item,
            Position = worldPosition.SnapToGrid(GridSize),
            Angle = item.Item.Angle,
            Placeholder = placeholder,
        };


        GameFlow.GF.Cues.UiPlaceItem.PlayUiSource();

        _preplacements.Add(ppi);
        _preplacementTable.Add(ppi.Placeholder.GetInstanceID(), ppi);
        _currentButton.SetCount(_currentButton.Count - 1);
        return ppi;
    }

    // ----------------------------------------------------------------------------
    private void ClearBuffer(Collider2D[] buffer)
    {
        for (int i = 0; i < buffer.Length; i++)
            buffer[i] = null;
    }

    // ----------------------------------------------------------------------------
    private PrePlacementInstance FindPlacementAtPoint(Vector3 worldPosition)
    {
        var testPoint = new Vector2(worldPosition.x, worldPosition.y);

        PlaceableItem item = null;
        var collider = Physics2D.OverlapPoint(testPoint, 1 << LayerMask.NameToLayer("Placement"));
        if (collider)
            item = collider.transform.FindUpHeirarchy<PlaceableItem>();
        return GetFromPlaceholder(item);
    }

    private PreplacementObject FindPreplacementAtPoint(Vector3 worldPosition)
    {
        var testPoint = new Vector2(worldPosition.x, worldPosition.y);

        PreplacementObject item = null;
        var collider = Physics2D.OverlapPoint(testPoint, 1 << LayerMask.NameToLayer("Placement"));
        if (collider)
            item = collider.transform.FindUpHeirarchy<PreplacementObject>();
        return item;
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
    private bool CanBePlaced( SpriteRenderer s, bool checkForSelectedItem = true )
    {
        ClearBuffer(_resultBuffer);

        int count = Physics2D.OverlapBoxNonAlloc(s.transform.position, 0.75f * s.size, 0, _resultBuffer);
        PlaceableItem obj = null;
        for(int i = 0; i < count; i++)
        {
            obj = _resultBuffer[i].transform.FindUpHeirarchy<PlaceableItem>();
            if (checkForSelectedItem && obj && obj == _selection.Placeholder)
                continue;

            return false;
        }
        return true;
    }


    // ----------------------------------------------------------------------------
    private void OnItemButtonClicked(UiItemButton obj, PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            _currentButton = obj;
            _itemToPlace = obj.Item;

            if (PreviewSprite)
            {
                PreviewSprite.enabled = true;
                PreviewSprite.sprite = obj.Item.Item.PreviewSprite;
                PreviewSprite.transform.rotation = Quaternion.Euler(0, 0, _itemToPlace.Item.Angle);
            }

            if (_itemToPlace)
                _currentState = ObjectPlacementState;
        }
    }

    // ----------------------------------------------------------------------------
    private void SetDebugColor(Color c)
    {
        if(DebugSprite)
            DebugSprite.color = c;
    }


    public void GenerateItems(SceneContainer sceneContainer)
    {
        // Process and hide original objects
        foreach (var s in _startingObjects)
        {
            sceneContainer.CreateObject(s.Item.Item.Prefab, s.transform.position, s.transform.eulerAngles.z);
            s.gameObject.SetActive(false);
        }

        // Then do player-placed
        foreach (var p in _preplacements)
        {
            sceneContainer.CreateObject(p.Item.Item.Prefab, p.Position, p.Angle);
        }

        // Hide our container set
        _preplacementContainer.gameObject.SetActive(false);
    }

    public bool Validate()
    {
        return true;
    }

    public void RestoreState()
    {
        _preplacementContainer.gameObject.SetActive(true);

        foreach (var s in _startingObjects)
            s.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        float l = -0.5f * GridSize * GridSizeX + x;
        float r = 0.5f * GridSize * GridSizeX + x;
        float t = 0.5f * GridSize * GridSizeY + y;
        float b = -0.5f * GridSize * GridSizeY + y;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(l, t, 0), new Vector3(r, t, 0));
        Gizmos.DrawLine(new Vector3(l, b, 0), new Vector3(r, b, 0));
        Gizmos.DrawLine(new Vector3(l, t, 0), new Vector3(l, b, 0));
        Gizmos.DrawLine(new Vector3(r, t, 0), new Vector3(r, b, 0));
    }
}
