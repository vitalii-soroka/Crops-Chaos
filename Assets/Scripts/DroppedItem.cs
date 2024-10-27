using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{

    [SerializeField] Magnetization magnet;

    public enum DroppedItemState
    {
        Idle,
        Dropped,
        Magnetised
    }

    public DroppedItemState state;

    [SerializeField] private Item itemBasic;
    [SerializeField] private float despawnTime = 100.0f;
    [SerializeField] private float activateDelay = 2.0f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float swingSpeed = 2.0f;
    [SerializeField] private float swingAmount = 0.1f;
    [SerializeField] private Rigidbody2D rb;

    private Vector3 lastPosition;
    private float timeDropped;
    private Collider2D pickUpCollider;
    private Coroutine activationCoroutine;

    public void Start()
    {
        magnet = GetComponent<Magnetization>();
        pickUpCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        activationCoroutine = StartCoroutine(DelayedActivate());

        lastPosition = transform.position;
        state = DroppedItemState.Dropped;
    }

    private void Update()
    {
        if (state == DroppedItemState.Idle && magnet.target == null)
        {
            ApplySwing();
        }
        else
        {
            lastPosition = transform.position;
        }
    }

    private void ApplySwing()
    {
        float swing = Mathf.Sin(Time.time * swingSpeed) * swingAmount;
        transform.position = new Vector3(lastPosition.x, lastPosition.y + swing, lastPosition.z);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public Item GetInventoryItem()
    {
        return itemBasic;
    }
    public Item.ItemType GetInventoryItemType()
    {
        if (itemBasic != null) return itemBasic.type;
        return Item.ItemType.None;
    }

    public bool TrySetMagnetTarget(Transform target)
    {
        if (magnet == null || magnet.target != null) return false;

        if (magnet.TrySetTarget(target))
        {
            if (state == DroppedItemState.Idle) ChangeState(DroppedItemState.Magnetised);
            return true;
        }
        return false;
    }

    public bool TryClearMagnetTarget(Transform target)
    {
        if (magnet != null && magnet.target == target)
        {
            magnet.target = null;
            ChangeState(DroppedItemState.Idle);
            return true;
        }
        return false;
    }

    public bool CanBePicked()
    {
        return state == DroppedItemState.Idle;
    }

    public bool CanBePicked(Transform target)
    {
        if (state != DroppedItemState.Dropped && target == magnet.target) return true;
        
        return false;
    }

    private IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(activateDelay);

        // Stop falling after drop
        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.isKinematic = true;
            rb.gravityScale = 0.0f;
            rb.velocity = Vector3.zero;
        }

        lastPosition = transform.position;

        magnet.bCanMagnetize = true;

        if (magnet.target == null) ChangeState(DroppedItemState.Idle);

        else ChangeState(DroppedItemState.Magnetised);
    }

    // TODO
    public void ChangeState(DroppedItemState newState)
    {
        state = newState;

        switch (newState)
        {
            case DroppedItemState.Idle:
                if (magnet) magnet.bCanMagnetize = true;
                if (magnet.target != null) ChangeState(DroppedItemState.Magnetised);
               

                break;

            case DroppedItemState.Dropped:
                
                break;

            case DroppedItemState.Magnetised:
                

                break;
        }

    }


    public void Throw(Vector2 throwDirection, float throwForce)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.isKinematic = false;

            rb.velocity = throwDirection.normalized * throwForce;

            rb.gravityScale = 1.0f;
        }
    }

    // NEW FEATURE

    

    public void AddTarget(Transform t,  int p)
    {
        magnet.AddTarget(t, p);
    }

    public void RemoveTarget(Transform t)
    {
        magnet.RemoveTarget(t);
    }
}
