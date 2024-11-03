//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class TilemapPathfinding : MonoBehaviour
//{
//    public Tilemap tilemap;
//    public TileBase walkableTile;
//    public TileBase obstacleTile;

//    private Dictionary<Vector3Int, Node> grid = new Dictionary<Vector3Int, Node>();

//    void Start()
//    {
//        BuildGrid();
//    }

//    void BuildGrid()
//    {
//        BoundsInt bounds = tilemap.cellBounds;

//        foreach (var pos in bounds.allPositionsWithin)
//        {
//            if (tilemap.HasTile(pos))
//            {
//                TileBase tile = tilemap.GetTile(pos);
//                bool isWalkable = tile == walkableTile;
//                grid[pos] = new Node(pos, isWalkable);
//            }
//        }
//    }

//    public List<Node> FindPath(Vector3Int start, Vector3Int target)
//    {
//        if (!grid.ContainsKey(start) || !grid.ContainsKey(target)) return null;

//        Node startNode = grid[start];
//        Node targetNode = grid[target];

//        List<Node> openSet = new List<Node> { startNode };
//        HashSet<Node> closedSet = new HashSet<Node>();

//        while (openSet.Count > 0)
//        {
//            Node currentNode = openSet[0];
//            for (int i = 1; i < openSet.Count; i++)
//            {
//                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
//                    currentNode = openSet[i];
//            }

//            openSet.Remove(currentNode);
//            closedSet.Add(currentNode);

//            if (currentNode == targetNode)
//                return RetracePath(startNode, targetNode);

//            foreach (Node neighbor in GetNeighbors(currentNode))
//            {
//                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
//                    continue;

//                int newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
//                if (newGCost < neighbor.gCost || !openSet.Contains(neighbor))
//                {
//                    neighbor.gCost = newGCost;
//                    neighbor.hCost = GetDistance(neighbor, targetNode);
//                    neighbor.parent = currentNode;

//                    if (!openSet.Contains(neighbor))
//                        openSet.Add(neighbor);
//                }
//            }
//        }

//        return null;
//    }

//    List<Node> RetracePath(Node startNode, Node endNode)
//    {
//        List<Node> path = new List<Node>();
//        Node currentNode = endNode;

//        while (currentNode != startNode)
//        {
//            path.Add(currentNode);
//            currentNode = currentNode.parent;
//        }

//        path.Reverse();
//        return path;
//    }

//    List<Node> GetNeighbors(Node node)
//    {
//        List<Node> neighbors = new List<Node>();
//        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

//        foreach (var direction in directions)
//        {
//            Vector3Int neighborPos = node.gridPosition + direction;
//            if (grid.ContainsKey(neighborPos))
//                neighbors.Add(grid[neighborPos]);
//        }

//        return neighbors;
//    }

//    int GetDistance(Node a, Node b)
//    {
//        int dx = Mathf.Abs(a.gridPosition.x - b.gridPosition.x);
//        int dy = Mathf.Abs(a.gridPosition.y - b.gridPosition.y);
//        return dx + dy;
//    }
//}



using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPathfinding : MonoBehaviour
{
    public Tilemap tilemap;

    public TileBase walkableTile;
    public TileBase obstacleTile;

    public bool skipCorners = true;

    private Dictionary<Vector3Int, Node> grid = new Dictionary<Vector3Int, Node>();

    void Start()
    {
        BuildGrid();
    }

    void BuildGrid()
    {
        BoundsInt bounds = tilemap.cellBounds;

        // Step 1: Initialize all nodes in the grid
        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                TileBase tile = tilemap.GetTile(pos);
                bool isWalkable = tile != obstacleTile;
                grid[pos] = new Node(pos, isWalkable);
            }
        }

        // Step 2: Pre-calculate neighbors for each node
        foreach (var kvp in grid)
        {
            Node node = kvp.Value;
            node.neighbors = CalculateNeighbors(node);
        }

    }

    List<Node> CalculateNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        // Cardinal directions
        Vector3Int[] cardinalDirections = {
        Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
    };

        // Diagonal directions and their corresponding "corner" tiles
        (Vector3Int direction, Vector3Int check1, Vector3Int check2)[] diagonalDirections = {
        (new Vector3Int(-1, 1, 0), Vector3Int.left, Vector3Int.up),    // Top-left
        (new Vector3Int(1, 1, 0), Vector3Int.right, Vector3Int.up),    // Top-right
        (new Vector3Int(-1, -1, 0), Vector3Int.left, Vector3Int.down), // Bottom-left
        (new Vector3Int(1, -1, 0), Vector3Int.right, Vector3Int.down)  // Bottom-right
    };

        // Add cardinal neighbors
        foreach (var direction in cardinalDirections)
        {
            Vector3Int neighborPos = node.gridPosition + direction;
            if (grid.ContainsKey(neighborPos) && grid[neighborPos].isWalkable)
            {
                neighbors.Add(grid[neighborPos]);
            }
        }

        // Add diagonal neighbors if both "corner" tiles are walkable
        foreach (var (direction, check1, check2) in diagonalDirections)
        {
            Vector3Int neighborPos = node.gridPosition + direction;
            Vector3Int corner1 = node.gridPosition + check1;
            Vector3Int corner2 = node.gridPosition + check2;

            if (grid.ContainsKey(neighborPos) && grid[neighborPos].isWalkable &&
                grid.ContainsKey(corner1) && grid[corner1].isWalkable &&
                grid.ContainsKey(corner2) && grid[corner2].isWalkable)
            {
                neighbors.Add(grid[neighborPos]);
            }
        }

        return neighbors;
    }

    public List<Node> FindPath(Vector3Int start, Vector3Int target)
    {
        if (!grid.ContainsKey(start) || !grid.ContainsKey(target)) return null;

        Node startNode = grid[start];
        Node targetNode = grid[target];

        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost 
                    || (openSet[i].FCost == currentNode.FCost 
                    && openSet[i].hCost < currentNode.hCost)
                )
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in currentNode.neighbors)
            {
                if (closedSet.Contains(neighbor))
                    continue;

                int newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newGCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    //public List<Node> FindPath(Vector3Int start, Vector3Int target)
    //{
    //    if (!grid.ContainsKey(start) || !grid.ContainsKey(target)) return null;

    //    Node startNode = grid[start];
    //    Node targetNode = grid[target];

    //    List<Node> openSet = new List<Node> { startNode };
    //    HashSet<Node> closedSet = new HashSet<Node>();

    //    while (openSet.Count > 0)
    //    {
    //        Node currentNode = openSet[0];
    //        for (int i = 1; i < openSet.Count; i++)
    //        {
    //            if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
    //                currentNode = openSet[i];
    //        }

    //        openSet.Remove(currentNode);
    //        closedSet.Add(currentNode);

    //        if (currentNode == targetNode)
    //            return RetracePath(startNode, targetNode);

    //        foreach (Node neighbor in GetNeighbors(currentNode))
    //        {
    //            if (!neighbor.isWalkable || closedSet.Contains(neighbor))
    //                continue;

    //            int newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
    //            if (newGCost < neighbor.gCost || !openSet.Contains(neighbor))
    //            {
    //                neighbor.gCost = newGCost;
    //                neighbor.hCost = GetDistance(neighbor, targetNode);
    //                neighbor.parent = currentNode;

    //                if (!openSet.Contains(neighbor))
    //                    openSet.Add(neighbor);
    //            }
    //        }
    //    }

    //    return null;
    //}

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        Vector3Int[] cardinalDirections = {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        // Diagonal directions and their corresponding "corner" tiles
        (Vector3Int direction, Vector3Int check1, Vector3Int check2)[] diagonalDirections = {
            (new Vector3Int(-1, 1, 0), Vector3Int.left, Vector3Int.up),    // Top-left
            (new Vector3Int(1, 1, 0), Vector3Int.right, Vector3Int.up),    // Top-right
            (new Vector3Int(-1, -1, 0), Vector3Int.left, Vector3Int.down), // Bottom-left
            (new Vector3Int(1, -1, 0), Vector3Int.right, Vector3Int.down)  // Bottom-right
        };

        // Add cardinal neighbors
        foreach (var direction in cardinalDirections)
        {
            Vector3Int neighborPos = node.gridPosition + direction;
            if (grid.ContainsKey(neighborPos) && grid[neighborPos].isWalkable)
            {
                neighbors.Add(grid[neighborPos]);
            }
        }

        // Add diagonal neighbors if both "corner" tiles are walkable
        foreach (var (direction, check1, check2) in diagonalDirections)
        {
            Vector3Int neighborPos = node.gridPosition + direction;
            Vector3Int corner1 = node.gridPosition + check1;
            Vector3Int corner2 = node.gridPosition + check2;

            if (grid.ContainsKey(neighborPos) && grid[neighborPos].isWalkable &&
                grid.ContainsKey(corner1) && grid[corner1].isWalkable &&
                grid.ContainsKey(corner2) && grid[corner2].isWalkable)
            {
                neighbors.Add(grid[neighborPos]);
            }
        }

        return neighbors;

        #region Old
        //List<Node> neighbors = new List<Node>();
        //Vector3Int[] directions = {
        //    Vector3Int.up, 
        //    Vector3Int.down, 
        //    Vector3Int.left,
        //    Vector3Int.right,

        //    new Vector3Int(-1, 1, 0), 
        //    new Vector3Int(1, 1, 0),
        //    new Vector3Int(-1, -1, 0), 
        //    new Vector3Int(1, -1, 0)
        //};

        //foreach (var direction in directions)
        //{
        //    Vector3Int neighborPos = node.gridPosition + direction;
        //    if (grid.ContainsKey(neighborPos))
        //        neighbors.Add(grid[neighborPos]);
        //}

        //return neighbors;
        #endregion
    }

    int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridPosition.x - b.gridPosition.x);
        int dy = Mathf.Abs(a.gridPosition.y - b.gridPosition.y);

        if (dx > dy)
            return 14 * dy + 10 * (dx - dy);
        return 14 * dx + 10 * (dy - dx);
    }

    public void RecalculateNode(Vector3 position)
    {
        RecalculateNode(tilemap.WorldToCell(position));
    }

    public void RecalculateNode(Vector3Int position)
    {
        // Check if the node exists in the grid
        if (!grid.ContainsKey(position)) return;

        // Get the tile at this position
        TileBase tile = tilemap.GetTile(position);
        bool isWalkable = tile != obstacleTile;

        // Update the node's walkability
        Node node = grid[position];
        node.isWalkable =  isWalkable; // false

        // Recalculate neighbors for this node
        node.neighbors = CalculateNeighbors(node);

        // Optionally, update neighbors for all adjacent nodes to ensure they correctly reference this node
        UpdateAdjacentNeighbors(node);
    }

    // Helper method to update the neighbors for adjacent nodes
    void UpdateAdjacentNeighbors(Node node)
    {
        Vector3Int[] allDirections = {
        Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right,
        new Vector3Int(-1, 1, 0), new Vector3Int(1, 1, 0),
        new Vector3Int(-1, -1, 0), new Vector3Int(1, -1, 0)
    };

        foreach (var direction in allDirections)
        {
            Vector3Int neighborPos = node.gridPosition + direction;
            if (grid.ContainsKey(neighborPos))
            {
                Node neighborNode = grid[neighborPos];
                neighborNode.neighbors = CalculateNeighbors(neighborNode); // Recalculate neighbors
            }
        }
    }
    void OnDrawGizmos()
    {
        if (grid == null) return;

        foreach (var kvp in grid)
        {
            Node node = kvp.Value;

            // Convert tile position to world position for Gizmos
            Vector3 worldPos = tilemap.CellToWorld(node.gridPosition) + tilemap.cellSize / 2;

            // Set Gizmos color based on walkability
            Gizmos.color = node.isWalkable ? Color.green : Color.red;

            // Draw a circle with small radius at the node's position
            Gizmos.DrawSphere(worldPos, 0.1f);
        }
    }

}
