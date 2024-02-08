using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    WaveSO curSO = null;
    [HideInInspector]
    public int curMonsterCnt = 0;

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
        while(curIdx < curSO.waveCnt)
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

        MonsterSpawnManager.Instance.selectWave[MapManager.Instance.CurIdxY, MapManager.Instance.CurIdxX] = null;
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
        int x = (MapManager.Instance.CurIdxX - MapManager.Instance.CorrectX) * (MapManager.Instance.roomGenarator.RoomWidth + MapManager.Instance.roomGenarator.BGLenth * 2);
        int y = (MapManager.Instance.CurIdxY - MapManager.Instance.CorrectY) * (MapManager.Instance.roomGenarator.RoomHeight + MapManager.Instance.roomGenarator.BGLenth * 2);
        GameObject obj = Instantiate(monsterobj);

        //공중인지 아닌지확인

        //공중이면 몬스터 곂치는것만

        //지상몹이면 곂치는거 + wall 타일맵
        while(true)
        {
            obj.transform.position = new Vector2(Random.Range(x - 5, x + 5), Random.Range(y - 5, y + 5));
            break;
        }
    }
}
