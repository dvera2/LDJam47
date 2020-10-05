using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    public IToggleable LinkedItem;
    public LineRenderer LinkLine;

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        if (LinkLine && LinkedItem != null)
        {
            LinkLine.positionCount = 2;
            LinkLine.SetPosition(0, transform.position);
            LinkLine.SetPosition(1, LinkedItem.Position);
        }
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hamster"))
        {
            LinkedItem?.Toggle();
        }
    }
}
