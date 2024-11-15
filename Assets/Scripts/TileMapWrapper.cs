using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public enum TileMapWrapperLayers
{
    Background,
    Ground,
    OnGround,
    Build
}

public class TileMapWrapper : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    
    [SerializeField] private CompositeCollider2D compositeCollider2D;

    public UnityEvent<Vector3Int, Tile> OnTileChanged;

    //public void DrawTileWrapper(TileWrapper tile, Vector3Int centerPos)
    //{
    //    for(int i = 0; i < tile.Length; ++i)
    //    {
    //        SetTile(centerPos, tiles[i]);

    //        centerPos += i % tile.Width == 0 ? new Vector3Int(0,1) : new Vector3Int(1,0);
    //    }
    //}

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        compositeCollider2D = GetComponent<CompositeCollider2D>();
    }

    public bool HasCollider()
    {
        return compositeCollider2D != null;
    }

    public void SetTileNotify(Vector3Int position, Tile tile)
    {
        tilemap.SetTile(position, tile);
        OnTileChanged?.Invoke(position, tile);
    }

    //public void SetTileNotify(Vector3Int position)
    //{
    //    SetTileNotify(position, tiles[9]);
    //    //tilemap.SetTile(position, tiles[9]);
    //}

    public void SetTile(Vector3Int position, TileBase tile)
    {
        tilemap.SetTile(position, tile);
    }

    public TileBase GetTile(Vector3Int position)
    {
        return tilemap.GetTile(position);
    }

    //public Tile Get(int index)
    //{
    //    return tiles.Length > index ? tiles[index] : null;
    //}

    public bool HasTile(Vector3 worldPosition)
    {
        Vector3Int tilePos = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(tilePos) != null;
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    public Vector3 GetCellCenterWorld(Vector3Int cellpos)
    {
       return tilemap.GetCellCenterWorld(cellpos);
    }

    public BoundsInt GetCellBounds()
    {
        return tilemap.cellBounds;
    }

    public Vector3 GetCellToWorld(Vector3Int pos)
    {
        return tilemap.CellToWorld(pos);
    }

    public Vector3 GetCellSize()
    {
        return tilemap.cellSize;
    }

}
