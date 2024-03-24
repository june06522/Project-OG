using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ATiedState : BossBaseState
{
    private float f_maxMoveDistance;
    
    private float f_movingDelay;

    private AltarBoss _altarBoss;

    private Vector3 v_patrolPos;

    public ATiedState(AltarBoss boss) : base(boss)
    {
        f_maxMoveDistance = 1;
        f_movingDelay = 1;
        v_patrolPos = _boss.transform.position;
        _altarBoss = boss;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _boss.B_isStop = false;
        _altarBoss.B_isTied = true;
        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
        _boss.StartCoroutine(TiedPatorl(f_movingDelay));
    }

    public override void OnBossStateUpdate()
    {
        if(!_altarBoss.B_isTied)
        {
            _boss.StopCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
            StopThisCoroutine();
        }

        if(_boss.B_blocked)
        {
            v_patrolPos = MakeNewPatrolPos();
            _boss.B_blocked = false;
        }
    }

    public IEnumerator RandomPattern(float waitTime)
    {
        if (!_altarBoss.B_isTied)
            yield break;

        yield return new WaitForSeconds(waitTime);

        int rand = Random.Range(1, 4);

        _boss.B_isRunning = true;

        switch (rand)
        {
            case 1:
                NowCoroutine(OmnidirAttack(20, 5, 1, 1));
                break;
            case 2:
                _boss.StopImmediately(_boss.transform);
                _boss.B_isStop = true;
                NowCoroutine(SoundAttack(6, 1));
                break;
            case 3:
                NowCoroutine(OmniGuidPlayerAttack(20, 5, 1, 1));
                break;
        }
    }

    private IEnumerator TiedPatorl(float waitTime)
    {
        while(_altarBoss.B_isTied)
        {
            if (Arrive(_boss.transform.position, v_patrolPos))
            {
                yield return new WaitForSeconds(waitTime);
                v_patrolPos = MakeNewPatrolPos();
            }
            else
            {
                if (_boss.B_isStop)
                {
                    v_patrolPos = _boss.transform.position;
                }
                else
                {
                    _boss.transform.position = Vector2.MoveTowards(_boss.transform.position, v_patrolPos, Time.deltaTime);
                }
            }

            yield return null;
        }
    }

    private bool Arrive(Vector3 myPos, Vector3 targetPos)
    {
        if (Mathf.Abs(Vector3.Distance(myPos, targetPos)) <= 0.5f)
            return true;

        return false;
    }

    private Vector3 MakeNewPatrolPos()
    {
        Vector3 newPatrolPos =  new Vector2(Random.Range(-f_maxMoveDistance, f_maxMoveDistance), Random.Range(-f_maxMoveDistance, f_maxMoveDistance));

        return newPatrolPos;
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
        if (!_altarBoss.B_isTied)
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
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.G_bulletCollector.transform);
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

        _boss.B_isRunning = false;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 전방향으로 탄막을 날리고 잠시 뒤 탄막들이 플레이어 방향으로 날아간다 - 플레이어가 근접하기 좋은 패턴
    private IEnumerator OmniGuidPlayerAttack(int bulletCount, float speed, float time, int burstCount)
    {
        if (!_altarBoss.B_isTied)
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
            _altarBoss.ChangeMat(3);

            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.G_bulletCollector.transform);
                bullets[i, j].GetComponent<BossBullet>().Attack(_boss.bossSo.Damage);
                bullets[i, j].transform.position = _boss.transform.position;
                bullets[i, j].transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time / 2);

            _altarBoss.ChangeMat();

            yield return new WaitForSeconds(time / 2);

            for (int j = 0; j < bulletCount; j++)
            {
                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.zero;
            }

            yield return new WaitForSeconds(Time.deltaTime);

            Vector3 nextDir = GameManager.Instance.player.transform.position;

            for (int j = 0; j < bulletCount; j++)
            {
                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = nextDir - bullets[i, j].transform.position;
                rigid.velocity = dir.normalized * speed * 2;
            }
        }

        yield return new WaitForSeconds(time);

        _boss.B_isRunning = false;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 범위안에 플레이어가 있으면 피해를 준다 - 플레이어가 멀어져야 좋은 패턴
    private IEnumerator SoundAttack(int radius, float waitTime)
    {
        if (!_altarBoss.B_isTied)
            yield break;

        GameObject warning = ObjectPool.Instance.GetObject(ObjectPoolType.WarningType1, _boss.G_bulletCollector.transform);
        warning.transform.localScale = warning.transform.localScale * radius * 2;
        warning.transform.position = _boss.transform.position;
        warning.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(waitTime);

        _boss.StartCoroutine(CameraManager.Instance.CameraShake(1, 0.5f));

        ObjectPool.Instance.ReturnObject(ObjectPoolType.WarningType1, warning);

        GameObject p = CheckPlayerCircleCastG(radius);

        if (p)
        {
            if (p.TryGetComponent<IHitAble>(out var IhitAble))
            {
                IhitAble.Hit(_boss.bossSo.Damage);
            }
        }

        yield return new WaitForSeconds(0.5f);

        _boss.B_isStop = false;
        _boss.B_isRunning = false;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }
}
