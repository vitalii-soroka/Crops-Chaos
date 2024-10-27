using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] Pickup pickup;

    // TODO
    [SerializeField] private LayerMask[] targetLayers;

    // All Layers if empty
    [SerializeField] private Item.ItemType[] targetTags;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag(StaticTags.Item) && collision.TryGetComponent<DroppedItem>(out var drop))
        {
            if (targetTags.Length == 0 || targetTags.Contains(drop.GetInventoryItemType()))
            {
                drop.AddTarget(transform, pickup != null ? pickup.priority : 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag(StaticTags.Item) && collision.TryGetComponent<DroppedItem>(out var drop))
        {
            if (targetTags.Length == 0 || targetTags.Contains(drop.GetInventoryItemType()))
            {
                drop.RemoveTarget(transform);
            }
        }
    }
}
