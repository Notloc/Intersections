using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road
{
    public Path path { get; private set; }
    public Node Start { get { return path.nodes[0]; } }
    public Node End { get { return path.nodes[path.nodes.Count - 1]; } }

    public Road(Path path)
    {
        this.path = path;
    }

    public Node NextNode(Node node)
    {
        int indexOf = path.nodes.IndexOf(node);
        if (indexOf >= path.nodes.Count)
            return null;

        return path.nodes[indexOf + 1];
    }
}
