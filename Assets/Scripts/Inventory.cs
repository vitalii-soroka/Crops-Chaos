using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slots;
    [SerializeField] private int maxSlots = 3;

    [SerializeField] private int currentIndex = 0;

    public UnityEvent<int> ItemSelected;
    public UnityEvent InventoryChanged;

    void Start()
    {
        slots = new List<InventorySlot>();

        for (int i = 0; i < maxSlots; ++i)
        {
            slots.Add(new InventorySlot());
        }

        ItemSelected.Invoke(currentIndex);
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            currentIndex = CalculateIndex(currentIndex, scroll > 0 ? -1 : 1);
            ItemSelected.Invoke(currentIndex);
        }
    }

    private int CalculateIndex(int value, int change)
    {
        if (slots.Count == 0) return -1;

        return (value + change + slots.Count) % slots.Count;
    }

    // TODO just PickUp mechanic
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.CompareTag(StaticTags.Item)) return;

        if (collision.TryGetComponent<PickupItem>(out var pickup))
        {
            TrySetPickup(pickup);
        }
    }

    private void TrySetPickup(PickupItem pickup)
    {
        foreach (var slot in slots)
        {
            bool hasItem = !slot.IsFull() && slot.HasItemType(pickup.GetInventoryType());

            if (hasItem || slot.IsEmpty())
            {
                pickup.SetTarget(transform);
                pickup.SetInventory(this);
                return;
            }
        }
    }

    public bool CanPickup(PickupItem pickup)
    {
        foreach (var slot in slots)
        {
            bool hasItem = !slot.IsFull() && slot.HasItemType(pickup.GetInventoryType());

            if (hasItem || slot.IsEmpty())
            {
                return true;
            }
        }
        return false;
    }

    // TODO
    public void AddItem(Item newItem, int amount = 1)
    {
        // fill others slots
        foreach (var slot in slots)
        {
            if (slot.HasItemType(newItem) && !slot.IsFull())
            {
                int spaceLeft = newItem.maxStack - slot.quantity;

                int addedAmount = Mathf.Min(amount, spaceLeft);

                slot.quantity += addedAmount;
                amount -= addedAmount;
            }
        }

        // add to other slot
        if (amount > 0)
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                if (slots[i].IsEmpty())
                {
                    slots[i].quantity = amount;
                    slots[i].item = newItem;

                    break;
                }
            }
        }

        InventoryChanged.Invoke();
    }

    public int GetSlotsCount()
    {
        return slots.Count;
    }

    public Sprite GetItemIcon(int index)
    {
        if (index < 0 || slots.Count < index || slots[index].item == null) 
            return null;

        return slots[index].item.icon;

        //return slots.Count > index ? slots[index].item != null ? slots[index].item.icon : null : null;
    }

    public int GetQuantity(int index)
    {
        return slots.Count > index ? slots[index].GetQuantity() : 0;
    }

    public bool IsFull()
    {
        foreach(var slot in slots)
        {
            if (!slot.IsFull())
            {
                return false;
            }
        }

        return true;
    }

    public void Drop()
    {
        Debug.Log("Drop");
        if (slots[currentIndex].IsEmpty()) return;

        if (slots[currentIndex].item == null || slots[currentIndex].item.itemPrefab == null) return;

        // TEMP
        var dropped = Instantiate(slots[currentIndex].item.itemPrefab);
        dropped.transform.position = transform.position;
        
        slots[currentIndex].DropItem();

        InventoryChanged.Invoke();
    }

}
