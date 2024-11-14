using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditorInternal.ReorderableList;


public class TileMapWrapperAdapter : MonoBehaviour
{
    TileMapWrapper tilemap;

    [SerializeField] public TileAdapt[] tileAdapters;

    private TileAdapt currentAdapter = null;

    void Start()
    {
        tilemap = GetComponent<TileMapWrapper>();
        tilemap.OnTileChanged.AddListener(OnTileChanged);
    }

    public void OnTileChanged(Vector3Int position, TileBase tile)
    {
        currentAdapter = FindAdapter(tile);

        // No rules for adaption
        if (currentAdapter == null) return;


        UpdateTile(position);
        UpdateNeiborTiles(position);

    }

    public TileAdapt FindAdapter(TileBase tile)
    {
        if (tileAdapters.Any(x => x.HasTile(tile))) return tileAdapters.First(x => x.HasTile(tile));
        return null;
    }

    public void UpdateTile(Vector3Int cellPos)
    {
        if (tilemap.GetTile(cellPos) == null || currentAdapter == null) return;

        bool hasFieldAbove = CheckForNeighborAdvanced(cellPos, Vector2Int.up);
        bool hasFieldBelow = CheckForNeighborAdvanced(cellPos, Vector2Int.down);
        bool hasFieldLeft = CheckForNeighborAdvanced(cellPos, Vector2Int.left);
        bool hasFieldRight = CheckForNeighborAdvanced(cellPos, Vector2Int.right);

        var spriteType = currentAdapter.GetSpriteType(hasFieldAbove, hasFieldBelow, hasFieldLeft, hasFieldRight);

        tilemap.SetTile(cellPos, currentAdapter.GetTile(spriteType));
    }

    bool CheckForNeighbor(Vector3Int basePos, Vector2Int direction)
    {
        return
            tilemap.GetTile(basePos + new Vector3Int(direction.x, direction.y, 0)) != null;
    }

    bool CheckForNeighborAdvanced(Vector3Int basePos, Vector2Int direction)
    {
        var tile = tilemap.GetTile(basePos + new Vector3Int(direction.x, direction.y, 0));

        if (tile == null) return false;

        return currentAdapter != null && currentAdapter.HasTile(tile);
    }

    public void UpdateNeiborTiles(Vector3Int tilePos)
    {
        UpdateTile(tilePos + Vector3Int.up);
        UpdateTile(tilePos + Vector3Int.down);
        UpdateTile(tilePos + Vector3Int.left);
        UpdateTile(tilePos + Vector3Int.right);
    }

    
    [Obsolete("Will possibly not be using in future.")]
    int GetSpriteIndex(bool hasAbove, bool hasBelow, bool hasLeft, bool hasRight)
    {
        // Debug.Log(hasAbove + " " + hasBelow + " " + hasLeft + " " + hasRight);
        /*
          0 1 2
          3 4 5
          6 7 8  
          
          9 - 11 <-- default sprites

          12 13 14
        
          15
          16
          17
        */
        // random default sprite
        if (!hasAbove && !hasBelow && !hasLeft && !hasRight) return Random.Range(9, 12);

        if (!hasAbove && hasBelow && !hasLeft && hasRight) return 0;
        if (!hasAbove && hasBelow && hasRight && hasLeft) return 1;
        if (!hasAbove && hasBelow && !hasRight && hasLeft) return 2;

        if (hasAbove && hasBelow && !hasLeft && hasRight) return 3;
        if (hasAbove && hasBelow && hasLeft && hasRight) return 4;
        if (hasAbove && hasBelow && hasLeft && !hasRight) return 5;

        if (hasAbove && !hasBelow && !hasLeft && hasRight) return 6;
        if (hasAbove && !hasBelow && hasLeft && hasRight) return 7;
        if (hasAbove && !hasBelow && hasLeft && !hasRight) return 8;

        if (!hasAbove && !hasBelow && hasRight && !hasLeft) return 12;
        if (!hasAbove && !hasBelow && hasRight && hasLeft) return 13;
        if (!hasAbove && !hasBelow && !hasRight && hasLeft) return 14;

        if (!hasAbove && hasBelow && !hasRight && !hasLeft) return 15;
        if (hasAbove && hasBelow && !hasRight && !hasLeft) return 16;
        if (hasAbove && !hasBelow && !hasRight && !hasLeft) return 17;

        // Default sprite
        return 11;
    }

}
