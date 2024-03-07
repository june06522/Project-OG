using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LoadInfo
{
    public int x;
    public int y;
    public int targetX = -1;
    public int targetY;
}

public class RoomTileMap : MonoBehaviour
{
    RoomGenarator roomGenarator;

    [Header("타일맵")]
    [SerializeField] Tilemap tile;
    public Tilemap Tile => tile;
    [SerializeField] Tilemap walltile;
    public Tilemap WallTile => walltile;

    [Header("타일")]
    [SerializeField] Tile[] wall;
    [SerializeField] Tile[] round;
    [SerializeField] Tile[] bottomWall;
    [SerializeField] Tile loadtile;
    [SerializeField] Tile bgTile;

    [Header("커스텀 방")]
    [SerializeField] List<CustomRoom> rooms;
    public List<CustomRoom> roomsList => rooms;

    public CustomRoom startMap;
    public CustomRoom shopMap;


    public List<LoadInfo> loadsInfo = new List<LoadInfo>();
    private Vector2[,] centerPos;

    private void Awake()
    {
        #region 예외처리

        if (wall.Length < 4)
        {
            Debug.LogError($"wall Tile is too less : {wall.Length} You need more {4 - wall.Length} tile");
        }

        if (round.Length < 4)
        {
            Debug.LogError($"round Tile is too less : {round.Length} You need more {4 - round.Length} tile");
        }

        if (bottomWall.Length < 8)
        {
            Debug.LogError($"bottomWall Tile is too less : {bottomWall.Length} You need more {4 - bottomWall.Length} tile");
        }

        #endregion

        roomGenarator = GetComponent<RoomGenarator>();
        centerPos = new Vector2[roomGenarator.Width, roomGenarator.Height];

        Shuffle();
    }

    private void Start()
    {
        //BGTileSetting();
    }

    private void BGTileSetting()
    {
        //int x = roomGenarator.Width * roomGenarator.WidthLength / 2;
        //int y = roomGenarator.Height * roomGenarator.HeightLength / 2;

        int x = 500;
        int y = 500;

        for (int i = -x; i < x; i++)
        {
            for (int j = -y; j < y; j++)
            {
                tile.SetTile(new Vector3Int(i, j, 0), bgTile);
            }
        }

    }

    public void SetTileMap()
    {
        if (roomGenarator.spawnType == MapSpawnType.TP)
            PoltarRoom();
        else if(roomGenarator.spawnType == MapSpawnType.Load)
            LoadRoom();
    }


    private void Shuffle()
    {
        CustomRoom temp;
        int randA = 0;
        int randB = 0;
        for (int i = 0; i < rooms.Count * 5; ++i)
        {
            randA = Random.Range(0, rooms.Count);
            randB = Random.Range(0, rooms.Count);
            temp = rooms[randA];
            rooms[randA] = rooms[randB];
            rooms[randB] = temp;
        }
    }

    private void PoltarRoom()
    {
        for (int i = 0; i < roomGenarator.useRooms.Count(); ++i)
        {
            if (rooms.Count > 0 && i != 0)
            {
                SetCustomRoom(i);
            }
            else
            {
                SetDefaultMap(i);
            }
        }
    }

    private void SetCustomRoom(int i)
    {
        CustomRoom select = rooms[0];
        rooms.Remove(select);

        Roomsize tempRoom = new Roomsize();
        tempRoom.width = select.width;
        tempRoom.height = select.height;

        roomGenarator.checkRoom[roomGenarator.useRooms[i].y + roomGenarator.Height / 2,
            roomGenarator.useRooms[i].x + roomGenarator.Width / 2] = tempRoom;


        int x = roomGenarator.useRooms[i].x * (roomGenarator.WidthLength);
        int y = roomGenarator.useRooms[i].y * (roomGenarator.HeightLength);
        if (roomGenarator.spawnType == MapSpawnType.Load)
        {
            x += Random.Range(-((roomGenarator.WidthLength - select.width) / 2 - 3), (roomGenarator.WidthLength - select.width) / 2 - 3);
            y += Random.Range(-((roomGenarator.HeightLength - select.height) / 2 - 3), (roomGenarator.HeightLength - select.height) / 2 - 3);
        }

        centerPos[roomGenarator.useRooms[i].y + MapManager.Instance.CorrectY,
            roomGenarator.useRooms[i].x + MapManager.Instance.CorrectX] = new Vector2(x, y);
        select.centerPos = new Vector2(x, y);

        for (int k = -select.height / 2; k < select.height / 2; k++)
        {
            for (int j = -select.width / 2; j < select.width / 2; j++)
            {

                if (select.tilemap.GetTile(new Vector3Int(j, k, 0)) != null)
                    tile.SetTile(new Vector3Int(j + x, k + y, 0), select.tilemap.GetTile(new Vector3Int(j, k, 0)));

                if (select.wallTilemap.GetTile(new Vector3Int(j, k, 0)) != null)
                {
                    walltile.SetTile(new Vector3Int(j + x, k + y, 0), select.wallTilemap.GetTile(new Vector3Int(j, k, 0)));
                }
            }
        }

        Transform trm = select.obstacleParent.transform;
        for (int j = 0; j < trm.childCount; j++)
        {
            Transform t = trm.GetChild(j);
            Transform obj = Instantiate(t,
                new Vector3(t.position.x + x, t.position.y + y, 0), Quaternion.identity);
        }
    }

    public void SetCustomRoom(BSPRoomInfo roomInfo)
    {
        CustomRoom select = roomInfo.room;
        rooms.Remove(select);

        Roomsize tempRoom = new Roomsize();
        tempRoom.width = select.width;
        tempRoom.height = select.height;

        

        for (int k = -select.height / 2; k < select.height / 2; k++)
        {
            for (int j = -select.width / 2; j < select.width / 2; j++)
            {

                if (select.tilemap.GetTile(new Vector3Int(j, k, 0)) != null)
                    tile.SetTile(new Vector3Int(j + roomInfo.roomRect.x, k + roomInfo.roomRect.y, 0), select.tilemap.GetTile(new Vector3Int(j, k, 0)));

                if (select.wallTilemap.GetTile(new Vector3Int(j, k, 0)) != null)
                {
                    walltile.SetTile(new Vector3Int(j + roomInfo.roomRect.x, k + roomInfo.roomRect.y, 0), select.wallTilemap.GetTile(new Vector3Int(j, k, 0)));
                }
            }
        }

        Transform trm = select.obstacleParent.transform;
        for (int j = 0; j < trm.childCount; j++)
        {
            Transform t = trm.GetChild(j);
            Transform obj = Instantiate(t,
                new Vector3(t.position.x + roomInfo.roomRect.x, t.position.y + roomInfo.roomRect.y, 0), Quaternion.identity);
        }
    }

    private void LoadGenerator()
    {
        for (int i = 0; i < loadsInfo.Count; i++)
        {
            Vector2 pos = centerPos[loadsInfo[i].y, loadsInfo[i].x];
            Vector2 targetpos = centerPos[loadsInfo[i].targetY, loadsInfo[i].targetX];

            int x = Random.Range(0, 2);

            if (x == 0)
            {
                if (CheckWallRow(pos, targetpos))
                    WidthLoad(pos, targetpos);
                else
                    HeightLoad(pos, targetpos);
            }
            else
            {
                 
                if (CheckWallCol(pos, targetpos))
                    HeightLoad(pos, targetpos);
                else
                    WidthLoad(pos, targetpos);
            }
        }
    }

    private void WidthLoad(Vector2 pos, Vector2 targetpos)
    {
        for (float j = Mathf.Min(pos.x, targetpos.x); j < Mathf.Max(pos.x, targetpos.x); j++)
        {
            for (int k = -1; k <= 1; k++)
            {
                tile.SetTile(new Vector3Int((int)j, (int)pos.y + k, 0), loadtile);
                if (walltile.GetTile(new Vector3Int((int)j, (int)pos.y + k, 0)) != null)
                    walltile.SetTile(new Vector3Int((int)j, (int)pos.y + k, 0), null);
            }
        }
        for (float j = Mathf.Min(pos.y, targetpos.y); j < Mathf.Max(pos.y, targetpos.y); j++)
        {
            for (int k = -1; k <= 1; k++)
            {
                tile.SetTile(new Vector3Int((int)targetpos.x + k, (int)j, 0), loadtile);
                if (walltile.GetTile(new Vector3Int((int)targetpos.x + k, (int)j, 0)) != null)
                    walltile.SetTile(new Vector3Int((int)targetpos.x + k, (int)j, 0), null);
            }
        }
    }

    private void HeightLoad(Vector2 pos, Vector2 targetpos)
    {
        for (float j = Mathf.Min(pos.y, targetpos.y); j < Mathf.Max(pos.y, targetpos.y); j++)
        {
            for (int k = -1; k <= 1; k++)
            {
                tile.SetTile(new Vector3Int((int)pos.x + k, (int)j, 0), loadtile);
                if (walltile.GetTile(new Vector3Int((int)pos.x + k, (int)j, 0)) != null)
                    walltile.SetTile(new Vector3Int((int)pos.x + k, (int)j, 0), null);
            }
        }
        for (float j = Mathf.Min(pos.x, targetpos.x); j < Mathf.Max(pos.x, targetpos.x); j++)
        {
            for (int k = -1; k <= 1; k++)
            {
                tile.SetTile(new Vector3Int((int)j, (int)targetpos.y + k, 0), loadtile);
                if (walltile.GetTile(new Vector3Int((int)j, (int)targetpos.y + k, 0)) != null)
                    walltile.SetTile(new Vector3Int((int)j, (int)targetpos.y + k, 0), null);
            }
        }
    }

    private void SetDefaultMap(int i)
    {
        int xLen = roomGenarator.checkRoom[roomGenarator.useRooms[i].y + roomGenarator.Height / 2, roomGenarator.useRooms[i].x + roomGenarator.Width / 2].width / 2;
        int yLen = roomGenarator.checkRoom[roomGenarator.useRooms[i].y + roomGenarator.Height / 2, roomGenarator.useRooms[i].x + roomGenarator.Width / 2].height / 2;
        int x = roomGenarator.useRooms[i].x * (roomGenarator.WidthLength);
        int y = roomGenarator.useRooms[i].y * (roomGenarator.HeightLength);
        for (int j = y - yLen; j < y + yLen; ++j)
        {
            for (int k = x - xLen; k < x + xLen; ++k)
            {
                Vector3Int pos = new Vector3Int(k, j, 0);
                //기본타일
                tile.SetTile(pos, loadtile);

                #region 벽
                //위
                if (j == y + yLen - 1)
                    walltile.SetTile(pos, wall[0]);

                //아래
                if (j == y - yLen)
                {
                    walltile.SetTile(pos, wall[3]);
                    walltile.SetTile(new Vector3Int(pos.x, pos.y - 1, pos.z),
                        bottomWall[(Mathf.Abs(k) % 2) + 1]);
                    walltile.SetTile(new Vector3Int(pos.x, pos.y - 2, pos.z),
                        bottomWall[(Mathf.Abs(k) % 2) + 5]);
                }

                //왼쪽
                if (k == x - xLen)
                    walltile.SetTile(pos, wall[1]);

                //오른쪽
                if (k == x + xLen - 1)
                    walltile.SetTile(pos, wall[2]);
                #endregion

                #region 모서리
                //왼위
                if (j == y + yLen - 1 && k == x - xLen)
                    walltile.SetTile(pos, round[0]);
                //오위
                if (j == y + yLen - 1 && k == x + xLen - 1)
                    walltile.SetTile(pos, round[1]);
                //왼아래
                if (j == y - yLen && k == x - xLen)
                {
                    walltile.SetTile(pos, round[2]);
                    walltile.SetTile(new Vector3Int(pos.x, pos.y - 1, pos.z),
                        bottomWall[0]);
                    walltile.SetTile(new Vector3Int(pos.x, pos.y - 2, pos.z),
                        bottomWall[4]);
                }
                //오아래
                if (j == y - yLen && k == x + xLen - 1)
                {
                    walltile.SetTile(pos, round[3]);
                    walltile.SetTile(new Vector3Int(pos.x, pos.y - 1, pos.z),
                        bottomWall[3]);
                    walltile.SetTile(new Vector3Int(pos.x, pos.y - 2, pos.z),
                        bottomWall[7]);
                }
                #endregion
            }
        }
    }

    private void LoadRoom()
    {
        for (int i = 0; i < roomGenarator.useRooms.Count(); ++i)
        {
            if (rooms.Count > 0 && i != 0)
            {
                SetCustomRoom(i);
            }
            else
            {
                SetDefaultMap(i);
            }
        }
        LoadGenerator();
    }


    bool CheckWallRow(Vector2 pos, Vector2 targetpos)
    {
        int cnt = 0;

        for(int i = Mathf.Min((int)pos.x,(int)targetpos.x); i < Mathf.Max((int)pos.x, (int)targetpos.x); i++)
        {
            if (walltile.GetTile(new Vector3Int(i,(int)pos.y, 0)) != null)
                cnt++;
        }


        return cnt < 3;
    }

    private bool CheckWallCol(Vector2 pos, Vector2 targetpos)
    {
        int cnt = 0;

        for (int i = Mathf.Min((int)pos.y, (int)targetpos.y); i < Mathf.Max((int)pos.y, (int)targetpos.y); i++)
        {
            if (walltile.GetTile(new Vector3Int((int)pos.x, i, 0)) != null)
                cnt++;
        }


        return cnt < 3;
    }
}
