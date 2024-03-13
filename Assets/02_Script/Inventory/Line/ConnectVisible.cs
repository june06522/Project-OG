using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectVisible : MonoBehaviour
{
    private WeaponInventory inventory;
    private InventoryActive inventoryActive;
    Canvas canvas;
    private InvenBrick[] brickList;

    [SerializeField]
    LineRenderer tempObj;

    List<LineRenderer> lendererList = new List<LineRenderer>();

    private void Awake()
    {
        inventoryActive = FindObjectOfType<InventoryActive>();
        inventory = FindObjectOfType<WeaponInventory>();
        canvas = GetComponentInParent<Canvas>();
        GameManager.Instance.Inventory.camerasetting += SettingOption;
    }

    public void SettingOption()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        for(int i = lendererList.Count - 1; i >= 0; i--)
        {
            var line = lendererList[i];
            lendererList.Remove(line);
            Destroy(line.gameObject);
        }

        if(inventoryActive.IsOn)
        {
            brickList = GetComponentsInChildren<InvenBrick>();
            List<InvenBrick> generatorList = new List<InvenBrick>();
            foreach (var brick in brickList)
            {
                if (brick.Type == ItemType.Generator && !brick.IsDrag)
                    generatorList.Add(brick);
            }

            foreach (var generator in generatorList)
            {
                LineRenderer line = Instantiate(tempObj,transform).GetComponent<LineRenderer>();
                lendererList.Add(line);

                line.positionCount += 1;
                Vector3 pos = generator.transform.position;
                pos.z = -4;
                line.SetPosition(line.positionCount - 1, pos);

                foreach(var vec in generator.InvenObject.sendPoints)
                {
                    Hashtable h = new Hashtable();
                    h.Add((Vector2)(generator.InvenObject.originPos), true);
                    Connect(line,generator.InvenObject.originPos + vec.dir + vec.point,vec.dir,pos,h);
                }
            }
        }
    }

    void Connect(LineRenderer line, Vector2 pos,Vector2Int dir, Vector2 originpos, Hashtable isVisited)
    {
        Vector3 tempVec = originpos - new Vector2(dir.x * -0.93f,dir.y * -0.93f);
        if (isVisited.ContainsKey(pos) && (bool)isVisited[pos])
            return;
        isVisited[pos] = true;
        Vector2Int fillCheckVec = new Vector2Int((int)pos.x, (int)pos.y);
        if(inventory.CheckFill2(fillCheckVec))
        {
            InventoryObjectData data = inventory.GetObjectData2(fillCheckVec, dir);
            if(data != null)
            {
                AddLineRenderPoint(line, tempVec);
                foreach(SignalPoint point in data.sendPoints)
                {
                    Vector2 tempPos = data.originPos + point.dir + point.point;
                    Connect(line, tempPos, point.dir,tempVec,isVisited);
                }
            }
        } 
        isVisited[pos] =  false;
    }

    private void AddLineRenderPoint(LineRenderer line, Vector3 pos)
    {
        line.positionCount += 1;
        pos.z = -4;
        line.SetPosition(line.positionCount - 1, pos);
    }

    IEnumerator CoShowLine()
    {
        List<LineRenderer> lendererList = new List<LineRenderer>();
        while (true)
        {
            brickList = GetComponentsInChildren<InvenBrick>();
            List<InvenBrick> generatorList = new List<InvenBrick>();
            foreach (var brick in brickList)
            {
                if (brick.Type == ItemType.Generator)
                    generatorList.Add(brick);
            }

            foreach (var generator in generatorList)
            {

            }

            Debug.Log(generatorList.Count);
            yield return new WaitForSeconds(3f);
        }
    }
}