using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions : MonoBehaviour
{
    /// <summary>
    /// 거리안에 target이 있는지 검사하는 함수. 있으면 true.
    /// </summary>
    public static bool CheckDistance(Transform targetTrm, Transform trm, float detectDistance)
    {
        return Vector3.Distance(targetTrm.position, trm.position) < detectDistance;
    }

    /// <summary>
    /// 특정 방향에 장애물이 있는지 확인하는 함수.
    /// </summary>
    /// <param name="trm"> 나 자신의 Transform </param>
    /// <param name="dir"> 어느 방향을 감지할지 </param>
    /// <param name="distance"> 어디까지 감지할지 </param>
    /// <param name="type"> 어느 장애물을 감지할지 </param>
    public static bool CheckObstacleAtPoint(Transform trm, Vector3 dir, float distance, LayerMask layerMask)
    {
        return true;
    }

    /// <summary>
    /// 사이에 장애물이 있는지 확인하는 함수. 없으면 true.
    /// </summary>
    public static bool CheckObstacleBetweenTarget(Transform targetTrm, Transform trm, LayerMask layerMask)
    {
        return !Physics2D.CircleCast(trm.position, 0.25f, (targetTrm.position-trm.position), 1, layerMask);
    }
}
