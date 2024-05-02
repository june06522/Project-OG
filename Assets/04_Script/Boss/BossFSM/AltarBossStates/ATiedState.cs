using System.Collections;
using UnityEngine;

public class ATiedState : BossBaseState
{
    private AltarBoss _altar;
    private AltarPattern _pattern;

    public ATiedState(AltarBoss boss, AltarPattern pattern) : base(boss, pattern)
    {
        _altar = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        _altar.bossMove.StopMovement(true);
    }

    public override void OnBossStateOn()
    {
        _altar.gameObject.layer = LayerMask.NameToLayer("Boss");

        _altar.isStop = false;
        _altar.isTied = true;

        _altar.StartCoroutine(RandomPattern(_altar.so.PatternChangeTime));
        _altar.StartCoroutine(_altar.bossMove.BossMovement(_altar.so.StopTime, 0.5f, 0.5f, _altar.so.Speed, _altar.so.WallCheckRadius));
    }

    public override void OnBossStateUpdate()
    {
        if(!_altar.isTied)
        {
            _altar.StopCoroutine(RandomPattern(_altar.so.PatternChangeTime));
            StopNowCoroutine();
        }
    }

    public IEnumerator RandomPattern(float waitTime)
    {
        while(_altar.isTied)
        {
            if (_altar.isAttacking)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(waitTime);

            int rand = Random.Range(1, 3);

            _altar.isAttacking = true;

            switch (rand)
            {
                case 1:
                    NowCoroutine(_pattern.OmnidirAttack(_altar, 18, 5, 1, 1));
                    break;
                case 2:
                    NowCoroutine(_pattern.OmniGuidPlayerAttack(_altar, 18, 5, 1, 1));
                    break;
            }
        }
    }
}
