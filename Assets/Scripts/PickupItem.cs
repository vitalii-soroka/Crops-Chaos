using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PickupItem : MonoBehaviour
{
    public enum PickupItemState
    {
        Idle,       
        Dropped,    
        Magnetised    
    }

    public PickupItemState state;

    [SerializeField] private Item item;
    [SerializeField] private float despawnTime = 100.0f;
    [SerializeField] private float activateDelay = 2.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float swingSpeed = 2.0f;
    [SerializeField] private float swingAmount = 0.1f;

    public Transform target;

    private Vector3 startPosition;
    private float timeDropped;
    private Collider2D pickUpCollider;
    private Coroutine activationCoroutine;

    public UnityEvent<PickupItem> OnReadyToPickup;
    public UnityEvent<PickupItem> OnItemApproach;

    private void Start()
    {
        pickUpCollider = GetComponent<Collider2D>();
        if (pickUpCollider != null)
            pickUpCollider.enabled = true;

        startPosition = transform.position;
        ChangeState(PickupItemState.Dropped);
        gameObject.layer = LayerMask.NameToLayer("Pickup");
    }

    private void FixedUpdate()
    {
        timeDropped += Time.fixedDeltaTime;

        if (timeDropped > despawnTime)
        {
            Despawn();
            return;
        }

        if (state == PickupItemState.Magnetised && target != null)
        {
            MoveTowardsTarget();
        }
        else if (state == PickupItemState.Idle)
        {
            ApplySwing();
        }
    }

    private void ChangeState(PickupItemState newState)
    {
        state = newState;
        
        switch (newState)
        {
            case PickupItemState.Idle:
                if (pickUpCollider) pickUpCollider.enabled = true;
                break;

            case PickupItemState.Dropped:
                pickUpCollider.enabled = true;
                timeDropped = 0.0f;
                activationCoroutine = StartCoroutine(DelayedActivate());
                break;

            case PickupItemState.Magnetised:
                if (pickUpCollider) pickUpCollider.enabled = false;

                if (activationCoroutine != null) 
                {
                    StopCoroutine(activationCoroutine);
                    activationCoroutine = null; 
                }

                break;
        }
    }

    private IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(activateDelay);
        ChangeState(PickupItemState.Idle);
        OnReadyToPickup.Invoke(this);
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = target.position - transform.position;
        if (direction.magnitude > stopDistance)
        {
            direction.Normalize();
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            OnItemApproach.Invoke(this);
            ResetTarget();
            Despawn();
        }
    }

    private void ApplySwing()
    {
        float swing = Mathf.Sin(Time.time * swingSpeed) * swingAmount;
        transform.position = new Vector3(startPosition.x, startPosition.y + swing, startPosition.z);
    }

    public void SetTarget(Transform newTarget)
    {
        if (target == null && newTarget != null)
        {
            target = newTarget;
            ChangeState(PickupItemState.Magnetised);

            OnReadyToPickup.RemoveAllListeners();
            OnItemApproach.RemoveAllListeners();
        }
    }

    private void ResetTarget()
    {
        target = null;
        ChangeState(PickupItemState.Idle);
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
}
