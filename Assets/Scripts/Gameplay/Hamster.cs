using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : MonoBehaviour
{
    public SpriteRenderer HamsterSprite;
    public Sprite[] HamsterWalkAnim;
    public Rigidbody2D RB;

    private int _spriteIndex = 0;
    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        if (RB == null)
            RB = GetComponent<Rigidbody2D>();
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
}
