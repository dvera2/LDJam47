using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContainer : MonoBehaviour
{
    private List<PlaceableItem> _objects;

    private void Awake()
    {
        _objects = new List<PlaceableItem>();
    }

    public PlaceableItem CreateObject(PlaceableItem prefab, Vector3 position, float rotation )
    {
        position = position.SnapToGrid(prefab.SnapGrideSize);

        var item = GameObject.Instantiate<PlaceableItem>(prefab, position, Quaternion.Euler(0, 0, rotation));
        _objects.Add(item);
        return item;
    }

    public void DestroyAll()
    {
        foreach (var s in _objects)
        {
            if(s)
                Destroy(s.gameObject);
        }

        _objects.Clear();
    }
}
