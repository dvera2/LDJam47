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

    public static void ClearChildren(this Transform t)
    {
        if (!t)
            return;

        int itemCount = t.childCount;
        for (int i = itemCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                GameObject.Destroy(t.GetChild(i).gameObject);
            else
                GameObject.DestroyImmediate(t.GetChild(i).gameObject);
        }
    }

    public static SurfaceType GetCurrentSurface(this RaycastHit2D hit, SurfaceType current)
    {
        if (Vector2.Dot(hit.normal, Vector2.up) > 0.1f)
        {
            if (hit.collider.CompareTag("Surface"))
            {
                var info = hit.collider.GetComponent<ColliderSurfaceInfo>();
                if (info)
                    return info.Surface;
            }
        }

        return current;
    }
    
    public static SurfaceType GetCurrentSurface(this Collision2D collision, ContactPoint2D[] buffer, SurfaceType current)
    {
        int count = collision.GetContacts(buffer);
        for (int i = 0; i < count; i++)
        {
            // Check if sloped on floor
            if (Vector2.Dot(buffer[i].normal, Vector2.up) > 0.1f)
            {
                if (buffer[i].enabled && buffer[i].collider.CompareTag("Surface"))
                {
                    var info = buffer[i].collider.GetComponent<ColliderSurfaceInfo>();
                    if (info)
                        return info.Surface;
                }
            }
        }
        // Didn't find a valid surface, so ignore and assume no change
        return current;
    }

    public static void PlayUiSource(this AudioClip clip)
    {
        if (clip == null)
            return;

        var src = GameManager.GM.MainUiSource;
        if (!src)
            return;

        src.PlayOneShot(clip);
    }
}
