using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] Collider2D triggerCollider;

    // TEMP
    [SerializeField] GameObject seedPrefab;

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
                Destroy(pickup.gameObject);
                var inst = Instantiate(seedPrefab);
                inst.transform.position = transform.position;
            }
        }
    }

}
