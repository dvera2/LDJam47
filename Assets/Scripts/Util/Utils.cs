using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void SnapObjectToGrid(this Transform t, Vector3 worldPosition, PlaceableItem item)
    {
        if(t && item)
        {
            if (item.SnapPosition)
            {
                float gridSize = item.SnapGrideSize;
                var pos = worldPosition;
                pos.x = gridSize * Mathf.RoundToInt(pos.x / gridSize);
                pos.y = gridSize * Mathf.RoundToInt(pos.y / gridSize);
                pos.z = 0;
                t.position = pos;
            }

            if (item.SnapRotation)
            {
                float rotIncrement = item.RotIncInDegs;
                var rot = t.eulerAngles;
                rot.z = rotIncrement * (int)(rot.z / rotIncrement);
                t.eulerAngles = rot;
            }
        }
    }
}
