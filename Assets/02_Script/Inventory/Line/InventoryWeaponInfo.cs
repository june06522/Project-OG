using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class InventoryWeaponInfo : MonoBehaviour
{
    public static InventoryWeaponInfo Instance;
    private WeaponInventory inventory;

    private Hashtable isVisit = new Hashtable();
    InvenBrick[] brickList = null;
    int[] dx = { 0, 0, 1, -1 };
    int[] dy = { 1, -1, 0, 0 };
    List<string> list = new List<string>();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.Log($"{transform} : InventoryWeaponInfo is multiple running!");

        inventory = FindObjectOfType<WeaponInventory>();
    }

    public List<string> GetConnect(int x, int y)
    {
        //변수
        list = new List<string>();
        isVisit = new Hashtable();
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
                Find(weapon.InvenObject.originPos + weapon.InvenObject.bricks[i].point
                    , new Vector2Int(dx[j], dy[j]),hash);
            }
        }

        return list;
    }

    private void Find(Vector2Int pos, Vector2Int dir,Hashtable data)
    {
        bool isfind = false;
        InventoryObjectData tempData = inventory.GetObjectData2(pos + dir, dir);
        if (tempData == null)
            return;

        if (tempData.skills.Length > 0)
        {
            foreach (var brick in tempData.sendPoints)
            {
                if (dir + brick.dir == Vector2Int.zero)
                {
                    isfind = true;
                }

            }

            if (isfind)
                list.Add(tempData.skills[0]);
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
        if(isVisit.ContainsKey(pos))
        {
            return;
        }

        isVisit.Add(pos,true);

        for (int i = 0; i < 4; i++)
        {
            Vector2Int dir = new Vector2Int(dx[i], dy[i]);
            Vector2Int tempPos = pos + dir;
            InventoryObjectData tempData = inventory.GetObjectData2(tempPos, dir);

            if (tempData == null)
                continue;

            if(!data.ContainsKey(tempData))
                data.Add(tempData, true);

            bool isfind = false;

            //무기 찾음
            if (tempData.skills.Length > 0)
            {
                foreach (var brick in tempData.sendPoints)
                {
                    if (dir + brick.dir == Vector2Int.zero)
                    {
                        isfind = true;
                    }

                }

                if (isfind)
                {
                    Debug.Log(data.Count);
                    list.Add(tempData.skills[data.Count - 2]); // 나중에 Min으로 제한두기 무기랑 생성기 땜에 -2
                }
            }

            //foreach(var vec in tempData.inputPoints)
            //{
            //    if (vec.dir == dir)//칮있디!
            //    {
                    
                    Hashtable visited = new();
                    foreach (DictionaryEntry item in isVisit)
                    {
                        visited.Add(item.Key, item.Value);
                    }

                    Hashtable datas= new();
                    foreach (DictionaryEntry item in data)
                    {
                        datas.Add(item.Key, item.Value);
                    }
                    Research(visited, datas, tempPos);
            //    }
            //}
        }
    }
}