using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slots;
    [SerializeField] private int maxSlots = 3;

    [SerializeField] private int currentIndex = 0;

    public UnityEvent<int> ItemSelected;
    public UnityEvent InventoryChanged;

    [SerializeField] Pickup pickup;
    [SerializeField] Magnet magnet;

    //[SerializeField] Transform dropPosition;
    [SerializeField] float dropPosOffset = 1.0f;

    void Start()
    {
        slots = new List<InventorySlot>();

        for (int i = 0; i < maxSlots; ++i)
        {
            slots.Add(new InventorySlot());
        }

        if (pickup != null)
        {
            pickup.OnPickup.AddListener(OnPickup);
        }
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

    public void OnPickup(DroppedItem dropItem)
    {
        if (dropItem.CanBePicked(magnet.transform))
        {
            if (CanPickup(dropItem))
            {
                AddItem(dropItem.GetInventoryItem());
                dropItem.Delete();
            }
            else
            {
                dropItem.TryClearMagnetTarget(transform);
            }
        }
    }


    public Item GetSelectedItem()
    {
        return slots[currentIndex].item;
    }
    private int CalculateIndex(int value, int change)
    {
        if (slots.Count == 0) return -1;

        return (value + change + slots.Count) % slots.Count;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision == null) return;

        //if (collision.CompareTag(StaticTags.Item) && collision.TryGetComponent<DroppedItem>(out var drop))
        //{
        //    if (CanPickup(drop))
        //    {
        //        drop.AddTarget(transform, pickup != null ? pickup.priority : 0);
        //    }
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision == null) return;

        //if (collision.CompareTag(StaticTags.Item) && collision.TryGetComponent<DroppedItem>(out var drop))
        //{
        //    drop.RemoveTarget(transform);
        //}
    }

    private void TrySetPickup(PickupItem pickup)
    {

        foreach (var slot in slots)
        {
            bool hasItem = !slot.IsFull() && slot.HasItemType(pickup.GetInventoryItem());

            if (hasItem || slot.IsEmpty())
            {
                Debug.Log("target");
                pickup.SetTarget(transform);
                return;
            }
        }
        Debug.Log("ntarget");
    }

    public bool CanPickup(DroppedItem dropped)
    {
        foreach (var slot in slots)
        {
            bool hasItem = !slot.IsFull() && slot.HasItemType(dropped.GetInventoryItem());

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

    public void SubstructItem()
    {
        slots[currentIndex].SubstructItem();
        InventoryChanged.Invoke();
    }

    public void Drop()
    {
        if (slots[currentIndex].IsEmpty()) return;

        if (slots[currentIndex].item == null || slots[currentIndex].item.itemPrefab == null) return;

        slots[currentIndex].SubstructItem();
        // TEMP
        var dropped = Instantiate(slots[currentIndex].item.itemPrefab);
        dropped.transform.position = transform.position;
       

        InventoryChanged.Invoke();
    }

    public void Drop(Vector2 throwDirection, float throwForce)
    {
        if (slots[currentIndex].IsEmpty()) return;

        if (slots[currentIndex].item == null || slots[currentIndex].item.itemPrefab == null) return;

        var dropItem = Instantiate(slots[currentIndex].item.itemPrefab);
        dropItem.transform.position = transform.position + new Vector3(
            dropPosOffset * throwDirection.normalized.x,
            dropPosOffset * throwDirection.normalized.y,
            0.0f);

        if (dropItem.TryGetComponent(out DroppedItem dropComponent))
        {
            dropComponent.Throw(throwDirection, throwForce);
        }

        slots[currentIndex].SubstructItem();

        InventoryChanged.Invoke();
    }

}
