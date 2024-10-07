using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static BuildingSelectionMenu;

public class BuildingSelectionMenu : MonoBehaviour
{
    public delegate void OnBuildingSelected(GameObject newBuilding, TileMapWrapper map);
    public event OnBuildingSelected BuildingSelected;

    public GameObject[] buildingPrefabs;
    public TileMapWrapper[] tiles;

    private GameObject selectedPrefab;

    public void SelectBuilding(int buildingIndex)
    {
        if (buildingPrefabs.Length > buildingIndex && tiles.Length > buildingIndex)
        {
            selectedPrefab = buildingPrefabs[buildingIndex]; 
            BuildingSelected?.Invoke(selectedPrefab, tiles[buildingIndex]);
        }
    }
    public GameObject GetSelectedBuilding()
    {
        return selectedPrefab;
    }

    
}
