using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerController playerController;

    [SerializeField] private TilemapPathfinding tilemapPathfinding; // pathFinder, maybe later separate pathfinding and path builder

    [SerializeField] BuildingManager buildingSystem; // object/tilemap builder

    [SerializeField] TileField cropsField; // Field with crops using tilemap
    [SerializeField] Tilemap baseTilemap;  // Base tile map

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        #region PathFinder

        if (tilemapPathfinding != null)
        {
            tilemapPathfinding.BuildGrid();
        }

        #endregion 

        #region BuildManager

        if (buildingSystem != null)
        {
            buildingSystem.OnTileBuild.AddListener(OnTileBuild);
            // NEED TO ADD ON BUILD 
        }

        #endregion

    }

    // Called from BuildManager when tile placed
    private void OnTileBuild(TileBase tile, Vector3 position)
    {
        // Add check for tile if it's blocker

        // Update tile as not walkable
        baseTilemap.SetTile(baseTilemap.WorldToCell(position), tile);

        // Update node connected to that tile for pathfinder
        tilemapPathfinding.RecalculateNode(position);
    }

    void OnDrawGizmos()
    {
        var grid = tilemapPathfinding.grid;

        if (grid == null) return;

        foreach (var kvp in grid)
        {
            Node node = kvp.Value;

            // Convert tile position to world position for Gizmos
            Vector3 worldPos = baseTilemap.CellToWorld(node.gridPosition) + baseTilemap.cellSize / 2;

            // Set Gizmos color based on walkability
            Gizmos.color = node.isWalkable ? Color.green : Color.red;

            // Draw a circle with small radius at the node's position
            Gizmos.DrawSphere(worldPos, 0.1f);
        }
    }


    // TODO Field Manager?

    #region FieldActions

    public void GatherCrop(Vector3 position)
    {
        if (cropsField == null) return;

        cropsField.Gather(position);
    }

    public void PlantCrop(Vector3 position, GameObject prefab)
    {
        if (cropsField == null) return;

        cropsField.Plant(position, prefab);
    }

    public bool HasField(Vector3 position)
    {
        return cropsField != null && cropsField.IsField(position);
    }

    public bool HasCrop(Vector3 position)
    {
        return cropsField != null && cropsField.HasCrop(position);
    }

    public void Dig(Vector3 position)
    {
        if (cropsField == null || baseTilemap == null) return;

        if (baseTilemap.HasTile(baseTilemap.WorldToCell(position)))
        {
            cropsField.Dig(position);
        }
    }

    #endregion

}
