using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunLine : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] EdgeCollider2D _edgeCollider;

    public LineRenderer LineRenderer => _lineRenderer;
    public EdgeCollider2D EdgeCollider => _edgeCollider;


    public void SetLine(Vector2 startPos, Vector2 endPos)
    {
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.startWidth = 1;
        _lineRenderer.SetPosition(1, endPos);
        _lineRenderer.endWidth = 1;
    }
}
