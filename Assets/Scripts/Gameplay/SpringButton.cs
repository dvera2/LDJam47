using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringButton : MonoBehaviour
{
    public float Speed = 1.0f;
    
    public SpriteRenderer SpringButtonSprite;
    public Sprite NeutralPuppy;
    public Sprite ActivatedPuppy;
    public AudioSource PuppyAudio;
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

        Rigidbody2D rb = collision.attachedRigidbody;

        // HAX - AddForce does not have any mass-less velocity changing options
        // so we cancel out existing forces in current direction and set the velocity
        // to our desired speed.
        var dir = transform.TransformDirection(Vector3.up).normalized;
        var dir2D = new Vector2(dir.x, dir.y).normalized;

        var velocity = rb.velocity;
        velocity = velocity - (Vector2.Dot(dir2D, rb.velocity) * dir2D) + (Speed * dir2D);
        rb.velocity = velocity;

        StartCoroutine(DoSpringTrigger());
    }

    private IEnumerator DoSpringTrigger()
    {
        SpringButtonSprite.sprite = ActivatedPuppy;
        PuppyAudio.Play();
        PuppyAudio.pitch = Random.Range(0.97f, 1.03f);

        yield return new WaitForSeconds(0.5f);

        SpringButtonSprite.sprite = NeutralPuppy;
    }
}
