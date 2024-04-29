using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarBoss : Boss
{
    public GameObject altarCollector;
    public GameObject bigestBody;
    public GameObject mediumSizeBody;
    public GameObject smallestBody;

    public AudioClip fireClip;
    public AudioClip deadClip;
    public AudioClip burnClip;
    public AudioClip dashClip;
    public AudioClip unChainClip;

    public Sprite bigTriangleSprite;

    public bool isTied;
    public bool isOneBroken;
    public bool isDashing;
    public bool isIdleEnded;

    public Vector3 originPos;

    private BossState _curBossState;

    private BossFSM _bossFSM;

    private AltarPattern _pattern;

    private GameObject[] _restraints = new GameObject[2];

    private GameObject[,] _chains = new GameObject[2, 5];

    private int _restraintIndex = 0;
    private int _restrainCount = 0;
    private int _chainCount = 0;
    private int _shortenChainIndex = 0;

    [SerializeField]
    private float _restraintDistance;
    [SerializeField]
    private float _unChainTime;
    private float _currentTime = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
        _pattern = GetComponent<AltarPattern>();
        bossMove = GetComponent<BossMove>();
        gameObject.layer = LayerMask.NameToLayer("Boss");
        _currentTime = 0;
        _restraintIndex = 0;
        _restrainCount = _restraints.Length;
        _chainCount = _chains.GetLength(1);
        _shortenChainIndex = 0;
        isTied = true;
        isOneBroken = false;
        isDashing = false;
        isIdleEnded = false;
        originPos = transform.position;

        ChainSetting();

        _curBossState = BossState.Idle;
        _bossFSM = new BossFSM(new AIdleState(this, _pattern));

        StartCoroutine(ShortenChain());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();

        if (isIdleEnded)
        {
            ChangeBossState(BossState.Tied);
        }

        if(_curBossState != BossState.Idle)
        {
            if (!isDead)
            {
                ChangeState();
            }

            if (!IsDie)
            {
                if (_restraintIndex < _restrainCount)
                {
                    TimeChecker(Time.deltaTime * (_restraintIndex + 1));
                }

                ChainsFollowBoss();

                _bossFSM.UpdateBossState();
            }
        }
    }

    public void ChainReturnAll()
    {
        int childCount = 0;
        GameObject[] objs;

        childCount = altarCollector.transform.childCount;
        objs = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            objs[i] = altarCollector.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < childCount; i++)
        {
            if (objs[i] == null)
                continue;
            ObjectPool.Instance.ReturnObject(objs[i]);
        }
    }

    private void ChainSetting()
    {
        for (int i = 0; i < _restrainCount; i++)
        {
            _restraints[i] = ObjectPool.Instance.GetObject(ObjectPoolType.AltarRestraint, altarCollector.transform);
            var rad = Mathf.Deg2Rad * i * 360 / _restrainCount;
            var x = _restraintDistance * Mathf.Cos(rad);
            var y = _restraintDistance * Mathf.Sin(rad);
            _restraints[i].transform.position = transform.GetChild(i).position + new Vector3(x, y, 0);
            _restraints[i].transform.rotation = Quaternion.identity;
            for (int j = 1; j <= _chainCount; j++)
            {
                var xx = j * _restraintDistance / _chainCount * Mathf.Cos(rad);
                var yy = j * _restraintDistance / _chainCount * Mathf.Sin(rad);
                _chains[i, j - 1] = ObjectPool.Instance.GetObject(ObjectPoolType.AltarChain, _restraints[i].transform.GetChild(0).transform);
                _chains[i, j - 1].transform.position = transform.GetChild(i).position + new Vector3(xx, yy, 0);
                _chains[i, j - 1].transform.rotation = Quaternion.identity;
            }
        }
    }

    private void ChainsFollowBoss()
    {
        for (int i = 0; i < _restrainCount; i++)
        {
            float angle = Mathf.Atan2(transform.position.y - _restraints[i].transform.position.y, transform.position.x - _restraints[i].transform.position.x) * Mathf.Rad2Deg;

            _restraints[i].transform.GetChild(0).rotation = Quaternion.AngleAxis(angle + 180 - i * 180, Vector3.forward);
        }
    }

    private IEnumerator ShortenChain()
    {
        while(_restraintIndex < 2)
        {
            if (_restraintIndex > 0)
            {
                if (Vector3.Distance(transform.position, _chains[_restraintIndex, _shortenChainIndex].transform.position) < 0.5f)
                {
                    if (_shortenChainIndex < _chainCount - 1)
                    {
                        _chains[_restraintIndex, _shortenChainIndex].gameObject.SetActive(false);
                        _shortenChainIndex++;
                    }
                    yield return null;
                }
                else if (Vector3.Distance(transform.position, _chains[_restraintIndex, _shortenChainIndex].transform.position) > 0.5f)
                {
                    if (_shortenChainIndex > 0)
                    {
                        _chains[_restraintIndex, _shortenChainIndex].gameObject.SetActive(true);
                        _shortenChainIndex--;
                    }
                    yield return null;
                }
            }

            yield return null;
        }
        
    }

    private void ChangeState()
    {
        if(IsDie)
        {
            if(_restraintIndex < 2)
                ReturnRestraintAndChains();
            CameraManager.Instance.StopCameraShake();
            isTied = false;
            isOneBroken = false;
            isDead = true;
            ChangeBossState(BossState.Dead);
        }
        else
        {
            if (!isAttacking)
            {
                switch (_curBossState)
                {
                    case BossState.Tied:
                        if (_restraintIndex > 0)
                        {
                            ChangeBossState(BossState.OneBroken);
                        }
                        break;
                    case BossState.OneBroken:
                        if (_restraintIndex > 1)
                        {
                            ChangeBossState(BossState.Free);
                        }
                        break;
                }
            }
        }
    }

    private void ChangeBossState(BossState nextBossState)
    {
        _curBossState = nextBossState;

        switch (_curBossState)
        {
            case BossState.Idle:
                Debug.Log(_bossFSM);
                _bossFSM.ChangeBossState(new AIdleState(this, _pattern));
                break;
            case BossState.Tied:
                _bossFSM.ChangeBossState(new ATiedState(this, _pattern));
                break;
            case BossState.OneBroken:
                _bossFSM.ChangeBossState(new AOneBrokenState(this, _pattern));
                break;
            case BossState.Free:
                _bossFSM.ChangeBossState(new AFreeState(this, _pattern));
                break;
            case BossState.Dead:
                _bossFSM.ChangeBossState(new ABossDeadState(this, _pattern));
                break;
        }
    }

    private void TimeChecker(float time)
    {
        if (_currentTime <= 0)
            _currentTime = 0;

        if (_currentTime >= _unChainTime - 0.5f && _currentTime < _unChainTime)
        {
            isTied = false;
            isOneBroken = false;
        }

        if (_currentTime <= _unChainTime)
            _currentTime += time;
        else
        {
            SoundManager.Instance.SFXPlay("UnChain", unChainClip, 1);
            CameraManager.Instance.CameraShake(5, 1f);
            StartCoroutine(UnChain(3));
            _currentTime = 0;
        }
    }

    private IEnumerator UnChain(float speed)
    {
        for (int i = 0; i < _chainCount; i++)
        {
            GameObject chainFragment = ObjectPool.Instance.GetObject(ObjectPoolType.ChainFragment, altarCollector.transform);
            chainFragment.GetComponent<BossBullet>().Attack(so.Damage);
            chainFragment.transform.position = _chains[_restraintIndex, i].transform.position;
            chainFragment.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = chainFragment.GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * UnityEngine.Random.Range(0, 361) / 360), Mathf.Sin(Mathf.PI * 2 * UnityEngine.Random.Range(0, 361) / 360));
            rigid.velocity = dir.normalized * speed;
        }
        for(int i = 0; i < _chainCount; i++)
        {
            GameObject split = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, altarCollector.transform);
            split.GetComponent<BossBullet>().Attack(so.Damage);
            split.transform.position = _restraints[_restraintIndex].transform.position;
            split.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = split.GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / _chainCount), Mathf.Sin(Mathf.PI * 2 * i / _chainCount));
            rigid.velocity = dir.normalized * speed;
        }

        ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarRestraint, _restraints[_restraintIndex]);
        for (int i = 0; i < _chainCount; i++)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarChain, _chains[_restraintIndex, i]);
        }

        _restraintIndex++;
        if(_restraintIndex == 1)
        {
            originPos = _restraints[_restraintIndex].transform.localPosition;
        }

        yield return null;
    }

    private void ReturnRestraintAndChains()
    {
        if(_restraintIndex > 0)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarRestraint, _restraints[_restraintIndex]);
            for (int j = 0; j < _chainCount; j++)
            {
                ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarChain, _chains[_restraintIndex, j]);
            }
        }
        else
        {
            for (int i = 0; i < _restrainCount; i++)
            {
                ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarRestraint, _restraints[i]);
                for (int j = 0; j < _chainCount; j++)
                {
                    ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarChain, _chains[i, j]);
                }
            }
        }
        
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "Player")
        {
            if(isDashing)
            {
                if (collision.gameObject.TryGetComponent<IHitAble>(out var IhitAble))
                {
                    IhitAble.Hit(so.Damage);
                }
            }
        }
    }

}
