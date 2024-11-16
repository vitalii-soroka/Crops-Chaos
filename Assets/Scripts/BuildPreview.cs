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

    private Vector3 lastMousePosInWorld = Vector3.zero;
    private Vector2 spriteOffset = Vector2.zero;

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
            if (previewInstance.TryGetComponent<Collider2D>(out var collider))
            {
                collider.enabled = false;
            }

            if (previewInstance.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
               //Debug.Log($"spriteRenderer: {spriteRenderer.size.x} : {spriteRenderer.size.y}");
               spriteOffset.x = (spriteRenderer.size.x % 2 == 0) ? 0 : cellSize / 2; 
               spriteOffset.y = (spriteRenderer.size.y % 2 == 0) ? 0 : cellSize / 2;
               //Debug.Log($"spriteOffset: {spriteOffset.x} : {spriteOffset.y}");

               Color color = spriteRenderer.material.color;
               color.a = 0.5f;
               spriteRenderer.material.color = color;
            }
            //SetPreviewTransparency(previewInstance, 0.1f);
        }
    }

    private void OnDisable()
    {
        CancelPlacement();
    }

    void Update()
    {
        if (previewInstance == null) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos != lastMousePosInWorld)
        {
            AdjustPreviewPosition();
            lastMousePosInWorld = mousePos;
        }

        //Vector3 snappedPosition = SnapPointToGrid(mousePosition);
        //previewInstance.transform.position = snappedPosition;
    }

    public void Clear()
    {
        if (previewInstance != null) Destroy(previewInstance);

        lastMousePosInWorld = Vector3.zero;
        spriteOffset = Vector2.zero;
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

    public Vector3 SnapPointToGrid(Vector3 rawPosition)
    {
        
        float x = Mathf.Floor(rawPosition.x / cellSize) * cellSize;
        float y = Mathf.Floor(rawPosition.y / cellSize) * cellSize;

        return new Vector3(x, y, 0);
    }

    private void AdjustPreviewPosition()
    {
        if (previewInstance == null || previewInstance.transform == null) return;

        float x = Mathf.Floor(lastMousePosInWorld.x / cellSize) * cellSize + spriteOffset.x;
        float y = Mathf.Floor(lastMousePosInWorld.y / cellSize) * cellSize + spriteOffset.y;

        previewInstance.transform.position = new Vector3(x, y, 0);
    }
}
