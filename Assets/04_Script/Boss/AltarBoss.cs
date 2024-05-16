using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

// inspector에서 보이는 변수들 
public partial class AltarBoss
{
    public GameObject altarCollector;
    public GameObject bigestBody;
    public GameObject mediumSizeBody;
    public GameObject smallestBody;
    public GameObject warningLine;
    public GameObject IW_1_1;
    public GameObject IW_1_2;
    public GameObject IW_2_1;
    public GameObject IW_2_2;
    public GameObject IW_3_1;
    public GameObject IWW_1_1;
    public GameObject IWW_1_2;
    public GameObject IWW_2_1;
    public GameObject IWW_2_2;
    public GameObject IWW_3_1;

    [field: Header("AudioClip")]
    public AudioClip fireClip;
    public AudioClip deadClip;
    public AudioClip rotateClip;
    public AudioClip dashClip;
    public AudioClip unChainClip;

    [field: Header("TileMap")]
    public Tilemap smallStage;
    public Tilemap smallStageLeft;
    public Tilemap smallStageRight;
    public Tilemap bigStage;

    [field: Header("UI")]
    public Image blink;

    [field: Header("Sprite")]
    public Sprite bigTriangleSprite;

    [field: Header("Transform")]
    public Transform[] restraints = new Transform[2];

    [field: Header("AltarOnly")]
    [SerializeField]
    private float _restraintDistance;
    [SerializeField]
    private float _unChainTime;
}

public partial class AltarBoss : Boss
{
    [HideInInspector] public bool isTied;
    [HideInInspector] public bool isOneBroken;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isIdleEnded;
    [HideInInspector] public bool isUnChained;
    [HideInInspector] public bool isIW;

    [HideInInspector] public Vector3 originPos;

    private BossState _curBossState;

    private BossFSM _bossFSM;

    private AltarPattern _pattern;

    private GameObject[,] _chains = new GameObject[2, 5];

    private int _restraintIndex = 0;
    private int _restrainCount = 0;
    private int _chainCount = 0;
    private int _shortenChainIndex = 0;

    private float _currentTime = 0;

    private bool _isAnimationEnd;

    private void Awake()
    {
        _pattern = GetComponent<AltarPattern>();
        bossMove = GetComponent<BossMove>();
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        bossColor = new Color(1, 0.1960784f, 0.427451f);

        _currentTime = 0;
        _restraintIndex = 0;
        _restrainCount = restraints.Length;
        _chainCount = _chains.GetLength(1);
        _shortenChainIndex = 0;

        isTied = true;
        isOneBroken = false;
        isDashing = false;
        isIdleEnded = false;
        isUnChained = false;
        isIW = false;
        _isAnimationEnd = false;

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

        if (isIdleEnded && _curBossState == BossState.Idle)
        {
            ChangeBossState(BossState.Tied);
        }

        if (_curBossState != BossState.Idle)
        {
            if (!isDead)
            {
                ChangeState();
            }

            if (!IsDie)
            {
                ChainsFollowBoss();

                if (_restraintIndex < _restrainCount)
                {
                    TimeChecker(Time.deltaTime);
                }

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
            Destroy(objs[i]);
        }
    }

    private IEnumerator ChangeStageSize(float changeTime)
    {
        blink.gameObject.SetActive(true);
        smallStage.gameObject.SetActive(false);
        smallStageLeft.gameObject.SetActive(false);
        smallStageRight.gameObject.SetActive(false);
        bigStage.gameObject.SetActive(true);

        float currentTime = 0;
        float a = 1;

        while(currentTime < changeTime)
        {
            currentTime += Time.deltaTime;
            a -= Time.deltaTime / changeTime;
            blink.color = new Color(1, 1, 1, a);
            yield return null;
        }

        blink.gameObject.SetActive(false);
    }

    private void ChainSetting()
    {
        for (int i = 0; i < _restrainCount; i++)
        {
            var rad = Mathf.Deg2Rad * i * 360 / _restrainCount;
            var x = _restraintDistance * Mathf.Cos(rad);
            var y = _restraintDistance * Mathf.Sin(rad);
            restraints[i].position = transform.GetChild(i).position + new Vector3(x, y, 0);

            for (int j = 1; j <= _chainCount; j++)
            {
                var xx = j * _restraintDistance / _chainCount * Mathf.Cos(rad);
                var yy = j * _restraintDistance / _chainCount * Mathf.Sin(rad);
                _chains[i, j - 1] = ObjectPool.Instance.GetObject(ObjectPoolType.AltarChain, restraints[i]);
                _chains[i, j - 1].transform.position = transform.GetChild(i).position + new Vector3(xx, yy, 0);
                _chains[i, j - 1].transform.rotation = Quaternion.identity;
            }
        }
    }

    private void ChainsFollowBoss()
    {
        for (int i = 0; i < _restrainCount; i++)
        {
            float angle = Mathf.Atan2(transform.position.y - restraints[i].position.y, transform.position.x - restraints[i].position.x) * Mathf.Rad2Deg;

            restraints[i].rotation = Quaternion.AngleAxis(angle + 180 - i * 180, Vector3.forward);
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
                ReturnChains();
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
                            if (_isAnimationEnd)
                            {
                                ChangeBossState(BossState.OneBroken);
                                _isAnimationEnd = false;
                            }
                        }
                        break;
                    case BossState.OneBroken:
                        if (_restraintIndex > 1)
                        {
                            if (_isAnimationEnd)
                            {
                                ChangeBossState(BossState.Free);
                                _isAnimationEnd = false;
                            }
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
            isUnChained = true;
            StartCoroutine(UnChain(3));
            _currentTime = 0;
        }
    }

    private IEnumerator UnChainMove()
    {
        Vector3 dir = (transform.position - restraints[_restraintIndex].transform.position).normalized;
        while(true)
        {
            for (int i = 0; i < _chainCount; i++)
            {
                _chains[_restraintIndex, i].transform.position = Vector3.MoveTowards(_chains[_restraintIndex, i].transform.position, _chains[_restraintIndex, i].transform.position + dir, Time.deltaTime);
            }

            yield return null;
        }
        
    }

    private IEnumerator UnChain(float speed)
    {
        IEnumerator co = UnChainMove();
        StartCoroutine(co);
        CameraManager.Instance.SetLookObj(this.gameObject, 8, 1.5f);
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(UnChainAnimation(_restraintIndex, 2, 500, 1f, 0.5f));
        CameraManager.Instance.Shockwave(restraints[_restraintIndex].position, -0.1f, 0.3f, 0.5f);
        if (_restraintIndex < 1)
        {
            // 더 길게
            smallStageRight.gameObject.transform.DOMoveX(-3, 0.25f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    smallStageRight.gameObject.transform.DOMoveX(0, 0.25f).SetEase(Ease.InOutSine);
                });
        }
        else
        {
            smallStageLeft.gameObject.transform.DOMoveX(3, 0.25f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    smallStageLeft.gameObject.transform.DOMoveX(0, 0.25f).SetEase(Ease.InOutSine);
                });
        }


        CameraManager.Instance.CameraShake(5, 0.5f);
        StopCoroutine(co);

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

        for (int i = 0; i < _chainCount; i++)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarChain, _chains[_restraintIndex, i]);
        }

        _restraintIndex++;

        yield return new WaitForSeconds(0.5f);
        CameraManager.Instance.SetLookObj(null, 10.8f, 1.5f);
        yield return new WaitForSeconds(0.5f);

        if (_restraintIndex > 1)
        {
            StartCoroutine(ChangeStageSize(0.5f));
        }

        if(_restraintIndex == 1)
        {
            originPos = restraints[_restraintIndex].transform.localPosition;
        }

        yield return new WaitForSeconds(0.5f);
        _isAnimationEnd = true;
        isUnChained = false;
    }

    private IEnumerator UnChainAnimation(int num, float speed, float rotateSpeed, float moveTime, float moveDistance)
    {
        float currentTime = 0;
        float deg = 0;
        int plus;
        if(num < 1)
        {
            plus = 1;
        }
        else
        {
            plus = -1;
        }
        Vector3 dir = -(restraints[num].transform.position - transform.position).normalized;

        while(currentTime < moveTime)
        {
            currentTime += Time.deltaTime * speed;
            deg += Time.deltaTime * rotateSpeed * plus;

            if(deg < 360)
            {
                bigestBody.transform.rotation = Quaternion.Euler(0, 0, deg);
            }
            else
            {
                deg = 0;
            }

            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir * moveDistance, Time.deltaTime * speed);

            yield return null;
        }

        SetBody(bigestBody, Vector3.one, Vector3.zero, bossColor, 0.5f);
    }

    private void ReturnChains()
    {
        if(_restraintIndex > 0)
        {
            for (int j = 0; j < _chainCount; j++)
            {
                ObjectPool.Instance.ReturnObject(ObjectPoolType.AltarChain, _chains[_restraintIndex, j]);
            }
        }
        else
        {
            for (int i = 0; i < _restrainCount; i++)
            {
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
