using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject selectImage; // Better to place on 0 slot place from inspector because of the bug

    [SerializeField] private InventorySlotUI[] inventorySlotsUI;

    // TODO Create from prefab
    [SerializeField] private InventorySlotUI slotPrefab;
    [SerializeField] private int slotsMax = 3;

    void Start()
    {
        // Subscribe to the inventory events
        if (inventory != null)
        {
            inventory.ItemSelected.AddListener(OnItemSelect);
            inventory.InventoryChanged.AddListener(OnInventoryChange);
        }
    }

    private void OnInventoryChange()
    {
        Debug.Log("OnInventoryChange");

        if (inventory == null) return;

        for (int i = 0; i < inventory.GetSlotsCount(); ++i)
        {
            if (i >= inventorySlotsUI.Length || inventorySlotsUI[i] == null) return;

            var itemImage = inventory.GetItemIcon(i);
            if (itemImage != null)
            {
                inventorySlotsUI[i].ShowItemImage(itemImage);
            }
            else
            {
                inventorySlotsUI[i].HideItemImage();
            }

            inventorySlotsUI[i].SetAmount(inventory.GetQuantity(i));
        }
    }

    public void OnItemSelect(int index)
    {
        if (index < 0 || index >= inventorySlotsUI.Length) return;
        if (selectImage == null || inventorySlotsUI[index] == null) return;

        selectImage.transform.position = inventorySlotsUI[index].transform.position;
    }
   
}
