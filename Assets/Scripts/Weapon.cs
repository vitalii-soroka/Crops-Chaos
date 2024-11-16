using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Collider2D hitCollider;
    [SerializeField] float damage = 10.0f;
    [SerializeField] float coolDown = 0.5f;
    [SerializeField] float attackTime = 0.2f;

    [SerializeField] float sinceLastAttack = 0.0f;


    [SerializeField] private float attackRange = 0.0f;
    [SerializeField] private Vector2 attackBoxSize = new Vector2(2.0f, 1.0f);

    [SerializeField] private List<string> collisionTags = new List<string> { "Enemy" };

    void Start()
    {
        hitCollider = GetComponent<Collider2D>();
        SetCollider(false);
    }

    void Update()
    {
        if (sinceLastAttack <= coolDown)
        {
            sinceLastAttack += Time.deltaTime;
        }

        if (sinceLastAttack > attackTime)
        {
            SetCollider(false);
        }
    }

    private void RotateTowardsMouse()
    {
        if (Camera.main == null) return;

        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Calculate the direction from the player to the mouse
        Vector3 direction = (mousePosition - this.gameObject.transform.position).normalized;

        // Rotate the weapon to face the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Attack()
    {
        if (sinceLastAttack < coolDown) return;

        RotateTowardsMouse();

        sinceLastAttack = 0.0f;

        // Calculate the attack area center based on weapon's position and direction
        Vector3 attackCenter = transform.position + transform.right * attackRange;

        // Perform an OverlapBox check
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, attackBoxSize, transform.eulerAngles.z);
        foreach (Collider2D hit in hits)
        {
            if (hit == null || hit.gameObject == gameObject) continue;

            if (collisionTags.Contains(hit.tag))
            {
                if (hit.TryGetComponent<Health>(out var health))
                {
                    health.TakeDamage(damage);
                    Debug.Log("Damage");
                }
            }

            if (hit.TryGetComponent<Dropable>(out var drop))
            {
                drop.Drop();
            }
        }
    }

    public void AttackOld()
    {
        if (sinceLastAttack >= coolDown)
        {
            sinceLastAttack = 0.0f;
            SetCollider(true);
        }
    }

    public void SetCollider(bool newstate)
    {
        if (hitCollider != null)
        {
            hitCollider.enabled = newstate;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null || other.gameObject == null) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(damage);
            }
            return;
        }

        if (other.TryGetComponent<Dropable>(out var drop))
        {
            drop.Drop();
            //if (drop.IsDropAfterBreak()) drop.Drop();
            //else Destroy(drop.gameObject);
        }
    }
    private enum WeaponState
    {
        None = 0,
        Attack = 1,
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (hitCollider && hitCollider.enabled)
        {
            Gizmos.DrawCube(hitCollider.transform.position, 
                new Vector3(2, 1f, 1f));
        }


        // Calculate the attack area center based on the weapon's position and direction
        Vector3 attackCenter = transform.position + transform.right * attackRange;

        // Set Gizmo color for visibility
        Gizmos.color = Color.red;

        // Draw the attack box area in the scene view for debugging
        Gizmos.DrawWireCube(attackCenter, attackBoxSize);

        // Optionally, draw a line from the player to show the direction
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, attackCenter);
    }
#endif

}
