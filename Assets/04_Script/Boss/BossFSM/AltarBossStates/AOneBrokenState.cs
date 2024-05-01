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
    public AOneBrokenState(AltarBoss boss, AltarPattern pattern) : base(boss, pattern)
    {
        _maxMoveDistance = 5;
        _speed = 10;
        _altar = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        _altar.isOneBroken = false;
    }

    public override void OnBossStateOn()
    {
        _altar.SetBodyToBasic(_altar.bigestbody, _altar.bigestBody);
        _altar.SetBodyToBasic(_altar.mediumSizebody, _altar.mediumSizeBody);
        _altar.SetBodyToBasic(_altar.smallestbody, _altar.smallestBody);

        _altar.isStop = false;
        _altar.isOneBroken = true;

        _altar.StartCoroutine(RandomPattern(_altar.so.PatternChangeTime));
        _altar.StartCoroutine(OneBrokenMove());
    }

    public override void OnBossStateUpdate()
    {
        if(!_altar.isOneBroken)
        {
            _altar.StopCoroutine(RandomPattern(_altar.so.PatternChangeTime));
            StopNowCoroutine();
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

            int rand = Random.Range(1, 4);

            _altar.isAttacking = true;

            switch (rand)
            {
                case 1:
                    NowCoroutine(_pattern.OmnidirAttack(_altar, 20, 5, 1, 1));
                    break;
                case 2:
                    NowCoroutine(_pattern.OmniGuidPlayerAttack(_altar, 20, 5, 1, 1));
                    break;
                case 3:
                    NowCoroutine(_pattern.ThrowEnergyBall(_altar, 3, 10, 1, 2));
                    break;
            }
        }
    }

    private IEnumerator OneBrokenMove()
    {
        // 개같이 움직임 해결해줘
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
