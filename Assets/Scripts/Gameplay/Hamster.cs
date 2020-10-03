using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : MonoBehaviour
{
    public SpriteRenderer HamsterSprite;
    public Rigidbody2D RB;

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

    }
}
