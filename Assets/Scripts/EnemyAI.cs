using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;


    public float detectionRadius = 10f; 
    public float loseSightRadius = 12f;

    public float attackDistance = 1.5f;

    public float cropEatingTime = 2f;

    //private Transform currentTarget;

    private bool isEating = false;

    


    [SerializeField] float speed = 1.0f;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] EnemyState state;

    [SerializeField] LayerMask layerMask;


    [SerializeField] private LayerMask obstacleLayer; // Set this layer for objects blocking LOS

    public float detectionRadiuss = 2f;

    public enum EnemyState
    {
        Idle,
        Pursuing,
        Temp
    }

    private Vector3 lastKnownPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = EnemyState.Idle;
    }

    void Update()
    {
        // TODO On trigger check i guess

        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (state == EnemyState.Idle)
        {
            if (playerDistance <= detectionRadius && HasLineOfSight())
            {
                state = EnemyState.Pursuing;
                lastKnownPosition = player.position;
            }
        }

        if (state == EnemyState.Pursuing)
        {
            if (playerDistance <= detectionRadius && HasLineOfSight())
            {
                lastKnownPosition = player.position;
                rb.velocity = speed * (player.position - transform.position).normalized;
            }

            else if (playerDistance <= loseSightRadius)
            {
                state = EnemyState.Temp;
                lastKnownPosition = player.position;
                rb.velocity = speed * (lastKnownPosition - transform.position).normalized;
            }
            else
            {
                state = EnemyState.Idle;
                rb.velocity = Vector2.zero;
            }
        }

        if (state == EnemyState.Temp)
        {
            if (HasLineOfSight() && playerDistance <= detectionRadius)
            {
                state = EnemyState.Pursuing;
                lastKnownPosition = player.position;
            }
            else if (Vector3.Distance(transform.position, lastKnownPosition) < 1)
            {
                state = EnemyState.Idle;
                rb.velocity = Vector2.zero;
            }
        }
    }

    private bool HasLineOfSightCircle()
    {
        // TODO make circles colliders appear not line? Or two - three raycats

        return false; 
    }


    private bool HasLineOfSight()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            directionToPlayer, detectionRadius, obstacleLayer | LayerMask.GetMask("Player"));



        bool directHit = hit.collider != null && hit.collider.transform == player;

        if (directHit) return true;

        if (state != EnemyState.Pursuing) return false;
        
        Vector2 directionToPlayer2 = player.position - lastKnownPosition;
        RaycastHit2D hit2 = Physics2D.Raycast(lastKnownPosition,
            directionToPlayer2, detectionRadius, obstacleLayer | LayerMask.GetMask("Player"));

        return hit2.collider != null && hit2.collider.transform == player;
    }

    private void OnDrawGizmos()
    {
        // Draw the detection radius in green
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw the attack distance in red
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, attackDistance);

        if (player != null)
        {
            // Draw LOS line to the player, colored based on visibility
            Gizmos.color = HasLineOfSight() ? Color.green : Color.gray;
            Gizmos.DrawLine(transform.position, player.position);
            Gizmos.DrawLine(lastKnownPosition, player.position);
        }

    }

}