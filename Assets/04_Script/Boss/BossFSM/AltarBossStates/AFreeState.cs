using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// 플레이어를 따라가다가 플레이어가 벽 쪽에 붙으면 좀 문제가 생김
public class AFreeState : BossBaseState
{
    private bool _allThrow;
    private bool _lock;
    private bool _once;

    private float _rotatingBallRegenTime;
    private float _curWaitingRegenTime;

    private AltarBoss _altar;
    private AltarPattern _pattern;

    public AFreeState(AltarBoss boss, AltarPattern pattern) : base(boss, pattern)
    {
        _lock = false;
        _allThrow = false;
        _once = false;
        _altar = boss;
        _pattern = pattern;
        _rotatingBallRegenTime = 10;
        _curWaitingRegenTime = 0;
    }

    public override void OnBossStateExit()
    {
        _altar.StopCoroutine(RandomPattern(_altar.so.PatternChangeTime));
    }

    public override void OnBossStateOn()
    {
        _altar.SetBodyToBasic(_altar.bigestbody, _altar.bigestBody);
        _altar.SetBodyToBasic(_altar.mediumSizebody, _altar.mediumSizeBody);
        _altar.SetBodyToBasic(_altar.smallestbody, _altar.smallestBody);

        _altar.isStop = false;
        _altar.isBlocked = false;

        _altar.StartCoroutine(Dash(10, 20, 0.5f, 0.5f));
    }

    public override void OnBossStateUpdate()
    {
        if(_allThrow)
        {
            _curWaitingRegenTime += Time.deltaTime;
        }

        if(_curWaitingRegenTime >= _rotatingBallRegenTime)
        {
            _altar.StartCoroutine(RotatingBall(3, 100, _altar.transform, 5, 5, 5, 0.5f));
            _curWaitingRegenTime = 0;
            _allThrow = false;
        }   
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

            if(_lock && !_once)
            {
                _altar.StartCoroutine(Unlock(15));
            }

            yield return new WaitForSeconds(waitTime);

            int rand = Random.Range(1, 5);

            if (rand == 4)
            {
                if (_lock)
                {
                    rand = Random.Range(1, 4);
                }
            }

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
                case 4:
                    _altar.StopImmediately(_altar.transform);
                    _altar.isStop = true;
                    _lock = true;
                    NowCoroutine(_pattern.OmnidirShooting(_altar, 4, 3, 0.2f, 30));
                    break;
            }
        }
    }

    private IEnumerator Unlock(float waitTime)
    {
        _once = true;
        yield return new WaitForSeconds(waitTime);
        _lock = false;
        _once = false;
    }

    private IEnumerator Dash(float maxDistance, float speed, float waitTime, float dashTime)
    {
        SoundManager.Instance.SFXPlay("Dash", _altar.dashClip);
        _altar.isDashing = true;
        float curTime = 0;
        Vector3 dir = (GameManager.Instance.player.transform.position - _altar.transform.position).normalized;

        while (curTime < dashTime && !_altar.isBlocked)
        {
            curTime += Time.deltaTime;

            Vector3 target = _altar.transform.position + dir * maxDistance;
            _altar.transform.position = Vector2.MoveTowards(_altar.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        _altar.isDashing = false;
        _altar.StartCoroutine(RotatingBall(3, 100, _altar.transform, 5, 5, 5, 0.5f));
        _altar.StartCoroutine(_altar.bossMove.BossMovement(_altar.so.StopTime, _altar.so.MoveX, _altar.so.MoveY, _altar.so.Speed, _altar.so.WallCheckRadius));
        _altar.StartCoroutine(RandomPattern(_altar.so.PatternChangeTime));
    }

    private IEnumerator RotatingBall(int bulletCount, float speed, Transform trans, float r, float waitTime, float throwSpeed, float throwWaitTime)
    {
        float deg = 0;
        float curTime = 0;
        GameObject[] bullets = new GameObject[bulletCount];


        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType5, trans);
            bullets[i].GetComponent<BossBullet>().Attack(_altar.so.Damage);
        }

        SoundManager.Instance.SFXPlay("Burn", _altar.burnClip, _altar.bulletCollector.transform, 1);
        while (curTime < waitTime)
        {
            deg += Time.deltaTime * speed;
            curTime += Time.deltaTime;

            if (deg < 360)
            {
                for (int i = 0; i < bullets.Length; i++)
                {
                    var rad = Mathf.Deg2Rad * (deg + i * 360 / bullets.Length);
                    var x = r * Mathf.Cos(rad);
                    var y = r * Mathf.Sin(rad);
                    bullets[i].transform.position = trans.position + new Vector3(x, y);
                }
            }
            else
                deg = 0;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        for (int i = 0; i < bullets.Length; i++)
        {
            Vector2 dir = (GameManager.Instance.player.transform.position - bullets[i].transform.position).normalized;
            bullets[i].transform.SetParent(_altar.altarCollector.transform);
            bullets[i].GetComponent<Rigidbody2D>().velocity = dir * throwSpeed;
            _altar.StartCoroutine(ObjectPool.Instance.ReturnObject(bullets[i], 7));

            yield return new WaitForSeconds(throwWaitTime);
        }

        _allThrow = true;
    }
}
