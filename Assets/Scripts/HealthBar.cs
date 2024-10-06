using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Vector3 offsetY;

    [SerializeField] Transform _parent;
    void Start()
    {

    }

    void Update()
    {
        UpdatePosition();
    }

    public void SetParent(Transform parent)
    {
        _parent = parent;
    }

    public void UpdatePosition()
    {
        if (_parent != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(_parent.position + offsetY);
            transform.position = screenPosition;
        }
    }
}
