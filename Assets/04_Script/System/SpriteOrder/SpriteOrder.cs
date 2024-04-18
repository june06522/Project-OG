using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrder : MonoBehaviour
{
    [SerializeField]
    protected float _orderOffset = 0f;

    // when it create, play once
    protected SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
            _spriteRenderer.sortingOrder = -Mathf.RoundToInt((transform.position.y + _orderOffset) * 100);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position + new Vector3(-1, _orderOffset, 0);
        Vector3 to = transform.position + new Vector3(1, _orderOffset, 0);
        Gizmos.DrawLine(from, to);
    }
#endif

}
