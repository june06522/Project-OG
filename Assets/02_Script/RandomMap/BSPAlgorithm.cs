using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public bool CheckContain(RectInt r)
    {
        // 두 영역이 겹치는지 확인
        bool overlapX = (roomRect.x > r.x) && (roomRect.x < r.x + r.width);
        overlapX |= (roomRect.x + roomRect.width > r.x) && (roomRect.x + roomRect.width < r.x + r.width);
        bool overlapY = (roomRect.y > r.y) && (roomRect.y < r.y + r.height);
        overlapY |= (roomRect.y + roomRect.height > r.y) && (roomRect.y + roomRect.height < r.y + r.height);

        // 두 방향 모두 겹치면 true를 반환
        return overlapX && overlapY;
    }
}

public class BSPAlgorithm : MonoBehaviour
{
    [SerializeField] int roomCnt;

    [SerializeField] int xlen;
    [SerializeField] int ylen;

    [SerializeField] int minlen = 5;

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
        for (int i = 0; i < roomCnt; i++)
        {
            roomList.Add(new BSPRoomInfo(roomTilemap.roomsList[i],
                Random.Range(-xlen, xlen), Random.Range(-ylen, ylen)));
        }

        //안곂치게 방 밀어내기
        bool isClear = false;
        while(!isClear)
        {
            isClear = true;
            for (int i = 0; i < roomCnt; i++)
            {
                for (int j = 0; j < roomCnt; j++)
                {
                    if (i == j)
                        continue;
                    while (CheckOverlap(roomList[i].roomRect, roomList[j].roomRect))
                    {
                        isClear = false;
                        if (roomList[i].roomRect.x > roomList[j].roomRect.x)
                            roomList[i].roomRect.x += Random.Range(5, 15);
                        else
                            roomList[i].roomRect.x -= Random.Range(5, 15);

                        if (roomList[i].roomRect.y > roomList[j].roomRect.y)
                            roomList[i].roomRect.y += Random.Range(5, 15);
                        else
                            roomList[i].roomRect.y -= Random.Range(5, 15);
                    }
                }
            }
        }

        // 방 그리기
        for (int i = 0; i < roomCnt; i++)
        {
            roomTilemap.SetCustomRoom(roomList[i]);
        }

        // 각 방의 가운데 점 정보 가져오기
        List<Vector2> centerPos = new List<Vector2>();
        for (int i = 0; i < roomCnt; i++)
        {
            centerPos.Add(roomList[i].GetCenterPos());
        }

        //삼각형 그리기 들로네 삼각분할
        List<Triangle> triangles = delaunayTriangulation.Triangulation(centerPos);

        //로그 확인
        StartCoroutine(Debuging(triangles));

        //선택되지 않은 방 삭제 <- 이건 나중에 들로네 삼각분할에 버그 못 고치면 최후의 수단

        //길 선택 최소스패닝 트리


        //길 그리기

    }

    IEnumerator Debuging(List<Triangle> triangles)
    {
        while (true)
        {
            for(int i = 0; i < triangles.Count; i++)
            {
                Debug.DrawLine(triangles[i].a, triangles[i].b,Color.white, 3f);
                Debug.DrawLine(triangles[i].b, triangles[i].c,Color.white, 3f);
                Debug.DrawLine(triangles[i].c, triangles[i].a,Color.white, 3f);
                
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
}