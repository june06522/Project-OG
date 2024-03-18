using UnityEngine;
using DG.Tweening;
using System;

public class SwordClone : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;
    private float attackRadius;

    float t;

    Material material;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public bool EndDissolve { get; set; } = false;
    public bool EndAttack { get; set; } = false;
    public bool IsAttack { get; set; } = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        rb = GetComponent<Rigidbody2D>();
        Init();
    }

    public void Init()
    {
        material.SetFloat("_SourceGlowDissolveFade", 0f);
        EndDissolve = false;
        EndAttack = false;
        IsAttack = false;
        t = 0f;
    }

    private void Update()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }


    public void Setting(float dissolveTime, float attackRadius, Transform ownerTrm)
    {
        float value = 0;
       
        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), 1, dissolveTime)
            .OnComplete(() => EndDissolve = true);
        
    }

    public void Shooting(Vector2 dir)
    {
        rb.velocity = dir * speed; 
    }

    public void Search(LayerMask layerMask)
    {
        Transform targetTrm = FindClosestEnemy(layerMask);
        if(targetTrm != null)
        {
            Vector2 dir = (targetTrm.position - transform.position).normalized;
            Shooting(dir);
        }
    }


    private Transform FindClosestEnemy(LayerMask layerMask)
    {
        Vector2 detectStartPos = transform.position;
        Collider2D[] cols = new Collider2D[5];
        int colCount = Physics2D.OverlapCircleNonAlloc(detectStartPos, 5, cols, layerMask);
        //Collider2D col = Physics2D.OverlapCircle(detectStartPos, radius, layerMask);

        if (colCount == 0)
            return null;
        else
            return cols[UnityEngine.Random.Range(0, colCount)].transform;
    }

    public void Attack(Vector3 targetPos)
    {
        IsAttack = true;
        Vector3 dir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(0,0,angle), 0.2f));
        seq.Insert(0,transform.DOMove(transform.position + -dir, 0.5f));
        seq.Append(transform.DOMove(targetPos, 0.2f).OnComplete(() => DisAppear()));
        seq.Play();
    }

    private void DisAppear()
    {
        rb.velocity = Vector2.zero;
        float value = 1;
        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), 0, 1)
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
