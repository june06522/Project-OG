using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class LaserBullet : MonoBehaviour
{
    LineRenderer lineRenderer;
    float lineWidth;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineWidth = lineRenderer.startWidth;
        lineRenderer.enabled = false;
    }

    public void Shoot(Vector2 startPos, Vector2 endPos, float damage, bool player)
    {
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos);
        lineRenderer.enabled = true;

        Vector2 curPos = startPos;
        Tween laserTween = DOTween.To(() => startPos, pos => lineRenderer.SetPosition(1, pos), endPos, 0.3f).SetEase(Ease.InCubic);
        Tween laserwidthTween =
            DOTween.To(() => lineWidth, width => lineRenderer.startWidth = lineRenderer.endWidth = width, 0, 0.3f).SetEase(Ease.OutSine);

        Sequence seq = DOTween.Sequence();
        seq.Append(laserTween);
        seq.InsertCallback(0.2f, () => CheckHit(startPos, endPos, damage, player));
        seq.Append(laserwidthTween).AppendCallback(() => lineRenderer.enabled = false);
    }

    private void CheckHit(Vector2 startPos, Vector2 endPos, float damage, bool player)
    {
        LayerMask hitMask;
        if (player)
        {
            hitMask = LayerMask.GetMask("Boss", "TriggerEnemy", "Enemy");
        }
        else
        {
            hitMask = LayerMask.GetMask("Player");
        }

        Vector2 dir = endPos - startPos;
        RaycastHit2D[] hits =
            Physics2D.CircleCastAll(startPos, lineWidth / 2, dir.normalized, dir.magnitude, hitMask);
        
        foreach(var hit in hits)
        {
            if (hit.collider != null)
            {
                IHitAble hitAble;
                if (hit.collider.TryGetComponent<IHitAble>(out hitAble))
                {
                    hitAble.Hit(damage);
                    Debug.Log("Hit");
                }
            }
        }
    }
}
