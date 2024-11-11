using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameActionsManager : MonoBehaviour
{
    [SerializeField] TileField field;
    [SerializeField] Tilemap ground;

    

    //public void GatherCrop(Vector3 position)
    //{
    //    if (field == null) return;

    //    field.Gather(position);
    //}

    //public void PlantCrop(Vector3 position, GameObject prefab)
    //{
    //    if (field == null) return;

    //    field.Plant(position, prefab);
    //}

    //public bool HasField(Vector3 position)
    //{
    //    return field != null && field.IsField(position);
    //}

    //public bool HasCrop(Vector3 position)
    //{
    //    return field != null && field.HasCrop(position);
    //}

    //public void Dig(Vector3 position)
    //{
    //    if (field == null || ground == null) return;

    //    if (ground.HasTile(ground.WorldToCell(position)))
    //    {
    //        field.Dig(position);
    //    }
    //}

}
