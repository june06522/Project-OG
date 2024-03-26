using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public void Shoot(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
        lineRenderer.SetPosition(0, startPos);

        lineRenderer.enabled = true;

        Vector2 curPos = startPos;
        Tween laserTween = DOTween.To(() => startPos, pos => lineRenderer.SetPosition(1, pos), endPos, 0.3f).SetEase(Ease.InCubic); ;
        Tween laserwidthTween = DOTween.To(() => lineWidth, width => lineRenderer.startWidth = lineRenderer.endWidth = width, 0, 0.3f).SetEase(Ease.OutSine);
        Sequence seq = DOTween.Sequence();
        seq.Append(laserTween);
        seq.Append(laserwidthTween).AppendCallback(() => lineRenderer.enabled = false);
    }



}
