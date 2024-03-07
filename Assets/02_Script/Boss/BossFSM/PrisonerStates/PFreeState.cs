using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFreeState : BossBaseState
{
    private bool _isDash;

    private bool _healing;
    public PFreeState(Boss boss) : base(boss)
    {
        _healing = false;
        _isDash = true;
        _willChange = false;
    }

    public override void OnBossStateExit()
    {
        _willChange = true;
        _boss.StopCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    public override void OnBossStateOn()
    {
        _boss.StartCoroutine(Dash(5, 1));
    }

    public override void OnBossStateUpdate()
    {
        if(!_isDash && !_boss.isStop)
        {
            if (!DontNeedToFollow())
            {
                Vector2 dir = _boss.player.transform.position - _boss.transform.position;

                _boss.rigid.velocity = dir.normalized * _boss.bossSo.Speed;
            }
            else
            {
                if(_boss.rigid.velocity != Vector2.zero)
                    _boss.StopImmediately(_boss.rigid);
            }
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

    public override IEnumerator RandomPattern(float waitTime)
    {
        Debug.Log("Free");

        yield return new WaitForSeconds(waitTime);


        int rand = Random.Range(1, 6);

        switch (rand)
        {
            case 1:
                _boss.StartCoroutine(OmnidirAttack(20, 3, 1, 1));
                break;
            case 2:
                _boss.StopImmediately(_boss.rigid);
                _boss.isStop = true;
                _boss.StartCoroutine(SoundAttack(3, 1));
                break;
            case 3:
                _boss.StartCoroutine(OmniGuidPlayerAttack(20, 3, 1, 1, 1));
                break;
            case 4:
                _boss.StartCoroutine(ThrowEnergyBall(3, 4, 2));
                break;
            case 5:
                _healing = true;
                _boss.StopImmediately(_boss.rigid);
                _boss.isStop = true;
                _boss.StartCoroutine(Buff(4, 5));
                break;
        }
    }

    private IEnumerator Dash(float speed, float waitTime)
    {
        Vector2 dir = _boss.player.transform.position - _boss.transform.position;
        _boss.rigid.velocity = dir.normalized * speed;

        yield return new WaitForSeconds(waitTime);

        if(_boss.rigid.velocity != Vector2.zero)
            _boss.StopImmediately(_boss.rigid);
        _isDash = false;
        if (!_willChange)
            _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 전방향으로 공격한다
    private IEnumerator OmnidirAttack(int bulletCount, float speed, float time, int burstCount)
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
                ObjectPool.Instance.ReturnObject(ObjectPoolType.BossBulletType0, bullets[i, j]);

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
            //Debug.Log("데미지 줌");
            if (p.TryGetComponent<IHitAble>(out var IhitAble))
            {
                IhitAble.Hit(_boss.bossSo.Damage);
            }
        }

        _boss.isStop = false;

        if (!_willChange)
            _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 전방향으로 탄막을 날리고 잠시 뒤 탄막들이 플레이어 방향으로 날아간다
    private IEnumerator OmniGuidPlayerAttack(int bulletCount, float speed, float time, int returnCount, int burstCount)
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
                    ObjectPool.Instance.ReturnObject(ObjectPoolType.BossBulletType0, bullets[returnCounting, j]);
                }

                returnCounting++;
            }
        }

        yield return new WaitForSeconds(time);

        for (int i = returnCounting; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                ObjectPool.Instance.ReturnObject(ObjectPoolType.BossBulletType0, bullets[i, j]);
            }

            yield return new WaitForSeconds(time);
        }
        if (!_willChange)
            _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 플레이어 방향으로 에너지 볼을 던진다
    private IEnumerator ThrowEnergyBall(int burstCount, float speed, float waitTime)
    {
        for (int i = 0; i < burstCount; i++)
        {
            GameObject energyBall = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _boss.transform);
            energyBall.GetComponent<BossBullet>().Attack(_boss.bossSo.Damage);
            energyBall.transform.localScale *= 2;
            energyBall.transform.position = new Vector3(_boss.transform.position.x, _boss.transform.position.y + 0.5f, _boss.transform.position.z);
            energyBall.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = energyBall.GetComponent<Rigidbody2D>();
            Vector2 dir = _boss.player.transform.position - energyBall.transform.position;
            rigid.velocity = dir.normalized * speed;

            yield return new WaitForSeconds(waitTime);

            ObjectPool.Instance.ReturnObject(ObjectPoolType.BossBulletType0, energyBall);
        }
        if (!_willChange)
            _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
    }

    // 버프기
    private IEnumerator Buff(int division, float speed)
    {
        _boss.bossSo.Damage += 2;

        int heal = (int)(_boss.bossSo.MaxHP / division);
        float currentHeal = 0;

        if (_boss.currentHp < _boss.bossSo.MaxHP - heal)
        {
            while (currentHeal < heal)
            {
                currentHeal += Time.deltaTime * speed;
                _boss.currentHp += Time.deltaTime * speed;

                yield return null;
            }

            _healing = false;
            _boss.isStop = false;
            if (!_willChange)
                _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
        }
        else
        {
            _healing = false;
            _boss.isStop = false;
            if (!_willChange)
                _boss.StartCoroutine(RandomPattern(_boss.bossSo.PatternChangeTime));
        }
        
    }
}
