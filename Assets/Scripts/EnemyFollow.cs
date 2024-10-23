using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform target;


    public Transform cropTarget = null;
    [SerializeField] private float speed = 2.0f;

    void Start()
    {

    }

    void Update()
    {
        if (cropTarget == null)  
        {
            var temp = GameObject.FindGameObjectWithTag("Crop"); 
            if (temp != null) cropTarget = temp.GetComponent<Transform>();
        }

        // TODO maybe navMesh movemnt
        if (cropTarget != null)
        {
            MoveTowards();
        }
    }

    public void MoveTowards()
    {
        if (cropTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, cropTarget.position, speed * Time.deltaTime);
        }
    }
}
