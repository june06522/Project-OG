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
        int instantWallCount = _crab.instantWalls.transform.childCount;
        for(int i = 0; i < instantWallCount; i++)
        {
            _crab.instantWalls.transform.GetChild(i).gameObject.SetActive(false);
        }

        _crab.animator.speed = 0;
        _crab.ReturnAll();
        _crab.CrabReturnAll();
        _crab.StopAllCoroutines();
    }

    public override void OnBossStateOn()
    {
        _crab.gameObject.layer = LayerMask.NameToLayer("Boss");
        _crab.StartCoroutine(RandomPattern(_crab.so.PatternChangeTime));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        int beforeRand = 0;

        while (!_crab.IsDie)
        {
            if(_crab.isAttacking)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(waitTime);

            int rand = 1;// Random.Range(1, 6);
            if(beforeRand == rand)
            {
                if(rand == 1)
                {
                    rand = Random.Range(2, 6);
                }
                else if(rand == 5)
                {
                    rand = Random.Range(1, 5);
                }
                else
                {
                    rand = rand - 1;
                }
            }

            beforeRand = rand;

            switch(rand)
            {
                case 1:
                    _crab.StartCoroutine(_pattern.NipperLaserAttack());
                    break;
                case 2:
                    _pattern.SwingAttack();
                    break;
                case 3:
                    _crab.StartCoroutine(_pattern.BubbleAttack());
                    break;
                case 4:
                    _crab.StartCoroutine(_pattern.NipperPunch(_crab.crabLeftNipper, _crab.leftJoints));
                    break;
                case 5:
                    _crab.StartCoroutine(_pattern.NipperPunch(_crab.crabRightNipper, _crab.rightJoints, false));
                    break;
            }
        }
    }
}
