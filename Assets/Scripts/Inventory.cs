using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public UnityEvent<int> onSelect;
    public UnityEvent<Sprite, int> onItemAdd;

    public GameObject[] items;
    public int currentIndex = 0;
    public int slots = 3;
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
        if (collision != null && collision.CompareTag("Drop"))
        {
            if(collision.TryGetComponent<DroppedItem>(out var drop))
            {
                drop.SetTarget(this.transform);
            }
        }
    }
}
