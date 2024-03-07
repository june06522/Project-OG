using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerBoss : Boss
{
    // 새 패턴 하나 만들고,

    private enum BossState
    {
        Idle,
        Tied,
        OneBroken,
        TwoBroken,
        ThreeBroken,
        Free
    }

    private BossState _curBossState;

    private BossFSM _bossFSM;

    private GameObject[] _restraints = new GameObject[4];

    private GameObject[,] _chains = new GameObject[4, 10];

    private int _restraintIndex = 0;
    private int _restrainCount = 0;
    private int _chainCount = 0;

    [SerializeField]
    private float _restraintDistance;
    [SerializeField]
    private float _unChainTime;
    private float _currentTime = 0;

    void Start()
    {
        _currentTime = 0;
        _restraintIndex = 0;
        _restrainCount = _restraints.Length;
        _chainCount = _chains.GetLength(1);

        for(int i = 0; i < _restrainCount; i++)
        {
            _restraints[i] = ObjectPool.Instance.GetObject(ObjectPoolType.PrisonerRestraint);
            var rad = Mathf.Deg2Rad * i * 360 / _restrainCount;
            var x = _restraintDistance * Mathf.Cos(rad);
            var y = _restraintDistance * Mathf.Sin(rad);
            _restraints[i].transform.position = transform.position + new Vector3(x, y, 0);
            _restraints[i].transform.rotation = Quaternion.identity;
            for(int j = 0; j < _chainCount; j++)
            {
                var xx = j * _restraintDistance / 10 * Mathf.Cos(rad); 
                var yy = j * _restraintDistance / 10 * Mathf.Sin(rad);
                _chains[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.PrisonerChain);
                if (j % 2 == 0)
                    _chains[i, j].GetComponent<SpriteRenderer>().color = Color.grey;
                else
                    _chains[i, j].GetComponent<SpriteRenderer>().color = Color.black;
                _chains[i, j].transform.position = transform.position + new Vector3(xx, yy, 0);
                _chains[i, j].transform.rotation = Quaternion.identity;
            }
        }

        _curBossState = BossState.Idle;
        _bossFSM = new BossFSM(new BossIdleState(this));

        ChangeBossState(BossState.Tied);

        StartCoroutine(ChainShake(0.3f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, bossSo.StopRadius);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    protected override void Update()
    {
        base.Update();

        if(_restraintIndex < _restrainCount)
        {
            TimeChecker(Time.deltaTime * (_restraintIndex + 1));
            HpChecker();
        }

        ChangeState();

        _bossFSM.UpdateBossState();
    }

    private void ChangeState()
    {
        switch (_curBossState)
        {
            case BossState.Tied:
                if (_restraintIndex > 0)
                    ChangeBossState(BossState.OneBroken);
                break;
            case BossState.OneBroken:
                if (_restraintIndex > 1)
                    ChangeBossState(BossState.TwoBroken);
                break;
            case BossState.TwoBroken:
                if (_restraintIndex > 2)
                    ChangeBossState(BossState.ThreeBroken);
                break;
            case BossState.ThreeBroken:
                if (_restraintIndex > _restrainCount - 1)
                    ChangeBossState(BossState.Free);
                break;
        }
    }

    private void ChangeBossState(BossState nextBossState)
    {
        _curBossState = nextBossState;

        switch (_curBossState)
        {
            case BossState.Idle:
                _bossFSM.ChangeBossState(new BossIdleState(this));
                break;
            case BossState.Tied:
                _bossFSM.ChangeBossState(new PTiedState(this));
                break;
            case BossState.OneBroken:
                _bossFSM.ChangeBossState(new POneBrokenState(this));
                break;
            case BossState.TwoBroken:
                _bossFSM.ChangeBossState(new PTwoBrokenState(this));
                break;
            case BossState.ThreeBroken:
                _bossFSM.ChangeBossState(new PThreeBrokenState(this));
                break;
            case BossState.Free:
                _bossFSM.ChangeBossState(new PFreeState(this));
                break;
        }
    }

    private void HpChecker()
    {
        if(_currentTime < _unChainTime)
        {
            if (currentHp <= bossSo.MaxHP - bossSo.MaxHP / _restrainCount * (_restraintIndex + 1))
            {
                StartCoroutine(UnChain(8, 3, 2));
                _currentTime = 0;
            }
        }
    }

    private void TimeChecker(float time)
    {
        if (_currentTime <= 0)
            _currentTime = 0;

        if (_currentTime <= _unChainTime)
            _currentTime += time;
        else
        {
            StartCoroutine(UnChain(8, 3, 2));
            _currentTime = 0;
        }

        if (_hit)
        {
            _currentTime -= time;
            _hit = false;
        }
    }

    private IEnumerator ChainShake(float speed)
    {
        while(_restraintIndex < _restrainCount)
        {
            int temp = 0;
            if (_restraintIndex == _restrainCount)
                temp = _restraintIndex - 1;
            else
                temp = _restraintIndex;
            for (int i = 0; i < _chainCount; i++)
            {
                Rigidbody2D rigid = _chains[temp, i].GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * temp / _restrainCount), Mathf.Sin(Mathf.PI * 2 * temp / _restrainCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return null;

            for (int i = 0; i < _chainCount; i++)
            {
                Rigidbody2D rigid = _chains[temp, i].GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * temp / _restrainCount), Mathf.Sin(Mathf.PI * 2 * temp / _restrainCount));
                rigid.velocity = -dir.normalized * speed;
            }

            yield return null;
        }
        
    }

    private IEnumerator UnChain(int splitCount, float speed, float returnTime)
    {
        GameObject[] chains = new GameObject[_chainCount];
        GameObject[] splits = new GameObject[splitCount];

        for (int i = 0; i < _chainCount; i++)
        {
            chains[i] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, transform);
            chains[i].GetComponent<BossBullet>().Attack(bossSo.Damage);
            chains[i].transform.position = _chains[_restraintIndex, i].transform.position;
            chains[i].transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = chains[i].GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * UnityEngine.Random.Range(0, 361) / 360), Mathf.Sin(Mathf.PI * 2 * UnityEngine.Random.Range(0, 361) / 360));
            rigid.velocity = dir.normalized * speed;
        }
        for(int i = 0; i < splitCount; i++)
        {
            splits[i] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, transform);
            splits[i].GetComponent<BossBullet>().Attack(bossSo.Damage);
            splits[i].transform.position = _restraints[_restraintIndex].transform.position;
            splits[i].transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = splits[i].GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / splitCount), Mathf.Sin(Mathf.PI * 2 * i / splitCount));
            rigid.velocity = dir.normalized * speed;
        }

        ObjectPool.Instance.ReturnObject(ObjectPoolType.PrisonerRestraint, _restraints[_restraintIndex]);
        for(int i = 0; i < _chainCount; i++)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.PrisonerChain, _chains[_restraintIndex, i]);
        }

        _restraintIndex++;

        if (_restraintIndex >= _restrainCount - 1)
        {
            StopCoroutine(ChainShake(0));
        }

        yield return new WaitForSeconds(returnTime);

        for(int i = 0; i < _chainCount; i++)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.BossBulletType0, chains[i]);
        }
        for(int i = 0; i < splitCount; i++)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.BossBulletType0, splits[i]);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

}
