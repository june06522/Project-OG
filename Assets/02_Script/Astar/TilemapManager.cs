using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager
{
    public static TilemapManager Instance;

    private Tilemap _mainMap;
    private Tilemap _wallMap;
    public Tilemap MainMap => _mainMap;
    public Tilemap WallMap => _wallMap;

    public TilemapManager(Transform tilemapObject)
    {
        _wallMap = tilemapObject.Find("Wall").GetComponent<Tilemap>();
        _mainMap = tilemapObject.Find("Ground").GetComponent<Tilemap>();
        _mainMap.CompressBounds();
    }

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
