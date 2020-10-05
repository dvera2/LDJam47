using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IToggleable
{
    bool IsOn { get; }
    void Toggle();
    Vector3 Position { get; }

}

public class Blower : MonoBehaviour, IToggleable
{
    public float Force = 1.0f;
    public ParticleSystem Particles;
    public Collider2D Trigger;

    [SerializeField]
    private bool _isOn;

    public bool IsOn => _isOn;
    public Vector3 Position => transform.position;

    void Start()
    {
        UpdateState();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.attachedRigidbody)
            return;

        collision.attachedRigidbody.AddForce(transform.TransformDirection(Vector2.up) * Force, ForceMode2D.Force);
    }

    public void Toggle()
    {
        _isOn = !_isOn;
        UpdateState();
    }

    private void UpdateState()
    {
        if (Trigger)
            Trigger.enabled = IsOn;

        if (Particles)
        {
            if (!IsOn && Particles.isPlaying)
                Particles.Stop();
            else if (IsOn && !Particles.isPlaying)
                Particles.Play();
        }
    }
}
