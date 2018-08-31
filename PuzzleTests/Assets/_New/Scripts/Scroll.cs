using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Scroll : MonoBehaviour
{
    private Material _material;
    public Vector2 speed, currentscroll;
    int i = 0;

    void Start()
    {
        if(GetComponent<SpriteRenderer>())
            _material = GetComponent<SpriteRenderer>().material;

        if (GetComponent<TilemapRenderer>())
            _material = GetComponent<TilemapRenderer>().material;

    }

    void Update()
    {
        //currentscroll += speed * Time.deltaTime;
        //_material.mainTextureOffset = currentscroll;

        i++;
        if (i > Random.Range(80, 100))
        {
            currentscroll = speed * Time.deltaTime;
            _material.mainTextureOffset = currentscroll;
            //i = 0;
        }
        if (i > Random.Range(60, 100))
        {
            currentscroll += speed * Time.deltaTime;
            _material.mainTextureOffset = currentscroll * Random.insideUnitCircle.normalized;
            i = 0;
        }
        else
        {
            currentscroll += speed * Time.deltaTime;
            _material.mainTextureOffset = currentscroll;
        }
    }

    public void SetDirection(Vector2 dir)
    {
        speed = dir;
    }
}
