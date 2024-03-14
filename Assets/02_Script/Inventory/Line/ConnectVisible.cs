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
    [SerializeField] Material lineRenderMat;

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
        for (int i = lendererList.Count - 1; i >= 0; i--)
        {
            var line = lendererList[i];
            lendererList.Remove(line);
            Destroy(line.gameObject);
        }

        if (inventoryActive.IsOn)
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
                LineRenderer line = Instantiate(tempObj, transform).GetComponent<LineRenderer>();
                line.material = lineRenderMat;
                lendererList.Add(line);

                line.positionCount += 1;
                Vector3 pos = generator.transform.position;
                pos.z = -4;
                line.SetPosition(line.positionCount - 1, pos);

                foreach (var vec in generator.InvenObject.sendPoints)
                {
                    Hashtable h = new Hashtable();
                    h.Add((Vector2)(generator.InvenObject.originPos), true);
                    Connect(line, generator.InvenObject.originPos + vec.dir + vec.point, vec.dir, pos, h);
                }
            }
        }
    }

    void Connect(LineRenderer line, Vector2 pos, Vector2Int dir, Vector2 originpos, Hashtable isVisited)
    {
        Vector2 tempVec = originpos - new Vector2(dir.x * -0.93f, dir.y * -0.93f);

        //스택 오버 플로우 방지 <- 방문한곳 체크
        if (isVisited.ContainsKey(pos) && (bool)isVisited[pos])
            return;

        isVisited[pos] = true;
        Stack<BrickPoint> s = new Stack<BrickPoint>();

        Vector2Int fillCheckVec = new Vector2Int((int)pos.x, (int)pos.y);
        if (inventory.CheckFill2(fillCheckVec))
        {
            InventoryObjectData data = inventory.GetObjectData2(fillCheckVec, dir);
            if (data != null)
            {
                bool isConnect = false;

                //무기만 예외처리하는 일종 하드코딩 ㅎㅎ...
                if (data.sendPoints.Count == 0)
                    isConnect = true;

                //연결 되었는지 확인
                foreach (var point in data.inputPoints)
                {
                    if (point.dir == dir && point.point == fillCheckVec - data.originPos)
                    {
                        foreach (var point1 in data.bricks)
                        {
                            if (point.point == point1.point)
                            {
                                BrickPoint b;
                                b.point = point.point;
                                b.dir = point1.dir;
                                s.Push(b);
                            }
                        }
                        isConnect = true;
                    }
                }

                if (!isConnect) return;

                //라인 렌더러에 추가
                AddLineRenderPoint(line, tempVec);

                if (s.Count == 0)
                    return;

                do
                {
                    BrickPoint tmpVec = s.Pop(); // 0,1  아래 오른쪽
                                                 // 만약 이어진 블럭이 sendpoint면 데이터 보내기
                    foreach (SignalPoint point1 in data.sendPoints)
                    {
                        if (point1.point == tmpVec.point)
                        {
                            Vector2 tempPos1 = data.originPos + point1.dir + point1.point;
                            Connect(line, tempPos1, point1.dir, tempVec - new Vector2(point1.point.x * -0.93f, point1.point.y * -0.93f), isVisited);
                        }
                    }
                    //이친구와 이어진 블록 연결
                    foreach (BrickPoint point in data.bricks)
                    {
                        //방향검사
                        foreach (var v in tmpVec.dir)
                        {
                            //전블록과 이어지면 연결
                            if (point.point == tmpVec.point + v)// 0,0 1,1
                            {
                                Vector2 tempPos = data.originPos + point.point;

                                if (isVisited.ContainsKey(tempPos) && (bool)isVisited[tempPos])
                                    continue;

                                isVisited[tempPos] = true;

                                AddLineRenderPoint(line,
                                    tempVec - new Vector2(point.point.x * -0.93f, point.point.y * -0.93f));

                                BrickPoint b;
                                b.point = point.point;
                                b.dir = point.dir;
                                s.Push(b);
                            }
                        }
                    }
                } while (s.Count > 0);
            }
        }
        isVisited[pos] = false;
    }

    private void AddLineRenderPoint(LineRenderer line, Vector3 pos)
    {
        line.positionCount += 1;
        pos.z = -4;
        line.SetPosition(line.positionCount - 1, pos);
    }
}