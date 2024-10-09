using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] TileMapWrapper ground;
    [SerializeField] TileMapWrapper onGround;

    [SerializeField] BuildPreview buildPreview;

    [SerializeField] private BuildingSelectionMenu menu;

    private GameObject buildPrefab;
    private TileMapWrapper buildTileMap;

    private Vector3 lastPreviewPosition;

    void Start()
    {
        if (menu != null)
        {
            menu.BuildingSelected += OnSelect;
            menu.enabled = false;
        }
    }
    private bool IsMenuValidAndEnabled()
    {
        return menu != null && menu.gameObject.activeSelf;
    }

    private bool IsBuildingSelected() { return menu.GetSelectedBuilding() != null; }

    void UpdatePreviewUI()
    {
        if (IsValidPlace())
        {
            if (buildPreview != null) buildPreview.SetPreviewColor(Color.white);
        }
        else
        {
            if (buildPreview != null) buildPreview.SetPreviewColor(Color.red);
        }
    }

    void Update()
    {
        // TODO Add last transform pos

        if (!IsMenuValidAndEnabled()) return;

        if (!IsBuildingSelected()) return;

        if (lastPreviewPosition != buildPreview.GetPreviewPosition())
        {
            UpdatePreviewUI();
        }

        if (Input.GetMouseButtonDown(0) && IsValidPlace())
        {
            ConfirmBuild();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelBuild();
        }
    }

    private void ConfirmBuild()
    {
        if (buildTileMap != null && buildPreview != null )
        {
            buildTileMap.SetTileNotify(buildTileMap.WorldToCell(buildPreview.GetPreviewPosition()));
        }
    }

    private void CancelBuild()
    {
        if (buildPreview != null) buildPreview.Clear();
    }

    private bool IsValidPlace()
    {
        if (buildPrefab == null) return false;

        if (buildTileMap == null || buildPreview == null) return false;
       
        if (buildTileMap.HasTile(buildPreview.GetPreviewPosition()))
        {
            return false;
        }

        return true;
    }

    private void OnSelect(GameObject prefab, TileMapWrapper map = null)
    {
        if (prefab == null) return;


        buildPrefab = prefab;
        if (map != null) buildTileMap = map;

        if (buildPreview != null)
        {
            buildPreview.Preview(prefab);
            lastPreviewPosition = buildPreview.GetPreviewPosition();
        }
    }
}
