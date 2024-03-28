using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateBoss : Boss
{
    public int MinimiCount { get => i_minimiCount; set => i_minimiCount = value; }

    public GameObject G_slateOnlyCollector;

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
    }

    protected override void Update()
    {
        base.Update();

        if (B_dead && !B_isDead)
        {
            B_isDead = true;
            B_fullHP = false;
            B_halfHP = false;
            ChangeBossState(BossState.Dead);
        }

        if (!B_isDead)
        {
            if (!B_isRunning)
            {
                ChangeState();
            }
            _bossFSM.UpdateBossState();

            if(!DontNeedToFollow() && !B_isStop)
            {
                transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.player.transform.position, Time.deltaTime * bossSo.Speed);
            }
            else
            {
                StopImmediately(transform);
            }
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

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
    }
}
