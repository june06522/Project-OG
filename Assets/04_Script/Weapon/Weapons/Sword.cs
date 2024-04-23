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

    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] Transform[] wayPointTrms;
    private Vector3[] wayPoints;
    
    Transform wayPointTrmParent;

    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _col = transform.GetComponent<Collider2D>();
        
        wayPoints = new Vector3[wayPointTrms.Length];
      
        for(int i = 0; i < wayPointTrms.Length; i++)
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

    private Vector3[] GetWorldWayPoints(Vector3 target)
    {
        Vector3[] points = new Vector3[wayPoints.Length];
        for(int i = 0; i < points.Length; i++)
        {
            points[i] = wayPointTrms[i].position + target;// + transform.position;
        }
        return points;
    }

    public override void Attack(Transform target)
    {

        Vector3 dir = (target.position - transform.position);
        float magnitude = dir.magnitude;
        dir.Normalize();

        Vector3 crossVec = Vector3.Cross(dir, transform.right);

        float dot = Vector2.Dot(crossVec, Vector2.up);
        int sign = dot > 0 ? -1 : 1;
        
        
        Vector3[] wayPoints = GetWorldWayPoints(dir * (magnitude - 1.5f));
        if (sign == -1)
            wayPoints.Reverse();

        transform.position = wayPoints[0];
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z - sign * 60));

        DOTween.Sequence().

            Append(transform.DOPath(wayPoints, 0.25f, PathType.CatmullRom, PathMode.TopDown2D, 30).SetEase(ease)).
            Insert(0f, transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z + sign * 90), 0.25f));

        

        StartCoroutine(AttackTween());

    }

    private IEnumerator AttackTween()
    {

        isAttack = true;
        _col.enabled = true;
        yield return new WaitForSeconds(0.2f);
        _col.enabled = false;
        yield return new WaitForSeconds(0.2f);
        isAttack = false;
        transform.DOKill();
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

        if (target == null) return;
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
            hitAble.Hit(Data.AttackDamage.GetValue());

        }
    }

}
