using UnityEngine;
using DG.Tweening;
using System;
using UnityEditor;
using System.Collections.Generic;

public enum ECloneType
{
    SMALL,
    BIG,
}


public class SwordClone : MonoBehaviour
{
    [SerializeField]
    protected float damage = 10f;
    [SerializeField]
    protected float speed = 20f;

    Material material;
    SpriteRenderer spriteRenderer;
    Transform checkOrderPointTrm;

    public bool EndDissolve { get; set; } = false;
    public bool EndAttack { get; set; } = false;
    public bool IsAttack { get; set; } = false;

    public ECloneType CloneType;
    
    public float CurAngle { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    protected virtual void Awake()
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


    public virtual void Setting(float dissolveTime, float width, float height)
    {
        this.Width = width + 0.5f; //0.5f는 판정 널널히 주기 위함
        this.Height = height + 0.5f;

        float initValue = CloneType == ECloneType.BIG ? 1f : 0f;
        float endValue = Mathf.Abs(initValue - 1);
        float value = initValue;

        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), endValue, dissolveTime)
            .OnComplete(() => EndDissolve = true);
    }

    public virtual void Attack(Vector3 targetPos)
    {
       
    }

    public void DisAppear()
    {
        float initValue = CloneType == ECloneType.SMALL ? 1f : 0f;
        float endValue = Mathf.Abs(initValue - 1);
        float value = initValue;

        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), endValue, 1)
            .OnComplete(() => EndAttackEvt());
    }

    private void EndAttackEvt()
    {
        EndAttack = true;
        if (CloneType == ECloneType.BIG) 
            DestroyThis();
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

    List<Vector2> points = new(); 
    public virtual void CheckHit(Vector2 targetPos)
    {
        float radius; 
        
            radius = 1f;
        

        Collider2D[] enemyCols = Physics2D.OverlapCircleAll(transform.position, radius,
               LayerMask.GetMask("Enemy", "TriggerEnemy"));

        Debug.Log("EnemyCOlCount : " + enemyCols.Length);
        foreach (var enemyCol in enemyCols)
        {
            if(CloneType == ECloneType.BIG)
            {
                if (!IsInElipse(enemyCol.transform.position, targetPos))
                    continue;
                else
                    points.Add(enemyCol.transform.position);
            }

            Enemy enemy;
            if (enemyCol.TryGetComponent<Enemy>(out enemy))
            {
                enemy.Hit(damage);
                Debug.Log("Gang");
            }
        }

        DisAppear();
    }

    

    public bool IsInElipse(Vector2 centerPos, Vector2 targetPos)
    {
        Vector2 dot1 = targetPos;
        dot1.x -= Mathf.Sqrt(Width * Width - Height * Height);
        Vector2 dot2 = targetPos;
        dot2.x += Mathf.Sqrt(Width * Width - Height * Height);

        float dist = 0;

        dist += Vector3.Distance(centerPos, dot1);
        dist += Vector3.Distance(centerPos, dot2);

        if (dist <= Width * 2)
            return true;

        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(points.Count > 0) 
        {
            points.ForEach(p =>
            {
                Gizmos.DrawSphere(p, 0.5f);
            });
        }
        //if(CloneType == ECloneType.BIG)
        //    Gizmos.DrawSphere(transform.position, Width);
    }
#endif
}
