using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventorySlot 
{
    public Item item;
    public int quantity; 

    public GameObject itemPrefab; 


    public int GetQuantity()
    {
        return quantity;
    }

    public bool IsFull()
    {
        return quantity >= item.maxStack;
    }
}
