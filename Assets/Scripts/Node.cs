using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Vector3Int gridPosition;
    public bool isWalkable;
    public int gCost; // Distance from start node
    public int hCost; // Heuristic distance to the target node
    public Node parent;

    public List<Node> neighbors = new List<Node>(); // Pre-calculated neighbors

    public int FCost => gCost + hCost;

    public Node(Vector3Int gridPosition, bool isWalkable)
    {
        this.gridPosition = gridPosition;
        this.isWalkable = isWalkable;
    }
}
