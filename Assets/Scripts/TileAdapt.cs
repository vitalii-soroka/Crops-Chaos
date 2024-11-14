using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileAdapt", menuName = "Tiles/TileAdapt")]
public class TileAdapt : ScriptableObject
{
    /// <summary>
    /// Object keeps inforamtion about how tiles should adapt to each other, later I can add different adapt behaviour
    /// </summary>

    [SerializeField] private TileAdaptBase[] tiles;

    public Tile GetTile(int index)
    {
        return tiles.Length > index ? tiles[index].tile : null;
    }

    public Tile GetTile(TileAdaptType type)
    {
        return tiles.First(x => x.type == type).tile;
    }

    public bool HasTile(TileBase tile)
    {
        return tiles.Any(x => x.tile == tile);
    }

    public TileAdaptType GetSpriteType(bool hasAbove, bool hasBelow, bool hasLeft, bool hasRight)
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
        if (!hasAbove && !hasBelow && !hasLeft && !hasRight) return TileAdaptType.Default;

        if (!hasAbove && hasBelow && !hasLeft && hasRight) return TileAdaptType.TopLeft;
        if (!hasAbove && hasBelow && hasRight && hasLeft) return TileAdaptType.TopMidle;
        if (!hasAbove && hasBelow && !hasRight && hasLeft) return TileAdaptType.TopRight;

        if (hasAbove && hasBelow && !hasLeft && hasRight) return TileAdaptType.MidleLeft;
        if (hasAbove && hasBelow && hasLeft && hasRight) return TileAdaptType.MidleMidle;
        if (hasAbove && hasBelow && hasLeft && !hasRight) return TileAdaptType.MidleRight;

        if (hasAbove && !hasBelow && !hasLeft && hasRight) return TileAdaptType.BottomLeft;
        if (hasAbove && !hasBelow && hasLeft && hasRight) return TileAdaptType.BottomMidle;
        if (hasAbove && !hasBelow && hasLeft && !hasRight) return TileAdaptType.BottomRight;

        if (!hasAbove && !hasBelow && hasRight && !hasLeft) return TileAdaptType.Left;
        if (!hasAbove && !hasBelow && hasRight && hasLeft) return TileAdaptType.MidleHorizontal;
        if (!hasAbove && !hasBelow && !hasRight && hasLeft) return TileAdaptType.Right;

        if (!hasAbove && hasBelow && !hasRight && !hasLeft) return TileAdaptType.Top;
        if (hasAbove && hasBelow && !hasRight && !hasLeft) return TileAdaptType.MidleVertical;
        if (hasAbove && !hasBelow && !hasRight && !hasLeft) return TileAdaptType.Down;

        return TileAdaptType.Default;
    }

    [System.Serializable]
    public class TileAdaptBase
    {
        public Tile tile;
        public TileAdaptType type = TileAdaptType.None;
    }
}

public enum TileAdaptType
{
    None = -1,             

    TopLeft = 0,          // 0
    TopMidle = 1,         // 1
    TopRight = 2,         // 2

    MidleLeft = 3,        // 3
    MidleMidle = 4,       // 4
    MidleRight = 5,       // 5
    
    BottomLeft = 6,       // 6
    BottomMidle = 7,
    BottomRight = 8,

    Default = 9,      // 9 - 11

    Left = 12,             // 12
    MidleHorizontal = 13,  // 13
    Right = 14,            // 14

    Top = 15,              // 15
    MidleVertical = 16,    // 16
    Down = 17              // 17
}