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

    private List<string> collisionTags = new List<string>();

    void Start()
    {
        // TODO
        collisionTags.Add("Enemy");

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

    public void Attack()
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
    }
#endif

}
