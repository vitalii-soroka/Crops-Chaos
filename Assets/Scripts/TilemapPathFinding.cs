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

    private Dictionary<Vector3Int, Node> grid = new Dictionary<Vector3Int, Node>();

    void Start()
    {
        BuildGrid();
    }

    void BuildGrid()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                TileBase tile = tilemap.GetTile(pos);
                bool isWalkable = tile != obstacleTile;
                grid[pos] = new Node(pos, isWalkable);
            }
        }
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
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
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
        Vector3Int[] directions = {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right,
            new Vector3Int(-1, 1, 0), new Vector3Int(1, 1, 0),
            new Vector3Int(-1, -1, 0), new Vector3Int(1, -1, 0)
        };

        foreach (var direction in directions)
        {
            Vector3Int neighborPos = node.gridPosition + direction;
            if (grid.ContainsKey(neighborPos))
                neighbors.Add(grid[neighborPos]);
        }

        return neighbors;
    }

    int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridPosition.x - b.gridPosition.x);
        int dy = Mathf.Abs(a.gridPosition.y - b.gridPosition.y);

        if (dx > dy)
            return 14 * dy + 10 * (dx - dy);
        return 14 * dx + 10 * (dy - dx);
    }
}
