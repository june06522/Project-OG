using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Eclipse
{
    /// <summary>
    /// 타원 위치 반환
    /// </summary>
    /// <param name="centerPos"></param>
    /// <param name="angle"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="theta"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector2 GetElipsePos(Vector2 centerPos, float angle, float width, float height, float theta)
    {
        float cx = centerPos.x;
        float cy = centerPos.y;

        float weight = angle * Mathf.Deg2Rad;
        float x = cx + width * Mathf.Cos(weight);
        float y = cy + height * Mathf.Sin(weight);

        float dx = cx + (x - cx) * Mathf.Cos(theta) - (y - cy) * Mathf.Sin(theta);
        float dy = cy + (x - cx) * Mathf.Sin(theta) + (y - cy) * Mathf.Cos(theta);

        return new Vector2(dx, dy);
    }


}
