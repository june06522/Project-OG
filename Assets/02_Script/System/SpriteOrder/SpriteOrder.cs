using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrder : MonoBehaviour
{
    // when it create, play once
    protected SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
            _spriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100);
    }
}
