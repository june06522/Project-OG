using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapSpawnType
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
    public int Width => width;
    public int Height => height;

    [Header("방 사이즈")]
    [SerializeField] int roomWidth;
    [SerializeField] int roomHeight;
    public int RoomWidth => roomWidth;
    public int RoomHeight => roomHeight;

    [Header("길 길이")]
    [SerializeField] int loadLength = 15;

    [Header("포탈 - 벽 길이")]
    [SerializeField] int portalLenth = 2;
    public int PortalLenth => portalLenth;

    [Header("뒷배경 길이")]
    [SerializeField] int bgLenth = 2;
    public int BGLenth => bgLenth;

    [Header("방 갯수")]
    [SerializeField] int normalRoomCnt;

    [Header("맵 타입")]
    public MapSpawnType spawnType = MapSpawnType.Load;

    [HideInInspector] public List<RoomInfo> useRooms = new List<RoomInfo>();
    [HideInInspector] public bool[,] checkRoom;
    List<RoomInfo> roomInfos = new List<RoomInfo>();

    [HideInInspector] public RoomTileMap roomTilemap;

    private void Awake()
    {
        roomTilemap = GetComponent<RoomTileMap>();

        if (RoomWidth % 2 != 0)
        {
            Debug.LogError($"RoomWidth is odd number : {RoomWidth}, You should change Even number");
            roomWidth++;
        }

        if (RoomHeight % 2 != 0)
        {
            Debug.LogError($"RoomHeight is odd number : {RoomHeight}, You should change Even number");
            roomHeight++;
        }
    }

    private void Start()
    {
        CreateBoard();
        ShuffleAlgorithm();
        SelectBoard();
        MonsterSpawnManager.Instance.DecideWave(useRooms,height,width);
        roomTilemap.SetTileMap();
        MapManager.Instance.RoomClear();
    }

    void CreateBoard()
    {
        // 보드생성
        for (int i = -(height / 2); i < (height / 2); i++)
        {
            for (int j = -(width / 2); j < (width / 2); j++)
            {
                RoomInfo temproom = new RoomInfo();
                temproom.x = j; // 가로
                temproom.y = i; // 세로

                if (i == 0 && j == 0)
                {
                    useRooms.Add(temproom);
                }
                else
                {
                    roomInfos.Add(temproom);
                }
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
        checkRoom = new bool[height, width];

        int correctionX = width / 2;
        int correctionY = height / 2;
        int cnt = 0;

        checkRoom[correctionY, correctionX] = true;

        while (cnt < normalRoomCnt)
        {
            RoomInfo temp = roomInfos[0];
            roomInfos.Remove(temp);

            int x = temp.x + correctionX;
            int y = temp.y + correctionY;

            //인접하는 방
            int adjCnt = 0;

            if (y < height - 1 && checkRoom[y + 1, x])
                adjCnt++;
            if (y > 0 && checkRoom[y - 1, x])
                adjCnt++;
            if (x < width - 1 && checkRoom[y, x + 1])
                adjCnt++;
            if (x > 0 && checkRoom[y, x - 1])
                adjCnt++;

            if (adjCnt == 1)
            {
                checkRoom[y, x] = true;
                useRooms.Add(temp);
                cnt++;
            }
            else
            {
                roomInfos.Add(temp);
            }
        }
    }
}