using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlot 
{
    public Item item;
    public int quantity; 

    public GameObject itemPrefab; 

    public InventorySlot()
    {
        item = null;
        quantity = 0;
        itemPrefab = null;
    }

    public bool HasItemType(Item other)
    {
        return item != null && other != null && item.Equals(other);
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public bool IsFull()
    {
        return item != null ? quantity >= item.maxStack : false;
    }

    public bool IsEmpty()
    {
        return item == null;
    }
}
