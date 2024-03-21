using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// 플레이어를 따라가다가 플레이어가 벽 쪽에 붙으면 좀 문제가 생김
public class PFreeState : BossBaseState
{
    private bool _wasHealing;
    private bool _nowMove;

    public PFreeState(Boss boss) : base(boss)
    {
        _wasHealing = false;
        _nowMove = false;
    }

    public override void OnBossStateExit()
    {
        _boss.StopCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    public override void OnBossStateOn()
    {
        _boss.ReturnAll(true);
        _boss.isStop = false;
        _boss.blocked = false;
        _boss.StartCoroutine(Dash(10, 20, 0.5f, 0.5f));
    }

    public override void OnBossStateUpdate()
    {
        if(!_boss.isStop && !_boss.blocked && _nowMove)
        {
            if (!DontNeedToFollow())
            {
                _boss.transform.position = Vector2.MoveTowards(_boss.transform.position, _boss.player.transform.position, Time.deltaTime);
            }
            else
            {
                _boss.StopImmediately(_boss.transform);
            }
        }
            
    }

    public IEnumerator RandomPattern(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        int rand = Random.Range(1, 7);

        if(rand == 6)
        {
            if(_wasHealing)
            {
                rand = Random.Range(1, 6);
                _wasHealing = false;
            }
        }

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
                _boss.StopImmediately(_boss.transform);
                _boss.isStop = true;
                NowCoroutine(OmnidirShooting(4, 2, 0.2f, 50));
                break;
            case 5:
                NowCoroutine(ThrowEnergyBall(3, 10, 1, 2));
                break;
            case 6:
                _wasHealing = true;
                _boss.StopImmediately(_boss.transform);
                _boss.isStop = true;
                NowCoroutine(Buff(5, 150));
                break;
        }
    }

    private bool DontNeedToFollow()
    {
        if (CheckPlayerCircleCastB(_boss.bossSo.StopRadius))
        {
            return true;
        }

        return false;
    }

    private bool CheckPlayerCircleCastB(float radius)
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(_boss.transform.position, radius, Vector2.zero);
        foreach (var h in hit)
        {
            if (h.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }

        return false;
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

    // 풀린 즉시 한 번만 하는 패턴
    private IEnumerator Dash(float maxDistance, float speed, float waitTime, float dashTime)
    {
        float curTime = 0;
        Vector2 dir = (_boss.player.transform.position - _boss.transform.position).normalized;

        while(curTime < dashTime)
        {
            curTime += Time.deltaTime;
            if(_boss.blocked)
            {
                _boss.blocked = false;
                break;
            }
            _boss.transform.position = Vector2.MoveTowards(_boss.transform.position, dir * maxDistance, speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        _nowMove = true;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime * 2));
    }

    // 전방향으로 공격한다 - 플레이어가 근접하기 좋은 패턴
    private IEnumerator OmnidirAttack(int bulletCount, float speed, float time, int burstCount)
    {
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

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 전방향으로 탄막을 날리고 잠시 뒤 탄막들이 플레이어 방향으로 날아간다 - 플레이어가 근접하기 좋은 패턴
    private IEnumerator OmniGuidPlayerAttack(int bulletCount, float speed, float time, int burstCount)
    {
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

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 플레이어 방향으로 에너지 볼을 던진다 - 플레이어가 근접하기 좋은 패턴
    private IEnumerator ThrowEnergyBall(int burstCount, float speed, float waitTime, float returnTime)
    {
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

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 버프기 - 체력 회복 및 공격력 영구 증가
    private IEnumerator Buff(int division, float speed)
    {
        _boss.bossSo.Damage += 2;

        int heal = (int)(_boss.bossSo.MaxHP / division);
        float currentHeal = 0;

        if (_boss.currentHp < _boss.bossSo.MaxHP - heal)
        {
            ChangeMat(1);

            while (currentHeal < heal)
            {
                currentHeal += Time.deltaTime * speed;
                _boss.currentHp += Time.deltaTime * speed;

                yield return null;
            }

            ChangeMat();

            _boss.isStop = false;
            _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
        }
        else
        {
            _wasHealing = false;
            _boss.isStop = false;
            _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
        }
        
    }

    // 범위안에 플레이어가 있으면 피해를 준다 - 플레이어가 멀어져야 좋은 패턴
    private IEnumerator SoundAttack(int radius, float waitTime)
    {
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

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 총알을 전 방향으로 난사한다 - 플레이어가 멀어져야 좋은 패턴
    private IEnumerator OmnidirShooting(int bulletCount, float speed, float time, int turnCount)
    {
        ChangeMat(2);

        for (int i = 0; i < turnCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.bulletCollector.transform);
                bullet.GetComponent<BossBullet>().Attack(_boss.bossSo.Damage);
                bullet.transform.position = _boss.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount + i * 2), Mathf.Sin(Mathf.PI * 2 * j / bulletCount + i * 2));
                rigid.velocity = dir.normalized * speed;
            }
            yield return new WaitForSeconds(time);
        }

        ChangeMat();
        _boss.isStop = false;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }
}
