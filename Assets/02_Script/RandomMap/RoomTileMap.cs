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
                {
                    for (int i = 0; i < roomGenarator.useRooms.Count(); ++i)
                    {
                        tile.SetTile(new Vector3Int
                            (roomGenarator.useRooms[i].y, roomGenarator.useRooms[i].x, 0)
                            , loadtile);
                    }
                }
                break;
            case MapSpawnType.Potal:
                {

                }
                break;
        }
    }
}
