using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BuildingSelectionMenu;

public class BuildingSelectionMenu : MonoBehaviour
{
    public delegate void OnBuildingSelected(GameObject newBuilding);
    public event OnBuildingSelected BuildingSelected;

    public GameObject[] buildingPrefabs;

    private GameObject selectedPrefab;

    public void SelectBuilding(int buildingIndex)
    {
        if (buildingPrefabs.Length > buildingIndex)
        {
            selectedPrefab = buildingPrefabs[buildingIndex];
            BuildingSelected?.Invoke(selectedPrefab);
        }

            Debug.Log("Selected building: " + selectedPrefab.name);
    }

    public GameObject GetSelectedBuilding()
    {
        return selectedPrefab;
    }

    
}
