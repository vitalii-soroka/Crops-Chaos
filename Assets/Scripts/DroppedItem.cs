using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private float dispawnTime = 100.0f;
    [SerializeField] private float timeToActivate = 2.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float stopDistance = 0.5f;

    [SerializeField] private GameObject inventoryItemPrefab;

    private float timeSpent = 0.0f;

    private Collider2D pickUpCollider;

    private Transform moveTo = null;

    void Start()
    {
        pickUpCollider = GetComponent<Collider2D>();
        if (pickUpCollider != null )
            pickUpCollider.enabled = false;
    }

    void FixedUpdate()
    {
        timeSpent += Time.fixedDeltaTime;

        if (timeSpent > dispawnTime)
        {
            Despawn();
        }

        if (timeSpent > timeToActivate)
        {
            pickUpCollider.enabled = true;
        }

        if (moveTo != null)
        {
            Vector3 direction = moveTo.position - transform.position;
            if (direction.magnitude > stopDistance)
            {
                direction.Normalize();

                Vector3 newPosition = Vector3.Lerp(transform.position, moveTo.position, moveSpeed * Time.deltaTime);

                transform.position = newPosition;
            }
            else
            {
                // TEMP
                var managerObject = GameObject.Find("GameManager");
                if (managerObject && managerObject.TryGetComponent<GameManager>(out var manager))
                {
                    Debug.Log("GameManager");

                    manager.AddItemToInventory(inventoryItemPrefab);

                    manager.AddItemToInventory(this.gameObject);


                    manager.AddCoins(1);
                }
                Despawn();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {     
        if (collision == null) return;

        if(collision.gameObject != null 
            && collision.gameObject.transform != null)
        {
            // TEMP
            //moveTo = collision.gameObject.transform;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (newTarget != null)
        {
            moveTo = newTarget;
        }
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }
}
