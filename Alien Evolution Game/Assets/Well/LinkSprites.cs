using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkSprites : MonoBehaviour
{
    public SpriteRenderer targetSprite;
    public SpriteRenderer[] sprites;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(SpriteRenderer s in sprites)
        {
            s.sprite = targetSprite.sprite;
        }
    }
}
