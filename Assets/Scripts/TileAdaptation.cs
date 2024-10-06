using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileAdaptation : MonoBehaviour
{
    TileMapWrapper tilemap;
    

    void Start()
    {
        tilemap = GetComponent<TileMapWrapper>();
        tilemap.OnTileChanged += OnTileChanged;
        tilemap.OnTileChange += OnTileChanged;
    }

    public void OnTileChanged(Vector3Int position, TileBase tile)
    {
        UpdateTile(position);
        UpdateNeiborTiles(position);

    }
    public void OnTileChanged(Vector3Int position)
    {
        UpdateTile(position);
        UpdateNeiborTiles(position);
    }
    public void UpdateTile(Vector3Int cellPos)
    {
        if (tilemap.GetTile(cellPos) == null) return;

        // Check the neighboring fields
        bool hasFieldAbove = CheckForNeighbor(cellPos, Vector2Int.up);
        bool hasFieldBelow = CheckForNeighbor(cellPos, Vector2Int.down);
        bool hasFieldLeft = CheckForNeighbor(cellPos, Vector2Int.left);
        bool hasFieldRight = CheckForNeighbor(cellPos, Vector2Int.right);

        // Determine sprite based on the neighbors
        int spriteIndex = GetSpriteIndex(hasFieldAbove, hasFieldBelow, hasFieldLeft, hasFieldRight);

        tilemap.SetTile(cellPos, tilemap.Get(spriteIndex));
    }

    bool CheckForNeighbor(Vector3Int basePos, Vector2Int direction)
    {
        return
            tilemap.GetTile(basePos + new Vector3Int(direction.x, direction.y, 0)) != null;
    }

    int GetSpriteIndex(bool hasAbove, bool hasBelow, bool hasLeft, bool hasRight)
    {
        Debug.Log(hasAbove + " " + hasBelow + " " + hasLeft + " " + hasRight);
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

    public void UpdateNeiborTiles(Vector3Int tilePos)
    {
        UpdateTile(tilePos + Vector3Int.up);
        UpdateTile(tilePos + Vector3Int.down);
        UpdateTile(tilePos + Vector3Int.left);
        UpdateTile(tilePos + Vector3Int.right);
    }
}
