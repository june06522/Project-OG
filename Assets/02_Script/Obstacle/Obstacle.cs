using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObstacleType
{
    None,
    Wall = 1 << 1,
    Thorn = 1 << 2,
}

public class Obstacle : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    [SerializeField] EObstacleType _type;
    public int Weight = 0;
    public EObstacleType Type => _type;
}
