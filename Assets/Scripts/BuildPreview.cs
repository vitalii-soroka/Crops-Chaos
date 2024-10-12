using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.UIElements;

public class BuildPreview : MonoBehaviour
{
    
    private Color defaultColor = Color.white;
    public Color errorColor = Color.red;

    public float cellSize = 1f; 
    private GameObject previewInstance; 

    public bool HasPreview()
    {
        return previewInstance != null;
    }

    public Vector3 GetPreviewPosition()
    {
        if (previewInstance == null) return default(Vector3); 
        else return previewInstance.transform.position;
    }

    public void CreatePreview(GameObject prefab)
    {
        if (prefab == null) return;

        if (previewInstance != null) Destroy(previewInstance);

        previewInstance = Instantiate(prefab, transform.position, Quaternion.identity);
        
        if (previewInstance != null)
        {
            SetPreviewTransparency(previewInstance, 0.5f);
            if (previewInstance.TryGetComponent<Collider2D>(out var collider))
            {
                collider.enabled = false;
            }
        }
    }

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
    }

    void CancelPlacement()
    {
        Destroy(previewInstance);
    }

    void SetPreviewTransparency(GameObject obj, float alpha)
    {
        if (obj.TryGetComponent<SpriteRenderer>(out var renderer))
        {
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;
        }
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

    public Vector3 SnapToGrid(Vector3 rawPosition)
    {
        float x = Mathf.Floor(rawPosition.x / cellSize) * cellSize + cellSize / 2;
        float y = Mathf.Floor(rawPosition.y / cellSize) * cellSize + cellSize / 2;
        return new Vector3(x, y, 0);
    }
}
