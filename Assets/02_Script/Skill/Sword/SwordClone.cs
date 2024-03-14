using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SwordClone : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;
    private float attackRadius;

    private bool block = true;
    public bool EndDissolve { get; set; } = false;

    Material material;
    Rigidbody2D rb;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        rb = GetComponent<Rigidbody2D>();
        Init();
    }

    public void Init()
    {
        material.SetFloat("_SourceGlowDissolveFade", 0f);
        EndDissolve = false;
        block = true;
    }

    public void Setting(float dissolveTime, float attackRadius, Transform ownerTrm)
    {
        float value = 0;
       
        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), 1, dissolveTime)
            .OnComplete(() => EndDissolve = true);
        
    }

    public void Shooting(Vector2 dir)
    {
        block = false;
        rb.velocity = dir * speed; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(block == false)
        {
            float value = 1;
            DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), 0, 1f)
                .OnComplete(() => Destroy(this.gameObject));
        }
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
}
