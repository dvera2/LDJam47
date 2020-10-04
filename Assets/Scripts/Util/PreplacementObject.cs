using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreplacementObject : MonoBehaviour
{
    public ItemAsset Item;
    public SpriteRenderer LockIcon;
    public bool Locked = true;

    private void Start()
    {
        if (LockIcon)
            LockIcon.enabled = Locked;
    }

    public void SetLocked(bool locked)
    {
        Locked = locked;

        if (LockIcon)
            LockIcon.enabled = Locked;
    }

    public void OnHoverEnded()
    {
        if (LockIcon)
            LockIcon.enabled = false;
    }

    public void OnHoverStarted()
    {
        if (LockIcon)
        {
            LockIcon.enabled = Locked;
            LockIcon.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
