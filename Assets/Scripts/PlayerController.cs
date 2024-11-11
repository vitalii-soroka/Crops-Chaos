using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;

    [SerializeField] private float itemThrowForce = 5f;

    [SerializeField] private Vector3 itemthrowDirection = Vector3.left + Vector3.up;

    [SerializeField] GameObject buildShop;

    private Rigidbody2D rb;
    private Vector2 movement;

    private CharacterState state;

    [SerializeField] Weapon weapon;

    private Animator animator;

    // TO EDIT FOR USING ITEM FROM INVENTORY LIKE SEEDS
    //[SerializeField] GameObject cropPrefabTest;
    
    [SerializeField] TriggerChecker smallTrigger;
    [SerializeField] TriggerChecker mediumTrigger;
    [SerializeField] TriggerChecker bigTrigger;

    //[SerializeField] TileField field;

    //[SerializeField] GameActionsManager gameManager;
    //[SerializeField] GameActionsManager gameManager;

    Inventory inventory;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // TO EDIT
        if(smallTrigger == null) smallTrigger = GetComponentInChildren<TriggerChecker>();


        if (inventory == null) inventory = GetComponentInChildren<Inventory>();
    }

    //void FieldTileInteraction()
    //{
    //    if (field == null) return;

    //    if (field.IsField(transform.position))
    //    {
    //        if (field.HasCrop(transform.position))
    //        {
    //            field.Gather(transform.position);
    //            return;
    //        }

    //        if (inventory != null)
    //        {
    //            var selectedItem = inventory.GetSelectedItem();
    //            if (selectedItem != null && selectedItem.type == Item.ItemType.Seed)
    //            {
    //                var crop = ((SeedItem)selectedItem).cropPrefab;
    //                field.Plant(transform.position, crop);
    //                inventory.SubstructItem();
    //            }
    //        }

    //    }
    //    else
    //    {
    //        Debug.Log("Dig");
    //        if (gameManager != null)
    //        {
    //            gameManager.Dig(transform.position);
    //        }
    //    }
    //}
    void FieldTileInteraction2()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.HasField(transform.position))
        {
            if (GameManager.Instance.HasCrop(transform.position))
            {
                GameManager.Instance.GatherCrop(transform.position);
                return;
            }

            if (inventory != null)
            {
                var selectedItem = inventory.GetSelectedItem();
                if (selectedItem != null && selectedItem.type == Item.ItemType.Seed)
                {
                    var crop = ((SeedItem)selectedItem).cropPrefab;
                    GameManager.Instance.PlantCrop(transform.position, crop);
                    inventory.SubstructItem();
                }
            }
        }
        else
        {
            GameManager.Instance.Dig(transform.position);
        }
    }

    void Update()
    {
        UpdateAxis();

        if (Input.GetMouseButtonDown(1))
        {
            FieldTileInteraction2();
            //else Attack();
        }
        if (Input.GetKeyDown(KeyCode.J) && buildShop != null)
        {
            buildShop.SetActive(!buildShop.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Q) && inventory != null)
        {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector3 directionToMouse = (mousePosition - transform.position).normalized;

            inventory.Drop(directionToMouse, itemThrowForce);
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
