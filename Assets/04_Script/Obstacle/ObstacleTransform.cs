using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection
{ 
    UP,
    DOWN,
    LEFT,
    RIGHT,
}


public class ObstacleTransform : MonoBehaviour
{
    public EDirection Direction;
    public Vector3 GetPos() => transform.position;

    public Quaternion GetRot()
    {
        float angle = 0f;
        switch (Direction)
        {
            case EDirection.UP:
                angle = 0f;
                break;
            case EDirection.DOWN:
                angle = 180f;
                break;
            case EDirection.LEFT:
                angle = 90f;
                break;
            case EDirection.RIGHT:
                angle = 270f;
                break;
        }

        return Quaternion.Euler(0,0,angle);
    }

}
