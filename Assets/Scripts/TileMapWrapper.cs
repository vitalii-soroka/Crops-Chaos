using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private Tile[] tiles;

    //[SerializeField] private TileMapWrapperLayers[] ignoreLayers;
    [SerializeField] private CompositeCollider2D compositeCollider2D; 

    public void DrawTileWrapper(TileWrapper tile, Vector3Int centerPos)
    {
        for(int i = 0; i < tile.Length; ++i)
        {
            SetTile(centerPos, tiles[i]);

            centerPos += i % tile.Width == 0 ? new Vector3Int(0,1) : new Vector3Int(1,0);
        }
    }

    public delegate void TileChangeHandler(Vector3Int position, TileBase newTile);
    public event TileChangeHandler OnTileChanged;

    public delegate void TileChange(Vector3Int position);
    public event TileChange OnTileChange;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        compositeCollider2D = GetComponent<CompositeCollider2D>();
    }

    public bool HasCollider()
    {
        return compositeCollider2D != null;
    }

    public void SetTileNotify(Vector3Int position, TileBase tile)
    {
        tilemap.SetTile(position, tile);
        OnTileChanged?.Invoke(position, tile);
    }

    public void SetTileNotify(Vector3Int position)
    {
        tilemap.SetTile(position, tiles[9]);
        OnTileChange?.Invoke(position);
    }

    public void SetTile(Vector3Int position, TileBase tile)
    {
        tilemap.SetTile(position, tile);
    }

    public TileBase GetTile(Vector3Int position)
    {
        return tilemap.GetTile(position);
    }

    public Tile Get(int index)
    {
        return tiles.Length > index ? tiles[index] : null;
    }

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

}
