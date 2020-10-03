using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(PlaceableItem))]
public class PlaceableItemInspector : Editor
{
    private void OnSceneGUI()
    {
        if (!Application.isPlaying)
        {
            PlaceableItem pi = target as PlaceableItem;
            if (pi)
            {
                if (pi.transform.hasChanged)
                {
                    if (pi.SnapPosition)
                    {
                        float gridSize = pi.SnapGrideSize;
                        var pos = pi.transform.position;
                        pos.x = gridSize * Mathf.RoundToInt(pos.x / gridSize);
                        pos.y = gridSize * Mathf.RoundToInt(pos.y / gridSize);
                        pi.transform.position = pos;
                    }

                    if (pi.SnapRotation)
                    {
                        float rotIncrement = pi.RotIncInDegs;
                        var rot = pi.transform.eulerAngles;
                        rot.z = rotIncrement * (int)(rot.z / rotIncrement);
                        pi.transform.eulerAngles = rot;
                    }
                    //pi.transform.hasChanged = false;
                }
            }
        }
    }
}
