using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float speed;

    private SpriteRenderer _sprt;

    private void Awake()
    {
        _sprt = GetComponent<SpriteRenderer>();

        if (gameObject.name.Contains("left"))
            speed *= -1;
    }

    void FixedUpdate ()
    {
        if (speed != 0)
        {
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (transform.name.Contains("still"))
            TrapStill(player);
        if (transform.name.Contains("left") || transform.name.Contains("right"))
            TrapLine(player);
    }

    private void TrapStill(Player player)
    {
        if (player)
        {
            if (player.isHurtable())
                player.HurtPlayer(10);
        }
    }

    private void TrapLine(Player player)
    {
        if (player)
        {
            if (player.isHurtable())
                player.HurtPlayer(1);
        }
        else
        {
            speed *= -1;
            _sprt.flipX = !_sprt.flipX;
        }
    }
}
