using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    public Tilemap walkable;
    
    private Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
    private Dictionary<Vector2Int, float> costSoFar = new Dictionary<Vector2Int, float>();
    private BoundsInt bounds;
    private Vector3Int[,] grid;

    void Awake()
    {
        walkable.CompressBounds();
        bounds = walkable.cellBounds;
        CreateGrid();
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int destination)
    {
        if (grid[start.x, start.y].z == 1)
            start = FindClosestWalkable(start, 1);

        if (grid[destination.x, destination.y].z == 1)
            destination = FindClosestWalkable(destination, 1);

        if (start == new Vector2Int(-1, -1) || destination == new Vector2Int(-1, -1))
            return new List<Vector2Int>();

        cameFrom.Clear();
        costSoFar.Clear();

        List<Vector2Int> path = AStar(start, destination);

        ShortenPath(path);

        return path;
    }

    private Vector2Int FindClosestWalkable(Vector2Int location, int depth)
    {
        if(depth == 0)
            return new Vector2Int(-1, -1);

        var candidates = GetNeighbors(location);

        foreach (var can in candidates)
            if (grid[can.x, can.y].z == 0)
                return can;

        foreach (var can in candidates)
        {
            var newCan = FindClosestWalkable(can, depth - 1);
            if (newCan != new Vector2Int(-1,-1))
                return newCan;
        }
            

        return new Vector2Int(-1, -1);
    }

    public Vector3 NextStep(Vector3 start, Vector3 destination)
    {
        var startGrid = TranslateFromGrid(walkable.WorldToCell(start));
        var destGrid = TranslateFromGrid(walkable.WorldToCell(destination));

        if (startGrid != destGrid)
        {
            var path = FindPath(startGrid, destGrid);

            if (path.Count > 1)
                return walkable.CellToWorld(TranslateToGrid(path[1]));
            else
                return start;
        }
        else
            return start;
    }

    private void CreateGrid()
    {
        grid = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < bounds.size.x; x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < bounds.size.y; y++, j++)
            {
                if (walkable.HasTile(new Vector3Int(x, y, 0)))
                    grid[i, j] = new Vector3Int(x, y, 0);
                else
                    grid[i, j] = new Vector3Int(x, y, 1);
            }
        }
    }

    private void ShortenPath(List<Vector2Int> path)
    {
        if (path.Count < 3)
            return;

        for (int i = 0; i + 2< path.Count; i++)
        {
            var a = path[i];
            var b = path[i + 2];

            while (IsInLineOfSight(a, b))
            {
                path.RemoveAt(i + 1);

                if (path.Count > i + 2)
                    b = path[i + 2];
                else break;
            }
        }
    }
    
    private bool IsInLineOfSight(Vector2Int a, Vector2Int b)
    {
        Vector2 delta = a - b;

        if (delta.x == 0)
        {
            if (delta.y < 0)
            {
                for (int y = a.y + 1; y < b.y; y++)
                {
                    if (grid[a.x, y].z == 1)
                        return false;
                }
            }
            else
            {
                for (int y = a.y - 1; y > b.y; y--)
                {
                    if (grid[a.x, y].z == 1)
                        return false;
                }
            }
        }
        else if (delta.y == 0)
        {
            if (delta.x < 0)
            {
                for (int x = a.x + 1; x < b.x; x++)
                {
                    if (grid[x, a.y].z == 1)
                        return false;
                }
            }
            else
            {
                for (int x = a.x - 1; x > b.x; x--)
                {
                    if (grid[x, a.y].z == 1)
                        return false;
                }
            }
        }
        else
        {
            var m = delta.y / delta.x;
            var mb = (-m * a.x) + a.y;

            if (delta.x < 0)
            {
                for (int x = a.x + 1; x < b.x; x++)
                {
                    if (!CheckByX(x, m, mb))
                        return false;
                }
            }
            else
            {
                for (int x = a.x - 1; x > b.x; x--)
                {
                    if (!CheckByX(x, m, mb))
                        return false;
                }
            }

            if (delta.y < 0)
            {
                for (int y = a.y + 1; y < b.y; y++)
                {
                    if (!CheckByY(y, m, mb))
                        return false;
                }
            }
            else
            {
                for (int y = a.y - 1; y > b.y; y--)
                {
                    if (!CheckByY(y, m, mb))
                        return false;
                }
            }
        }

        return true;
    }

    private bool CheckByY(int y, float m, float mb)
    {
        var real = (1 / m) * (y - mb);
        var floor = (int)Math.Floor(real);
        var ceiling = (int)Math.Ceiling(real);

        if (Math.Abs(ceiling - real) > Math.Abs(floor - real))
        {
            if (grid[floor, y].z == 1)
                return false;
        }
        else if (Math.Abs(ceiling - real) < Math.Abs(floor - real))
        {
            if (grid[ceiling, y].z == 1)
                return false;
        }
        else
        {
            if (grid[floor, y].z == 1 || grid[ceiling, y].z == 1)
                return false;
        }
        return true;
    }

    private bool CheckByX(int x, float m, float mb)
    {

        var real = m * x + mb;
        var floor = (int)Math.Floor(real);
        var ceiling = (int)Math.Ceiling(real);

        if (Math.Abs(ceiling - real) > Math.Abs(floor - real))
        {
            if (grid[x, floor].z == 1)
                return false;
        }
        else if (Math.Abs(ceiling - real) < Math.Abs(floor - real))
        {
            if (grid[x, ceiling].z == 1)
                return false;
        }
        else
        {
            if (grid[x, floor].z == 1 || grid[x, ceiling].z == 1)
                return false;
        }
        return true;
    }

    public Vector3Int TranslateToGrid(Vector2Int a)
    {
        return new Vector3Int(grid[a.x, a.y].x, grid[a.x, a.y].y, 0); 
    }

    public Vector2Int TranslateFromGrid(Vector3Int a)
    {
        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
            {
                if (grid[i, j].x == a.x && grid[i, j].y == a.y)
                    return new Vector2Int(i, j);
            }
        }
        return new Vector2Int();
    }

    private List<Vector2Int> AStar(Vector2Int start, Vector2Int destination)
    {
        var frontier = new PriorityQueue<Vector2Int>();
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Equals(destination))
                break;

            foreach (var next in GetNeighbors(current))
            {
                var newCost = costSoFar[current] + Cost(current,next);
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    var priority = newCost + Heuristic(next, destination);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        var path = new List<Vector2Int>();
        if (cameFrom.ContainsKey(destination))
            path = ReconstructPath(start, destination);

        return path;
    }

    private float Cost(Vector2Int from, Vector2Int to)
    {
        var cost = 1.0f;
        var nudge = 0.0f;

        if ((from.x + from.y) % 2 == 0 && to.x != from.x)
            nudge = 1.0f;

        if ((from.x + from.y) % 2 == 1 && to.y != from.y)
            nudge = 1.0f;

        return cost + 0.01f * nudge;
    }

    private List<Vector2Int> ReconstructPath(Vector2Int start, Vector2Int destination)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        var current = destination;
        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(current);
        path.Reverse();
        return path;
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    private List<Vector2Int> GetNeighbors(Vector2Int location)
    {
        var neighbors = new List<Vector2Int>();

        foreach (var dir in DIRS)
        {
            var next = location + dir;
            if (InBounds(next, grid))
                if (grid[next.x, next.y].z < 1)
                    neighbors.Add(next);
        }

        return neighbors;
    }

    private Vector2Int[] DIRS = new[]
       {
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1)
        };

    private bool InBounds(Vector2 id, Vector3Int[,] grid)
    {
        return 0 <= id.x && id.x < grid.GetLength(0)
            && 0 <= id.y && id.y < grid.GetLength(1);
    }
}


public class PriorityQueue<T>
{
    private List<Tuple<T, float>> elements = new List<Tuple<T, float>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}

