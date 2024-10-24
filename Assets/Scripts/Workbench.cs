using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] Collider2D triggerCollider;

    // TEMP RESULT OF DESTRUCTION
    [SerializeField] GameObject go;

    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.TryGetComponent<PickupItem>(out var pickup))
        {
            if (pickup.GetInventoryType() == Item.ItemType.Crop)
            {
                Debug.Log("WOnTriggerEnter2D");
                pickup.SetTarget(this.transform);
                pickup.ItemApproach.AddListener(OnItemApproach);
            }
        }
    }

    public void OnItemApproach(PickupItem pickItem)
    {
        Debug.Log("OnItemApproach");

        if (go != null)
        {
            var prizeObject = Instantiate(go);
            prizeObject.transform.position = transform.position;
        }

        //var seedItem = (SeedItem)pickItem.GetInventoryItem();
        //var seed = Instantiate(seedItem.cropPrefab, transform);
        //seed.transform.position = transform.position;
    }

}
