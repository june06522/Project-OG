using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawn : MonoBehaviour
{
    WaveSO curSO = null;
    [HideInInspector]
    public int curMonsterCnt = 0;

    int[] distx = new int[] {
        -1,0,1,
        -1,0,1,
        -1,0,1
    };
    int[] disty = new int[] {
        1,1,1,
        0,0,0,
        -1,-1,-1
    };


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
        while (curIdx < curSO.waveCnt)
        {
            if (curMonsterCnt == 0)
            {
                curIdx++;
                Spawn();
            }
            yield return null;
        }


        while (curMonsterCnt > 0)
        {
            yield return null;
        }

        MonsterSpawnManager.Instance.selectWave[MapManager.Instance.CurIdxY, MapManager.Instance.CurIdxX] = null;
        MapManager.Instance.RoomClear();
    }

    private void Spawn()
    {
        int cnt = Random.Range(curSO.minCnt, curSO.maxCnt + 1);
        curMonsterCnt += cnt;
        int maxVal = 0;
        int randomVal = 0;
        List<MonsterWaveInfo> tempList = curSO.monsterWaveInfo;

        for (int i = 0; i < tempList.Count; i++)
        {
            maxVal += tempList[i].percentage;
        }

        for (int i = 0; i < cnt; i++)
        {
            randomVal = Random.Range(0, maxVal);
            int tempVal = 0;


            for (int j = 0; j < tempList.Count; j++)
            {
                tempVal += tempList[j].percentage;
                if (tempVal > randomVal)
                {
                    SpawnMonster(tempList[j].monsterObj);
                    break;
                }
            }
        }
    }

    private void SpawnMonster(Enemy monsterobj)
    {
        int cnt = 0;
        int xidx = MapManager.Instance.CurIdxX;
        int yidx = MapManager.Instance.CurIdxY;

        int x = (xidx - MapManager.Instance.CorrectX) * 
            (MapManager.Instance.roomGenarator.WidthLength);
        int y = (yidx - MapManager.Instance.CorrectY) * 
            (MapManager.Instance.roomGenarator.HeightLength);
        Enemy obj = Instantiate(monsterobj, transform);
        obj.name = Random.Range(0,100).ToString();

        while (true)
        {
            if (cnt++ > 100)
            {
                Debug.LogError($"Too many try spawn");
                break;
            }
            
            Vector3 pos = new Vector3(
                Random.Range(x - MapManager.Instance.roomGenarator.checkRoom[yidx,xidx].width / 2,
                x + MapManager.Instance.roomGenarator.checkRoom[yidx, xidx].width / 2),
                Random.Range(y - MapManager.Instance.roomGenarator.checkRoom[yidx, xidx].height / 2,
                y + MapManager.Instance.roomGenarator.checkRoom[yidx, xidx].height / 2));
            obj.transform.position = pos;
             
            //wall 타일맵확인
            if (obj.EnemyDataSO.CheckObstacle)
            {
                if (CheckWall(pos))
                    continue;
            }

            if (!CheckCollider(obj))
                break;

        }
    }

    private bool CheckWall(Vector3 pos)
    {
        for (int i = 0; i < 9; i++)
        {
            Vector3Int tempPos = new Vector3Int((int)pos.x + distx[i], (int)pos.y + disty[i], 0);
            if (MapManager.Instance.roomGenarator.roomTilemap.WallTile.GetTile(tempPos) != null)
                return true;
        }
        return false;
    }

    private bool CheckCollider(Enemy obj)
    {
        Vector3 pos = obj.transform.position;
        Collider[] colliders = Physics.OverlapSphere(pos, 2f);

        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                return true;
            }
        }

        return false;
    }
}