using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlot 
{
    public Item item;
    public int quantity; 

    //public GameObject itemPrefab; 

    public InventorySlot()
    {
        item = null;
        quantity = 0;
        //itemPrefab = null;
    }

    // Remove Later?
    public void SetItem(Item other, int amount = 1)
    {
        item.itemName = other.itemName;
        item.maxStack = other.maxStack;
        item.icon = other.icon;
        item.itemPrefab = other.itemPrefab;

        quantity = amount;  
    }

    public void SubstructItem()
    {
        if (item == null || quantity == 0) return;

        --quantity;
        
        if (quantity < 1)
        {
            item = null;
            quantity = 0;
        }
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
