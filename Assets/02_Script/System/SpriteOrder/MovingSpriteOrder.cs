using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpriteOrder : SpriteOrder
{
    private void Start()
    {
        if(_spriteRenderer != null)
        {
            SpriteOrderManager.Instance.RegisterMovingSprite(this);
        }
    }

    private void OnDestroy()
    {
        SpriteOrderManager.Instance.DestroyMovingSprite(this);
    }

    public void SetSprite()
    {
        _spriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100);
    }
}
