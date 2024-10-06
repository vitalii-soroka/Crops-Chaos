using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileMapWrapper : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile[] tiles;

    public delegate void TileChangeHandler(Vector3Int position, TileBase newTile);
    public event TileChangeHandler OnTileChanged;

    public delegate void TileChange(Vector3Int position);
    public event TileChange OnTileChange;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
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
