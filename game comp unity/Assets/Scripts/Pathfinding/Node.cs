using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector2 worldPosition;
    public int gCost = int.MaxValue;
    public int hCost = int.MaxValue;
    public Node parent;

    public Node(Vector2 _worldPosition) {
        worldPosition = _worldPosition;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
