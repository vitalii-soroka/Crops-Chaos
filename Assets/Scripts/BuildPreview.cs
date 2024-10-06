using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildPreview : MonoBehaviour
{
    /// <summary>
    /// Make to work with common objects and tilemaps seperatly
    /// </summary>

    public BuildingSelectionMenu menu;
    public GameObject buildingPrefab;  // The building to be placed

    // TEMP
    public TileMapWrapper tileMapWrapper;
    public TileMapWrapper tileMapWrapper2;

    private GameObject previewInstance; // The preview instance

    public float cellSize = 1f; // Size of each grid cell

    public void Start()
    {
        menu.BuildingSelected += OnSelect;
    }

    public void OnSelect(GameObject prefab)
    {
        if (prefab == null) return;

        Destroy(previewInstance);
        buildingPrefab = prefab;

        previewInstance = Instantiate(buildingPrefab, transform.position, Quaternion.identity);
        SetPreviewTransparency(previewInstance, 0.5f);
    }

    public Vector3 SnapToGrid(Vector3 rawPosition)
    {
        float x = Mathf.Floor(rawPosition.x / cellSize) * cellSize + cellSize / 2;
        float y = Mathf.Floor(rawPosition.y / cellSize) * cellSize + cellSize / 2;
        return new Vector3(x, y, 0);
    }

    private void OnDisable()
    {
       Destroy(previewInstance);
    }

    void Update()
    {
        // TODO Remake to use tileSprite

        if (menu.GetSelectedBuilding() == null) return;

        //if (buildingPrefab == null)
        //{
        //    buildingPrefab = menu.GetSelectedBuilding();
        //}

        //if (previewInstance == null && buildingPrefab != null)
        //{
            
        //}

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 snappedPosition = SnapToGrid(mousePosition);
        previewInstance.transform.position = snappedPosition;

        if (Input.GetMouseButtonDown(0))  
        {
            ConfirmPlacement(snappedPosition);
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            CancelPlacement();
        }
    }

    void ConfirmPlacement(Vector3 position)
    {
        // TEMP
        if (previewInstance.TryGetComponent<Field>(out var x)) 
            tileMapWrapper.SetTileNotify(tileMapWrapper.WorldToCell(position));
        else
            tileMapWrapper2.SetTileNotify(tileMapWrapper.WorldToCell(position));

        if (IsValidPlacement(position))
        {
            //Destroy(previewInstance);
        }

    }

    void CancelPlacement()
    {
        Destroy(previewInstance);
    }

    bool IsValidPlacement(Vector3 position)
    {
        return true;
    }

    void SetPreviewTransparency(GameObject building, float alpha)
    {
        Renderer[] renderers = building.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            Color color = rend.material.color;
            color.a = alpha; // Set transparency
            rend.material.color = color;
        }
    }
}
