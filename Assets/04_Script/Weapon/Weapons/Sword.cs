using DG.Tweening;
using System.Collections;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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

    private int _leftAttack = 0;
    private bool _isTween;

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
        SkillManager.Instance.RegisterSkill(data.TriggerID, this, data);

    }

    private Vector3[] GetLocalWayPoints(Vector3 target)
    {
        Vector3[] points = new Vector3[wayPoints.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Quaternion.Euler(transform.eulerAngles) * wayPointTrms[i].localPosition; //+ target;// + transform.position;
        }
        return points;
    }

    public override void Attack(Transform target)
    {
        if (isAttack)
        {
            _leftAttack = Mathf.Min(7,_leftAttack + 1);
            return;
        }
        if (_leftAttack > 0)
            _leftAttack--;
        if (attackCor != null)
            StopCoroutine(attackCor);

        attackCor = StartCoroutine(AttackTween(false, target));
    }


    public IEnumerator ReinforceAttack(Transform target, Vector3 targetScale)
    {
        if (!_isTween)
            SetScaleTween(targetScale);

        yield return new WaitForSeconds(2f);

        transform.localScale = Vector3.one;
        _isTween = false;

    }

    private void SetScaleTween(Vector3 targetScale)
    {
        _isTween = true;
        transform.DOScale(targetScale, duration).SetEase(ease);
    }

    int sign = 1;
    private void AttackSequence(Transform target)
    {
        Vector3[] wayPoints;
        //sign = -sign;
        if (target != null)
        {
            Vector3 dir = (target.position - transform.position);
            float magnitude = dir.magnitude;
            dir.Normalize();

            Vector3 crossVec = Vector3.Cross(dir, transform.right);

            float dot = Vector2.Dot(crossVec, Vector2.up);
            //sign = dot > 0 ? -1 : 1;
            //Vector3[] wayPoints = GetWorldWayPoints(dir.normalized * (magnitude -1.5f));
            wayPoints = GetLocalWayPoints(dir.normalized);
            wayPoints.Append(transform.localPosition);
            if (sign == -1)
                wayPoints.Reverse();
        }
        else
        {
            wayPoints = GetLocalWayPoints(Vector3.zero);
            sign = 1;
        }

        //transform.localPosition = wayPoints[0];
        float startAngle = transform.rotation.eulerAngles.z - sign * 60;
        float endAngle = startAngle + sign * 120;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, startAngle));

        float duration = this.duration - 0.2f;
        DOTween.Sequence().
            Append(transform.DOLocalPath(wayPoints, duration * GetCoolDown(), PathType.CatmullRom, PathMode.TopDown2D, 30).SetEase(ease)).
            Insert(0.05f, transform.DORotate(new Vector3(0, 0, endAngle), (duration + 0.05f) * GetCoolDown()).SetEase(ease));

        if (_attackSoundClip != null)
        {

            SoundManager.Instance.SFXPlay("AttackSound", _attackSoundClip, 0.5f);

        }


        //StartCoroutine(AttackTween(false));

    }

    private IEnumerator AttackTween(bool reinforce, Transform target = null)
    {

        Debug.Log($"Reinforce : " + reinforce);
        isAttack = true;
        _col.enabled = true;


        //transform.DOKill();

        if (reinforce)
        {
            AttackSequence(target);
            yield return new WaitForSeconds(duration * GetCoolDown());
            transform.DOScale(Vector3.one, 0.35f * GetCoolDown())
                .SetEase(Ease.InOutSine);
        }
        else
        {
            AttackSequence(target);
            yield return new WaitForSeconds(duration * GetCoolDown());
        }

        _col.enabled = false;
        //_col.transform.localPosition = origin;
        yield return new WaitForSeconds(0.2f * GetCoolDown());
        isAttack = false;
    }

    public override void Run(Transform target, bool isSkill = false)
    {
        this.target = target;

        RotateWeapon(target);

        if ((!Data.isAttackCoolDown || isSkill) && target != null)
        {
            if (!Data.isAttackCoolDown)
                Data.SetCoolDown();

            EventTriggerManager.Instance?.BasicAttackExecute(this);

            Attack(target);

        }

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

    private float GetCoolDown()
    {
        if (_leftAttack > 0)
            return 1f / 5f;
        return Mathf.Max(Data.GetCool() / Data.GetOriginCool() , 1f / 5f);
    }
}