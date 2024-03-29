using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateBoss : Boss
{
    public int MinimiCount { get => i_minimiCount; set => i_minimiCount = value; }

    public GameObject G_slateOnlyCollector;
    public GameObject G_minimisPositions;

    public List<Material> L_materials;
    public List<Sprite> L_sprite;

    public float F_minimiAwayDistance;

    public bool B_fullHP;
    public bool B_halfHP;

    [SerializeField]
    private int i_minimiCount;

    private enum BossState
    {
        Idle,
        FullHP,
        HalfHP,
        Dead
    }

    private BossState _curBossState;

    private BossFSM _bossFSM;

    private void OnEnable()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("Boss");
        transform.GetComponent<SpriteRenderer>().sprite = L_sprite[0];
        B_fullHP = true;
        B_halfHP = false;
        _curBossState = BossState.Idle;
        _bossFSM = new BossFSM(new BossIdleState(this));

        ChangeState();
        StartCoroutine(BossPatorl(bossSo.StopTime, bossSo.MoveX, bossSo.MoveY, bossSo.Speed));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();

        Debug.Log(B_patorl);

        if (B_dead && !B_isDead)
        {
            B_isDead = true;
            B_fullHP = false;
            B_halfHP = false;
            B_isStop = true;
            B_patorl = false;
            ChangeBossState(BossState.Dead);
        }

        if (!B_isDead)
        {
            ChangeState();
            _bossFSM.UpdateBossState();
        }
    }

    private void ChangeState()
    {
        switch (_curBossState)
        {
            case BossState.Idle:
                ChangeBossState(BossState.FullHP);
                break;
            case BossState.FullHP:
                if (F_currentHp < bossSo.MaxHP / 2)
                {
                    ChangeBossState(BossState.HalfHP);
                }
                break;
            case BossState.HalfHP:
                if (B_isDead)
                {
                    ChangeBossState(BossState.Dead);
                }
                break;
        }
    }

    private void ChangeBossState(BossState nextBossState)
    {
        _curBossState = nextBossState;

        switch(_curBossState)
        {
            case BossState.FullHP:
                _bossFSM.ChangeBossState(new SFullHPState(this));
                break;
            case BossState.HalfHP:
                _bossFSM.ChangeBossState(new SHalfHPState(this));
                break;
            case BossState.Dead:
                _bossFSM.ChangeBossState(new SDeadState(this));
                break;
        }
    }

    public void ReturnMinimi(GameObject[] objs)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            ObjectPool.Instance.ReturnObject(ObjectPoolType.SlateMinimi, objs[i]);
        }
    }

    public void LaserReturnAll()
    {
        int childCount = 0;
        GameObject[] objs;

        childCount = G_slateOnlyCollector.transform.childCount;
        objs = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            objs[i] = G_slateOnlyCollector.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < childCount; i++)
        {
            if (objs[i] == null)
                continue;
            ObjectPool.Instance.ReturnObject(objs[i]);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
}
