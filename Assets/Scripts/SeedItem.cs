using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/SeedItem")]
public class SeedItem : Item
{
    [SerializeField]
    public GameObject cropPrefab; // Preafab for crops
}
