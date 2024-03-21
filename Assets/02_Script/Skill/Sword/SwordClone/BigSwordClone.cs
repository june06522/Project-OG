using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSwordClone : SwordClone
{
    ShockWaveEffect shockWaveEffect;
    BlastWave blastWave;

    protected override void Awake()
    {
        CloneType = ECloneType.BIG;

        base.Awake();
        shockWaveEffect = transform.Find("ShockWaveEffect").GetComponent<ShockWaveEffect>();
        blastWave = transform.Find("BlastWave").GetComponent<BlastWave>();  
    }

    public override void Attack(Vector3 targetPos)
    {
        base.Attack(targetPos); 
        
        Vector3 dir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(targetPos, 0.15f).OnComplete(() => CheckHit(targetPos)));
        seq.Play();
    }

    public override void CheckHit(Vector2 targetPos)
    {
        float radius = Width;
        
        Collider2D[] enemyCols = Physics2D.OverlapCircleAll(transform.position, radius,
             LayerMask.GetMask("Enemy", "TriggerEnemy"));

        foreach (var enemyCol in enemyCols)
        {
            if (!IsInElipse(enemyCol.transform.position, targetPos))
                continue;

            Enemy enemy;
            if (enemyCol.TryGetComponent<Enemy>(out enemy))
            {
                enemy.Hit(damage);
                Debug.Log("Gang");
            }
        }

        DisAppear();
    }
}

