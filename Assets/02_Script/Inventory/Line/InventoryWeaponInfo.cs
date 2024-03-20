using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWeaponInfo : MonoBehaviour
{
    public static InventoryWeaponInfo Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.Log($"{transform} : InventoryWeaponInfo is multiple running!");
    }

    public List<string> GetConnect(int x, int y)
    {
        //변수
        List<string> list = new List<string>();
        Hashtable isVisit = new Hashtable();
        InvenBrick[] brickList = GetComponentsInChildren<InvenBrick>();
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

        return list;
    }

    public void Find()
    {

    }
}
