using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Collections;

public struct ConnectInfo
{
    public ConnectInfo(Vector2 pos)
    {
        this.pos = pos;
    }

    public Vector2 pos;
}

public struct FinalInfo
{
    public FinalInfo(InvenBrick generator, Dictionary<InventoryObjectData, List<Vector2Int>> savePoints)
    {
        this.generator = generator;
        this.savePoints = savePoints;
    }

    public InvenBrick generator;
    public Dictionary<InventoryObjectData, List<Vector2Int>> savePoints;
}

public class ConnectVisible : MonoBehaviour
{
    Dictionary<InvenBrick, List<LineRenderer>> lineRenderDic = new(); // 바뀌면 Brick 연결된 친구들 싹 지우고 다시 할당
    Dictionary<InvenBrick, List<Coroutine>> coroutineDic = new();

    List<FinalInfo> finalInfos = new List<FinalInfo>();

    private WeaponInventory inventory;
    private InvenBrick[] brickList;

    [SerializeField]
    LineRenderer tempObj;

    [SerializeField] Material lineRenderMat;
    Material curMat = null;

    [HideInInspector] public float mulX = 2.0f;
    [HideInInspector] public float mulY = 2.0f;
    public float width = 0.2f;

    private int maxCnt = 0;

    public int ConnectCnt { private set; get; }

    List<Task<int>> allTasks = new List<Task<int>>();
    bool isRun = false;
    public bool IsRun => isRun;

    private readonly object lockObj = new();
    private readonly object coroutineLockObj = new();

    private float delayTime = 0.15f;

    //인벤토리 할당
    private void Awake()
    {
        inventory = FindObjectOfType<WeaponInventory>();
    }

    //잡은 블록 지우기
    public void EraseBrickLine(InvenBrick hoverBrick)
    {
        if (hoverBrick.Type == ItemType.Generator)
        {
            EraseLine(hoverBrick);
        }
        else
        {
            AddBrick(hoverBrick);
        }
    }

    //라인 처음부터 다시 그릴때
    public async void VisibleLineAllChange(bool isSkip = false)
    {
        finalInfos = new();

        ClearLineRender();

        brickList = GetComponentsInChildren<InvenBrick>();
        List<InvenBrick> generatorList = new List<InvenBrick>();
        foreach (var brick in brickList)
        {
            if (brick.Type == ItemType.Generator && !brick.IsDrag)
                generatorList.Add(brick);
        }

        

        isRun = true;
        foreach (var generator in generatorList)
        {
            Draw(generator);
        }
        await Task.WhenAll(allTasks);
        DrawLine(isSkip);
        allTasks = new();
        isRun = false;
    }

    //비동기 호출
    private void Draw(object generator)
    {
        if (!GameManager.Instance.InventoryActive.IsOn)
            return;

        var task = Task.Run(() => VisibleLineOneChange(generator));
        allTasks.Add(task);
        //var result = await task;
    }

    //하나만 그릴때
    private int VisibleLineOneChange(object brick)
    {
        InvenBrick generator = (InvenBrick)brick;

        if (generator == null)
            return 0;

        maxCnt = 0;

        Dictionary<InventoryObjectData, List<Vector2Int>> savePoints = new();

        foreach (var vec in generator.InvenObject.sendPoints)
        {
            Dictionary<Vector2Int, int> weaponData = new();
            InventoryObjectData b = null;
            do
            {
                Dictionary<ConnectInfo, bool> dic = new Dictionary<ConnectInfo, bool>();
                List<Vector2Int> points = CreateLine(new List<Vector2Int>());

                Vector2Int invenpoint = Vector2Int.zero;
                AddLineRenderPoint(ref points, invenpoint);

                b = null;
                Connect(ref points, generator.InvenObject.originPos + vec.dir + vec.point, vec.dir, invenpoint, dic, maxCnt, ref weaponData, ref b, ref savePoints, generator.InvenObject.originPos);
                AddLineRenderPoint(ref points, invenpoint);
            } while (b != null);
        }
        FinalInfo finalInfo = new FinalInfo(generator, savePoints);
        lock (lockObj)
        { 
            finalInfos.Add(finalInfo);
        }
        return 1;
    }

    /// <summary>
    /// 다른 블록 이동
    /// </summary>
    /// <param name="points">정점들 담는 배열</param>
    /// <param name="pos">시작 정점</param>
    /// <param name="dir">이동한 방향</param>
    /// <param name="originpos">생성기 위치 정점</param>
    /// <param name="isVisited">방문 기록</param>
    /// <param name="cnt">이동한 갯수</param>
    /// <param name="weaponData">정점에 따른 최고 갯수</param>
    /// <param name="findWeapon">무기 찾았는지 정보</param>
    /// <param name="savePoints">여태까지 정점들 저장하는 친구</param>
    /// <returns></returns>
    bool Connect(ref List<Vector2Int> points, Vector2Int pos, Vector2Int dir, Vector2Int originpos, Dictionary<ConnectInfo, bool> isVisited, int cnt, ref Dictionary<Vector2Int, int> weaponData, ref InventoryObjectData findWeapon, ref Dictionary<InventoryObjectData, List<Vector2Int>> savePoints, Vector2Int originPos)
    {
        Vector2Int tempVec = originpos + dir;
        ConnectInfo info = new ConnectInfo(pos);
        #region 스택 오버 플로우 방지 <- 방문한곳 체크
        if (isVisited.ContainsKey(info) && isVisited[info])
            return false;

        isVisited[info] = true;
        #endregion

        Vector2Int fillCheckVec = new Vector2Int(pos.x, pos.y);

        if (inventory.CheckFill2(fillCheckVec))
        {
            InventoryObjectData data = inventory.GetObjectData2(fillCheckVec, dir);

            if (data != null)
            {

                BrickPoint b = new BrickPoint();
                bool isConnect = false;
                bool isconnect = false;

                #region 무기 예외처리
                if (data.sendPoints.Count == 0)
                {
                    ConnectCnt = Mathf.Max(1, ConnectCnt);
                    isConnect = true;
                    Dictionary<ConnectInfo, bool> copiedDict = new Dictionary<ConnectInfo, bool>();
                    foreach (var kvp in isVisited)
                    {
                        copiedDict.Add(kvp.Key, kvp.Value);
                    }

                    List<Vector2Int> tempPoint = new();
                    foreach (var temppoint in points)
                        tempPoint.Add(temppoint);

                    isconnect = BrickCircuit(b, tempVec, ref points, data, copiedDict, cnt + 1, ref weaponData, ref findWeapon, ref savePoints, originPos);

                    points = tempPoint;
                }
                #endregion

                #region 연결된 블록 있으면 스택에 추가 없으면 리턴

                foreach (var point in data.inputPoints)
                {
                    if (point.dir == dir && point.point == fillCheckVec - data.originPos)
                    {
                        foreach (var point1 in data.bricks)
                        {
                            if (point.point == point1.point)
                            {
                                b.point = point.point;
                                b.dir = point1.dir;

                                Dictionary<ConnectInfo, bool> copiedDict = new Dictionary<ConnectInfo, bool>();
                                foreach (var kvp in isVisited)
                                {
                                    copiedDict.Add(kvp.Key, kvp.Value);
                                }

                                List<Vector2Int> tempPoint = new();
                                foreach (var temppoint in points)
                                    tempPoint.Add(temppoint);

                                if (BrickCircuit(b, tempVec, ref points, data, copiedDict, cnt + 1, ref weaponData, ref findWeapon, ref savePoints, originPos))
                                {
                                    isconnect = true;
                                    AddLineRenderPoint(ref points, tempVec);
                                }

                                points = tempPoint;
                            }
                        }
                        isConnect = true;
                    }
                }

                if (!isConnect) return false;
                #endregion

                return isconnect;
            }
        }

        return false;
    }

    /// <summary>
    /// 같은 블록 순회
    /// </summary>
    /// <param name="tmpVec">넘어간 블록 타입</param>
    /// <param name="tempVec">이동 된 위치</param>
    /// <param name="points">정점들 담는 배열</param>
    /// <param name="data">인벤토리 오브젝트</param>
    /// <param name="isVisited">방문했는지</param>
    /// <param name="cnt">파워</param>
    /// <param name="weaponData">정점에 따른 최고 갯수</param>
    /// <param name="findWeapon">무기 찾았는지 정보</param>
    /// <param name="savePoints">정점들 보관하는 친구</param>
    /// <returns></returns>
    private bool BrickCircuit(BrickPoint tmpVec, Vector2Int tempVec, ref List<Vector2Int> points, InventoryObjectData data, Dictionary<ConnectInfo, bool> isVisited, int cnt, ref Dictionary<Vector2Int, int> weaponData, ref InventoryObjectData findWeapon, ref Dictionary<InventoryObjectData, List<Vector2Int>> savePoints, Vector2Int originPos)
    {
        //라인 렌더러에 추가
        AddLineRenderPoint(ref points, tempVec);

        bool isConnect = false;

        #region 무기면 리턴
        if (tmpVec.dir == null)
        {
            if (findWeapon != null && findWeapon != data)
            {
                DeleteLineRenderPoint(ref points);
                return false;
            }
            if (weaponData.ContainsKey(data.originPos))
            {
                if (weaponData[data.originPos] < cnt)
                {
                    findWeapon = data;
                    weaponData[data.originPos] = cnt;
                    RenewalSave(ref savePoints, ref points, tempVec, originPos);
                    points = CreateLine(points);
                    return true;
                }
                else
                {
                    DeleteLineRenderPoint(ref points);
                    return false;

                }
            }
            else
            {
                findWeapon = data;
                weaponData.Add(data.originPos, cnt);
                RenewalSave(ref savePoints, ref points, tempVec, originPos);
                points = CreateLine(points);
                return true;
            }

        }
        #endregion

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
                    ConnectInfo info = new ConnectInfo(tempPos);

                    if (isVisited.ContainsKey(info) && isVisited[info])
                        continue;

                    isVisited[info] = true;


                    tempVec += v;
                    BrickPoint b;
                    b.point = point.point;
                    b.dir = point.dir;

                    Dictionary<ConnectInfo, bool> copiedDict = new Dictionary<ConnectInfo, bool>();
                    foreach (var kvp in isVisited)
                    {
                        copiedDict.Add(kvp.Key, kvp.Value);
                    }

                    List<Vector2Int> tempPoint = new();
                    foreach (var temppoint in points)
                        tempPoint.Add(temppoint);

                    if (BrickCircuit(b, tempVec, ref points, data, copiedDict, cnt, ref weaponData, ref findWeapon, ref savePoints, originPos))
                    {
                        AddLineRenderPoint(ref points, tempVec);
                        isConnect = true;
                    }

                    points = tempPoint;

                    tempVec -= v;
                }
            }
        }

        #region 연결된 곳 순회
        foreach (SignalPoint point1 in data.sendPoints)
        {
            if (point1.point == tmpVec.point)
            {
                Vector2Int tempPos1 = data.originPos + point1.dir + point1.point;

                Dictionary<ConnectInfo, bool> copiedDict = new Dictionary<ConnectInfo, bool>();
                foreach (var kvp in isVisited)
                {
                    copiedDict.Add(kvp.Key, kvp.Value);
                }

                List<Vector2Int> tempPoint = new();
                foreach (var temppoint in points)
                    tempPoint.Add(temppoint);

                if (Connect(ref points, tempPos1, point1.dir, tempVec, copiedDict, cnt + 1, ref weaponData, ref findWeapon, ref savePoints, originPos))
                {
                    AddLineRenderPoint(ref points, tempVec);
                    isConnect = true;
                }

                points = tempPoint;

                //라인 렌더러에 추가
            }
        }
        if (isConnect)
            return true;

        DeleteLineRenderPoint(ref points);
        return false;
        #endregion
    }

    private void RenewalSave(ref Dictionary<InventoryObjectData, List<Vector2Int>> savePoints, ref List<Vector2Int> points, Vector2Int pos, Vector2Int originPos)
    {
        InventoryObjectData data = GameManager.Instance.Inventory.GetObjectData2(pos + originPos, Vector2Int.zero);

        if (data != null)
        {
            if (!savePoints.ContainsKey(data))
            {

                savePoints.Add(data, points);

            }
            else
            {

                if (savePoints[data].Count <= points.Count)
                    savePoints[data] = points;

            }
        }
        else
            Debug.LogError($"What the fork : {pos}");
    }

    //라인렌더러 그리기
    private void DrawLine(bool isSkip = false)
    {
        List<FinalInfo> modifierInfos = new();
        foreach(var a  in finalInfos)
            modifierInfos.Add(a);

        if (modifierInfos.Count > 0)
        {
            foreach (var info1 in modifierInfos)
            {
                if (GameManager.Instance.InventoryActive.IsOn)
                {
                    Vector3 localPos = info1.generator.RectTransform.localPosition;

                    if (GameManager.Instance.Inventory.StartWidth % 2 == 0)
                        localPos.x += 50;
                    if (GameManager.Instance.Inventory.StartHeight % 2 == 0)
                        localPos.y -= 50;
                    Vector2Int invenpoint = Vector2Int.RoundToInt(localPos / 100);


                    foreach (var info in info1.savePoints)
                    {

                        //튜토리얼 퀘스트
                        if (info.Value.Count > 2)
                            ConnectCnt = 2;
                        if (info.Value.Count == 0 || (info.Value.Count == 2
                            && info.Value[0] == info.Value[1]))
                            continue;

                        LineRenderer line = CreateLine(info1.generator);

                        if (!lineRenderDic.ContainsKey(info1.generator))
                            lineRenderDic.Add(info1.generator, new());
                        lineRenderDic[info1.generator].Add(line);

                        if (!coroutineDic.ContainsKey(info1.generator))
                            coroutineDic.Add(info1.generator, new());
                        coroutineDic[info1.generator].Add(StartCoroutine(LineAnimation(line, info1, invenpoint, info.Value, isSkip)));

                    }
                }
            }
        }
    }

    //라인렌더러 생성
    private LineRenderer CreateLine(InvenBrick trigger)
    {
        LineRenderer line = Instantiate(tempObj, transform);
        curMat = trigger.InvenObject.colorMat;
        if (curMat == null)
            line.material = lineRenderMat;
        else
            line.material = curMat;
        line.startWidth = width;

        if (!lineRenderDic.ContainsKey(trigger))
            lineRenderDic.Add(trigger, new());

        lineRenderDic[trigger].Add(line);

        return line;
    }

    //블록 추가 됐을때 연결된 애들만 바꾸게 하는 함수
    public async void AddBrick(InvenBrick brick)
    {
        finalInfos = new();
        if (brick.Type == ItemType.Generator)
        {
            isRun = true;
            Draw(brick);
            await Task.WhenAll(allTasks);
            DrawLine();
            allTasks = new();
            isRun = false;
        }
        else
        {
            if (brick.Type == ItemType.Connector)
                FindGenerator(brick, false);
            else
                FindGenerator(brick, true);
        }
    }

    //생성기 찾기
    private void FindGenerator(InvenBrick brick, bool isWeapon)
    {
        HashSet<InvenBrick> generatorSet = new();
        HashSet<InventoryObjectData> visitSet = new();
        Vector2Int[] dxy =
        {
           new Vector2Int(1,0),
           new Vector2Int(-1,0),
           new Vector2Int(0,1),
           new Vector2Int(0,-1)
        };

        visitSet.Add(brick.InvenObject);

        if (isWeapon)
        {
            foreach (var singlePoint in brick.InvenObject.bricks)
                foreach (var dir in dxy)
                    FindGeneratorRe(ref generatorSet, ref visitSet, singlePoint.point + dir + brick.InvenObject.originPos);
        }
        else
        {
            foreach (var singlePoint in brick.InvenObject.sendPoints)
                FindGeneratorRe(ref generatorSet, ref visitSet, singlePoint.point + singlePoint.dir + brick.InvenObject.originPos);
        }
        DrawSomeLine(generatorSet);
    }

    //재귀로 연결된 생성기 찾기
    private void FindGeneratorRe(ref HashSet<InvenBrick> generatorSet, ref HashSet<InventoryObjectData> visitSet, Vector2Int pos)
    {
        InventoryObjectData data = GameManager.Instance.Inventory.GetObjectData2(pos, Vector2Int.zero);

        if (data == null) return;

        if (visitSet.Contains(data))
            return;
        else
            visitSet.Add(data);

        if (data.sendPoints.Count == 0)//무기
        {
            return;
        }
        else if (data.inputPoints.Count == 0) // 생성기
        {
            if (data.invenBrick == null)
                Debug.LogError("InvenBrick is null!");
            generatorSet.Add(data.invenBrick);
        }
        else // 연결기
        {
            foreach (var singlePoint in data.sendPoints)
                FindGeneratorRe(ref generatorSet, ref visitSet, singlePoint.point + singlePoint.dir + data.originPos);
        }
    }

    // 특정 생성기만 그리기
    private async void DrawSomeLine(HashSet<InvenBrick> generatorSet)
    {
        finalInfos = new();
        isRun = true;
        foreach (var v in generatorSet)
        {
            EraseLine(v);
            Draw(v);
        }
        await Task.WhenAll(allTasks);
        DrawLine();
        allTasks = new();
        isRun = false;
    }

    //포인트 추가
    private void AddLineRenderPoint(ref List<Vector2Int> points, Vector2Int point, int index = -1)
    {
        if (index == -1)
        {
            points.Add(point);
        }
        else
            points[index] = point;

        if (points.Count > 2)
            ConnectCnt = 2;
    }

    //포인트 하나 지워줌
    private void DeleteLineRenderPoint(ref List<Vector2Int> points)
    {
        points.RemoveAt(points.Count - 1);
    }

    //라인 새로 생성
    private List<Vector2Int> CreateLine(List<Vector2Int> v)
    {
        List<Vector2Int> lineVec = new();

        for (int i = 0; i < v.Count; i++)
            lineVec.Add(v[i]);

        return lineVec;
    }

    //생성기 관련 라인 전부 지우기
    private void EraseLine(InvenBrick brick)
    {
        if (brick == null || !lineRenderDic.ContainsKey(brick))
            return;

        StopCo(brick);

        for (int i = lineRenderDic[brick].Count; i > 0; i--)
        {
            var line = lineRenderDic[brick][i - 1];
            if (line != null)
                Destroy(line.gameObject);
        }
        lineRenderDic[brick].Clear();
    }

    //그려진 라인렌더러 다 지워는 함수
    public void ClearLineRender()
    {
        foreach (var key in lineRenderDic.Keys)
        {
            EraseLine(key);
        }
        lineRenderDic = new();
    }

    //애니메이션 멈추는 예외처리
    
    private void StopCo(InvenBrick brick)
    {
        lock (coroutineLockObj)
        {
            if (!coroutineDic.ContainsKey(brick))
                foreach (var v in coroutineDic[brick])
                {
                    StopCoroutine(v);
                }
            coroutineDic[brick] = new();
        }
    }

    //애니메이션
    IEnumerator LineAnimation(LineRenderer line, FinalInfo info, Vector2Int invenpoint, List<Vector2Int> points, bool skip = false)
    {
        //첫번째 점 그리기
        {
            Vector3? realPos = GameManager.Instance.Inventory.FindInvenPointPos(points[0] + invenpoint);

            InventoryObjectData data = GameManager.Instance.Inventory.GetObjectData2(info.generator.InvenObject.originPos + points[0], Vector2Int.zero);

            line.positionCount++;

            Vector3 endPos = new Vector3(realPos.Value.x, realPos.Value.y, -4);

            line.SetPosition(line.positionCount - 1, endPos);
        }

        float curTime = 0f;
        for (int i = 1; i < points.Count; i++)
        {
            if (line == null)
                break;

            // 인벤토리 포지션 찾기
            Vector3? realPos = GameManager.Instance.Inventory.FindInvenPointPos(points[i] + invenpoint);

            Vector3? beginPos = GameManager.Instance.Inventory.FindInvenPointPos(points[i - 1] + invenpoint);

            // 포지션에 있는 블록 찾기
            InventoryObjectData data = GameManager.Instance.Inventory.GetObjectData2(info.generator.InvenObject.originPos + points[i], Vector2Int.zero);

            #region 예외처리
            if (realPos == null)
                Destroy(line.gameObject);
            
            if (line == null)
                break;

            if (data == null)
                Destroy(line.gameObject);

            if (line == null)
                break;
            #endregion


            if (line != null)
                line.positionCount++;

            Vector3 endPos = new Vector3(realPos.Value.x, realPos.Value.y, -4);
            Vector3 startPos = new Vector3(beginPos.Value.x, beginPos.Value.y, -4);
            Vector3 drawPos = new Vector3(0, 0, -4);


            // 애니메이션
            while (curTime <= delayTime)
            {
                curTime += Time.deltaTime;

                if(skip)
                {
                    curTime = delayTime + 0.01f;
                }

                drawPos.x = Mathf.Lerp(startPos.x, endPos.x, curTime / delayTime);
                drawPos.y = Mathf.Lerp(startPos.y, endPos.y, curTime / delayTime);
                if (line != null)
                    line.SetPosition(line.positionCount - 1, drawPos);
                if(!skip)
                    yield return null;
            }
            curTime = 0.0f;

            #region 예외처리
            if (realPos == null)
                Destroy(line.gameObject);

            if (line == null)
                break;

            if (data == null)
                Destroy(line.gameObject);

            if (line == null)
                break;
            #endregion
        }
        yield return null;
    }
}