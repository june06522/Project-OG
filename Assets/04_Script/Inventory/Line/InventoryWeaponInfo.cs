using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWeaponInfo : MonoBehaviour
{
    public static InventoryWeaponInfo Instance;
    private WeaponInventory inventory;

    private Hashtable isVisit = new Hashtable();
    private Hashtable end = new Hashtable();
    InvenBrick[] brickList = null;
    int[] dx = { 0, 0, 1, -1 };
    int[] dy = { 1, -1, 0, 0 };
    List<Tuple<string, int>> list = new List<Tuple<string, int>>();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.Log($"{transform} : InventoryWeaponInfo is multiple running!");

        inventory = FindObjectOfType<WeaponInventory>();
    }

    public List<Tuple<string, int>> GetConnect(int x, int y)
    {
        //변수
        list = new List<Tuple<string, int>>();
        isVisit = new Hashtable();
        end = new Hashtable();
        brickList = GetComponentsInChildren<InvenBrick>();
        InvenBrick weapon = null;
        Vector2Int pos = new Vector2Int(x, y);

        //무기 찾기
        foreach (InvenBrick b in brickList)
        {
            if (b.InvenObject.originPos == pos)
                weapon = b;
        }

        // 예외처리
        if (weapon == null)
            Debug.LogError("404: not found");

        Hashtable hash = new Hashtable
        {
            { weapon, true }
        };

        //신호 보내기 백트래킹
        for (int i = 0; i < weapon.InvenObject.bricks.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2Int dir = new Vector2Int(dx[j], dy[j]);
                Find(weapon.InvenObject.originPos + weapon.InvenObject.bricks[i].point, dir, hash);
            }
        }

        foreach (DictionaryEntry item in end)
        {
            InventoryObjectData obj = (InventoryObjectData)item.Key;
            list.Add(Tuple.Create(WeaponExplainManager.weaponExplain[obj.generatorID],(int)item.Value)); // 나중에 Min으로 제한두기 생성기 땜에
        }

        return list;
    }

    private void Find(Vector2Int pos, Vector2Int dir, Hashtable data)
    {
        bool isfind = false;
        InventoryObjectData tempData = inventory.GetObjectData2(pos + dir, dir);
        if (tempData == null)
            return;

        bool canGo = (tempData.inputPoints.Count == 0 && tempData.sendPoints.Count != 0);

        foreach (var v in tempData.inputPoints)
        {
            if (v.point + tempData.originPos == pos + dir && v.dir == dir)
            {
                canGo = true;
            }
        }

        if (!canGo)
            return;
        if (tempData.generatorID != GeneratorID.None)
        {
            foreach (var brick in tempData.sendPoints)
            {
                if (dir + brick.dir == Vector2Int.zero)
                {
                    isfind = true;
                }

            }

            //if (isfind)
            //{
            //    list.Add(Tuple.Create(WeaponExplainManager.weaponExplain[tempData.generatorID], (int)end[tempData]));
            //}
        }

        Hashtable datas = new Hashtable();
        foreach (DictionaryEntry item in data)
        {
            datas.Add(item.Key, item.Value);
        }
        datas.Add(tempData, true);


        foreach (var vec in tempData.inputPoints)
        {
            if (vec.dir == dir)
            {
                Hashtable visited = new()
                {
                    { pos, true }
                };

                Hashtable dataes = new();
                foreach (DictionaryEntry item in datas)
                {
                    dataes.Add(item.Key, item.Value);
                }

                Research(visited, dataes, pos + dir);
            }
        }
    }

    private void Research(Hashtable isVisit, Hashtable data, Vector2Int pos)
    {
        //스택 오버 플로우 방지 방문 체크
        if (isVisit.ContainsKey(pos))
        {
            return;
        }

        isVisit.Add(pos, true);

        for (int i = 0; i < 4; i++)
        {
            Vector2Int dir = new Vector2Int(dx[i], dy[i]);
            Vector2Int tempPos = pos + dir;
            InventoryObjectData tempData = inventory.GetObjectData2(tempPos, dir);
            Hashtable datas = new();
            foreach (DictionaryEntry item in data)
            {
                datas.Add(item.Key, item.Value);
            }


            if (tempData == null)
                continue;

            if (!datas.ContainsKey(tempData))
                datas.Add(tempData, true);

            bool isfind = false;

            //생성기 찾음
            if (tempData.generatorID != GeneratorID.None)
            {
                foreach (var brick in tempData.sendPoints)
                {
                    if (dir + brick.dir == Vector2Int.zero)
                    {
                        //블록 갖고와야댐
                        Vector2Int tmpPos = tempData.originPos - dir;
                        InventoryObjectData tmpData = inventory.GetObjectData2(tmpPos, dir);
                        if(tmpData != null)
                        {
                            foreach (SignalPoint item in tmpData.sendPoints)
                            {
                                if(tmpPos == item.point + tmpData.originPos &&
                                    item.dir - dir == Vector2Int.zero)
                                    isfind = true;
                            }
                        }

                    }

                }

                if (isfind)
                {
                    int index = Mathf.Min(data.Count - 1,5);
                    if (!end.ContainsKey(tempData))
                        end.Add(tempData, index);
                    else
                    {
                        if ((int)end[tempData] < index)
                            end[tempData] = index;
                    }

                }
            }

            //이동되는 위치 타일 찾아서 방향검사해야대 ㅇㅋ? ㅇㅋ
            bool isCan = false;

            //여기 머가 문제니...
            foreach (var v in tempData.bricks)
            {
                foreach (var d in v.dir)
                {
                    if (v.point + tempData.originPos == tempPos && d + dir == Vector2Int.zero)
                    {
                        isCan = true;
                    }

                }
            }

            if (isCan)
            {
                Hashtable visited = new();
                foreach (DictionaryEntry item in isVisit)
                {
                    visited.Add(item.Key, item.Value);
                }


                Research(visited, datas, tempPos);
            }
        }
    }
}