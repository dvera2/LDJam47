using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringButton : MonoBehaviour
{
    public float Force = 1.0f;
    public ForceMode2D ForceMode = ForceMode2D.Impulse;
    
    public SpriteRenderer SpringButtonSprite;
    public Sprite NeutralPuppy;
    public Sprite ActivatedPuppy;

    private void Awake()
    {
        SpringButtonSprite.sprite = NeutralPuppy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.attachedRigidbody)
            return;

        if (!collision.gameObject.CompareTag("Hamster"))
            return;

        collision.attachedRigidbody.AddForce(transform.TransformDirection(Vector2.up) * Force, ForceMode);
        StartCoroutine(DoSpringTrigger());
    }

    private IEnumerator DoSpringTrigger()
    {
        SpringButtonSprite.sprite = ActivatedPuppy;

        yield return new WaitForSeconds(0.5f);

        SpringButtonSprite.sprite = NeutralPuppy;
    }
}
