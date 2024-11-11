using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
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

    private List<Node> path;
    private int targetIndex = 0;
    public TilemapPathfinding pathfinding;

    public float pathUpdateDelay = 0.5f; // Delay between path updates in seconds

    public enum EnemyState
    {
        Idle,
        Pursuing,
        Searching
    }

    private Vector3 lastSeenPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = EnemyState.Idle;

        StartCoroutine(UpdatePath());
    }

    private IEnumerator UpdatePath()
    {
        while (true)
        {
            //Vector3Int startPos = pathfinding.tilemap.WorldToCell(transform.position);
            //Vector3Int targetPos = pathfinding.tilemap.WorldToCell(player.position);

            // Recalculate the path to the player's current position
            path = pathfinding.FindPath(transform.position, player.position);

            targetIndex = 0; // Reset the index to start from the first node

            yield return new WaitForSeconds(pathUpdateDelay); // Wait before recalculating the path again
        }
    }

    private void FollowPath()
    {
        if (path == null || path.Count == 0 || targetIndex >= path.Count) return;

        // Get the next position in the path and convert it to world space
        Vector3 targetPosition = pathfinding.tilemap.CellToWorld(path[targetIndex].gridPosition) + new Vector3(0.5f, 0.5f);

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if we have reached the current target position, then move to the next node in the path
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetIndex++;
        }
    }

    // TEST IMPROVE
    //private void FollowPath()
    //{
    //    if (path == null || path.Count == 0 || targetIndex >= path.Count) return;

    //    Vector3 targetPosition = pathfinding.tilemap.CellToWorld(path[targetIndex].gridPosition) + new Vector3(0.5f, 0.5f);

    //    // Check if the movement is diagonal
    //    Vector3 direction = (targetPosition - transform.position).normalized;
    //    if (Mathf.Abs(direction.x) > 0.1f && Mathf.Abs(direction.y) > 0.1f)
    //    {
    //        // Apply a small perpendicular offset to avoid corner collision
    //        Vector3 offset = new Vector3(-direction.y, direction.x) * 0.1f;
    //        targetPosition += offset;
    //    }

    //    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

    //    if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
    //    {
    //        targetIndex++;
    //    }
    //}

    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (state == EnemyState.Idle)
        {
            if (playerDistance <= detectionRadius && HasLineOfSight())
            {
                state = EnemyState.Pursuing;
                //lastSeenPosition = player.position;
            }
        }

        if (state == EnemyState.Pursuing)
        {
            if (playerDistance <= detectionRadius && HasLineOfSight())
            {
                //lastSeenPosition = player.position;
                //rb.velocity = speed * (player.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else if (playerDistance <= detectionRadius || !HasLineOfSight())
            {
                state = EnemyState.Searching;
            }
            else
            {
                state = EnemyState.Idle;
                //rb.velocity = Vector2.zero;
            }
        }

        if (state == EnemyState.Searching)
        {
            if (playerDistance <= detectionRadius && HasLineOfSight())
            {
                state = EnemyState.Pursuing;
                //lastSeenPosition = player.position;
                //rb.velocity = Vector2.zero;
            }

            else if (playerDistance <= detectionRadius)
            {
                FollowPath();
            }
        }
    }


    private bool HasLineOfSightCircle()
    {
        // TODO make circles colliders appear not line? Or two - three raycats

        return false; 
    }

    // To improve
    private bool HasLineOfSight(Transform from, Transform to)
    {
        Vector2 direction = to.position - from.position;
        RaycastHit2D hit = Physics2D.Raycast(from.position, direction, detectionRadius, obstacleLayer | LayerMask.GetMask("Player"));

        return hit.collider != null && hit.collider.transform == player;
    }

    private bool HasLineOfSight()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            directionToPlayer, detectionRadius, obstacleLayer | LayerMask.GetMask("Player"));

        return hit.collider != null && hit.collider.transform == player;

        //if (directHit) return true;

        //if (state != EnemyState.Searching) return false;
        
        //Vector2 directionToPlayer2 = player.position - lastKnownPosition;
        //RaycastHit2D hit2 = Physics2D.Raycast(lastKnownPosition,
        //    directionToPlayer2, detectionRadius, obstacleLayer | LayerMask.GetMask("Player"));

        //return hit2.collider != null && hit2.collider.transform == player;
    }


    private void OnDrawGizmos()
    {
        if (state == EnemyState.Searching)
        {
            if (path != null && path.Count > 0)
            {
                Gizmos.color = Color.red;

                // Draw a line between each consecutive node in the path
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 currentNodePosition = pathfinding.tilemap.CellToWorld(path[i].gridPosition) + new Vector3(0.5f, 0.5f);
                    Vector3 nextNodePosition = pathfinding.tilemap.CellToWorld(path[i + 1].gridPosition) + new Vector3(0.5f, 0.5f);

                    Gizmos.DrawLine(currentNodePosition, nextNodePosition);
                }

                // Draw a sphere at each node position
                foreach (var node in path)
                {
                    Vector3 nodePosition = pathfinding.tilemap.CellToWorld(node.gridPosition) + new Vector3(0.5f, 0.5f);
                    Gizmos.DrawSphere(nodePosition, 0.1f);
                }
            }
        }

        else
        {
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
                //Gizmos.DrawLine(lastSeenPosition, player.position);
            }
        }
        
    }
}