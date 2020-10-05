using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Hamster : MonoBehaviour
{
    public SpriteRenderer HamsterSprite;
    public Sprite[] HamsterWalkAnim;
    public Rigidbody2D RB;
    public AudioSource RollingAudio;
    public AudioSource HamsterAudio;
    public float MaxAudioSpeed;
    public float MinImpactSpeed = 0.25f;


    private int _spriteIndex = 0;
    private float _time;
    private ContactPoint2D[] _contactBuffer = new ContactPoint2D[10];
    private RaycastHit2D[] _raycastHitBuffer = new RaycastHit2D[10];
    private SurfaceType _currentSurface;
    private SurfaceType _prevSurface;

    // Start is called before the first frame update
    void Start()
    {
        if (RB == null)
            RB = GetComponent<Rigidbody2D>();

        _currentSurface = null;
        _prevSurface = null;
    }

    private void OnEnable()
    {
        if (RollingAudio)
            RollingAudio.Stop();

        GameEvents.LevelCompleted += OnLevelCompleted;
    }

    private void OnDisable()
    {
        GameEvents.LevelCompleted -= OnLevelCompleted;
    }

    private void OnLevelCompleted()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        HamsterSprite.transform.rotation = Quaternion.identity;

        if (RB.velocity.x < 0 && !HamsterSprite.flipX)
            HamsterSprite.flipX = true;
        else if( RB.velocity.x > 0 && HamsterSprite.flipX )
            HamsterSprite.flipX = false;

        HamsterSprite.sprite = GetNextWalkSprite(Time.deltaTime, RB.velocity.x);
        UpdateAudio();
    }

    private void FixedUpdate()
    {
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(1 << LayerMask.NameToLayer("Tile"));
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = false;

        float minDistance = float.MaxValue;
        SurfaceType surface = null;
        int count = Physics2D.CircleCast(RB.position, 0.32f, Vector2.down, contactFilter, _raycastHitBuffer, 0.1f);
        for(int i = 0; i < count; i++)
        {
            var surf = _raycastHitBuffer[i].GetCurrentSurface(null);
            if (surf != null)
            {
                // get closest surface
                if(_raycastHitBuffer[i].distance < minDistance)
                {
                    surface = surf;
                    minDistance = _raycastHitBuffer[i].distance;
                }
            }
        }

        if(_prevSurface != _currentSurface)
            _prevSurface = _currentSurface;

        _currentSurface = surface;

        // DEBUG colors
        /*
        if (_currentSurface == null)
            HamsterSprite.color = Color.black;
        else 
            HamsterSprite.color = _currentSurface.DebugSurfaceColor;
        */
    }

    private void UpdateAudio()
    {
        if (RollingAudio)
        {
            if (_currentSurface != _prevSurface)
            {
                if (_currentSurface && _currentSurface.RollAudio)
                {
                    if (!RollingAudio.isPlaying || _currentSurface.RollAudio != RollingAudio.clip)
                    {
                        RollingAudio.clip = _currentSurface.RollAudio;
                        RollingAudio.loop = true;
                        RollingAudio.Play();
                    }

                    RollingAudio.volume = Mathf.Lerp(0, 1f, Mathf.Clamp01(Mathf.Abs(RB.velocity.x) / MaxAudioSpeed));
                }
                else
                {
                    RollingAudio.Stop();
                }
            }
        }
    }

    private Sprite GetNextWalkSprite(float deltaTime, float xSpeed)
    {
        _time += deltaTime * Mathf.Abs(xSpeed);
        if (_time >= 0.25f)
        {
            _spriteIndex = (_spriteIndex + 1) % HamsterWalkAnim.Length;
            _time -= 0.25f;
        }
        return HamsterWalkAnim[_spriteIndex];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var c = collision.GetContact(0);

        bool shouldPlay = false;

        int contactCount = collision.GetContacts(_contactBuffer);
        for (int i = 0; i < contactCount; i++)
        {
            var speedalongNormal = Vector2.Dot(c.relativeVelocity, c.normal);
            if (speedalongNormal >= MinImpactSpeed )
            {
                shouldPlay = true;
                break;
            }
        }

        if (shouldPlay && HamsterAudio && !HamsterAudio.isPlaying)
            HamsterAudio.Play();
    }

}
