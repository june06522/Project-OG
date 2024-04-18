using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBoss : Boss
{
    public GameObject flowerOnlyCollector;
    public GameObject bigestBody;
    public GameObject mediumSizeBody;
    public GameObject smallestBody;

    public bool flowering;
    public bool fullBloom;
    public bool withered;

    private BossState _curBossState;

    private BossFSM _bossFSM;

    private FlowerPattern _pattern;

    [SerializeField]
    private float _fullBloomTime;

    private bool _change = false;

    [HideInInspector]
    public Body bigestbody;
    [HideInInspector]
    public Body mediumsizebody;
    [HideInInspector]
    public Body smallestbody;

    protected override void OnEnable()
    {
        base.OnEnable();

        SetBody(ref bigestbody, bigestBody);
        SetBody(ref mediumsizebody, mediumSizeBody);
        SetBody(ref smallestbody, smallestBody);

        _pattern = GetComponent<FlowerPattern>();
        _bossFSM = new BossFSM(new BossIdleState(this, _pattern));
        StartCoroutine(FullBloomTimer(_fullBloomTime));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();

        ChangeState();
    }

    public void ReturnFlowerCollector()
    {
        int num = flowerOnlyCollector.transform.childCount;

        for(int i = num - 1; i >= 0; i--)
        {
            ObjectPool.Instance.ReturnObject(flowerOnlyCollector.transform.GetChild(i).gameObject);
        }
    }

    

    private IEnumerator FullBloomTimer(float waitTime)
    {
        fullBloom = true;
        yield return new WaitForSeconds(waitTime);
        fullBloom = false;
    }

    private IEnumerator FullBloomHp()
    {
        while(!IsDie)
        {
            _currentHP -= Time.deltaTime;
            yield return null;
        }
        _currentHP = 0;
        MakeDie(true);
    }

    private IEnumerator NextState(BossState state)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        SetBodyToBasic(bigestbody, bigestBody);
        SetBodyToBasic(mediumsizebody, mediumSizeBody);
        SetBodyToBasic(smallestbody, smallestBody);
        ReturnAll();
        flowering = false;

        while (_currentHP < so.MaxHP)
        {
            _currentHP++;
            yield return null;
        }

        _currentHP = so.MaxHP;

        ChangeBossState(state);
        gameObject.layer = LayerMask.NameToLayer("Boss");
        _fakeDie = false;
    }

    private void ChangeState()
    {
        switch (_curBossState)
        {
            case BossState.Idle:
                _fakeDie = true;
                ChangeBossState(BossState.Flowering);
                break;
            case BossState.Flowering:
                if(fullBloom && _currentHP <= 0 && !_change)
                {
                    StopAllCoroutines();
                    StartCoroutine(NextState(BossState.FullBloom));
                    StartCoroutine(FullBloomHp());
                    _change = true;
                }
                else if(!fullBloom && _currentHP <= 0 && !_change)
                {
                    StopAllCoroutines();
                    StartCoroutine(NextState(BossState.Withered));
                    _change = true;
                }
                break;
            case BossState.FullBloom:
                if (IsDie)
                {
                    ChangeBossState(BossState.Dead);
                }
                break;
            case BossState.Withered:
                if (IsDie)
                {
                    ChangeBossState(BossState.Dead);
                }
                break;
        }
    }

    private void ChangeBossState(BossState nextBossState)
    {
        _curBossState = nextBossState;

        switch (_curBossState)
        {
            case BossState.Idle:
                _bossFSM.ChangeBossState(new BossIdleState(this, _pattern));
                break;
            case BossState.Flowering:
                _bossFSM.ChangeBossState(new FloweringState(this, _pattern));
                break;
            case BossState.FullBloom:
                _bossFSM.ChangeBossState(new FullBloomState(this, _pattern));
                break;
            case BossState.Withered:
                _bossFSM.ChangeBossState(new WitheredState(this, _pattern));
                break;
            case BossState.Dead:
                isDead = true;
                _bossFSM.ChangeBossState(new FDeadState(this, _pattern));
                break;
        }
    }
}
