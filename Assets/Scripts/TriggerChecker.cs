using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    [SerializeField] private string compareTag = "Field";
    // [SerializeField] 
    // TODO Ignore Layers

    public GameObject trigger;

    void Start()
    {
        
    }

    public bool CheckTrigger()
    {
        return trigger != null;
    }

    public bool CheckTrigger(string Tag)
    {
        return trigger != null && trigger.tag == Tag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || collision.gameObject == null) return;

        if (collision.CompareTag(compareTag))
        {
            trigger = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null || collision.gameObject == null) return;

        if (collision.CompareTag(compareTag))
        {
            if (collision.gameObject == trigger) trigger = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (trigger)
        {
            Gizmos.DrawCube(trigger.transform.position, new Vector3(0.1f, 0.1f, 0.1f));
        }
    }
#endif

}
