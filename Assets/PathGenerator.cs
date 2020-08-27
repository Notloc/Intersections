using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public List<Node> nodes = new List<Node>();
}

public class Node
{
    public Node parent;
    public float Cost { get { return gCost + hCost; } }

    public float gCost;
    public float hCost;

    public Vector2Int position;
    public Direction direction;
    public int directionCombo;

    public Node(CellEntranceExit entrance)
    {
        this.position = entrance.Position;
        direction = entrance.Side.Opposite();
    }

    public Node(Node other, Vector2Int direction)
    {
        this.parent = other;
        this.position = parent.position + direction;

        Direction dir = GetDirection(direction);
        this.direction = dir;

        if (dir == parent.direction)
            this.directionCombo = parent.directionCombo + 1;
        else
            this.directionCombo = 0;
    }

    public static Direction GetDirection(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
            return Direction.NORTH;
        else if (dir == Vector2Int.down)
            return Direction.SOUTH;
        else if (dir == Vector2Int.right)
            return Direction.EAST;
        else
            return Direction.WEST;
    }
}

public static class PathGenerator
{
    private const int MINIMUM_COMBO = 2;

    private static Node GetLowestCostNode(ICollection<Node> nodes)
    {
        float lowestCost = float.MaxValue;
        Node lowestNode = null;
        foreach (Node node in nodes)
        {
            if (node.Cost < lowestCost)
            {
                lowestCost = node.Cost;
                lowestNode = node;
            }
        }
        return lowestNode;
    }

    private static void SetCost(Node node, Vector2Int target)
    {
        node.hCost = (node.position - target).magnitude;
        node.gCost = node.parent != null ? node.parent.gCost + 1: 0;
    }

    public static Path GeneratePath(Cell cell, CellEntranceExit entrance, CellEntranceExit exit, ErrorCollector errors)
    {
        // A*

        Path path = null;
        bool complete = false;

        HashSet<Node> openNodes = new HashSet<Node>();
        List<Node> closedNodes = new List<Node>();


        Vector2Int target = exit.Position + (exit.Side.Opposite().Vector()) * 2;

        Node startNode = new Node(entrance);
        SetCost(startNode, target);
        openNodes.Add(startNode);

        int depth = 0;


        while (openNodes.Count > 0 && complete == false && depth < 100000f)
        {
            Node node = GetLowestCostNode(openNodes);
            openNodes.Remove(node);
            closedNodes.Add(node);

            List<Node> newNodes = PerformStep(cell, node, target);
            foreach (Node newNode in newNodes)
            {
                if (IsComplete(newNode, target))
                {
                    if (newNode.directionCombo < MINIMUM_COMBO)
                        continue;

                    path = CreatePath(newNode, exit);
                    complete = true;
                }
                else
                {
                    openNodes.Add(newNode);
                }
            }
            //closedNodes.Add();
            depth++;
        }

        if (!complete)
        {
            errors.Add(new System.Exception("Could not generate a path! " + startNode.position + " -> " + target));
            path = new Path();
            path.nodes = closedNodes;
        }
        

        return path;
    }

    private static bool IsComplete(Node node, Vector2Int target)
    {
        return node.position == target;
    }

    private static Path CreatePath(Node node, CellEntranceExit exit)
    {
        Path path = new Path();
        List<Node> nodes = new List<Node>();

        node = new Node(node, exit.Side.Vector());
        node = new Node(node, exit.Side.Vector());

        nodes.Add(node);
        while (node.parent != null)
        {
            node = node.parent;
            nodes.Add(node);
        }

        nodes.Reverse();
        path.nodes = nodes;
        return path;
    }

    private static List<Node> PerformStep(Cell cell, Node node, Vector2Int target)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        List<Node> nodes = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Node newNode = new Node(node, direction);
            if (IsValid(cell, newNode))
            {
                SetCost(newNode, target);
                nodes.Add(newNode);
            }
        }

        return nodes;
    }

    public static bool IsValid(Cell cell, Node node)
    {
        Vector2Int position = node.position;
        if (position.x >= cell.Size || position.y >= cell.Size || position.x < 0 || position.y < 0)
        {
            return false;
        }

        // No U-turns
        if (node.direction.Opposite() == node.parent.direction)
            return false;

        // No squiggles
        if (node.direction != node.parent.direction && node.parent.directionCombo < MINIMUM_COMBO)
            return false;

        return true;

        BlockType type = (BlockType)cell.Get(position.x, position.y);

        switch (type)
        {
            case BlockType.NONE:
                return true;
            case BlockType.ROAD:
                return true;
        }

        return false;

    }
}
