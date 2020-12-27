using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector2 position;
    public int gCost = int.MaxValue;
    public int hCost = int.MaxValue;
    public Node parent;

    public Node(Vector2 _position) {
        position = _position;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
