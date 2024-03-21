using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class POneBrokenState : BossBaseState
{
    private float _maxMoveDistance;
    private float _speed;

    public POneBrokenState(Boss boss) : base(boss)
    {
        _maxMoveDistance = 5;
        _speed = 2;
    }

    public override void OnBossStateExit()
    {
        _boss.StopCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
        StopThisCoroutine();
    }

    public override void OnBossStateOn()
    {
        _boss.isStop = false;
        _boss.ReturnAll(true);
        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime * 2));
        _boss.StartCoroutine(OneBrokenMove());
    }

    public override void OnBossStateUpdate()
    {
        if (_boss.blocked)
        {
            _boss.blocked = false;
        }
    }

    public IEnumerator RandomPattern(float waitTime)
    {
        if (!_boss.isOneBroken)
            yield break;

        yield return new WaitForSeconds(waitTime);

        int rand = Random.Range(1, 5);

        _boss.isRunning = true;

        switch (rand)
        {
            case 1:
                NowCoroutine(OmnidirAttack(20, 5, 1, 1));
                break;
            case 2:
                _boss.StopImmediately(_boss.transform);
                _boss.isStop = true;
                NowCoroutine(SoundAttack(4, 1));
                break;
            case 3:
                NowCoroutine(OmniGuidPlayerAttack(20, 5, 1, 1));
                break;
            case 4:
                NowCoroutine(ThrowEnergyBall(3, 10, 1, 2));
                break;
        }
    }

    private IEnumerator OneBrokenMove()
    {
        while(_boss.isOneBroken)
        {
            if(!_boss.isStop)
            {
                if (Vector3.Distance(_boss.transform.position, _boss.originPos) < _maxMoveDistance)
                {
                    Vector3 dir = (_boss.player.transform.position - _boss.transform.position).normalized;

                    _boss.transform.position = Vector2.MoveTowards(_boss.transform.position, _boss.transform.position + dir * _maxMoveDistance, Time.deltaTime * _speed);
                }
                else
                {
                    Vector3 dir = (_boss.originPos - _boss.transform.position).normalized;

                    _boss.transform.position = Vector2.MoveTowards(_boss.transform.position, _boss.transform.position + dir, Time.deltaTime * _speed);
                }
            }

            yield return null;
        }
    }

    private GameObject CheckPlayerCircleCastG(float radius)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(_boss.transform.position, radius, Vector2.zero);
        foreach (var h in hit)
        {
            if (h.collider.gameObject.tag == "Player")
            {
                return h.collider.gameObject;
            }
        }

        return null;
    }

    // 전방향으로 공격한다 - 플레이어가 근접하기 좋은 패턴
    private IEnumerator OmnidirAttack(int bulletCount, float speed, float time, int burstCount)
    {
        if (!_boss.isOneBroken)
            yield break;

        Vector3 originSize = _boss.transform.localScale;

        _boss.transform.DOScale(originSize * 1.1f, 0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _boss.transform.DOScale(originSize, 0.2f)
                .SetEase(Ease.OutQuad);
            });

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.bulletCollector.transform);
                bullet.GetComponent<BossBullet>().Attack(_boss.bossSo.Damage);
                bullet.transform.position = _boss.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(time);

        _boss.isRunning = false;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 전방향으로 탄막을 날리고 잠시 뒤 탄막들이 플레이어 방향으로 날아간다 - 플레이어가 근접하기 좋은 패턴
    private IEnumerator OmniGuidPlayerAttack(int bulletCount, float speed, float time, int burstCount)
    {
        if (!_boss.isOneBroken)
            yield break;

        Vector3 originSize = _boss.transform.localScale;
        GameObject[,] bullets = new GameObject[burstCount, bulletCount];

        _boss.transform.DOScale(originSize * 1.1f, 0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _boss.transform.DOScale(originSize, 0.2f)
                .SetEase(Ease.OutQuad);
            });

        for (int i = 0; i < burstCount; i++)
        {
            ChangeMat(3);

            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.bulletCollector.transform);
                bullets[i, j].GetComponent<BossBullet>().Attack(_boss.bossSo.Damage);
                bullets[i, j].transform.position = _boss.transform.position;
                bullets[i, j].transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time / 2);

            ChangeMat();

            yield return new WaitForSeconds(time / 2);

            for (int j = 0; j < bulletCount; j++)
            {
                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.zero;
            }

            yield return new WaitForSeconds(Time.deltaTime);

            Vector3 nextDir = _boss.player.transform.position;

            for (int j = 0; j < bulletCount; j++)
            {
                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = nextDir - bullets[i, j].transform.position;
                rigid.velocity = dir.normalized * speed * 2;
            }
        }

        yield return new WaitForSeconds(time);

        _boss.isRunning = false;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 플레이어 방향으로 에너지 볼을 던진다 - 플레이어가 근접하기 좋은 패턴
    private IEnumerator ThrowEnergyBall(int burstCount, float speed, float waitTime, float returnTime)
    {
        if (!_boss.isOneBroken)
            yield break;

        Vector3 originSize = _boss.transform.localScale;

        for (int i = 0; i < burstCount; i++)
        {
            _boss.transform.DOScale(originSize * 1.1f, 0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _boss.transform.DOScale(originSize, 0.2f)
                .SetEase(Ease.OutQuad);
            });

            GameObject energyBall = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.bulletCollector.transform);
            energyBall.GetComponent<BossBullet>().Attack(_boss.bossSo.Damage);
            energyBall.transform.localScale *= 2;
            energyBall.transform.position = new Vector3(_boss.transform.position.x, _boss.transform.position.y + 0.5f, _boss.transform.position.z);
            energyBall.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = energyBall.GetComponent<Rigidbody2D>();
            Vector2 dir = _boss.player.transform.position - energyBall.transform.position;
            rigid.velocity = dir.normalized * speed;

            yield return new WaitForSeconds(waitTime);
        }

        _boss.isRunning = false;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 범위안에 플레이어가 있으면 피해를 준다 - 플레이어가 멀어져야 좋은 패턴
    private IEnumerator SoundAttack(int radius, float waitTime)
    {
        if (!_boss.isOneBroken)
            yield break;

        GameObject warning = ObjectPool.Instance.GetObject(ObjectPoolType.WarningType1, _boss.bulletCollector.transform);
        warning.transform.localScale = warning.transform.localScale * radius * 2;
        warning.transform.position = _boss.transform.position;
        warning.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(waitTime);

        _boss.StartCoroutine(CameraManager.Instance.CameraShake(1, 0.5f));

        ObjectPool.Instance.ReturnObject(ObjectPoolType.WarningType1, warning);

        GameObject p = CheckPlayerCircleCastG(radius);

        if (p)
        {
            Debug.Log("데미지 줌");
            if (p.TryGetComponent<IHitAble>(out var IhitAble))
            {
                IhitAble.Hit(_boss.bossSo.Damage);
            }
        }

        yield return new WaitForSeconds(0.5f);

        _boss.isStop = false;
        _boss.isRunning = false;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }
}
