using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform[] crops;
    public float detectionRadius = 10f;
    public float attackDistance = 1.5f;
    public float cropEatingTime = 2f;

    private Transform currentTarget;
    private NavMeshAgent agent;
    private bool isEating = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindTarget();
    }

    void Update()
    {
        if (isEating) return;

        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= detectionRadius)
        {
            currentTarget = player;
            MoveToTarget();
        }
        else
        {
            FindTarget();
        }

        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) <= attackDistance)
        {
            if (currentTarget == player)
                AttackPlayer();
            else
                StartCoroutine(EatCrop(currentTarget));
        }
    }

    private void FindTarget()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestCrop = null;

        foreach (Transform crop in crops)
        {
            float distance = Vector3.Distance(transform.position, crop.position);
            if (distance < closestDistance && distance <= detectionRadius)
            {
                closestDistance = distance;
                closestCrop = crop;
            }
        }

        currentTarget = closestCrop;
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (currentTarget != null)
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void AttackPlayer()
    {
        Debug.Log("Enemy attacks the player!");
    }

    private IEnumerator EatCrop(Transform crop)
    {
        isEating = true;
        agent.isStopped = true;
        Debug.Log("Enemy starts eating the crop!");

        yield return new WaitForSeconds(cropEatingTime);

        Debug.Log("Enemy finishes eating the crop!");
        Destroy(crop.gameObject);

        isEating = false;
        FindTarget();
    }
}
