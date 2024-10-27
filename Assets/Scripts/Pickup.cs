using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public UnityEvent<DroppedItem> OnPickup;

    [SerializeField] public int priority = 0;

    // All Layers if empty
    [SerializeField] private Item.ItemType[] targetTags;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag(StaticTags.Item)
            && collision.TryGetComponent(out DroppedItem dropItem))
        {
            if (targetTags.Length == 0 || targetTags.Contains(dropItem.GetInventoryItemType()))
            {
                OnPickup.Invoke(dropItem);
            }

        }
    }
}
