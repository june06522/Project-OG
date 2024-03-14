using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 접촉시 방향 벡터 반환 스크립트
public class SwordTargetDetector : MonoBehaviour
{
    private bool canCheckTarget;
    public Vector2 detectPoint;

    private Transform targetTrm;

    public Transform CurTargetTrm
    {
        get { return targetTrm; }
        set
        {
            targetTrm = value;
            canCheckTarget = true;
            IsDetect = false;
        }
    }

    public bool IsDetect { get; private set; } = false;

    //처음 접촉시기 Dir 반환
    public Vector2 GetDir()
    {
        return detectPoint - (Vector2)targetTrm.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(canCheckTarget == false || targetTrm == null)
            return;

        if(collision.transform == targetTrm)
        {
            detectPoint = transform.position;
            IsDetect = true;
            canCheckTarget = false;
        }
    }
}
