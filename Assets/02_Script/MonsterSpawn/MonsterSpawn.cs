using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    WaveSO curSO = null;
    int curMonsterCnt = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            curMonsterCnt = 0;
    }

    public void StartSpawn()
    {
        curSO = MonsterSpawnManager.Instance.selectWave[MapManager.Instance.CurIdxY, MapManager.Instance.CurIdxX];
        if (curSO != null)
        {
            StartCoroutine(StartWave());
        }
    }

    IEnumerator StartWave()
    {
        int curIdx = 0;
        while(curIdx <= curSO.waveCnt)
        {
            if(curMonsterCnt == 0)
            {
                curIdx++;
                Spawn();
            }
            yield return null;
        }    


        while(curMonsterCnt > 0)
        {
            yield return null;
        }
        MapManager.Instance.RoomClear();
    }

    private void Spawn()
    {
        int cnt = Random.Range(curSO.minCnt,curSO.maxCnt);
        curMonsterCnt += cnt;
        int maxVal = 0;
        int randomVal = 0;
        List<MonsterWaveInfo> tempList = curSO.monsterWaveInfo;

        for(int i = 0; i < tempList.Count; i++)
        {
            maxVal += tempList[i].percentage;
        }

        for(int i = 0; i <  cnt; i++)
        {
            randomVal = Random.Range(0, maxVal);
            int tempVal = 0;


            for (int j = 0; j < tempList.Count; j++)
            {
                tempVal += tempList[j].percentage;
                if(tempVal >randomVal)
                {
                    SpawnMonster(tempList[j].monsterObj);
                    break;
                }
            }
        }
    }

    private void SpawnMonster(GameObject monsterobj)
    {
        GameObject obj = Instantiate(monsterobj);
        obj.transform.position = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
    }
}
