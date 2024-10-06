using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TileField : MonoBehaviour
{
    
    private Dictionary<Vector3Int, GameObject> crops;
    private TileMapWrapper tileMap;
    
    void Start()
    {
        tileMap = GetComponent<TileMapWrapper>();
        crops = new Dictionary<Vector3Int, GameObject>();
    }
    
    private void Plant(Vector3Int position, GameObject cropPrefab)
    {
        if (cropPrefab == null || crops.ContainsKey(position)) return;

        crops[position] = Instantiate(cropPrefab);

        if (crops[position] != null)
        {
            crops[position].transform.position = tileMap.GetCellCenterWorld(position);
            crops[position].transform.SetParent(this.transform, true);
        }
    }
    private void Gather(Vector3Int position)
    {
        if (!crops.ContainsKey(position) || !IsCropReady(position)) return;

        var dropComponent = crops[position].GetComponent<Dropable>();
        if (dropComponent) dropComponent.Drop();
        crops.Remove(position);
    }
    private void UnPlant(Vector3Int position)
    {
        if (crops.ContainsKey(position))
        {
            if (crops[position].TryGetComponent<Crop>(out var crop) )
            {
                crop.BreakCrop();
                crops.Remove(position);
            }
        }
    }
    private bool HasCrop(Vector3Int position) 
    {
        return crops.ContainsKey(position);
    }
    private bool IsCropReady(Vector3Int position)
    {
        if (!crops.ContainsKey(position)) return false;

        var cropComponent = crops[position].GetComponent<Crop>();

        return cropComponent && cropComponent.IsGrown();
    }

    private Vector3Int WordToCell(Vector3 worldPos)
    {
        return tileMap.WorldToCell(worldPos);
    }

    public void Plant(Vector3 worldPosition,  GameObject cropPrefab)
    {
        Vector3Int tilePos = tileMap.WorldToCell(worldPosition);
        Plant(tilePos, cropPrefab);
    }

    public void Gather(Vector3 worldPosition)
    {
        Gather(WordToCell(worldPosition));
    }

    public void UnPlant(Vector3 worldPosition)
    {
        UnPlant(WordToCell(worldPosition));
    }

    public bool HasCrop(Vector3 worldPosition) 
    {
        return HasCrop(WordToCell(worldPosition));
    }

    public bool IsField(Vector3 worldPosition)
    {
        return tileMap.HasTile(worldPosition);
    }
}
