using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] Collider2D triggerCollider;

    // TEMP
    [SerializeField] GameObject seedPrefab;

    //[SerializeField] 

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
                //pickup.ItemApproach.AddListener(OnItemApproach);
            }
        }
    }

    public void OnItemApproach(PickupItem pickItem)
    {
        Debug.Log("OnItemApproach");
        //var seed = Instantiate(pickItem.GetInventoryItem().itemPrefab);
        //seed.transform.position = transform.position;
    }

}
