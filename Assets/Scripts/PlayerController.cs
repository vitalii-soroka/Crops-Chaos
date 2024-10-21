using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;

    [SerializeField] GameObject buildShop;

    private Rigidbody2D rb;
    private Vector2 movement;

    private CharacterState state;

    [SerializeField] Weapon weapon;

    private Animator animator;

    // TO EDIT FOR USING ITEM FROM INVENTORY LIKE SEEDS
    [SerializeField] GameObject cropPrefabTest;
    
    [SerializeField] TriggerChecker smallTrigger;
    [SerializeField] TriggerChecker mediumTrigger;
    [SerializeField] TriggerChecker bigTrigger;

    [SerializeField] TileField field;

    Inventory inventory;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // TO EDIT
        if(smallTrigger == null) smallTrigger = GetComponentInChildren<TriggerChecker>();


        if (inventory == null) inventory = GetComponentInChildren<Inventory>();
    }

    void FieldInteraction()
    {
        if (smallTrigger == null || smallTrigger.trigger == null || cropPrefabTest == null) return;

        if (smallTrigger.CheckTrigger("Field") 
            && smallTrigger.trigger.TryGetComponent<Field>(out var field))
        {
            if (field.HasCrop())
            {
                field.Gather();
            }
            else
            {
                field.Plant(cropPrefabTest);
            }
        }
    }

    void FieldTileInteraction()
    {
        if (field == null) return;

        if (field.IsField(transform.position))
        {
            if (field.HasCrop(transform.position))
            {
                field.Gather(transform.position);
                return;
            }

            if (inventory != null)
            {
                var selectedItem = inventory.GetSelectedItem();
                if (selectedItem != null && selectedItem.type == Item.ItemType.Seed)
                {
                    var crop = ((SeedItem)selectedItem).cropPrefab;
                    field.Plant(transform.position, crop);
                    inventory.SubstructItem();
                }
            }
           
        }
    }

    void Update()
    {
        UpdateAxis();

        if (Input.GetMouseButtonDown(0))
        {
            FieldTileInteraction();
            //else Attack();
        }
        if (Input.GetKeyDown(KeyCode.J) && buildShop != null)
        {
            buildShop.SetActive(!buildShop.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Q) && inventory != null)
        {
            inventory.Drop();
        }
    }

    private void FixedUpdate()
    {
        Walk();
    }

    private void Attack()
    {
        weapon.Attack();
    }

    private void UpdateAxis()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.magnitude > 1) movement.Normalize();
    }

    private void Walk()
    {
        Move(walkSpeed);
    }

    private void Move(float speed)
    {
        rb.velocity = speed * movement;
        animator.SetFloat("speed", rb.velocity.magnitude);
    }

    private void Slide()
    {

    }

    private void SetState(CharacterState newState)
    {
        switch (state)
        {
            case CharacterState.Attack:
                state = newState;
                break;
            case CharacterState.Walk:
                if (state != CharacterState.Attack)
                    state = newState;
                break;

        }
    }
}
