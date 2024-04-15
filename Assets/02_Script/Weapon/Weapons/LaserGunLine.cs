using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunLine : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] EdgeCollider2D _edgeCollider;

    public LineRenderer LineRenderer=> _lineRenderer;
    public EdgeCollider2D EdgeCollider=> _edgeCollider;
}
