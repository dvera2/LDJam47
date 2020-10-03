using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringButton : MonoBehaviour
{
    public float Force = 1.0f;
    public ForceMode2D ForceMode = ForceMode2D.Impulse;
    
    public Transform SpringButtonSprite;

    private Vector3 _localSpringButton;
    private Vector3 _offset;

    private void Awake()
    {
        _localSpringButton = SpringButtonSprite.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.attachedRigidbody)
            return;

        collision.attachedRigidbody.AddForce(transform.TransformDirection(Vector2.up) * Force, ForceMode);
        StartCoroutine(DoSpringTrigger());
    }

    private IEnumerator DoSpringTrigger()
    {
        _offset = Vector3.zero;
        float t = 0;
        float time = 0;
        float duration = 0.1f;
        while( time < duration )
        {
            time += Time.deltaTime;
            t = time / duration;
            _offset.y = Mathf.Lerp(0, 0.1f, t);
            SpringButtonSprite.localPosition = _localSpringButton + _offset;
            yield return new WaitForEndOfFrame();
        }
        t = 0;
        time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            t = time / duration;
            _offset.y = Mathf.Lerp(0.1f, 0f, t);
            SpringButtonSprite.localPosition = _localSpringButton + _offset;
            yield return new WaitForEndOfFrame();
        }
    }
}
