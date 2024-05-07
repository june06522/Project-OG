using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

// �׽�Ʈ��
// ���߿� �����ѹ��� Ÿ�� 1ȸ�� ���ľ���
public class Sword : InvenWeapon
{

    SpriteRenderer _spriteRenderer;
    Collider2D _col;
    private bool isAttack = false;

    [SerializeField] private float duration = 0.75f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] Transform[] wayPointTrms;
    private Vector3[] wayPoints;

    Transform wayPointTrmParent;
    Coroutine attackCor;

    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _col = transform.GetComponent<Collider2D>();

        wayPoints = new Vector3[wayPointTrms.Length];

        for (int i = 0; i < wayPointTrms.Length; i++)
        {
            wayPoints.SetValue(wayPointTrms[i].localPosition, i);
        }

        wayPointTrmParent = transform.Find("Paths");

    }

    [BindExecuteType(typeof(float))]
    public override void GetSignal([BindParameterType(typeof(float))] object signal)
    {

        var data = (SendData)signal;
        if (!sendDataList.ContainsKey(data.index))
        {
            sendDataList.Add(data.index, data);
        }
        else
        {
            sendDataList[data.index].Power = sendDataList[data.index].Power > data.Power ? sendDataList[data.index].Power : data.Power;
        }

    }

    private Vector3[] GetLocalWayPoints(Vector3 target)
    {
        Vector3[] points = new Vector3[wayPoints.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.InverseTransformPoint(wayPointTrms[i].position); //+ target;// + transform.position;
        }
        return points;
    }

    public override void Attack(Transform target)
    {
        if (isAttack) return;
        if (attackCor != null)
            StopCoroutine(attackCor);
        
        AttackSequence(target);

        attackCor = StartCoroutine(AttackTween(false));
    }


    public void ReinforceAttack(Transform target)
    {
        if (attackCor != null)
            StopCoroutine(attackCor);

        AttackSequence(target);

        attackCor = StartCoroutine(AttackTween(true));

        Debug.Log("Gang");
    }

    private void AttackSequence(Transform target)
    {
        Debug.Log($"Target: {target}");
        Vector3[] wayPoints;
        int sign;
        if(target != null)
        {
            Vector3 dir = (target.position - transform.position);
            float magnitude = dir.magnitude;
            dir.Normalize();

            Vector3 crossVec = Vector3.Cross(dir, transform.right);

            float dot = Vector2.Dot(crossVec, Vector2.up);
            sign = dot > 0 ? -1 : 1;

            //Vector3[] wayPoints = GetWorldWayPoints(dir.normalized * (magnitude -1.5f));
            wayPoints = GetLocalWayPoints(dir.normalized);
            if (sign == -1)
                wayPoints.Reverse();
        }else
        {
            wayPoints = GetLocalWayPoints(Vector3.zero);
            sign = 1;
        }

        transform.localPosition = wayPoints[0];
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z - sign * 60));

        DOTween.Sequence().
            Append(transform.DOPath(wayPoints, 0.25f, PathType.CatmullRom, PathMode.TopDown2D, 30).SetEase(ease)).
            Insert(0f, transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z + sign * 90), 0.25f));

        if (_attackSoundClip != null)
        {

            SoundManager.Instance.SFXPlay("AttackSound", _attackSoundClip, 0.5f);

        }


        StartCoroutine(AttackTween(false));

    }

    private IEnumerator AttackTween(bool reinforce)
    {

        Debug.Log($"Reinforce : " + reinforce);
        isAttack = true;
        _col.enabled = true;
        yield return new WaitForSeconds(duration);
        _col.enabled = false;
        yield return new WaitForSeconds(0.2f);
        //transform.DOKill();

        if(reinforce)
        {
            transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => isAttack = false);
            Debug.Log("Gangng");
        }
        else
        {
            isAttack = false;
        }

    }

    public override void Run(Transform target)
    {
        base.Run(target);

        if (!isAttack)
        {

            _col.transform.localPosition = origin;

        }

    }

    public override void OnRePosition()
    {
        origin = transform.localPosition;
    }

    protected override void RotateWeapon(Transform target)
    {
        if (target == null)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 60f);
            return;
        }
        if (isAttack == true) return;

        var dir = target.position - transform.position;
        dir.Normalize();
        dir.z = 0;

        _spriteRenderer.flipY = dir.x switch
        {

            var x when x > 0 => false,
            var x when x < 0 => true,
            _ => _spriteRenderer.flipY

        };

        transform.right = dir;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHitAble>(out var hitAble))
        {
            //Debug.Log(1);
            hitAble.Hit(Data.GetDamage());

        }
    }

}
