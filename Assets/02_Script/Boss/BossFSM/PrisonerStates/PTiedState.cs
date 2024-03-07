using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTiedState : BossBaseState
{
    public PTiedState(Boss boss) : base(boss)
    {
        _willChange = false;
    }

    public override void OnBossStateExit()
    {
        _willChange = true;
        _boss.StopCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    public override void OnBossStateOn()
    {
        _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    public override void OnBossStateUpdate()
    {

    }

    public override IEnumerator RandomPattern(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        int rand = Random.Range(1, 4);

        switch (rand)
        {
            case 1:
                _boss.StartCoroutine(OmnidirAttack(20, 3, 1, 3, 1));
                break;
            case 2:
                _boss.StartCoroutine(SoundAttack(3, 1));
                break;
            case 3:
                _boss.StartCoroutine(OmniGuidPlayerAttack(20, 3, 1, 3, 1, 1));
                break;
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

    // 전방향으로 공격한다
    private IEnumerator OmnidirAttack(int bulletCount, float speed, float time, float returnTime, int burstCount)
    {
        GameObject[,] bullets = new GameObject[burstCount, bulletCount];
        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.transform);
                bullets[i, j].GetComponent<BossBullet>().Attack(_boss.bossSo.Damage);
                bullets[i, j].transform.position = _boss.transform.position;
                bullets[i, j].transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(time);

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
                _boss.StartCoroutine(ObjectPool.Instance.ReturnObject(returnTime, ObjectPoolType.BossBulletType0, bullets[i, j]));

            yield return new WaitForSeconds(time);
        }

        if (!_willChange)
            _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 범위안에 플레이어가 있으면 피해를 준다
    private IEnumerator SoundAttack(int radius, float waitTime)
    {
        GameObject warning = ObjectPool.Instance.GetObject(ObjectPoolType.WarningType1, _boss.transform);
        warning.transform.localScale = warning.transform.localScale * radius * 2;
        warning.transform.position = _boss.transform.position;
        warning.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(waitTime);

        ObjectPool.Instance.ReturnObject(ObjectPoolType.WarningType1, warning);

        GameObject p = CheckPlayerCircleCastG(radius);

        if (p)
        {
            if (p.TryGetComponent<IHitAble>(out var IhitAble))
            {
                IhitAble.Hit(_boss.bossSo.Damage);
            }
        }

        if (!_willChange)
            _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 전방향으로 탄막을 날리고 잠시 뒤 탄막들이 플레이어 방향으로 날아간다
    private IEnumerator OmniGuidPlayerAttack(int bulletCount, float speed, float time, float returnTime, int returnCount, int burstCount)
    {
        GameObject[,] bullets = new GameObject[burstCount, bulletCount];

        int returnCounting = 0;

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.transform);
                bullets[i, j].GetComponent<BossBullet>().Attack(_boss.bossSo.Damage);
                bullets[i, j].transform.position = _boss.transform.position;
                bullets[i, j].transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time);

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

            if (i >= returnCount)
            {
                for (int j = 0; j < bulletCount; j++)
                {
                    _boss.StartCoroutine(ObjectPool.Instance.ReturnObject(returnTime, ObjectPoolType.BossBulletType0, bullets[returnCounting, j]));
                }

                returnCounting++;
            }
        }

        yield return new WaitForSeconds(time);

        for (int i = returnCounting; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                _boss.StartCoroutine(ObjectPool.Instance.ReturnObject(returnTime, ObjectPoolType.BossBulletType0, bullets[i, j]));
            }

        }

        if (!_willChange)
            _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }
}
