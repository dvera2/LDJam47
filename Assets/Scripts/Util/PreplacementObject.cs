using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script marking an object as a placeholder instance that is used during the
/// game's edit-mode.
/// </summary>
[SelectionBase]
public class PreplacementObject : MonoBehaviour
{
    public ItemAsset Item;
    public SpriteRenderer LockIcon;
    public bool Locked = true;

    // -------------------------------------------------------------
    private void Start()
    {
        if (LockIcon)
        {
            // Update sorting order such that lock is above of all placement sprites
            var spriteComponents = GetComponentsInChildren<SpriteRenderer>();
            int sortingOrder = -100000;
            foreach (var p in spriteComponents)
                if (p != LockIcon)
                    sortingOrder = Mathf.Max(p.sortingOrder, sortingOrder);

            LockIcon.enabled = false;
            LockIcon.sortingOrder = sortingOrder + 1;
        }
    }

    // -------------------------------------------------------------
    public void SetLocked(bool locked)
    {
        Locked = locked;

        if (LockIcon)
            LockIcon.enabled = Locked;
    }

    // -------------------------------------------------------------
    public void OnHoverEnded()
    {
        if (LockIcon)
            LockIcon.enabled = false;
    }

    // -------------------------------------------------------------
    public void OnHoverStarted()
    {
        if (LockIcon)
        {
            LockIcon.enabled = Locked;
            LockIcon.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
