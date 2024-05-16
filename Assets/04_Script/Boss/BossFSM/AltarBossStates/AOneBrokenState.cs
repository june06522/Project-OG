using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AOneBrokenState : BossBaseState
{
    private float _maxMoveDistance;
    private float _speed;
    private AltarBoss _altar;
    private AltarPattern _pattern;

    private IEnumerator RandomPatternCo;
    private IEnumerator patternCo;
    public AOneBrokenState(AltarBoss boss, AltarPattern pattern) : base(boss, pattern)
    {
        _maxMoveDistance = 10;
        _speed = 3;
        _altar = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        _altar.isOneBroken = false;
        _altar.isAttacking = false;

        _altar.SetBody(_altar.bigestBody, Vector3.one, Vector3.zero, _altar.bossColor, 0.5f);
        _altar.SetBody(_altar.mediumSizeBody, Vector3.one, Vector3.zero, _altar.bossColor, 0.5f);
        _altar.SetBody(_altar.smallestBody, Vector3.one, Vector3.zero, _altar.bossColor, 0.5f);
    }

    public override void OnBossStateOn()
    {
        _altar.isStop = false;
        _altar.isOneBroken = true;

        RandomPatternCo = RandomPattern(_altar.so.PatternChangeTime / 2);
        _altar.StartCoroutine(RandomPatternCo);
        _altar.StartCoroutine(OneBrokenMove());
    }

    public override void OnBossStateUpdate()
    {
        if(!_altar.isOneBroken || _altar.isUnChained)
        {
            _altar.StopCoroutine(RandomPatternCo);
            _altar.StopCoroutine(patternCo);
            _altar.isAttacking = false;
        }
    }

    public IEnumerator RandomPattern(float waitTime)
    {
        while(_altar.isOneBroken)
        {
            if (_altar.isAttacking)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(waitTime);

            _altar.isAttacking = true;

            patternCo = _pattern.OmniGuidPlayerAttack(_altar, 20, 5, 2, 2);
            NowCoroutine(patternCo);
        }
    }

    private IEnumerator OneBrokenMove()
    {
        while(_altar.isOneBroken)
        {
            if(!_altar.isStop)
            {
                if (Vector3.Distance(_altar.transform.localPosition, _altar.originPos) < _maxMoveDistance)
                {
                    Vector3 dir = (GameManager.Instance.player.transform.position - _altar.transform.position).normalized;

                    _altar.transform.localPosition = Vector2.MoveTowards(_altar.transform.localPosition, _altar.transform.localPosition + dir * _maxMoveDistance, Time.deltaTime * _speed);
                }
                else
                {
                    Vector3 dir = (_altar.originPos - _altar.transform.localPosition).normalized;

                    _altar.transform.localPosition = Vector2.MoveTowards(_altar.transform.localPosition, _altar.transform.localPosition + dir, Time.deltaTime * _speed);
                }
            }

            yield return null;
        }
    }
}
