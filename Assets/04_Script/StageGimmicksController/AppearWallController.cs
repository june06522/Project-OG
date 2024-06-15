using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearWallController : GimmickController
{
    protected override void Start()
    {
        base.Start();

        _stage.OnStageStartEvent += StartApperWalls;
    }

    private void StartApperWalls()
    {
        StartCoroutine(AppearWalls(1, 3));
    }

    private IEnumerator AppearWalls(float waitTime, float reStartTime)
    {
        while(!_isEnded)
        {
            for (int i = 3; i < 6; i++)
            {
                _gimmicksList[i].SetActive(false);
            }

            yield return new WaitForSeconds(reStartTime);

            for (int i = 0; i < 3; i++)
            {
                _gimmicksList[i].SetActive(true);
                _gimmicksList[i].GetComponent<AppearWall>().Appear();
            }

            yield return new WaitForSeconds(waitTime);

            for (int i = 0; i < 3; i++)
            {
                _gimmicksList[i].SetActive(false);
            }

            for (int i = 3; i < 6; i++)
            {
                _gimmicksList[i].SetActive(true);
                _gimmicksList[i].GetComponent<AppearWall>().Appear();
            }

            yield return new WaitForSeconds(waitTime);
        }
    }
}
