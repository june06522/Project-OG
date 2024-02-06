using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTileMap : MonoBehaviour
{
    RoomGenarator roomGenarator;

    [SerializeField] Tilemap tile;
    [SerializeField] Tile wallTile;
    [SerializeField] Tile loadtile;

    private void Awake()
    {
        roomGenarator = GetComponent<RoomGenarator>();
    }

    public void SetTileMap()
    {
        switch (roomGenarator.spawnType)
        {
            case MapSpawnType.Load:
                {

                }
                break;
            case MapSpawnType.Stuck:
                StuckRoom();
                break;
            case MapSpawnType.Potal:
                {

                }
                break;
        }
    }

    private void StuckRoom()
    {
        for (int i = 0; i < roomGenarator.useRooms.Count(); ++i)
        {
            int x = roomGenarator.useRooms[i].x * (roomGenarator.RoomWidth + roomGenarator.BGLenth * 2);
            int y = roomGenarator.useRooms[i].y * (roomGenarator.RoomHeight + roomGenarator.BGLenth * 2);
            for (int j = y - roomGenarator.RoomHeight / 2; j < y + roomGenarator.RoomHeight / 2; ++j)
            {
                for(int k = x - roomGenarator.RoomWidth / 2; k < x + roomGenarator.RoomWidth / 2; ++k)
                {
                    tile.SetTile(new Vector3Int(k, j, 0), loadtile);
                }
            }



        }
    }
}
