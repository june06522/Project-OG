using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AOneBrokenState : BossBaseState
{
    private float f_maxMoveDistance;
    private float f_speed;
    private AltarBoss _altarBoss;
    public AOneBrokenState(AltarBoss boss) : base(boss)
    {
        f_maxMoveDistance = 5;
        f_speed = 2;
        _altarBoss = boss;
    }

    public override void OnBossStateExit()
    {

    }

    public override void OnBossStateOn()
    {
        _boss.B_isStop = false;
        _altarBoss.B_isOneBroken = true;
        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
        _boss.StartCoroutine(OneBrokenMove());
    }

    public override void OnBossStateUpdate()
    {
        if(!_altarBoss.B_isOneBroken)
        {
            _boss.StopCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
            StopThisCoroutine();
        }
        if (_boss.B_blocked)
        {
            _boss.B_blocked = false;
        }
    }

    public IEnumerator RandomPattern(float waitTime)
    {
        if (!_altarBoss.B_isOneBroken)
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
                NowCoroutine(OmniGuidPlayerAttack(20, 5, 1, 1));
                break;
            case 3:
                NowCoroutine(ThrowEnergyBall(3, 10, 1, 2));
                break;
        }
    }

    private IEnumerator OneBrokenMove()
    {
        while(_altarBoss.B_isOneBroken)
        {
            if(!_boss.B_isStop)
            {
                if (Vector3.Distance(_boss.transform.localPosition, _boss.V_originPos) < f_maxMoveDistance)
                {
                    Vector3 dir = (GameManager.Instance.player.transform.position - _boss.transform.position).normalized;

                    _boss.transform.localPosition = Vector2.MoveTowards(_boss.transform.localPosition, _boss.transform.localPosition + dir * f_maxMoveDistance, Time.deltaTime * f_speed);
                }
                else
                {
                    Vector3 dir = (_boss.V_originPos - _boss.transform.localPosition).normalized;

                    _boss.transform.localPosition = Vector2.MoveTowards(_boss.transform.localPosition, _boss.transform.localPosition + dir, Time.deltaTime * f_speed);
                }
            }

            yield return null;
        }
    }

    // 전방향으로 공격한다 - 플레이어가 근접하기 좋은 패턴
    private IEnumerator OmnidirAttack(int bulletCount, float speed, float time, int burstCount)
    {
        if (!_altarBoss.B_isOneBroken)
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
        if (!_altarBoss.B_isOneBroken)
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

    // 플레이어 방향으로 에너지 볼을 던진다 - 플레이어가 근접하기 좋은 패턴
    private IEnumerator ThrowEnergyBall(int burstCount, float speed, float waitTime, float returnTime)
    {
        if (!_altarBoss.B_isOneBroken)
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

            GameObject energyBall = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.G_bulletCollector.transform);
            energyBall.GetComponent<BossBullet>().Attack(_boss.bossSo.Damage);
            energyBall.transform.localScale *= 2;
            energyBall.transform.position = new Vector3(_boss.transform.position.x, _boss.transform.position.y + 0.5f, _boss.transform.position.z);
            energyBall.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = energyBall.GetComponent<Rigidbody2D>();
            Vector2 dir = GameManager.Instance.player.transform.position - energyBall.transform.position;
            rigid.velocity = dir.normalized * speed;

            yield return new WaitForSeconds(waitTime);
        }

        _boss.B_isRunning = false;

        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }
}
