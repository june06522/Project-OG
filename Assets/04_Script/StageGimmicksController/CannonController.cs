using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : GimmickController
{
    protected override void Start()
    {
        base.Start();

        _stage.OnStageStartEvent += StartFireCannone;
    }

    private void StartFireCannone()
    {
        StartCoroutine(FireCannone(2));
    }

    private IEnumerator FireCannone(float waitTime)
    {
        while(!_isEnded)
        {
            yield return new WaitForSeconds(waitTime);

            foreach(GameObject obj in _gimmicksList)
            {
                obj.SetActive(true);
                obj.GetComponent<Cannon>().Shoot();
            }
        }
    }
}
