using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> slotsl = new List<InventorySlot>();
    public int GetSlotsLength() { return slotsl.Count; }
    public Sprite GetItemIcon(int index) 
    { 
        return slotsl.Count > index ? slotsl[index].item.icon : null;
    }

    public UnityEvent<int> onSelect;
    public UnityEvent<Sprite, int> onItemAdd;

    public UnityEvent onInventoryChanged;

    public GameObject[] items;
    public int currentIndex = 0;
    public int slots = 3;

    public int MaxSlots = 3;

    void Start()
    {
        items = new GameObject[slots];

    }
    public void AddItem(GameObject newItem)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            if (items[i] == null) items[i] = newItem;

            onItemAdd.Invoke(newItem.GetComponent<SpriteRenderer>().sprite, i);
            break;
        }
    }
    public void RemoveItem(GameObject item)
    {
    }
    public void RemoveItem()
    {
        if (items[currentIndex] != null) items[currentIndex] = null;
        // TODO
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            currentIndex = CalculateIndex(currentIndex, scroll > 0 ? -1 : 1);
            onSelect.Invoke(currentIndex);
        }
    }

    void SelectItem(int index)
    {
        if (index < items.Length)
        {
            currentIndex = index;
            // TODO
        }
    }

    private int CalculateIndex(int value, int change)
    {
        return (value + change + slots) % slots;
    }

    // TODO just PickUp mechanic
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Item"))
        {
            if (collision.TryGetComponent<PickupItem>(out var pickup))
            {
                pickup.SetTarget(this.transform);
                pickup.SetInventory(this);
            }
        }
    }

    // TODO
    public void AddItem(Item newItem, int amount = 1)
    {
       

        foreach (var slot in slotsl)
        {
            if (slot.item == newItem && !slot.IsFull())
            {
                int spaceLeft = newItem.maxStack - slot.quantity;
                int addedAmount = Mathf.Min(amount, spaceLeft);
                slot.quantity += addedAmount;
                amount -= addedAmount;

                if (amount <= 0) return;
            }
        }


        if (amount > 0 && slotsl.Count < MaxSlots)
        {
            InventorySlot newSlot = new InventorySlot { item = newItem, quantity = amount };
            slotsl.Add(newSlot);
            Debug.Log(slotsl.Count);
        }

        onInventoryChanged.Invoke();
    }
}
