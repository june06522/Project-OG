using UnityEngine;
using DG.Tweening;
using System;
using UnityEditor;

public enum ECloneType
{
    SMALL,
    BIG,
}


public class SwordClone : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;
    private float attackRadius;

    Material material;
    SpriteRenderer spriteRenderer;
    Transform checkOrderPointTrm;

    public bool EndDissolve { get; set; } = false;
    public bool EndAttack { get; set; } = false;
    public bool IsAttack { get; set; } = false;

    public float CurAngle;
    public ECloneType CloneType;

    private void Awake()
    {
        checkOrderPointTrm = transform;//transform.Find("OrderPoint").GetComponent<Transform>();
        spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        Init();
    }

    public void Init()
    {
        float initValue = CloneType == ECloneType.BIG ? 1f : 0f;
        material.SetFloat("_SourceGlowDissolveFade", initValue);

        EndDissolve = false;
        EndAttack = false;
        IsAttack = false;
    }

    private void Update()
    {
        spriteRenderer.sortingOrder  = Mathf.RoundToInt(checkOrderPointTrm.position.y * -100);
    }


    public virtual void Setting(float dissolveTime)
    {
        float initValue = CloneType == ECloneType.BIG ? 1f : 0f;
        float endValue = Mathf.Abs(initValue - 1);
        float value = initValue;

        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), endValue, dissolveTime)
            .OnComplete(() => EndDissolve = true);
    }

    public void Attack(Vector3 targetPos)
    {
        IsAttack = true;
        Vector3 dir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Sequence seq = DOTween.Sequence();
        if(CloneType == ECloneType.SMALL)
        {
            seq.Append(transform.DORotate(new Vector3(0,0,angle), 0.2f));
            seq.Insert(0,transform.DOMove(transform.position + -dir, 0.5f));
        }
        seq.Append(transform.DOMove(targetPos, 0.15f).OnComplete(() => DisAppear()));
        
        seq.Play();    
    }

    private void DisAppear()
    {
        float initValue = CloneType == ECloneType.SMALL ? 1f : 0f;
        float endValue = Mathf.Abs(initValue - 1);
        float value = initValue;

        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), endValue, 1)
            .OnComplete(() => EndAttack = true);
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    public void Move(Vector3 pos)
    {
        if(IsAttack)
        {
            return;
        }
        else
        {
            transform.position = pos;
        }
    }
}
