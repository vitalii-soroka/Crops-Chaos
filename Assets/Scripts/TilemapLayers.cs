using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapLayers : MonoBehaviour
{
    public static TilemapLayers Instance { get; private set; }

    /* 0 - backGround
     * 1 - field
     * 2 - onField
     * 3 - ?
     */

    public static Dictionary<string, TileMapWrapper> tileMaps = new Dictionary<string, TileMapWrapper>();

    public static TileMapWrapper someMap = null;

    public void Start()
    {
        var fieldMap = GameObject.Find("FieldMap").GetComponent<TileMapWrapper>();
        if (fieldMap != null)
        {
            tileMaps.Add("FieldMap", fieldMap);
        }

        var onfieldMap = GameObject.Find("OnFieldMap").GetComponent<TileMapWrapper>();
        if (onfieldMap != null)
        {
            tileMaps.Add("OnFieldMap", onfieldMap);
        }

    }

    private void Awake()
    {
        // Singleton pattern logic
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public TileMapWrapper GetTileMap(string name)
    {
        if (tileMaps.ContainsKey(name)) return tileMaps[name];
        else return null;
    }
}
