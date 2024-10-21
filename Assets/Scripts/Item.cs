using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "Default Item";
    public int maxStack = 16;

    public Sprite icon;
    
    public GameObject itemPrefab; // Preafab for dropping

    public ItemType type;

    public enum ItemType
    { 
        None,
        Seed,
        Crop
    }
}


