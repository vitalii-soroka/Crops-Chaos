using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform player;  
    public float smoothSpeed = 0.125f; 
    public Vector3 offset;

    [SerializeField] bool smoothMove = false;

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        
        desiredPosition.z = transform.position.z;

        if (smoothMove)
        {
            Vector3 smoothedPosition
            = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
        else
        {
            transform.position = desiredPosition;
        }
    }
}
