using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;

    public Transform currentTarget;
    public bool IsOutOfDistance = false;
    public int GetTargetsCount() => targets == null ? 0 : targets.Count;

    public bool IsDetectObstacle()
    {
        if (obstacles != null)
        {
            return obstacles.Length > 0;
        }
        return false;
    }
}
