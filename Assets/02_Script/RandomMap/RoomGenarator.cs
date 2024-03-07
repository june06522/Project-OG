using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Roomsize
{
    public int width;
    public int height;
}

public enum MapSpawnType
{
    TP,
    Load,
    BSP
}


public class RoomGenarator : MonoBehaviour
{
    [Header("보드 사이즈")]
    [SerializeField] int width;
    [SerializeField] int height;
    public int Width => width;
    public int Height => height;
    [SerializeField] int widthLength;
    [SerializeField] int heightLength;
    public int WidthLength => widthLength;
    public int HeightLength => heightLength;


    [Header("사이즈")]
    public Roomsize smallRoom;
    public Roomsize normalRoom;
    public Roomsize bigRoom;



    [Header("길 길이")]
    [SerializeField] int loadLength = 15;

    [Header("포탈 - 벽 길이")]
    [SerializeField] int portalLenth = 2;
    public int PortalLenth => portalLenth;

    [Header("뒷배경 길이")]
    [SerializeField] int bgLenth = 2;
    public int BGLenth => bgLenth;

    [Header("방 갯수")]
    [SerializeField] int normalRoomCnt = 5;

    [Header("맵 타입")]
    public MapSpawnType spawnType = MapSpawnType.Load;

    [HideInInspector] public List<RoomInfo> useRooms = new List<RoomInfo>();
    [HideInInspector] public Roomsize[,] checkRoom;
    List<RoomInfo> roomInfos = new List<RoomInfo>();

    [HideInInspector] public RoomTileMap roomTilemap;

    private void Awake()
    {
        roomTilemap = GetComponent<RoomTileMap>();

        #region 예외처리

        if (smallRoom.width % 2 != 0)
        {
            Debug.LogWarning($"Small Room width Size is odd number : {smallRoom.width}, You should change Even number");
            smallRoom.width++;
        }

        if (smallRoom.height % 2 != 0)
        {
            Debug.LogWarning($"Small Room height Size is odd number : {smallRoom.height}, You should change Even number");
            smallRoom.height++;
        }

        if (normalRoom.width % 2 != 0)
        {
            Debug.LogWarning($"Normal Room width Size is odd number : {normalRoom.width}, You should change Even number");
            normalRoom.width++;
        }

        if (normalRoom.height % 2 != 0)
        {
            Debug.LogWarning($"Normal Room height Size is odd number : {normalRoom.height}, You should change Even number");
            normalRoom.height++;
        }

        if (bigRoom.width % 2 != 0)
        {
            Debug.LogWarning($"Big Room width Size is odd number : {bigRoom.width}, You should change Even number");
            bigRoom.width++;
        }

        if (bigRoom.height % 2 != 0)
        {
            Debug.LogWarning($"Big Room height Size is odd number : {bigRoom.height}, You should change Even number");
            bigRoom.height++;
        }

        #endregion
    }

    private void Start()
    {
        CreateBoard();
        ShuffleAlgorithm();

        if (spawnType == MapSpawnType.Load)
            SelectBoardLoad ();
        else if(spawnType == MapSpawnType.TP)
            SelectBoardPotal();
        MonsterSpawnManager.Instance.DecideWave(useRooms, height, width);
        roomTilemap.SetTileMap();
        //MapManager.Instance.RoomClear();
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

    void SelectBoardPotal()
    {
        checkRoom = new Roomsize[height, width];

        int correctionX = width / 2;
        int correctionY = height / 2;
        int cnt = 0;

        checkRoom[correctionY, correctionX] = bigRoom;

        while (cnt < normalRoomCnt)
        {
            RoomInfo temp = roomInfos[0];
            roomInfos.Remove(temp);

            int x = temp.x + correctionX;
            int y = temp.y + correctionY;

            //인접하는 방
            int adjCnt = 0;

            if (y < height - 1 && checkRoom[y + 1, x] != null)
                adjCnt++;
            if (y > 0 && checkRoom[y - 1, x] != null)
                adjCnt++;
            if (x < width - 1 && checkRoom[y, x + 1] != null)
                adjCnt++;
            if (x > 0 && checkRoom[y, x - 1] != null)
                adjCnt++;

            if (adjCnt == 1)
            {
                int ranVal = Random.Range(0, 3);

                if (ranVal == 0)
                {
                    checkRoom[y, x] = smallRoom;

                }
                else if (ranVal == 1)
                {
                    checkRoom[y, x] = normalRoom;

                }
                else if (ranVal == 2)
                {
                    checkRoom[y, x] = bigRoom;

                }

                useRooms.Add(temp);
                cnt++;
            }
            else
            {
                roomInfos.Add(temp);
            }
        }
    }

    void SelectBoardLoad()
    {
        checkRoom = new Roomsize[height, width];

        int correctionX = width / 2;
        int correctionY = height / 2;
        int cnt = 0;

        checkRoom[correctionY, correctionX] = bigRoom;

        while (cnt < normalRoomCnt)
        {
            RoomInfo temp = roomInfos[0];
            roomInfos.Remove(temp);

            int x = temp.x + correctionX;
            int y = temp.y + correctionY;

            //인접하는 방
            int adjCnt = 0;
            
            if (y < height - 1 && checkRoom[y + 1, x] != null)
                adjCnt++;
            if (y > 0 && checkRoom[y - 1, x] != null)
                adjCnt++;
            if (x < width - 1 && checkRoom[y, x + 1] != null)
                adjCnt++;
            if (x > 0 && checkRoom[y, x - 1] != null)
                adjCnt++;

            if (adjCnt >= 1)
            {
                int ranVal = Random.Range(0, 3);

                if (ranVal == 0)
                {
                    checkRoom[y, x] = smallRoom;

                }
                else if (ranVal == 1)
                {
                    checkRoom[y, x] = normalRoom;

                }
                else if (ranVal == 2)
                {
                    checkRoom[y, x] = bigRoom;
                }

                #region 길 셋팅
                LoadInfo loadInfo = new LoadInfo();
                loadInfo.x = x;
                loadInfo.y = y;

                if (y < height - 1 && checkRoom[y + 1, x] != null)
                {
                    loadInfo.targetX = x;
                    loadInfo.targetY = y + 1;
                }
                ranVal = Random.Range(0, 2);
                if (y > 0 && checkRoom[y - 1, x] != null && (loadInfo.targetX == -1 || ranVal == 1))
                {
                    loadInfo.targetX = x;
                    loadInfo.targetY = y - 1;
                }
                ranVal = Random.Range(0, 2);
                if (x < width - 1 && checkRoom[y, x + 1] != null && (loadInfo.targetX == -1 || ranVal == 1))
                {
                    loadInfo.targetX = x + 1;
                    loadInfo.targetY = y;
                }
                ranVal = Random.Range(0, 2);
                if (x > 0 && checkRoom[y, x - 1] != null && (loadInfo.targetX == -1 || ranVal == 1))
                {
                    loadInfo.targetX = x - 1;
                    loadInfo.targetY = y;
                }

                roomTilemap.loadsInfo.Add(loadInfo);
                #endregion

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