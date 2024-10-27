using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] Pickup pickup;    
    
    // TEMP RESULT OF DESTRUCTION
    [SerializeField] GameObject go;

    void Start()
    {
        if (pickup != null)
            pickup.OnPickup.AddListener(OnPickUp);
    }


    void OnPickUp(DroppedItem drop)
    {
        Destroy(drop.gameObject);

        var dropItem = Instantiate(go);
        dropItem.transform.position = transform.position;

        if (dropItem.TryGetComponent(out DroppedItem dropComponent))
        {
            dropComponent.Throw(Vector3.right + Vector3.up, 4.0f);
        }
    }
}
