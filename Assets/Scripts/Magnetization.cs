using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Magnetization : MonoBehaviour
{

    [SerializeField] public float magnetizationSpeed = 1.0f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float despawnTime = 100.0f;
    [SerializeField] private float activateDelay = 2.0f;

    public Transform target;

    private float timeDropped;

    public bool bCanMagnetize = false;


    // NEW FEATURE
    public Dictionary<Transform, int> possibleTargets = new Dictionary<Transform, int>();
    public void AddTarget(Transform t, int priority = 0)
    {
        possibleTargets.Add(t, priority);
        UpdateTarget();
    }
    public void RemoveTarget(Transform t)
    {
        possibleTargets.Remove(t);
        UpdateTarget();
    }
    private void UpdateTarget()
    {
        target = GetLowestPriorityTarget();
    }

    private Transform GetLowestPriorityTarget()
    {
        if (possibleTargets.Count == 0)
            return null; 

        return possibleTargets
            .OrderBy(target => target.Value) 
            .First().Key; 
    }

    //

    void Start()
    {

    }

    void Update()
    {
        timeDropped += Time.fixedDeltaTime;

        if (timeDropped > despawnTime)
        {
            Despawn();
            return;
        }

        if (target != null && bCanMagnetize)
        {
            MoveTowardsTarget();
        }
    }

    public bool TrySetTarget(Transform newTarget)
    {
        if (target == null)
        {
            target = newTarget;
        }

        return target != null;
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = target.position - transform.position;
        if (direction.magnitude > stopDistance)
        {
            direction.Normalize();
            transform.position = Vector3.Lerp(transform.position, target.position, magnetizationSpeed * Time.deltaTime);
        }
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }
}
