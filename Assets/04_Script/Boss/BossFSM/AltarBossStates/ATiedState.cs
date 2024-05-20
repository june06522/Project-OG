using System.Collections;
using UnityEngine;

public class ATiedState : BossBaseState
{
    private AltarBoss _altar;
    private AltarPattern _pattern;

    private IEnumerator RandomPatternCo;
    private IEnumerator patternCo;

    public ATiedState(AltarBoss boss, AltarPattern pattern) : base(boss, pattern)
    {
        _altar = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        StopNowCoroutine();
        _altar.bossMove.StopMovement(true);

        _altar.SetBody(_altar.bigestBody, Vector3.one, Vector3.zero, _altar.bossColor, 0.5f);
        _altar.SetBody(_altar.mediumSizeBody, Vector3.one, Vector3.zero, _altar.bossColor, 0.5f);
        _altar.SetBody(_altar.smallestBody, Vector3.one, Vector3.zero, _altar.bossColor, 0.5f);
    }

    public override void OnBossStateOn()
    {
        _altar.gameObject.layer = LayerMask.NameToLayer("Boss");

        _altar.isStop = false;
        _altar.isTied = true;

        RandomPatternCo = RandomPattern(_altar.so.PatternChangeTime / 2);
        _altar.StartCoroutine(RandomPatternCo);
        _altar.StartCoroutine(_altar.bossMove.BossMovement(_altar.so.StopTime / 2, 1, -1, 1, -1, _altar.so.Speed, _altar.so.WallCheckRadius));
    }

    public override void OnBossStateUpdate()
    {
        if(!_altar.isTied || _altar.isUnChained)
        {
            _altar.StopCoroutine(RandomPatternCo);
            _altar.StopCoroutine(patternCo);
            _altar.isAttacking = false;
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

            _altar.isAttacking = true;

            patternCo = _pattern.OmniGuidPlayerAttack(_altar, 18, 5, 2, 1);
            NowCoroutine(patternCo);
        }
    }
}
