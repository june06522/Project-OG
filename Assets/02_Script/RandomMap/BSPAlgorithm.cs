using Astar;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Dir
{
    left, right, top, bottom
}

public class BSPRoomInfo
{
    public CustomRoom room;

    public RectInt roomRect;

    public BSPRoomInfo(CustomRoom _room, int xpos, int ypos)
    {
        room = _room;
        roomRect = new RectInt(xpos, ypos, _room.width, _room.height);
    }

    public Vector2Int GetCenterPos()
    {
        return new Vector2Int(roomRect.x, roomRect.y);
    }
}

public class BSPAlgorithm : MonoBehaviour
{
    public GameObject debugObject;

    [SerializeField] int roomCnt;

    [SerializeField] int xlen;
    [SerializeField] int ylen;

    [SerializeField] int minlen = 5;

    [SerializeField] int shopCnt = 1;

    [Header("벽")]
    [SerializeField] WallDoor sideWall;
    [SerializeField] WallDoor topWall;
    [SerializeField] WallDoor bottomWall;

    [Header("타일")]
    [SerializeField] Tile loadtile;

    [SerializeField] Tile leftWallTile;
    [SerializeField] Tile righttWalltile;
    [SerializeField] Tile bottomtile;
    [SerializeField] Tile top1tile;
    [SerializeField] Tile top2tile;
    [SerializeField] Tile corner1;
    [SerializeField] Tile corner2;

    [SerializeField] Tile leftEndWall1;
    [SerializeField] Tile leftEndWall2;
    [SerializeField] Tile rightEndWall1;
    [SerializeField] Tile rightEndWall2;

    RoomTileMap roomTilemap;
    private List<BSPRoomInfo> roomList;

    DelaunayTriangulation delaunayTriangulation;
    private void Awake()
    {
        roomTilemap = GetComponent<RoomTileMap>();
        roomList = new List<BSPRoomInfo>();
        delaunayTriangulation = new DelaunayTriangulation();
    }

    private void Start()
    {
        //랜덤 위치에 생성

        //시작방
        roomList.Add(new BSPRoomInfo(roomTilemap.startMap, 0, 0));

        //상점
        for (int i = 0; i < shopCnt; i++)
            roomList.Add(new BSPRoomInfo(roomTilemap.shopMap, 0, 0));

        //몬스터 방
        for (int i = 0; i < roomCnt; i++)
        {
            roomList.Add(new BSPRoomInfo(roomTilemap.roomsList[i],
                Random.Range(-xlen, xlen), Random.Range(-ylen, ylen)));
        }

        //안곂치게 방 밀어내기
        bool isClear = false;
        while (!isClear)
        {
            isClear = true;
            for (int i = 0; i < roomList.Count; i++)
            {
                for (int j = 0; j < roomList.Count; j++)
                {
                    if (i == j)
                        continue;
                    while (CheckOverlap(roomList[i].roomRect, roomList[j].roomRect))
                    {
                        isClear = false;
                        if (roomList[i].roomRect.x > roomList[j].roomRect.x)
                            roomList[i].roomRect.x += Random.Range(-5, 15);
                        else
                            roomList[i].roomRect.x -= Random.Range(-5, 15);

                        if (roomList[i].roomRect.y > roomList[j].roomRect.y)
                            roomList[i].roomRect.y += Random.Range(-5, 15);
                        else
                            roomList[i].roomRect.y -= Random.Range(-5, 15);
                    }
                }
            }
        }

        // 방 그리기
        for (int i = 0; i < roomList.Count; i++)
        {
            roomTilemap.SetCustomRoom(roomList[i]);
        }

        // 각 방의 가운데 점 정보 가져오기
        List<Vector2> centerPos = new List<Vector2>();
        for (int i = 0; i < roomList.Count; i++)
        {
            centerPos.Add(roomList[i].GetCenterPos());
        }

        //삼각형 그리기 들로네 삼각분할
        List<Triangle> triangles = delaunayTriangulation.Triangulation(centerPos);


        //길 선택 최소스패닝 트리
        List<TriEdge> edges = MinimumSpanningTree.FindLine(triangles, triangles[0].a);

        //디버깅
        //StartCoroutine(Debuging(triangles));
        //StartCoroutine(Debuging(edges));

        //길 그리기
        for (int i = 0; i < edges.Count; i++)
        {
            GeneratorLoad(edges[i].a, edges[i].b);
        }

        //플레이어 위치 할당
        GameManager.Instance.player.position = centerPos[0];
    }

    //들로네 삼각분할 디버그용
    IEnumerator Debuging(List<TriEdge> triangles)
    {
        while (true)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                Debug.DrawLine(triangles[i].a, triangles[i].b, Color.red, 3f);

                //Debug.Log($"{triangles[i].A.position} : {triangles[i].B.position} : {triangles[i].C.position}");
            }
            yield return null;
        }
    }
    IEnumerator Debuging(List<Triangle> triangles)
    {
        while (true)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                Debug.DrawLine(triangles[i].a, triangles[i].b, Color.white, 3f);
                Debug.DrawLine(triangles[i].b, triangles[i].c, Color.white, 3f);
                Debug.DrawLine(triangles[i].c, triangles[i].a, Color.white, 3f);

                //Debug.Log($"{triangles[i].A.position} : {triangles[i].B.position} : {triangles[i].C.position}");
            }
            yield return null;
        }
    }

    //방 곂치는지 확인
    public bool CheckOverlap(RectInt rect1, RectInt rect2)
    {
        bool condition1 = rect1.x - minlen < rect2.x + rect2.width;
        bool condition2 = rect1.x + rect1.width > rect2.x - minlen;
        bool condition3 = rect1.y - minlen < rect2.y + rect2.height;
        bool condition4 = rect1.y + rect1.height > rect2.y - minlen;

        return condition1 && condition2 && condition3 && condition4;
    }

    //길 생성
    public void GeneratorLoad(Vector2 start, Vector2 end)
    {
        Vector2 midPos = new Vector2(start.x, end.y);

        int idx = 1;
        if (start.x > end.x)
        {
            midPos.x -= 3;
        }
        else if (start.x < end.x)
        {
            midPos.x += 3;
        }
        if (start.y > end.y)
        {
            midPos.y += 3;
        }
        else if (start.y < end.y)
        {
            midPos.y -= 3;
        }

        if (start.x > end.x)
        {
            while (roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y - 1, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y - 2, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y + 1, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y + 2, 0)) != null &&
                !CanCreate(midPos, end.x, Dir.left))
                if (idx % 2 == 0)
                    midPos.y += idx++;
                else
                    midPos.y -= idx++;
        }
        else if (start.x < end.x)
        {
            while (roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y - 1, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y - 2, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y + 1, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)end.x, (int)midPos.y + 2, 0)) != null &&
                !CanCreate(midPos, end.x, Dir.right))
                if (idx % 2 == 0)
                    midPos.y += idx++;
                else
                    midPos.y -= idx++;
        }

        //길 보정
        idx = 1;
        if (start.y > end.y)
        {
            while (roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x, (int)start.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x - 1, (int)start.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x - 2, (int)start.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x + 1, (int)start.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x + 2, (int)start.y, 0)) != null &&
                !CanCreate(midPos, start.y, Dir.top))
                if (idx % 2 == 0)
                    midPos.x += idx++;
                else
                    midPos.x -= idx++;
        }
        else if (start.y < end.y)
        {
            while (roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x, (int)start.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x - 1, (int)start.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x - 2, (int)start.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x + 1, (int)start.y, 0)) != null &&
                roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x + 2, (int)start.y, 0)) != null &&
                !CanCreate(midPos, start.y, Dir.bottom))
                if (idx % 2 == 0)
                    midPos.x += idx++;
                else
                    midPos.x -= idx++;
        }


        if (end.x > midPos.x)
        {
            for (int i = (int)midPos.x - 1; i < (int)end.x; i++)
            {
                if (roomTilemap.Tile.GetTile(new Vector3Int(i, (int)midPos.y, 0)) == null)
                {
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 1, 0)) != null)
                        if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 1, 0)) != null)
                            if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y, 0)) != null)
                            {
                                if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == leftWallTile)
                                {
                                    WallDoor obj = Instantiate(sideWall, new Vector3(i + 0.53f, midPos.y + 1.345f, 0), Quaternion.identity);
                                    MapManager.Instance.door.Add(obj);
                                }
                                else
                                {
                                    WallDoor obj = Instantiate(sideWall, new Vector3(i + 0.53f, midPos.y + 1.345f, 0), Quaternion.identity);
                                    obj.GetComponent<SpriteRenderer>().flipX = true;
                                    MapManager.Instance.door.Add(obj);
                                }
                            }

                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == leftWallTile)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y - 2, 0), corner1);
                    else if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == righttWalltile)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y - 2, 0), corner2);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 1, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y - 1, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 0, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 0, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 1, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 1, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 2, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 2, 0), null);

                    roomTilemap.Tile.SetTile(new Vector3Int(i, (int)midPos.y - 1, 0), loadtile);
                    roomTilemap.Tile.SetTile(new Vector3Int(i, (int)midPos.y + 0, 0), loadtile);
                    roomTilemap.Tile.SetTile(new Vector3Int(i, (int)midPos.y + 1, 0), loadtile);
                }
            }
        }
        else
        {
            for (int i = (int)midPos.x + 1; i > (int)end.x; i--)
            {
                if (roomTilemap.Tile.GetTile(new Vector3Int(i, (int)midPos.y, 0)) == null)
                {
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 1, 0)) != null)
                        if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 1, 0)) != null)
                            if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y, 0)) != null)
                            {
                                if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == leftWallTile)
                                {
                                    WallDoor obj = Instantiate(sideWall, new Vector3(i + 0.53f, midPos.y + 1.345f, 0), Quaternion.identity);
                                    MapManager.Instance.door.Add(obj);
                                }
                                else
                                {
                                    WallDoor obj = Instantiate(sideWall, new Vector3(i + 0.53f, midPos.y + 1.345f, 0), Quaternion.identity);
                                    obj.GetComponent<SpriteRenderer>().flipX = true;
                                    MapManager.Instance.door.Add(obj);
                                }
                            }

                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == leftWallTile)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y - 2, 0), corner1);
                    else if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == righttWalltile)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y - 2, 0), corner2);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 1, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y - 1, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 0, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 0, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 1, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 1, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 2, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 2, 0), null);
                    roomTilemap.Tile.SetTile(new Vector3Int(i, (int)midPos.y - 1, 0), loadtile);
                    roomTilemap.Tile.SetTile(new Vector3Int(i, (int)midPos.y + 0, 0), loadtile);
                    roomTilemap.Tile.SetTile(new Vector3Int(i, (int)midPos.y + 1, 0), loadtile);
                }
            }
        }

        bool isCreateTopWall = false;

        if (start.y > midPos.y)
        {
            for (int i = (int)midPos.y - 1; i < (int)start.y; i++)
            {
                if (roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x, i, 0)) == null)
                {
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 1, i, 0)) != null)
                        if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 1, i, 0)) != null)
                            if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x, i, 0)) != null)
                            {
                                if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 1, i, 0)) == bottomtile)
                                {
                                    WallDoor obj = Instantiate(bottomWall, new Vector3((int)midPos.x + 0.5f, i + 0.4687f, 0), Quaternion.identity);
                                    MapManager.Instance.door.Add(obj);
                                }
                                else
                                {
                                    if (!isCreateTopWall)
                                    {
                                        WallDoor obj = Instantiate(topWall, new Vector3((int)midPos.x + 0.5f, i + 0.05f, 0), Quaternion.identity);
                                        MapManager.Instance.door.Add(obj);
                                        isCreateTopWall = true;
                                    }

                                }

                            }


                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 2, i, 0)) == bottomtile)
                        roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x - 2, i, 0), corner1);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 1, i, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x - 1, i, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 0, i, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x + 0, i, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 1, i, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x + 1, i, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 2, i, 0)) == bottomtile)
                        roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x + 2, i, 0), corner2);
                    roomTilemap.Tile.SetTile(new Vector3Int((int)midPos.x - 1, i, 0), loadtile);
                    roomTilemap.Tile.SetTile(new Vector3Int((int)midPos.x + 0, i, 0), loadtile);
                    roomTilemap.Tile.SetTile(new Vector3Int((int)midPos.x + 1, i, 0), loadtile);
                }
            }
        }
        else
        {
            for (int i = (int)midPos.y + 1; i > (int)start.y; i--)
            {
                if (roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x, i, 0)) == null)
                {
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 1, i, 0)) != null)
                        if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 1, i, 0)) != null)
                            if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x, i, 0)) != null)
                            {
                                if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 1, i, 0)) == bottomtile)
                                {
                                    WallDoor obj = Instantiate(bottomWall, new Vector3((int)midPos.x + 0.5f, i + 0.41f, 0), Quaternion.identity);
                                    MapManager.Instance.door.Add(obj);
                                }
                                else
                                {
                                    if (!isCreateTopWall)
                                    {
                                        WallDoor obj = Instantiate(topWall, new Vector3((int)midPos.x + 0.5f, i + 0.05f, 0), Quaternion.identity);
                                        MapManager.Instance.door.Add(obj);
                                        isCreateTopWall = true;
                                    }
                                }
                            }

                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 1, i, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x - 1, i, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 0, i, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x + 0, i, 0), null);
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 1, i, 0)) != null)
                        roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x + 1, i, 0), null);
                    roomTilemap.Tile.SetTile(new Vector3Int((int)midPos.x - 1, i, 0), loadtile);
                    roomTilemap.Tile.SetTile(new Vector3Int((int)midPos.x + 0, i, 0), loadtile);
                    roomTilemap.Tile.SetTile(new Vector3Int((int)midPos.x + 1, i, 0), loadtile);
                }
            }
        }

        bool isFirst = true;
        if (end.x > midPos.x)
        {
            for (int i = (int)midPos.x - 1; i < (int)end.x; i++)
            {
                bool isEnd = true;
                if (roomTilemap.Tile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == null &&
                    roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == null)
                {
                    isEnd = false;
                    roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y - 2, 0), bottomtile);
                }

                if (roomTilemap.Tile.GetTile(new Vector3Int(i, (int)midPos.y + 2, 0)) == null &&
                    roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 2, 0)) == null)
                {
                    if (isFirst)
                    {
                        if (start.y < midPos.y)
                        {
                            roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 2, 0), top2tile);
                            roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 3, 0), top1tile);
                        }
                        else
                        {
                            roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 2, 0), leftEndWall1);
                            roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 3, 0), leftEndWall2);
                        }
                        isFirst = false;
                    }
                    else
                    {
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 2, 0), top2tile);
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 3, 0), top1tile);
                    }
                    isEnd = false;
                }

                if (isEnd && !isFirst)
                {
                    roomTilemap.WallTile.SetTile(new Vector3Int(i - 1, (int)midPos.y + 2, 0), rightEndWall1);
                    roomTilemap.WallTile.SetTile(new Vector3Int(i - 1, (int)midPos.y + 3, 0), rightEndWall2);
                    break;
                }
            }
        }
        else
        {
            for (int i = (int)midPos.x + 1; i > (int)end.x; i--)
            {
                bool isEnd = true;
                if (roomTilemap.Tile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == null &&
                    roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y - 2, 0)) == null)
                {
                    roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y - 2, 0), bottomtile);
                    isEnd = false;
                }

                if (roomTilemap.Tile.GetTile(new Vector3Int(i, (int)midPos.y + 2, 0)) == null &&
                    roomTilemap.WallTile.GetTile(new Vector3Int(i, (int)midPos.y + 2, 0)) == null)
                {
                    if (isFirst)
                    {
                        if (start.y < midPos.y)
                        {
                            roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 2, 0), top2tile);
                            roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 3, 0), top1tile);
                        }
                        else
                        {
                            roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 2, 0), rightEndWall1);
                            roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 3, 0), rightEndWall2);
                        }
                        isFirst = false;
                    }
                    else
                    {
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 2, 0), top2tile);
                        roomTilemap.WallTile.SetTile(new Vector3Int(i, (int)midPos.y + 3, 0), top1tile);
                    }
                    isEnd = false;
                }

                if (isEnd && !isFirst)
                {
                    roomTilemap.WallTile.SetTile(new Vector3Int(i + 1, (int)midPos.y + 2, 0), leftEndWall1);
                    roomTilemap.WallTile.SetTile(new Vector3Int(i + 1, (int)midPos.y + 3, 0), leftEndWall2);
                    break;
                }
            }
        }

        if (start.y > midPos.y)
        {
            for (int i = (int)midPos.y - 1; i < (int)start.y; i++)
            {
                if (roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x + 2, i, 0)) == null &&
                    roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 2, i, 0)) == null)
                    roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x + 2, i, 0), righttWalltile);

                if (roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x - 2, i, 0)) == null &&
                    roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 2, i, 0)) == null)
                    roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x - 2, i, 0), leftWallTile);
            }
        }
        else
        {
            for (int i = (int)midPos.y + 3; i > (int)start.y; i--)
            {
                if (roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x + 2, i, 0)) == null &&
                    roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 2, i, 0)) == null)
                    roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x + 2, i, 0), righttWalltile);

                if (roomTilemap.Tile.GetTile(new Vector3Int((int)midPos.x - 2, i, 0)) == null &&
                    roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 2, i, 0)) == null)
                    roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x - 2, i, 0), leftWallTile);

            }
        }

        if (start.y < midPos.y)
        {
            if (midPos.x < (int)start.x)
            {
                if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x - 2, (int)midPos.y - 2, 0)) != null)
                    roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x - 2, (int)midPos.y - 2, 0), corner1);
            }
            else
            {
                if (roomTilemap.WallTile.GetTile(new Vector3Int((int)midPos.x + 2, (int)midPos.y - 2, 0)) != null)
                    roomTilemap.WallTile.SetTile(new Vector3Int((int)midPos.x + 2, (int)midPos.y - 2, 0), corner2);
            }
        }

        //Instantiate(debugObject, midPos, Quaternion.identity);
    }

    //길 버그 해결용
    bool CanCreate(Vector2 start, float end, Dir dir)
    {
        int cnt = 0;
        switch (dir)
        {
            case Dir.left:
                while (start.x > end)
                {
                    start.x--;
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 1, 0)) != null)
                        if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 2, 0)) != null)
                            if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 2, 0)) != null)
                                if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 1, 0)) != null)
                                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 3, 0)) != null)
                                        if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 3, 0)) != null)
                                            if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y, 0)) != null)
                                            {
                                                cnt++;
                                                continue;
                                            }

                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 1, 0)) != null ||
                      roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 2, 0)) != null ||
                      roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 3, 0)) != null ||
                      roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 3, 0)) != null ||
                      roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 2, 0)) != null ||
                      roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 1, 0)) != null ||
                      roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y, 0)) != null)
                        return false;
                }
                break;
            case Dir.right:
                while (start.x < end)
                {
                    start.x++;
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 1, 0)) != null)
                        if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 1, 0)) != null)
                            if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 2, 0)) != null)
                                if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 3, 0)) != null)
                                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 2, 0)) != null)
                                        if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 3, 0)) != null)
                                            if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y, 0)) != null)
                                            {
                                                cnt++;
                                                continue;
                                            }

                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 1, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 1, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 2, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y - 3, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 3, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y + 2, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y, 0)) != null)
                        return false;
                }
                break;
            case Dir.top:
                while (start.y < end)
                {
                    start.y++;
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x + 1, (int)start.y, 0)) != null)
                        if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x - 1, (int)start.y, 0)) != null)
                            if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x + 2, (int)start.y, 0)) != null)
                                if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x - 2, (int)start.y, 0)) != null)
                                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y, 0)) != null)
                                    {
                                        cnt++;
                                        continue;
                                    }

                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x + 1, (int)start.y, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x - 1, (int)start.y, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x - 2, (int)start.y, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x + 2, (int)start.y, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y, 0)) != null)
                        return false;
                }
                break;
            case Dir.bottom:
                while (start.y > end)
                {
                    start.y--;
                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x + 1, (int)start.y, 0)) != null)
                        if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x - 2, (int)start.y, 0)) != null)
                            if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x + 2, (int)start.y, 0)) != null)
                                if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x - 1, (int)start.y, 0)) != null)
                                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y, 0)) != null)
                                    {
                                        cnt++;
                                        continue;
                                    }

                    if (roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x + 1, (int)start.y, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x - 2, (int)start.y, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x + 2, (int)start.y, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x - 1, (int)start.y, 0)) != null ||
                        roomTilemap.WallTile.GetTile(new Vector3Int((int)start.x, (int)start.y, 0)) != null)
                        return false;
                }
                break;
        }

        if (cnt > 3)
            return false;

        Debug.Log(cnt);
        return true;
    }
}

//엔드포스에 겟타일 해보기