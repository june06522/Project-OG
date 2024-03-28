using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSwordClone : SwordClone
{
    [SerializeField]
    ShockWaveEffect shockWaveEffect;
    [SerializeField]
    BlastWave blastWave;

    public override void Attack(Vector3 targetPos)
    {
        IsAttack = true;
        TargetPos = targetPos;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(TargetPos, 0.15f).OnComplete(() => AttackEndEvt()));
        seq.Play();
    }

    public override void AttackEndEvt()
    {
        Instantiate(blastWave, TargetPos, Quaternion.identity).Play();
        shockWaveEffect.Play(TargetPos);
        base.AttackEndEvt();
    }

    public override void CheckHit()
    {
        float radius = Width;
        
        Collider2D[] enemyCols = Physics2D.OverlapCircleAll(TargetPos, radius,
             LayerMask.GetMask("Enemy", "TriggerEnemy", "Boss"));

        foreach (var enemyCol in enemyCols)
        {
            if (!IsInElipse(enemyCol.transform.position, TargetPos))
                continue;

            IHitAble enemy;
            if (enemyCol.TryGetComponent<IHitAble>(out enemy))
            {
                enemy.Hit(damage);
                Debug.Log("Gang");
            }
        }
    }

    public bool IsInElipse(Vector2 centerPos, Vector2 targetPos)
    {
        //0.5는 보정치
        float width = Width + 0.5f;
        float height = Height + 0.5f;

        Vector2 dot1 = targetPos;
        dot1.x -= Mathf.Sqrt(width * width - height * height);
        Vector2 dot2 = targetPos;
        dot2.x += Mathf.Sqrt(width * width - height * height);

        float dist = 0;

        dist += Vector3.Distance(centerPos, dot1);
        dist += Vector3.Distance(centerPos, dot2);

        if (dist <= width * 2)
            return true;

        return false;
    }

}

