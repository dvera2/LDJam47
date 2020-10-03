using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    public float Force = 1.0f;
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.attachedRigidbody)
            return;

        collision.attachedRigidbody.AddForce(transform.TransformDirection(Vector2.up) * Force, ForceMode2D.Force);
    }
}
