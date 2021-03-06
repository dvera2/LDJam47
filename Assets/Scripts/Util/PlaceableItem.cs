﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableItem : MonoBehaviour
{
    public bool SnapRotation = true;
    public float AngleInDeg = 90f;

    public bool SnapPosition = true;
    public float SnapGrideSize = 0.32f;

    public void SnapToGrid(Vector3 position, Quaternion rotation)
    {
        transform.SnapObjectToGrid(position, this);
    }
}
