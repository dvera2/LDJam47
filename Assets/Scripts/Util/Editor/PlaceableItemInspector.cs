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
                    pi.SnapToGrid(pi.transform.position, pi.transform.rotation);

                    var po = pi.GetComponent<PreplacementObject>();
                    if (po && po.LockIcon)
                    {
                       // po.LockIcon.transform.position = pi.transform.position - new Vector3(0, 0.32f, 0);
                    }
                    //pi.transform.hasChanged = false;
                }
            }
        }
    }
}