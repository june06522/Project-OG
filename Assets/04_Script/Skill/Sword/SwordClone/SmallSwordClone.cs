using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSwordClone : SwordClone
{


    public override void Attack(Vector3 targetPos)
    {
        IsAttack = true;
        TargetPos = targetPos;

        Sequence seq = DOTween.Sequence();

        Vector3 dir = (TargetPos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        seq.Append(transform.DORotate(new Vector3(0, 0, angle), 0.2f));
        seq.Insert(0, transform.DOMove(transform.position + -dir, 0.5f)); 
        seq.Append(transform.DOMove(TargetPos, 0.15f).OnComplete(() => AttackEndEvt()));
        seq.Play();
    }

    public override void CheckHit()
    {
        float radius = 1f;

        Collider2D[] enemyCols = Physics2D.OverlapCircleAll(transform.position, radius,
              LayerMask.GetMask("Enemy", "TriggerEnemy"));

        foreach (var enemyCol in enemyCols)
        {
            Enemy enemy;
            if (enemyCol.TryGetComponent<Enemy>(out enemy))
            {
                enemy.Hit(damage);
            }
        }

    }
}
