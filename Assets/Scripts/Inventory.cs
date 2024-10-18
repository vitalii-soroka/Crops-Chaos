using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slotsl;

    [SerializeField] private int currentIndex = 0;
    [SerializeField] private int slots = 3;

    public UnityEvent<int> onSelect;
    public UnityEvent onInventoryChanged;

    //public UnityEvent<Sprite, int> onItemAdd;

    //public GameObject[] items;

    public int MaxSlots = 3;

    void Start()
    {
        slotsl = new List<InventorySlot>();
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

    private int CalculateIndex(int value, int change)
    {
        return (value + change + slots) % slots;
    }

    // TODO just PickUp mechanic

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag(StaticTags.Item))
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
            if (slot.item.Equals(newItem) && !slot.IsFull())
            {
                int spaceLeft = newItem.maxStack - slot.quantity;

                int addedAmount = Mathf.Min(amount, spaceLeft);

                slot.quantity += addedAmount;
                amount -= addedAmount;
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

    public int GetSlotsLength()
    {
        return slotsl.Count;
    }

    public Sprite GetItemIcon(int index)
    {
        return slotsl.Count > index ? slotsl[index].item.icon : null;
    }

    public int GetQuantity(int index)
    {
        return slotsl.Count > index ? slotsl[index].GetQuantity() : 0;
    }
}
