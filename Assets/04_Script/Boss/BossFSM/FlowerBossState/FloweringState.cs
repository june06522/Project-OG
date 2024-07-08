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
        _flower.gameObject.layer = LayerMask.NameToLayer("Boss");
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
        int beforeRand = 4;
        while(_flower.flowering)
        {
            if(_flower.isAttacking)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(waitTime);

            int rand = Random.Range(1, 4);
            if (beforeRand == rand)
            {
                if (rand == 1)
                {
                    rand = Random.Range(2, 4);
                }
                else if (rand == 3)
                {
                    rand = Random.Range(1, 3);
                }
                else
                {
                    int chooseRand = Random.Range(1, 3);
                    switch (chooseRand)
                    {
                        case 1:
                            rand = 1;
                            break;
                        case 2:
                            rand = 3;
                            break;
                    }
                }
            }
            beforeRand = rand;

            switch (rand)
            {
                case 1:
                    NowCoroutine(_pattern.FlowerShapeShot(_flower, 5, 5, 3, 10, 10, 1, false, 1f));
                    break;
                case 2:
                    NowCoroutine(_pattern.ScatterShot(_flower, 5, 3, 2));
                    break;
                case 3:
                    NowCoroutine(_pattern.ComeBackShot(_flower, 20, 5, 5));
                    break;
            }
        }
    }
}
