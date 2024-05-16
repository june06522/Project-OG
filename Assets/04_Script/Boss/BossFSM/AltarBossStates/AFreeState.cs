using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// 플레이어를 따라가다가 플레이어가 벽 쪽에 붙으면 좀 문제가 생김
public class AFreeState : BossBaseState
{
    private AltarBoss _altar;
    private AltarPattern _pattern;

    private IEnumerator _co;

    public AFreeState(AltarBoss boss, AltarPattern pattern) : base(boss, pattern)
    {
        _altar = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        _altar.StopCoroutine(RandomPattern(_altar.so.PatternChangeTime));
        _altar.StopCoroutine(_co);

        _altar.SetBody(_altar.bigestBody, Vector3.one, Vector3.zero, _altar.bossColor, 0.5f);
        _altar.SetBody(_altar.mediumSizeBody, Vector3.one, Vector3.zero, _altar.bossColor, 0.5f);
        _altar.SetBody(_altar.smallestBody, Vector3.one, Vector3.zero, _altar.bossColor, 0.5f);
    }

    public override void OnBossStateOn()
    {
        _altar.isStop = false;
        _altar.isBlocked = false;

        _co = _altar.bossMove.BossMovement(_altar.so.StopTime, -_altar.so.MoveX, _altar.so.MoveX, -_altar.so.MoveY, _altar.so.MoveY, _altar.so.Speed, _altar.so.WallCheckRadius);
        _altar.StartCoroutine(_co);
        _altar.StartCoroutine(RandomPattern(_altar.so.PatternChangeTime));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    public IEnumerator RandomPattern(float waitTime)
    {
        while(!_altar.IsDie)
        {
            if(_altar.isAttacking)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(waitTime);

            int rand = 3;// Random.Range(1, 4);

            if (rand == 3)
            {
                if(_altar.isIW)
                {
                    rand = Random.Range(1, 3);
                }
            }

            _altar.isAttacking = true;

            switch (rand)
            {
                case 1:
                    NowCoroutine(_pattern.OmniGuidPlayerAttack(_altar, 20, 5, 2, 3));
                    break;
                case 2:
                    _altar.isStop = true;
                    NowCoroutine(_pattern.Dash(_altar, int.MaxValue, 30, 500, 0.5f, 1f));
                    break;
                case 3:
                    _altar.isIW = true;
                    _pattern.MakeIntantWall(_altar, 1, 10, _altar.so.StopTime);
                    break;
            }
        }
    }
}
