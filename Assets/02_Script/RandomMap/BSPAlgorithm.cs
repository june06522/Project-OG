using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
        return ((roomRect.x > r.x && roomRect.x < r.x + r.width) ||
                (roomRect.x + roomRect.width > r.x && roomRect.x + roomRect.width < r.x + r.width) ||
                (roomRect.y > r.y && roomRect.y < r.y + r.height) ||
                (roomRect.y + roomRect.height > r.y && roomRect.y + roomRect.height < r.y + r.height));
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
        for (int i = 0; i < roomCnt; i++)
        {
            for (int j = 0; j < i; j++)
            {
                while (roomList[i].CheckContain(roomList[j].roomRect))
                {
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

        // 방 그리기
        for (int i = 0; i < roomCnt; i++)
        {
            roomTilemap.SetCustomRoom(roomList[i]);
        }

        //삼각형 그리기

        //길 선택

        //길 그리기
    }
}