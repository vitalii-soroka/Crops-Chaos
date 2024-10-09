using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSystem : MonoBehaviour
{
    [SerializeField] TileWrapper groundTile;
    [SerializeField] TileWrapper onGroundTile;

    public void CanPlace()
    {

    }

    public enum TileLayers
    {
        None,
        Ground,
        OnGround
    }
}
