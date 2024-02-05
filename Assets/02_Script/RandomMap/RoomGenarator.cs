using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MapSpawnType
{
    Load = 0,
    Potal = 1,
    Stuck = 2
}

public class RoomGenarator : MonoBehaviour
{
    [Header("보드 사이즈")]
    [SerializeField] int width;
    [SerializeField] int height;

    [Header("방 갯수")]
    [SerializeField] int normalRoomCnt;

    [Header("맵 타입")]
    [SerializeField] MapSpawnType spawnType = MapSpawnType.Load;

    List<RoomInfo> roomInfos = new List<RoomInfo>();
    List<RoomInfo> useRooms = new List<RoomInfo>();

    private void Start()
    {
        CreateBoard();
        ShuffleAlgorithm();
        SelectBoard();
    }

    void CreateBoard()
    {
        // 보드생성
        for (int i = -(height / 2); i < (height / 2); i++)
        {
            for (int j = -(width / 2); j < (width / 2); j++)
            {
                if (i == 0 && j == 0)
                    continue;

                RoomInfo temproom = new RoomInfo();
                temproom.x = j; // 가로
                temproom.y = i; // 세로
                roomInfos.Add(temproom);
            }
        }
    }

    void ShuffleAlgorithm()
    {
        //셔플 알고리즘
        for (int i = 0; i < width * height * 3; i++)
        {
            int tempIdx1 = Random.Range(0, roomInfos.Count - 1);
            int tempIdx2 = Random.Range(0, roomInfos.Count - 1);

            RoomInfo temp = roomInfos[tempIdx1];
            roomInfos[tempIdx1] = roomInfos[tempIdx2];
            roomInfos[tempIdx2] = temp;
        }
    }

    void SelectBoard()
    {
        int correctionX = width / 2;
        int correctionY = height / 2;
        bool[,] checkRoom = new bool[height, width];
        int cnt = 0;

        checkRoom[correctionY, correctionX] = true;

        while (cnt < normalRoomCnt)
        {
            RoomInfo temp = roomInfos[0];
            roomInfos.Remove(temp);

            int x = temp.x + correctionX;
            int y = temp.y + correctionY;

            if ((y < height - 1 && checkRoom[y + 1, x]) ||
                (y > 0          && checkRoom[y - 1, x]) ||
                (x < width - 1  && checkRoom[y, x + 1]) ||
                (x > 0          && checkRoom[y, x - 1]))
            {
                checkRoom[y, x] = true;
                useRooms.Add(temp);
                cnt++;
            }
            else
            {
                roomInfos.IndexOf(temp);
            }
        }

        string log = "";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (checkRoom[i, j])
                    log += 5;
                else
                    log += 0;
            }
            log += "\n";
        }
        Debug.Log(log);
    }
}