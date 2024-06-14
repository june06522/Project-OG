using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternBase : MonoBehaviour
{
    protected bool _isHit = false;

    protected virtual void Update()
    {
        if(_isHit)
        {
            Invoke("CanHit", 0.5f);
        }
    }

    private void CanHit()
    {
        _isHit = false;
    }

    protected void SetLineMaterial(Material mat, GameObject[] objs, LineRenderer[] lines)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            lines[i].material = mat;
        }
    }

    protected void ShowLineRenderer(Vector3 pos, LineRenderer line, Vector2 dir, float scale)
    {
        if (RayWallCheck(pos, dir) != Vector2.zero)
        {
            line.enabled = true;
            line.SetPosition(1, RayWallCheck(pos, dir));
            line.endWidth = scale;
        }
        else
        {
            line.enabled = false;
        }
    }

    protected void RayPlayerCheck(Vector3 pos, Vector2 dir, float damage)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, Mathf.Infinity, LayerMask.GetMask("Player"));

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<IHitAble>(out var hitAble) && !_isHit)
            {
                hitAble.Hit(damage);
                _isHit = true;
            }
        }

    }

    protected Vector2 RayWallCheck(Vector3 pos, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, Mathf.Infinity, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {
            return hit.point;
        }

        return Vector2.zero;
    }
}
