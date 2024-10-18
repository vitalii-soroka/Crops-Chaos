using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenoryUI : MonoBehaviour
{
    [SerializeField] public Inventory inventory;
    [SerializeField] public GameObject selectImage;
    //[SerializeField] public GameObject[] inventorySlots;

    [SerializeField] public InventorySlotUI[] inventorySlots;

    void Start()
    {
        if (inventory != null)
        {
            inventory.onSelect.AddListener(OnItemSelect);
            //inventory.onItemAdd.AddListener(OnSlotChanged);

            inventory.onInventoryChanged.AddListener(OnInventoryChange);
        }

        if (selectImage != null && inventorySlots.Length > 0)
        {
            selectImage.transform.position = inventorySlots[0].transform.position;
        }
    }

    private void OnInventoryChange()
    {
        Debug.Log("OnInventoryChange");
        for (int i = 0; i < inventory.GetSlotsLength(); ++i)
        {
            Debug.Log("for inventory");
            if (i >= inventorySlots.Length) return;

            if (selectImage == null || inventorySlots[i] == null) return;

            inventorySlots[i].SetItemImage(inventory.GetItemIcon(i));
            inventorySlots[i].MakeItemImageVisible();

            //if (inventorySlots[i].TryGetComponent<Image>(out var image))
            //{
            //    ;
            //    image.sprite = ;
            //    
            //}

            // TODO

            inventorySlots[i].SetItemImage(inventory.GetItemIcon(i));
        }
    }

    public void OnItemSelect(int index)
    {
        if (index < 0 || index >= inventorySlots.Length) return;
        if (selectImage == null || inventorySlots[index] == null) return;

        selectImage.transform.position = inventorySlots[index].transform.position;
    }
    //public void OnSlotChanged(Sprite itemSprite, int index)
    //{
    //    Debug.Log("OnSlot");

    //    if (index < 0 || index >= inventorySlots.Length) return;
    //    if (selectImage == null || inventorySlots[index] == null) return;


    //    var image = inventorySlots[index].GetComponentInChildren<Image>();
    //    image.sprite = itemSprite;

    //    MakeImageVisible(image);

    //}

    public void MakeImageInvisible(Image image)
    {
        Color tempColor = image.color;
        tempColor.a = 0f; // Set alpha to 0 (invisible)
        image.color = tempColor;
    }

    public void MakeImageVisible(Image image)
    {
        Color tempColor = image.color;
        tempColor.a = 1f; // Set alpha to 1 (fully visible)
        image.color = tempColor;
    }

}
