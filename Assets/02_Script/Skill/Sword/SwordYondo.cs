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

    Transform ownerTrm;
    Transform targetTrm;

    Vector3 startLocalPos;
    Quaternion startRot;

    Action AttackStartAction;
    AnimationCurve curve;

    private LayerMask layerMask;
    private float damage;
    private float radius;
    private float curSpeed;
    ESwordYondoState curState;
    bool isRotating;
    bool completlyAttach;
    bool attachTrigger;

    Rigidbody2D rb;
    private SwordTargetDetector detector;
    private float lerpAngleValue;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        detector = transform.Find("Detector").GetComponent<SwordTargetDetector>();

        AttackStartAction = () =>
        {
            DOTween.To(() => curSpeed, (spd) => curSpeed = spd, this.speed, 0.5f);
            SetTarget();
        };
        ownerTrm = GameManager.Instance.player.transform;
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

    public void Init(LayerMask layerMask, float power, float radius, float coolTime)
    {
        this.radius = radius;
        this.damage = power;
        this.layerMask = layerMask;
        //this.ownerTrm = ownerTrm;

        startLocalPos = transform.localPosition;
        startRot = transform.localRotation;

        StartCoroutine("DestroyThisObj", coolTime);
    }

    IEnumerator DestroyThisObj(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        Destroy(this.gameObject);
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
            case ESwordYondoState.Idle:
                Idle();
                break;
            case ESwordYondoState.Attack:
                Attack();
                break;
            case ESwordYondoState.Attach:
                Attach();
                break;
        }
    }
    private void CheckTransition()
    {
        switch (curState)
        {
            case ESwordYondoState.Idle:
                if (CheckEnemyInRadius())
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
                if (completlyAttach == true)
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
        transform.localPosition = startLocalPos;
        attachTrigger = false;
    }

    private void Attach()
    {
        if (attachTrigger == false)
        {
            attachTrigger = true;
            rb.velocity = Vector2.zero;
            transform.DORotateQuaternion(startRot, 1.5f).SetEase(Ease.OutQuad);
        }
        Vector3 targetPos = ownerTrm.position + startLocalPos;
        transform.position = Vector3.Lerp(targetPos, transform.position, 0.3f);

        if (Vector3.Distance(transform.localPosition, startLocalPos) < 0.1f)
        {
            completlyAttach = true;
            transform.position = targetPos;
        }
    }

    private void Attack()
    {
        if (targetTrm == null || targetTrm.gameObject.activeSelf == false)
        {
            ChangeState(ESwordYondoState.Attach);
            Destroy(this.gameObject);
            return;
        }

        if (detector.IsDetect)
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
        //        Debug.Log("��");
        //    }
        //}

        rb.velocity = transform.right * curSpeed;
    }

    private void SetTarget()
    {
        targetTrm = FindClosestEnemy();
        detector.CurTargetTrm = targetTrm;

        isRotating = true;
        completlyAttach = false;

        //float distance = Vector3.Distance(ownerTrm.position, targetTrm.position);
        //float rotateTime = Mathf.Lerp(minRotateTime, maxRotateTime, distance / radius);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHitAble hitAble;
        if(collision.TryGetComponent<IHitAble>(out hitAble))
        {
            // 적이 죽었으면
            if(hitAble.Hit(damage) == false)
            {
                //새로이 타겟 설정
                SetTarget();
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    //#if UNITY_EDITOR
    //    private void OnDrawGizmos()
    //    {
    //        if (Application.isPlaying == false) return;
    //        if (detector.detectPoint != Vector2.zero && targetTrm != null)
    //        {
    //            Gizmos.color = Color.magenta;
    //            Gizmos.DrawSphere(detector.detectPoint, 1f);
    //            Gizmos.DrawLine(detector.detectPoint, targetTrm.position);
    //        }
    //    }
    //#endif


}
