using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTileMap : MonoBehaviour
{
    RoomGenarator roomGenarator;

    [SerializeField] Tilemap tile;
    [SerializeField] Tilemap walltile;
    public Tilemap WallTile => walltile;

    [SerializeField] Tile[] wall;
    [SerializeField] Tile[] round;
    [SerializeField] Tile[] bottomWall;

    [SerializeField] Tile loadtile;


    private void Awake()
    {
        #region 예외처리

        if(wall.Length < 4)
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
    }

    public void SetTileMap()
    {
                PoltarRoom();
        //switch (roomGenarator.spawnType)
        //{
        //    case MapSpawnType.Load:
        //        {

        //        }
        //        break;
        //    case MapSpawnType.Stuck:
        //        StuckRoom();
        //        break;
        //    case MapSpawnType.Potal:
        //        {

        //        }
        //        break;
        //}
    }

    private void PoltarRoom()
    {
        for (int i = 0; i < roomGenarator.useRooms.Count(); ++i)
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
    }
}
