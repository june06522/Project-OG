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

    Collider2D collider;

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}
