using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector2Int pos = new Vector2Int(x,y);

        //무기 찾기
        foreach (InvenBrick b in brickList)
        {
            if (b.InvenObject.originPos == pos)
                weapon = b;
        }

        // 예외처리
        if (weapon == null)
            Debug.LogError("404: not found");

        //신호 보내기 백트래킹
        for(int i = 0; i < weapon.InvenObject.bricks.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Find(weapon.InvenObject.originPos + weapon.InvenObject.bricks[i].point
                    , new Vector2Int(dx[j], dy[j]));
            }
        }

        return list;
    }

    private void Find(Vector2Int pos, Vector2Int dir)
    {
        InventoryObjectData tempData = inventory.GetObjectData2(pos + dir, dir);
        if (tempData == null)
            return;

            Debug.Log("2");
        if (tempData.skills.Length > 0)
        {
            Debug.Log("1");
            list.Add(tempData.skills[0]);
        }

        foreach(var vec in tempData.inputPoints)
        {
            if (vec.dir == dir)
                Research();
        }
    }

    private void Research()
    {

    }
}