using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitheredState : BossBaseState
{
    private FlowerPattern _pattern;
    private FlowerBoss _flower;

    public WitheredState(FlowerBoss boss, FlowerPattern pattern) : base(boss, pattern)
    {
        _flower = boss;
        _pattern = pattern;
    }

    public override void OnBossStateExit()
    {
        _flower.StopAllCoroutines();
        _pattern.StopAllCoroutines();

        _flower.SetBody(_flower.bigestBody, Vector3.one, Vector3.zero, _flower.bossColor, 0.5f);
        _flower.SetBody(_flower.mediumSizeBody, Vector3.one, Vector3.zero, _flower.bossColor, 0.5f);
        _flower.SetBody(_flower.smallestBody, Vector3.one, Vector3.zero, _flower.bossColor, 0.5f);

        _flower.withered = false;
        _flower.isAttacking = false;
    }

    public override void OnBossStateOn()
    {
        _flower.gameObject.layer = LayerMask.NameToLayer("Boss");
        _flower.gameObject.tag = "HitAble";

        _flower.withered = true;
        _flower.StartCoroutine(RandomPattern(_flower.so.PatternChangeTime));
    }

    public override void OnBossStateUpdate()
    {
        if(!_flower.withered)
        {
            _flower.StopCoroutine(RandomPattern(_flower.so.PatternChangeTime));
            StopNowCoroutine();
        }
    }

    private IEnumerator RandomPattern(float waitTime)
    {
        int beforeRand = 4;
        while (_flower.withered)
        {
            if (_flower.isAttacking)
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
                    NowCoroutine(_pattern.OminidirShot(_flower, 3, 15, 10, 1, 2));
                    break;
                case 2:
                    NowCoroutine(_pattern.WarmShot(_flower, 5, 5, 5, 0.35f, 2));
                    break;
                case 3:
                    NowCoroutine(_pattern.ComeBackShot(_flower, 20, 5, 5));
                    break;
            }
        }
    }
}
