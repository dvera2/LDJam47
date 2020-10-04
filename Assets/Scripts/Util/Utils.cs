using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 SnapToGrid(this Vector3 input, float gridSize )
    {
        input.x = gridSize * Mathf.RoundToInt(input.x / gridSize);
        input.y = gridSize * Mathf.RoundToInt(input.y / gridSize);
        input.z = 0;
        return input;
    }

    public static void SnapToGrid(this Transform t, Vector3 worldPosition, float gridSize)
    {
        if (t)
        {
            t.position = worldPosition.SnapToGrid(gridSize);
        }
    }

    public static void SnapObjectToGrid(this Transform t, Vector3 worldPosition, PlaceableItem item)
    {
        if(t && item)
        {
            if (item.SnapPosition)
            {
                t.SnapToGrid(worldPosition, item.SnapGrideSize);
            }

            if (item.SnapRotation)
            {
                float rotIncrement = item.AngleInDeg;
                var rot = t.eulerAngles;
                rot.z = rotIncrement * (int)(rot.z / rotIncrement);
                t.eulerAngles = rot;
            }
        }
    }

    public static T FindUpHeirarchy<T>(this Transform t) where T : Component
    {
        if (t == null)
            return null;

        var pi = t.GetComponent<T>();
        if (pi)
            return pi;

        return FindUpHeirarchy<T>(t.parent);

    }
}
