using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Dropable : MonoBehaviour
{
    // Make sure to not change
    [SerializeField] private GameObject dropPrefab;

    [Tooltip("If object drops something after attacking item")]
    [SerializeField] private bool dropOnBreak = true;

    public void Drop()
    {
        var item = Instantiate(dropPrefab);
        if (item) item.transform.position = this.transform.position;
       
        Destroy(this.gameObject);
    }

    public bool IsDropAfterBreak() {  return  dropOnBreak; }
}
