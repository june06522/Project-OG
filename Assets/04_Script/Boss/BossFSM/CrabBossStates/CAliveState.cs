using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAliveState : BossBaseState
{
    private CrabBoss _crab;

    private CrabPattern _pattern;

    public CAliveState(CrabBoss boss, CrabPattern pattern) : base(boss, pattern)
    {
        _crab = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        Debug.Log("Exit Alive");
    }

    public override void OnBossStateOn()
    {
        Debug.Log("Alive");
    }

    public override void OnBossStateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            if (!_crab.isAttacking)
            {
                _crab.StartCoroutine(_pattern.NipperLaserAttack());
            }
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {
            if(!_crab.isAttacking)
            {
                _pattern.SwingAttack();
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            if (!_crab.isAttacking)
            {
                _crab.StartCoroutine(_pattern.BubbleAttack());
            }
        }
        else if(Input.GetKeyDown(KeyCode.H))
        {
            if (!_crab.isAttacking)
            {
                _crab.StartCoroutine(_pattern.NipperLeftPunch());
            }
        }
    }
}
