using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Node
{
    public float gCost, fCost, hCost;
    public List<Node> neightbors = new List<Node>();
    public bool walkable;
    public Node parent;
    public int GridX, GridY;
}
