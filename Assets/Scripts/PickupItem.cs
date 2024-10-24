using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class PickupItem : MonoBehaviour
{
    public enum PickupItemState
    {
        Idle,    // Waited for some time and now idle
        Dropped,  // Just Dropped 
        Dragged
    }

    private PickupItemState state;

    // Item representation in Inventory
    [SerializeField] private Item item;

    [SerializeField] private float dispawnTime = 100.0f;
    [SerializeField] private float timeToActivate = 2.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float stopDistance = 0.5f;

    [SerializeField] private float swingSpeed = 2.0f;  // Speed of the swinging
    [SerializeField] private float swingAmount = 0.1f; // Amount of rotation (swing)
    private Vector3 startPosition;


    private float timeDropped = 0.0f;

    private Collider2D pickUpCollider;

    private Transform moveTo = null;

    private Inventory playerInventory = null;

    public UnityEvent<PickupItem> ReadyPickup;
    public UnityEvent<PickupItem> ItemApproach;

    public void SetInventory(Inventory inventory)
    {
        playerInventory = inventory;
    }

    void Start()
    {
        pickUpCollider = GetComponent<Collider2D>();

        //if (pickUpCollider != null)
        //    pickUpCollider.enabled = false;

        startPosition = transform.position;

        ChangeState(PickupItemState.Dropped);

        gameObject.layer = LayerMask.NameToLayer("Pickup");
    }

    void FixedUpdate()
    {
        timeDropped += Time.fixedDeltaTime;

        if (timeDropped > dispawnTime)
        {
            Despawn();
        }

        if (timeDropped > timeToActivate)
        {
            ChangeState(PickupItemState.Idle);

            if (moveTo == null) 
            {
                ReadyPickup.Invoke(this);
            }
        }

        if (moveTo != null)
        {
            Vector3 direction = moveTo.position - transform.position;
            if (direction.magnitude > stopDistance)
            {
                direction.Normalize();

                Vector3 newPosition = Vector3.Lerp(transform.position, moveTo.position, moveSpeed * Time.fixedDeltaTime);

                transform.position = newPosition;
            }

            else
            {
                ItemApproach.Invoke(this);
                ItemApproach.RemoveAllListeners();
                Despawn();
            }
        }
        else
        {
            // Idle
            VerticalSwing();
        }
    }

    private void Update()
    {
        
    }

    void VerticalSwing()
    {
        float swing = Mathf.Sin(Time.time * swingSpeed) * swingAmount;
        transform.position = new Vector3(startPosition.x, startPosition.y + swing, startPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject != null
            && collision.gameObject.transform != null)
        {
            // TEMP
            //moveTo = collision.gameObject.transform;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (moveTo == null && newTarget != null)
        {
            moveTo = newTarget;
        }
    }

    public PickupItemState GetState()
    {
        return state;
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

    public Item GetInventoryItem()
    {
        return item;
    }

    public Item.ItemType GetInventoryType()
    {
        return item != null ? item.type : Item.ItemType.None;
    }

    private void ChangeState(PickupItemState newState)
    {
        switch(newState)
        {
            case PickupItemState.Idle:
                state = newState;

                break;

            case PickupItemState.Dropped:
                state = newState;
                timeDropped = 0.0f;

                break;
            case PickupItemState.Dragged:
                state = newState;
                break;

        }
    }
}
