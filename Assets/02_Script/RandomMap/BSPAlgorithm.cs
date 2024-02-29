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
        return new Vector2Int(roomRect.x + roomRect.width / 2, roomRect.y + roomRect.height / 2);
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



    private void Awake()
    {
        roomTilemap = GetComponent<RoomTileMap>();
        roomList = new List<BSPRoomInfo>();
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

        //삼각형 그리기


        //길 선택

        //길 그리기
    }

    public bool CheckOverlap(RectInt rect1, RectInt rect2)
    {
        // 조건1: 첫번째 사각형의 왼쪽 모서리가 두번째 사각형의 오른쪽 모서리를 넘지 않는다.
        bool condition1 = rect1.x - minlen < rect2.x + rect2.width;

        // 조건2: 첫번째 사각형의 오른쪽 모서리가 두번째 사각형의 왼쪽 모서리를 넘어야 한다.
        bool condition2 = rect1.x + rect1.width > rect2.x - minlen;

        // 조건3: 첫번째 사각형의 위쪽 모서리가 두번째 사각형의 아래쪽 모서리를 넘지 않는다.
        bool condition3 = rect1.y - minlen < rect2.y + rect2.height;

        // 조건4: 첫번째 사각형의 아래쪽 모서리가 두번째 사각형의 위쪽 모서리를 넘어야 한다.
        bool condition4 = rect1.y + rect1.height > rect2.y - minlen;

        // 모든 조건이 만족되면 두 사각형이 겹침
        return condition1 && condition2 && condition3 && condition4;
    }
}