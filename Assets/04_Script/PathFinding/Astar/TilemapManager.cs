using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoSingleton<TilemapManager>
{

    [SerializeField]
    private Tilemap _mainMap;
    [SerializeField]
    private Tilemap _wallMap;
    public Tilemap MainMap => _mainMap;
    public Tilemap WallMap => _wallMap;

    public bool HasWallTile(Vector3Int pos)
    {
        return _wallMap.GetTile(pos) != null;
    }

    public Vector3Int GetTilePos(Vector3 worldPos)
    {
        return _mainMap.WorldToCell(worldPos);
    }

    public Vector3 GetWorldPos(Vector3Int cellPos)
    {
        return _mainMap.GetCellCenterWorld(cellPos);
    }

}
