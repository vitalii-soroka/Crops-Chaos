using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.UIElements;

public class BuildPreview : MonoBehaviour
{
    /// <summary>
    /// Make to work with common objects and tilemaps seperatly
    /// </summary>
   
    public Color errorColor = Color.red;
    private Color defaultColor = Color.white;

    public float cellSize = 1f; // Size of each grid cell

    //public GameObject buildingPrefab;  // The building to be placed
    // public BuildingSelectionMenu menu;
    // public TileMapWrapper tileMap;

    private GameObject previewInstance; // The preview instance

    //[SerializeField] private TileWrapper tileWrapper;

    public void Start()
    {
        //menu.BuildingSelected += OnSelect;
    }

    public Vector3 GetPreviewPosition()
    {
        if (previewInstance == null) return default(Vector3); 
        else return previewInstance.transform.position;
    }

    public void Preview(GameObject preview)
    {
        if (preview == null) return;

        if (previewInstance != null) Destroy(previewInstance);

        previewInstance = Instantiate(preview, transform.position, Quaternion.identity);
        SetPreviewTransparency(previewInstance, 0.5f);
    }

    //public void OnSelect(GameObject prefab, TileMapWrapper map)
    //{
    //    if (prefab == null) return;

    //    if (previewInstance != null) Destroy(previewInstance);

    //    tileMap = map;
    //    buildingPrefab = prefab;
    //    previewInstance = Instantiate(buildingPrefab, transform.position, Quaternion.identity);

    //    SetPreviewTransparency(previewInstance, 0.5f);
    //}

    private void OnDisable()
    {
        CancelPlacement();
    }

    void Update()
    {

        if (previewInstance == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 snappedPosition = SnapToGrid(mousePosition);

        previewInstance.transform.position = snappedPosition;
    }

    public void Clear()
    {
        if (previewInstance != null) Destroy(previewInstance);
        //tileMap = null;
    }

    //void ConfirmPlacement(Vector3 position)
    //{
    //    tileMap.SetTileNotify(tileMap.WorldToCell(position));

    //    tileMap.DrawTileWrapper(tileWrapper, tileMap.WorldToCell(position));
    //}

    void CancelPlacement()
    {
        Destroy(previewInstance);
        //tileMap = null;
    }

    void SetPreviewTransparency(GameObject obj, float alpha)
    {
        if (obj.TryGetComponent<SpriteRenderer>(out var renderer))
        {
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;
        }

        //Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        //foreach (Renderer rend in renderers)
        //{
        //    Color color = rend.material.color;
        //    color.a = alpha; // Set transparency
        //    rend.material.color = color;
        //}
    }

    public void SetPreviewColor(Color color)
    {
        if (previewInstance != null 
            && previewInstance.TryGetComponent<SpriteRenderer>(out var renderer)
            && renderer.material.color != color)
        {
            renderer.material.color = color;
        }
    }

    //public void SetPreviewColor(GameObject obj, Color color)
    //{
    //    if (obj.TryGetComponent<SpriteRenderer>(out var renderer) 
    //        && renderer.material.color != color)
    //    {
    //        renderer.material.color = color;
    //    }
    //}

    public Vector3 SnapToGrid(Vector3 rawPosition)
    {
        float x = Mathf.Floor(rawPosition.x / cellSize) * cellSize + cellSize / 2;
        float y = Mathf.Floor(rawPosition.y / cellSize) * cellSize + cellSize / 2;
        return new Vector3(x, y, 0);
    }
}
