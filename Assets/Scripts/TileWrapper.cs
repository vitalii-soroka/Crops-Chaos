using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileWrapper : MonoBehaviour
{
    [SerializeField] private Tile[] tiles;
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;

    void Start()
    {

#if UNITY_EDITOR
        if (tiles.Length < width * height)
        {
            Debug.LogWarning("Tile wrapper has incorrect amount of tiles.");
        }
#endif

    }

    public Tile this[int index]
    {
        get
        {
            if (index >= 0 && index < tiles.Length)
            {
                return tiles[index];
            }
            else
            {
                return null;
            }
        }
    }

    public int Length { get { return width * height; } }

    public int Width { get { return width; } }
    public int Height { get { return height; } }
}
