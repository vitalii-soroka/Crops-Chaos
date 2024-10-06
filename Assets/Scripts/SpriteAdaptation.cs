using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("Component helps to change sprites depending on other sprites with the same layer")]
public class SpriteAdaptation : MonoBehaviour
{
    public Sprite[] sprites;

    [SerializeField] private SpriteRenderer spriteRenderer;

    public LayerMask fieldLayer;
    //public Vector2Int gridPosition;

    private Transform lastTransform;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateSprite();

        lastTransform = transform;
    }

    public void Update()
    {
        //if (lastTransform != transform)
        //{
        //    UpdateFieldSprite();
        //    lastTransform = transform;
        //}
    }

    public void UpdateNeiborSprites()
    {
        UpdateNeibor(Vector2Int.up);
        UpdateNeibor(Vector2Int.down);
        UpdateNeibor(Vector2Int.left);
        UpdateNeibor(Vector2Int.right);
    }

    public void UpdateNeibor(Vector2Int direction)
    {
        Vector2 neighborPosition = (Vector2)transform.position + (Vector2)direction;

        Collider2D neighbor = Physics2D.OverlapCircle(neighborPosition, 0.1f, fieldLayer);

        if (neighbor != null && neighbor.TryGetComponent<SpriteAdaptation>(out var spriteAdapt))
        {
            spriteAdapt.UpdateSprite();
        }
    }

    public void UpdateSprite()
    {
        // Check the neighboring fields
        bool hasFieldAbove = CheckForNeighbor(Vector2Int.up);
        bool hasFieldBelow = CheckForNeighbor(Vector2Int.down);
        bool hasFieldLeft = CheckForNeighbor(Vector2Int.left);
        bool hasFieldRight = CheckForNeighbor(Vector2Int.right);

        // Determine sprite based on the neighbors
        int spriteIndex = GetSpriteIndex(hasFieldAbove, hasFieldBelow, hasFieldLeft, hasFieldRight);
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    bool CheckForNeighbor(Vector2Int direction)
    {
        // Get the world position of the neighboring grid cell
        Vector2 neighborPosition = (Vector2)transform.position + (Vector2)direction;

        // Check if there's another field at the neighbor position using Physics2D or grid lookup
        Collider2D neighbor = Physics2D.OverlapCircle(neighborPosition, 0.1f, fieldLayer);

        return neighbor != null;
    }

    int GetSpriteIndex(bool hasAbove, bool hasBelow, bool hasLeft, bool hasRight)
    {
        //Debug.Log(hasAbove + " " + hasBelow + " " + hasLeft + " " + hasRight);
        /*
          0 1 2
          3 4 5
          6 7 8  
          
          9 - 11 <-- default sprites

          12 13 14
        
          15
          16
          17
        */
        // random default sprite
        if (!hasAbove && !hasBelow && !hasLeft && !hasRight) return Random.Range(9, 12);

        if (!hasAbove && hasBelow && !hasLeft  && hasRight) return 0;
        if (!hasAbove && hasBelow && hasRight && hasLeft) return 1;
        if (!hasAbove && hasBelow && !hasRight && hasLeft ) return 2;
        if (hasAbove && hasBelow && !hasLeft && hasRight) return 3;
        if (hasAbove && hasBelow && hasLeft && hasRight) return 4;
        if (hasAbove && hasBelow && hasLeft && !hasRight) return 5;
        if (hasAbove && !hasBelow && !hasLeft && hasRight) return 6;
        if (hasAbove && !hasBelow && hasLeft && hasRight) return 7;
        if (hasAbove && !hasBelow && hasLeft && !hasRight) return 8;

        if (!hasAbove && !hasBelow && hasRight && !hasLeft) return 12;
        if (!hasAbove && !hasBelow && hasRight && hasLeft) return 13;
        if (!hasAbove && !hasBelow && !hasRight && hasLeft) return 14;

        if (!hasAbove && hasBelow && !hasRight && !hasLeft) return 15;
        if (hasAbove && hasBelow && !hasRight && !hasLeft) return 16;
        if (hasAbove && !hasBelow && !hasRight && !hasLeft) return 17;


        // Default sprite
        return 11;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
    public void SelectSprite(int index)
    {
        if (index >= 0 && index < sprites.Length)
        {
            spriteRenderer.sprite = sprites[index]; // Assign selected sprite
        }
        else
        {
            Debug.LogError("Sprite index out of range!");
        }
    }
}
