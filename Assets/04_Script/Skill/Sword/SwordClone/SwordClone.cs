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

public abstract class SwordClone : MonoBehaviour
{
    [SerializeField]
    protected float damage = 10f;
    [SerializeField]
    protected float speed = 20f;

    Material material;
    SpriteRenderer spriteRenderer;
    //SwordSkills manager;

    public bool EndDissolve { get; set; } = false;
    public bool EndAttack { get; set; } = false;
    public bool IsAttack { get; set; } = false;

    public ECloneType CloneType;
    
    public float CurAngle { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public Vector3 TargetPos { get; set; }
    private Vector3 makePos;

    protected virtual void Awake()
    {
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

    public virtual void Setting(float dissolveTime, float width, float height, Vector2 makePos, float damage)
    {
        this.Width = width; //0.5f는 판정 널널히 주기 위함
        this.Height = height;
        this.makePos = makePos;
        this.damage = damage;

        float initValue = CloneType == ECloneType.BIG ? 1f : 0f;
        float endValue = Mathf.Abs(initValue - 1);
        float value = initValue;

        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), endValue, dissolveTime)
            .OnComplete(() => EndDissolve = true);
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

    public abstract void Attack(Vector3 targetPos);
    public virtual void AttackEndEvt()
    {
        CheckHit();
        DisAppear();
    }
    public abstract void CheckHit();
    public void DisAppear()
    {
        float initValue = CloneType == ECloneType.SMALL ? 1f : 0f;
        float endValue = Mathf.Abs(initValue - 1);
        float value = initValue;

        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), endValue, 1)
            .OnComplete(() => EndDisappearEvt());
    }
    private void EndDisappearEvt()
    {
        EndAttack = true;
        if (CloneType == ECloneType.BIG) 
            DestroyThis();
    }
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        //Rotate
        //Move(manager.GetElipsePos(makePos, CurAngle, Width, Height));

        #region SortingOrder
        if (CloneType == ECloneType.BIG)
            spriteRenderer.sortingOrder = 0;
        else
            spriteRenderer.sortingOrder = Mathf.RoundToInt((makePos.y - transform.position.y) * 100);
        #endregion
    }

}
