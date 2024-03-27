using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarBoss : Boss
{
    public Material MyMat { get => m_mat; set => m_mat = value; }

    public List<Material> L_mats;

    public GameObject G_altarOnlyCollector;

    private enum BossState
    {
        Idle,
        Tied,
        OneBroken,
        Free,
        Dead
    }

    public bool B_isTied;
    public bool B_isOneBroken;
    public bool B_isDashing;

    private BossState _curBossState;

    private BossFSM _bossFSM;

    private GameObject[] g_restraints = new GameObject[2];

    private GameObject[,] g_chains = new GameObject[2, 10];

    private int i_restraintIndex = 0;
    private int i_restrainCount = 0;
    private int i_chainCount = 0;
    private int i_shortenChainIndex = 0;

    [SerializeField]
    private float f_restraintDistance;
    [SerializeField]
    private float f_unChainTime;
    private float f_currentTime = 0;

    private void OnEnable()
    {
        f_currentTime = 0;
        i_restraintIndex = 0;
        i_restrainCount = g_restraints.Length;
        i_chainCount = g_chains.GetLength(1);
        i_shortenChainIndex = 0;
        V_originPos = Vector2.zero;
        B_isTied = true;
        B_isOneBroken = false;
        B_isDashing = false;

        ChainSetting();

        _curBossState = BossState.Idle;
        _bossFSM = new BossFSM(new BossIdleState(this));

        ChangeBossState(BossState.Tied);

        StartCoroutine(ShortenChain());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, bossSo.StopRadius);
    }

    protected override void Update()
    {
        base.Update();
        if(!B_isDead)
        {
            ChangeState();

            if (i_restraintIndex < i_restrainCount)
            {
                TimeChecker(Time.deltaTime * (i_restraintIndex + 1));
            }

            ChainsFollowBoss();

            _bossFSM.UpdateBossState();
        }
    }

    public void ChainReturnAll()
    {
        int childCount = 0;
        GameObject[] objs;

        childCount = G_altarOnlyCollector.transform.childCount;
        objs = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            objs[i] = G_altarOnlyCollector.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < childCount; i++)
        {
            if (objs[i] == null)
                continue;
            ObjectPool.Instance.ReturnObject(objs[i]);
        }
    }

    public void ChangeMat(int index = 0)
    {
        MyMat = L_mats[index];
    }

    private void ChainSetting()
    {
        for (int i = 0; i < i_restrainCount; i++)
        {
            g_restraints[i] = ObjectPool.Instance.GetObject(ObjectPoolType.AltarRestraint, G_altarOnlyCollector.transform);
            var rad = Mathf.Deg2Rad * i * 360 / i_restrainCount;
            var x = f_restraintDistance * Mathf.Cos(rad);
            var y = f_restraintDistance * Mathf.Sin(rad);
            g_restraints[i].transform.position = transform.GetChild(i).position + new Vector3(x, y, 0);
            g_restraints[i].transform.rotation = Quaternion.identity;
            for (int j = 0; j < i_chainCount; j++)
            {
                var xx = j * f_restraintDistance / i_chainCount * Mathf.Cos(rad);
                var yy = j * f_restraintDistance / i_chainCount * Mathf.Sin(rad);
                g_chains[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.AltarChain, g_restraints[i].transform.GetChild(0).transform);
                g_chains[i, j].transform.position = transform.GetChild(i).position + new Vector3(xx, yy, 0);
                g_chains[i, j].transform.rotation = Quaternion.identity;
            }
        }
    }

    private void ChainsFollowBoss()
    {
        for (int i = 0; i < i_restrainCount; i++)
        {
            float angle = Mathf.Atan2(transform.position.y - g_restraints[i].transform.position.y, transform.position.x - g_restraints[i].transform.position.x) * Mathf.Rad2Deg;

            g_restraints[i].transform.GetChild(0).rotation = Quaternion.AngleAxis(angle + 180 - i * 180, Vector3.forward);
        }
    }

    private IEnumerator ShortenChain()
    {
        while(i_restraintIndex < 2)
        {
            if (i_restraintIndex > 0)
            {
                if (Vector3.Distance(transform.position, g_chains[i_restraintIndex, i_shortenChainIndex].transform.position) < 0.5f)
                {
                    if (i_shortenChainIndex < i_chainCount - 1)
                    {
                        g_chains[i_restraintIndex, i_shortenChainIndex].gameObject.SetActive(false);
                        i_shortenChainIndex++;
                    }
                    yield return null;
                }
                else if (Vector3.Distance(transform.position, g_chains[i_restraintIndex, i_shortenChainIndex].transform.position) > 0.5f)
                {
                    if (i_shortenChainIndex > 0)
                    {
                        g_chains[i_restraintIndex, i_shortenChainIndex].gameObject.SetActive(true);
                        i_shortenChainIndex--;
                    }
                    yield return null;
                }
            }

            yield return null;
        }
        
    }

    private void ChangeState()
    {
        if(B_dead && !B_isDead)
        {
            if(i_restraintIndex < 2)
                ReturnRestraintAndChains();
            StartCoroutine(CameraManager.Instance.CameraShake(0, 0));
            B_isDead = true;
            B_isTied = false;
            B_isOneBroken = false;
            ChangeBossState(BossState.Dead);
        }
        else
        {
            if (!B_isRunning)
            {
                switch (_curBossState)
                {
                    case BossState.Tied:
                        if (i_restraintIndex > 0)
                        {
                            ChangeBossState(BossState.OneBroken);
                        }
                        break;
                    case BossState.OneBroken:
                        if (i_restraintIndex > 1)
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
                _bossFSM.ChangeBossState(new BossIdleState(this));
                break;
            case BossState.Tied:
                _bossFSM.ChangeBossState(new ATiedState(this));
                break;
            case BossState.OneBroken:
                _bossFSM.ChangeBossState(new AOneBrokenState(this));
                break;
            case BossState.Free:
                _bossFSM.ChangeBossState(new AFreeState(this));
                break;
            case BossState.Dead:
                _bossFSM.ChangeBossState(new ABossDeadState(this));
                break;
        }
    }

    private void TimeChecker(float time)
    {
        if (f_currentTime <= 0)
            f_currentTime = 0;

        if (f_currentTime >= f_unChainTime - 0.5f && f_currentTime < f_unChainTime)
        {
            B_isTied = false;
            B_isOneBroken = false;
        }

        if (f_currentTime <= f_unChainTime)
            f_currentTime += time;
        else
        {
            StartCoroutine(CameraManager.Instance.CameraShake(3, 0.5f));
            StartCoroutine(UnChain(3));
            f_currentTime = 0;
        }
    }

    private IEnumerator UnChain(float speed)
    {
        for (int i = 0; i < i_chainCount; i++)
        {
            GameObject chainFragment = ObjectPool.Instance.GetObject(ObjectPoolType.ChainFragment, G_altarOnlyCollector.transform);
            chainFragment.GetComponent<BossBullet>().Attack(bossSo.Damage);
            chainFragment.transform.position = g_chains[i_restraintIndex, i].transform.position;
            chainFragment.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = chainFragment.GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * UnityEngine.Random.Range(0, 361) / 360), Mathf.Sin(Mathf.PI * 2 * UnityEngine.Random.Range(0, 361) / 360));
            rigid.velocity = dir.normalized * speed;
        }
        for(int i = 0; i < i_chainCount; i++)
        {
            GameObject split = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, G_altarOnlyCollector.transform);
            split.GetComponent<BossBullet>().Attack(bossSo.Damage);
            split.transform.position = g_restraints[i_restraintIndex].transform.position;
            split.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = split.GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / i_chainCount), Mathf.Sin(Mathf.PI * 2 * i / i_chainCount));
            rigid.velocity = dir.normalized * speed;
        }

        ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarRestraint, g_restraints[i_restraintIndex]);
        for (int i = 0; i < i_chainCount; i++)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarChain, g_chains[i_restraintIndex, i]);
        }

        i_restraintIndex++;
        if(i_restraintIndex == 1)
        {
            V_originPos = g_restraints[i_restraintIndex].transform.localPosition;
        }

        yield return null;
    }

    private void ReturnRestraintAndChains()
    {
        if(i_restraintIndex > 0)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarRestraint, g_restraints[i_restraintIndex]);
            for (int j = 0; j < i_chainCount; j++)
            {
                ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarChain, g_chains[i_restraintIndex, j]);
            }
        }
        else
        {
            for (int i = 0; i < i_restrainCount; i++)
            {
                ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarRestraint, g_restraints[i]);
                for (int j = 0; j < i_chainCount; j++)
                {
                    ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarChain, g_chains[i, j]);
                }
            }
        }
        
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.tag == "Player")
        {
            if(B_isDashing)
            {
                if (collision.gameObject.TryGetComponent<IHitAble>(out var IhitAble))
                {
                    IhitAble.Hit(bossSo.Damage);
                }
            }
        }
    }

}
