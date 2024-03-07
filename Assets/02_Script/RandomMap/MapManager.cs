using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [NonSerialized]
    public RoomGenarator roomGenarator;

    [SerializeField] private MovePortal _portalPrefab;
    [SerializeField] PolygonCollider2D _vcamConfiner;
    [SerializeField] private float plusValue = 1.5f;
    [SerializeField] CinemachineConfiner2D _cmConfiner;

    public WallDoor[] door;

    Vector2 centerPos;
    public Vector2 CenterPos => centerPos;

    int curIdxX;
    public int CurIdxX => curIdxX;
    int curIdxY;
    public int CurIdxY => curIdxY;


    int correctX;
    public int CorrectX => correctX;
    int correctY;
    public int CorrectY => correctY;

    public Roomsize GetRoomSize()
    {
        return roomGenarator.checkRoom[curIdxY, curIdxX];
    }

    private void Awake()
    {
        #region 예외처리
        if (Instance != null)
        {
            Debug.LogError($"{transform} : MapManger is Multiple running!");
            Destroy(gameObject);
        }
        else
            Instance = this;

        if (_vcamConfiner == null)
        {
            Debug.LogError($"{transform} : _vcamConfiner is null");
        }

        if (roomGenarator == null)
            roomGenarator = GetComponent<RoomGenarator>();

        #endregion

        correctX = roomGenarator.Width / 2;
        correctY = roomGenarator.Height / 2;

        curIdxX = correctX;
        curIdxY = correctY;
    }

    private void Start()
    {
        centerPos = new Vector2(roomGenarator.WidthLength * (curIdxX - correctX),
            roomGenarator.HeightLength * (curIdxY - correctY));

        SetConfiner();
    }

    public void RoomClear()
    {
        if (roomGenarator.spawnType == MapSpawnType.BSP)
        {

        }
        else if(roomGenarator.spawnType == MapSpawnType.TP)
        {
            centerPos = new Vector2(roomGenarator.WidthLength * (curIdxX - correctX),
                roomGenarator.HeightLength * (curIdxY - correctY));

            int x = roomGenarator.checkRoom[curIdxY, curIdxX].width / 2 - roomGenarator.PortalLenth;
            int y = roomGenarator.checkRoom[curIdxY, curIdxX].height / 2 - roomGenarator.PortalLenth;

            if (roomGenarator.checkRoom[curIdxY + 1, curIdxX] != null)
            {
                MovePortal obj = Instantiate(_portalPrefab);
                obj.dir = MoveDir.up;
                obj.transform.position = new Vector2(centerPos.x, centerPos.y + y);
            }

            if (roomGenarator.checkRoom[curIdxY - 1, curIdxX] != null)
            {
                MovePortal obj = Instantiate(_portalPrefab);
                obj.dir = MoveDir.down;
                obj.transform.position = new Vector2(centerPos.x, centerPos.y - y);
            }

            if (roomGenarator.checkRoom[curIdxY, curIdxX + 1] != null)
            {
                MovePortal obj = Instantiate(_portalPrefab);
                obj.dir = MoveDir.right;
                obj.transform.position = new Vector2(centerPos.x + x, centerPos.y);
            }

            if (roomGenarator.checkRoom[curIdxY, curIdxX - 1] != null)
            {
                MovePortal obj = Instantiate(_portalPrefab);
                obj.dir = MoveDir.left;
                obj.transform.position = new Vector2(centerPos.x - x, centerPos.y);
            }
        }
    }

    public void RoomMove(MoveDir dir)
    {
        switch (dir)
        {
            case MoveDir.left:
                curIdxX--;
                break;
            case MoveDir.right:
                curIdxX++;
                break;
            case MoveDir.up:
                curIdxY++;
                break;
            case MoveDir.down:
                curIdxY--;
                break;
        }

        centerPos = new Vector2(roomGenarator.WidthLength * (curIdxX - correctX),
            roomGenarator.HeightLength * (curIdxY - correctY));

        MonsterSpawnManager.Instance.monsterSpawn.StartSpawn();

        SetConfiner();
    }

    void SetConfiner()
    {
        Vector2[] newpoints = { };
        switch (roomGenarator.spawnType)
        {
            case MapSpawnType.Load:
            case MapSpawnType.BSP:
                {
                    newpoints = new Vector2[]
                    {
                     new Vector2(100000, 100000),
                     new Vector2(-100000, 100000),
                     new Vector2(-100000, -100000),
                     new Vector2(100000, -100000)
                    };
                }
                break;
            case MapSpawnType.TP:
                {
                    int wid = roomGenarator.checkRoom[curIdxY, curIdxX].width / 2;
                    int hei = roomGenarator.checkRoom[curIdxY, curIdxX].height / 2;
                    newpoints = new Vector2[]
                    {
                  new Vector2((centerPos.x + wid) + plusValue,
                  (centerPos.y + hei) + plusValue),
                  new Vector2((centerPos.x - wid) - plusValue,
                  (centerPos.y + hei) + plusValue),
                  new Vector2((centerPos.x - wid) - plusValue,
                  (centerPos.y - hei) - 2 - plusValue),
                  new Vector2((centerPos.x + wid) + plusValue,
                  (centerPos.y - hei) - 2 - plusValue)
                    };
                }
                break;
        }
        _vcamConfiner.points = newpoints;
        _cmConfiner.InvalidateCache();
    }
}
