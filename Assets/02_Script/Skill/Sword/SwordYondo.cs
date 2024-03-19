using DG.Tweening;
using FSM_System;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum ESwordYondoState
{
    Idle = 0,
    Attack = 1,
    Attach = 2,
}


public class SwordYondo : MonoBehaviour
{

    [Header("Rotate")]
    [SerializeField] float minRotateTime = 0.05f;
    [SerializeField] float maxRotateTime = 0.5f;

    [Header("Speed")]
    [SerializeField] float speed = 500f;
    [SerializeField] AnimationCurve curve;

    [Header("ETC")]    
    [SerializeField] LayerMask layerMask;
    [SerializeField] float radius = 10f;
    [SerializeField] float damage = 5f;
    [SerializeField]

    Transform ownerTrm;
    Transform targetTrm;

    //시작 설정되있는 값
    Vector3 startLocalPos;
    Quaternion startRot;

    Action AttackStartAction;


    private float curSpeed;
    ESwordYondoState curState;
    bool isRotating;
    bool completlyAttach;

    Rigidbody2D rb;
    public SwordTargetDetector Detector;
    private float lerpAngleValue;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Detector = transform.Find("Detector").GetComponent<SwordTargetDetector>();

        AttackStartAction = () =>
        {
            DOTween.To(() => curSpeed, (spd) => curSpeed = spd, this.speed, 0.5f);
            SetTarget();
        };
    }

    private void OnEnable()
    {
        isRotating = false;
        targetTrm = null;

        startLocalPos = transform.localPosition;
        startRot = transform.localRotation;

        curSpeed = speed;
        curState = ESwordYondoState.Idle;
    }

    private void FixedUpdate()
    {
        CheckTransition();
        Run();
    }

    #region MiniFSM
    private void Run()
    {
        switch (curState)
        {
            case ESwordYondoState.Idle: Idle();
                break;
            case ESwordYondoState.Attack: Attack();
                break;
            case ESwordYondoState.Attach: Attach();
                break;
        }
    }
    private void CheckTransition()
    {
        switch (curState)
        {
            case ESwordYondoState.Idle:
                if(CheckEnemyInRadius())
                {
                    AttackStartAction?.Invoke();
                    ChangeState(ESwordYondoState.Attack);
                }
                break;
            case ESwordYondoState.Attack:
                //if (!CheckEnemyInRadius())
                //{
                //    ChangeState(ESwordYondoState.Attach);
                //}
                break;
            case ESwordYondoState.Attach:
                if(completlyAttach == true)
                {
                    ChangeState(ESwordYondoState.Idle);
                }
                break;
        }
    }
    private void ChangeState(ESwordYondoState state)
    {
        curState = state;
    }
    #endregion

    //Action
    private void Idle()
    {
        curSpeed = 0;
        rb.velocity = Vector2.zero;
        transform.position = ownerTrm.position + startLocalPos;
    }

    private void Attach()
    {
        rb.velocity = Vector2.zero;
        transform.DOMove(startLocalPos, 2f).SetEase(Ease.OutQuad);
        transform.DORotateQuaternion(startRot, 1.5f).SetEase(Ease.OutQuad);
        
        if(Vector3.Distance(transform.localPosition, startLocalPos) < 0.05f)
        {
            completlyAttach = true;
        }
        //StartCoroutine(Rotate(startDir, 0.5f, () => completlyAttach = true));   
    }

    private void Attack()
    {
        if(targetTrm == null || targetTrm.gameObject.activeSelf == false)
        {
            ChangeState(ESwordYondoState.Attach);
            return;
        }

        if(Detector.IsDetect)
        {
            SetTarget();
            lerpAngleValue = 0f;
        }
        else
        {   
            Vector2 dir = (targetTrm.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Quaternion result = Quaternion.Lerp(startRotation, targetRotation, 0.2f + lerpAngleValue);
            transform.rotation = result;
            lerpAngleValue += 0.01f;
        }
        //if ((targetTrm.position - transform.position).sqrMagnitude > 1)
        //{
        //    Vector2 targetDir = Detector.GetDir();
        //    float dot = Vector2.Dot(transform.right, targetDir.normalized);
        //    Debug.DrawRay(transform.position, transform.right * 5, Color.yellow);
        //    Debug.DrawRay(Detector.CurTargetTrm.position, targetDir, Color.green);
        //    if (dot < 0)
        //    {
        //        SetTarget();
        //    }
        //    else
        //    {
        //        Debug.Log("앞");
        //    }
        //}

        rb.velocity = transform.right * curSpeed;
    }

    private void SetTarget()
    {
        targetTrm = FindClosestEnemy();
        Detector.CurTargetTrm = targetTrm;

        isRotating = true;
        completlyAttach = false;

        float distance = Vector3.Distance(ownerTrm.position, targetTrm.position);
        float rotateTime = Mathf.Lerp(minRotateTime, maxRotateTime, distance / radius);

        //StopAllCoroutines();
        //StartCoroutine(Rotate(
        //            (targetTrm.position - transform.position).normalized,
        //            rotateTime,
        //            endRotateAct : () => isRotating = false
        //));
    }

    private IEnumerator Rotate(Vector2 dir, float rotateTime, Action endRotateAct)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float t = 0;
        while (t < rotateTime)
        {
            Quaternion result = Quaternion.Lerp(startRotation, targetRotation, t / rotateTime);
            transform.rotation = result;
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        endRotateAct?.Invoke();
    }

    private Transform FindClosestEnemy()
    {
        Vector2 detectStartPos = ownerTrm.position;
        Collider2D[] cols = new Collider2D[5];
        int colCount = Physics2D.OverlapCircleNonAlloc(detectStartPos, radius, cols, layerMask);
        //Collider2D col = Physics2D.OverlapCircle(detectStartPos, radius, layerMask);

        if (colCount == 0)
            return null;
        else
            return cols[UnityEngine.Random.Range(0, colCount)].transform;
    }
    private bool CheckEnemyInRadius()
    {
        return Physics2D.OverlapCircle(ownerTrm.position, radius, layerMask);
    }


    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false) return;
            if (Detector.detectPoint != Vector2.zero)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(Detector.detectPoint, 1f);
                Gizmos.DrawLine(Detector.detectPoint, targetTrm.position);
            }
        }
    #endif


}
