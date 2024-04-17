using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloweringState : BossBaseState
{
    private FlowerPattern _pattern;
    private FlowerBoss _flower;

    public FloweringState(FlowerBoss boss, FlowerPattern pattern) : base(boss, pattern)
    {
        _pattern = pattern;
        _flower = boss;
    }

    public override void OnBossStateExit()
    {
        _flower.isAttacking = false;
    }

    public override void OnBossStateOn()
    {
        _flower.flowering = true;
        _flower.StartCoroutine(RandomPattern(_flower.so.PatternChangeTime));
    }

    public override void OnBossStateUpdate()
    {
        if(!_flower.flowering)
        {
            _flower.StopCoroutine(RandomPattern(_flower.so.PatternChangeTime));
            StopNowCoroutine();
        }
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        while(_flower.flowering)
        {
            if(_flower.isAttacking)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(waitTime);

            int rand = Random.Range(1, 3);

            switch (rand)
            {
                case 1:
                    NowCoroutine(_pattern.FlowerShapeShot(_flower, 5, 3, 3, 10, 5, 1, false, 2));
                    break;
                case 2:
                    NowCoroutine(_pattern.ScatterShot(_flower, 5, 3, 2));
                    break;
            }
        }
    }
}
