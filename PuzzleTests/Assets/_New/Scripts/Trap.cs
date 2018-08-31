using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float speed;

    private SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        if (gameObject.name.Contains("left"))
            speed *= -1;
    }

    void FixedUpdate ()
    {
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player)
        {
            if (player.isHurtable())
                player.HurtPlayer(10);
        }
        else
        {
            speed *= -1;
            _sprite.flipX = !_sprite.flipX;
        }
    }
}
